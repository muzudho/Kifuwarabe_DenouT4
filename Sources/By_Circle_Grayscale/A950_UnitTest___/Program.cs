using Grayscale.A000_Platform___.B025_Machine____;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C400____Conv;
using System.Collections.Generic;
using System.Diagnostics;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

namespace Grayscale.A950_UnitTest___
{
    class Program
    {
        static void Main(string[] args)
        {
            KwLogger logger = Util_Loggers.ProcessUnitTest_DEFAULT;

            logger.AppendLine("テストＡ");
            logger.Flush(LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();



            Sky positionA = Util_SkyCreator.New_Hirate();
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
                    Move moveA = Conv_StringMove.ToMove(
                        out rest, commandLine, pv[pv.Count - 1],
                        positionA.GetKaisiPside(),
                        positionA, logger);
                    Move moveB;
                    commandLine = rest.Trim();

                    {
                        IttesasuResult syuryoResult;
                        moveB = moveA;
                        Util_IttesasuRoutine.DoMove_Normal(out syuryoResult,
                            ref moveB,// 駒を取った場合、moveは更新される。
                            positionA,
                            logger);
                        positionA = syuryoResult.SyuryoKyokumenW;

                        // 盤面をログ出力したいぜ☆
                        logger.AppendLine("sfen=[" + Conv_Move.ToSfen(moveB) + "] captured=[" + Conv_Komasyurui.ToStr_Ichimoji(Conv_Move.ToCaptured(moveB)) + "]");
                        logger.AppendLine(Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(
                            Conv_Move.ToPlayerside(moveB),
                            positionA, logger)
                            ,positionA,moveB));
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
                    logger.AppendLine("["+i+"]" + Conv_Move.ToLog(move));
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
                    IttemodosuResult syuryoResult2;
                    Util_IttemodosuRoutine.UndoMove(
                        out syuryoResult2,
                        move1,
                        Conv_Move.ToPlayerside(move1),
                        positionA,
                        "G900",
                        logger
                        );
                    positionA = syuryoResult2.SyuryoSky;
                    Debug.Assert(null != positionA, "局面がヌル");

                    // 盤面をログ出力したいぜ☆
                    logger.AppendLine("back sfen=[" + Conv_Move.ToSfen(move1) + "] captured=[" + Conv_Komasyurui.ToStr_Ichimoji(Conv_Move.ToCaptured(move1)) + "]");
                    logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(
                        Conv_Move.ToPlayerside(move1),
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
