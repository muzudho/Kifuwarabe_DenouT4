using System.Collections.Generic;
using System.Diagnostics;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.CliOfUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogTag logTag = LogTags.ProcessUnitTestDefault;

            var buf1 = Logger.FlushBuf();
            buf1.AppendLine("テストＡ");
            Logger.Flush(logTag, LogTypes.Plain, buf1);
            MachineImpl.GetInstance().ReadKey();



            ISky positionA = UtilSkyCreator.New_Hirate();
            Playerside psideA_init = Playerside.P1;

            // 盤面をログ出力したいぜ☆
            var buf2 = Logger.FlushBuf();
            var boardLog = Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(psideA_init, positionA, logTag));
            buf2.AppendLine($"初期局面\n{boardLog}");
            Logger.Flush(logTag, LogTypes.Plain, buf2);
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
            var buf3 = Logger.FlushBuf();
            buf3.AppendLine($"commandLine={commandLine}");
            Logger.Flush(logTag, LogTypes.Plain, buf3);

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
                        var buf4 = Logger.FlushBuf();
                        var boardLog4 = Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(
                            ConvMove.ToPlayerside(moveB),
                            positionA, logTag)
                            , positionA, moveB);
                        buf4.AppendLine($"sfen=[{ConvMove.ToSfen(moveB)}] captured=[{Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(moveB))}]\n{boardLog4}");
                        Logger.Flush(logTag, LogTypes.Plain, buf4);

                        while (true)
                        {
                            var buf5 = Logger.FlushBuf();
                            buf5.AppendLine("[n]next [d]debug");
                            Logger.Flush(logTag, LogTypes.Plain, buf5);
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

                    var buf6 = Logger.FlushBuf();
                    buf6.AppendLine($"commandLine={commandLine}");
                    Logger.Flush(logTag, LogTypes.Plain, buf6);
                }
            }

            //────────────────────────────────────────
            // 指し手を全て出力するぜ☆（＾～＾）
            //────────────────────────────────────────
            {
                var buf7 = Logger.FlushBuf();
                int i = 0;
                foreach (Move move in pv)
                {
                    buf7.AppendLine($"[{i}]{ConvMove.ToLog(move)}");
                    i++;
                }
                Logger.Flush(logTag, LogTypes.Plain, buf7);
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
                    var buf8 = Logger.FlushBuf();
                    var boardLog8 = Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(
                        ConvMove.ToPlayerside(move1),
                        positionA, logTag));
                    buf8.AppendLine($"back sfen=[{ConvMove.ToSfen(move1)}] captured=[{Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(move1))}]\n{boardLog8}");
                    Logger.Flush(logTag, LogTypes.Plain, buf8);

                    while (true)
                    {
                        var buf9 = Logger.FlushBuf();
                        buf9.AppendLine("[b]back [d]debug");
                        Logger.Flush(logTag, LogTypes.Plain, buf9);
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
