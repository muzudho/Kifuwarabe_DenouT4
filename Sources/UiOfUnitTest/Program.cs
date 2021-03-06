﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Grayscale.Kifuwaragyoku.Engine.Configuration;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Grayscale.Kifuwaragyoku.UseCases;

namespace Grayscale.Kifuwaragyoku.CliOfUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var engineConf = new EngineConf();
            EntitiesLayer.Implement(engineConf);

            var playing = new Playing(engineConf);

            Logger.Trace("テストＡ");
            MachineImpl.GetInstance().ReadKey();

            // 盤面をログ出力したいぜ☆
            var boardLog = Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(Playerside.P1, playing.StartingPosition));
            Logger.Trace($"初期局面\n{boardLog}");
            MachineImpl.GetInstance().ReadKey();


            //────────────────────────────────────────
            // 指し手☆
            //────────────────────────────────────────
            string commandLine = "7g7f 8b4b 8h3c+ 2b3c 9i9h 5a6b 7i8h 3c8h 2h8h 6b5a B*4e";
            // ▲７六歩
            // "7g7f 3c3d 8h2b+ 3a2b B*8h";

            //────────────────────────────────────────
            // 分解しながら、局面を進めるぜ☆（＾▽＾）
            //────────────────────────────────────────
            Logger.Trace($"commandLine={commandLine}");

            List<Move> pv = new List<Move>();
            pv.Add(Move.Empty);// 「同」（※同歩など）を調べるために１つ前を見にくるので、空を入れておく。
            {
                commandLine = commandLine.Trim();
                while ("" != commandLine)
                {
                    string rest;
                    Move moveA = ConvStringMove.ToMove(
                        out rest, commandLine, pv[pv.Count - 1],
                        playing.StartingPosition.GetKaisiPside(),
                        playing.StartingPosition);
                    Move moveB;
                    commandLine = rest.Trim();

                    {
                        IIttesasuResult syuryoResult;
                        moveB = moveA;
                        UtilIttesasuRoutine.DoMove_Normal(out syuryoResult,
                            ref moveB,// 駒を取った場合、moveは更新される。
                            playing.StartingPosition);
                        playing.StartingPosition = syuryoResult.SyuryoKyokumenW;

                        // 盤面をログ出力したいぜ☆
                        var boardLog4 = Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(
                            ConvMove.ToPlayerside(moveB),
                            playing.StartingPosition)
                            , playing.StartingPosition, moveB);
                        Logger.Trace($"sfen=[{ConvMove.ToSfen(moveB)}] captured=[{Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(moveB))}]\n{boardLog4}");

                        while (true)
                        {
                            Logger.Trace("[n]next [d]debug");
                            char key = MachineImpl.GetInstance().ReadKey();
                            switch (key)
                            {
                                case 'n': goto gt_Next;
                                case 'd': goto gt_Next;//ここにブレークポイントを仕掛けること。
                                default: break;
                            }
                        }
                    gt_Next:
                        ;
                    }
                    pv.Add(moveB);
                    Logger.Trace($"commandLine={commandLine}");
                }
            }

            //────────────────────────────────────────
            // 指し手を全て出力するぜ☆（＾～＾）
            //────────────────────────────────────────
            {
                var buf7 = new StringBuilder();
                int i = 0;
                foreach (Move move in pv)
                {
                    buf7.AppendLine($"[{i}]{ConvMove.ToLog(move)}");
                    i++;
                }
                Logger.Trace(buf7.ToString());
            }

            //────────────────────────────────────────
            // 逆回転☆（＾▽＾）
            //────────────────────────────────────────
            pv.Reverse();
            foreach (Move move1 in pv)
            {
                if (Move.Empty != move1)
                {
                    IIttemodosuResult syuryoResult2;
                    UtilIttemodosuRoutine.UndoMove(
                        out syuryoResult2,
                        move1,
                        ConvMove.ToPlayerside(move1),
                        playing.StartingPosition,
                        "G900");
                    playing.StartingPosition = syuryoResult2.SyuryoSky;
                    Debug.Assert(null != playing.StartingPosition, "局面がヌル");

                    // 盤面をログ出力したいぜ☆
                    var boardLog8 = Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(
                        ConvMove.ToPlayerside(move1),
                        playing.StartingPosition));
                    Logger.Trace($"back sfen=[{ConvMove.ToSfen(move1)}] captured=[{Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(move1))}]\n{boardLog8}");

                    while (true)
                    {
                        Logger.Trace("[b]back [d]debug");
                        char key = MachineImpl.GetInstance().ReadKey();
                        switch (key)
                        {
                            case 'b': goto gt_Next;
                            case 'd': goto gt_Next;//ここにブレークポイントを仕掛けること。
                            default: break;
                        }
                    }
                gt_Next:
                    ;
                }
            }
        }
    }
}
