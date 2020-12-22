using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.CliOfUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogTag logTag = LogTags.ProcessUnitTestDefault;

            Logger.Trace("テストＡ");
            MachineImpl.GetInstance().ReadKey();



            ISky positionA = UtilSkyCreator.New_Hirate();
            Playerside psideA_init = Playerside.P1;

            // 盤面をログ出力したいぜ☆
            Logger.Trace($"初期局面\n{Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(psideA_init, positionA, logTag))}");
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
                        Logger.Trace($@"sfen=[{ConvMove.ToSfen(moveB)}] captured=[{Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(moveB))}]
{Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(ConvMove.ToPlayerside(moveB), positionA, logTag), positionA, moveB)}");

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

                    Logger.Trace($"commandLine={commandLine}");
                }
            }

            //────────────────────────────────────────
            // 指し手を全て出力するぜ☆（＾～＾）
            //────────────────────────────────────────
            {
                int i = 0;
                var buf = new StringBuilder();
                foreach (Move move in pv)
                {
                    buf.AppendLine($"[{i}]{ConvMove.ToLog(move)}");
                    i++;
                }
                Logger.Trace(buf.ToString());
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
                    Logger.Trace($@"back sfen=[{ConvMove.ToSfen(move1)}] captured=[{Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(move1))}]
{Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(ConvMove.ToPlayerside(move1), positionA, logTag))}");

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
