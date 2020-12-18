using System.Collections.Generic;
using System.Diagnostics;
using Grayscale.A000Platform.B025Machine;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA;
using Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA;
using Grayscale.A210KnowNingen.B740KifuParserA.C400Conv;

namespace Grayscale.A950UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogTag logTag = LogTags.ProcessUnitTestDefault;

            Logger.AppendLine(logTag,"テストＡ");
            Logger.Flush(logTag, LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();



            ISky positionA = UtilSkyCreator.New_Hirate();
            Playerside psideA_init = Playerside.P1;

            // 盤面をログ出力したいぜ☆
            Logger.AppendLine(logTag, "初期局面");
            Logger.AppendLine(logTag, Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(psideA_init, positionA, logTag)));
            Logger.Flush(logTag, LogTypes.Plain);
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
            Logger.AppendLine(logTag, "commandLine=" + commandLine);
            Logger.Flush(logTag, LogTypes.Plain);

            List<Move> pv = new List<Move>();
            pv.Add(Move.Empty);// 「同」（※同歩など）を調べるために１つ前を見にくるので、空を入れておく。
            {
                commandLine = commandLine.Trim();
                while ("" != commandLine)
                {
                    string rest;
                    Move moveA = ConvStringMove.ToMove(
                        out rest, commandLine, pv[pv.Count - 1],
                        positionA.GetKaisiPside(),
                        positionA, logTag);
                    Move moveB;
                    commandLine = rest.Trim();

                    {
                        IIttesasuResult syuryoResult;
                        moveB = moveA;
                        UtilIttesasuRoutine.DoMove_Normal(out syuryoResult,
                            ref moveB,// 駒を取った場合、moveは更新される。
                            positionA,
                            logTag);
                        positionA = syuryoResult.SyuryoKyokumenW;

                        // 盤面をログ出力したいぜ☆
                        Logger.AppendLine(logTag, "sfen=[" + ConvMove.ToSfen(moveB) + "] captured=[" + Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(moveB)) + "]");
                        Logger.AppendLine(logTag, Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(
                            ConvMove.ToPlayerside(moveB),
                            positionA, logTag)
                            , positionA, moveB));
                        Logger.Flush(logTag, LogTypes.Plain);

                        while (true)
                        {
                            Logger.AppendLine(logTag, "[n]next [d]debug");
                            Logger.Flush(logTag, LogTypes.Plain);
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

                    Logger.AppendLine(logTag, "commandLine=" + commandLine);
                    Logger.Flush(logTag, LogTypes.Plain);
                }
            }

            //────────────────────────────────────────
            // 指し手を全て出力するぜ☆（＾～＾）
            //────────────────────────────────────────
            {
                int i = 0;
                foreach (Move move in pv)
                {
                    Logger.AppendLine(logTag, "[" + i + "]" + ConvMove.ToLog(move));
                    i++;
                }
                Logger.Flush(logTag, LogTypes.Plain);
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
                        positionA,
                        "G900",
                        logTag
                        );
                    positionA = syuryoResult2.SyuryoSky;
                    Debug.Assert(null != positionA, "局面がヌル");

                    // 盤面をログ出力したいぜ☆
                    Logger.AppendLine(logTag, "back sfen=[" + ConvMove.ToSfen(move1) + "] captured=[" + Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(move1)) + "]");
                    Logger.AppendLine(logTag, Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(
                        ConvMove.ToPlayerside(move1),
                        positionA, logTag)));
                    Logger.Flush(logTag, LogTypes.Plain);

                    while (true)
                    {
                        Logger.AppendLine(logTag, "[b]back [d]debug");
                        Logger.Flush(logTag, LogTypes.Plain);
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
