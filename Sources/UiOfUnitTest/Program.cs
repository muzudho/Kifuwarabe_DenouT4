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
            ILogger logger = ErrorControllerReference.ProcessUnitTestDefault;

            logger.AppendLine("テストＡ");
            logger.Flush(LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();



            ISky positionA = UtilSkyCreator.New_Hirate();
            Playerside psideA_init = Playerside.P1;

            // 盤面をログ出力したいぜ☆
            logger.AppendLine("初期局面");
            logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(psideA_init, positionA, logger)));
            logger.Flush(LogTypes.Plain);
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
            logger.AppendLine("commandLine=" + commandLine);
            logger.Flush(LogTypes.Plain);

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
                        positionA, logger);
                    Move moveB;
                    commandLine = rest.Trim();

                    {
                        IIttesasuResult syuryoResult;
                        moveB = moveA;
                        UtilIttesasuRoutine.DoMove_Normal(out syuryoResult,
                            ref moveB,// 駒を取った場合、moveは更新される。
                            positionA,
                            logger);
                        positionA = syuryoResult.SyuryoKyokumenW;

                        // 盤面をログ出力したいぜ☆
                        logger.AppendLine("sfen=[" + ConvMove.ToSfen(moveB) + "] captured=[" + Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(moveB)) + "]");
                        logger.AppendLine(Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(
                            ConvMove.ToPlayerside(moveB),
                            positionA, logger)
                            , positionA, moveB));
                        logger.Flush(LogTypes.Plain);

                        while (true)
                        {
                            logger.AppendLine("[n]next [d]debug");
                            logger.Flush(LogTypes.Plain);
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

                    logger.AppendLine("commandLine=" + commandLine);
                    logger.Flush(LogTypes.Plain);
                }
            }

            //────────────────────────────────────────
            // 指し手を全て出力するぜ☆（＾～＾）
            //────────────────────────────────────────
            {
                int i = 0;
                foreach (Move move in pv)
                {
                    logger.AppendLine("[" + i + "]" + ConvMove.ToLog(move));
                    i++;
                }
                logger.Flush(LogTypes.Plain);
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
                        logger
                        );
                    positionA = syuryoResult2.SyuryoSky;
                    Debug.Assert(null != positionA, "局面がヌル");

                    // 盤面をログ出力したいぜ☆
                    logger.AppendLine("back sfen=[" + ConvMove.ToSfen(move1) + "] captured=[" + Conv_Komasyurui.ToStr_Ichimoji(ConvMove.ToCaptured(move1)) + "]");
                    logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(
                        ConvMove.ToPlayerside(move1),
                        positionA, logger)));
                    logger.Flush(LogTypes.Plain);

                    while (true)
                    {
                        logger.AppendLine("[b]back [d]debug");
                        logger.Flush(LogTypes.Plain);
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
