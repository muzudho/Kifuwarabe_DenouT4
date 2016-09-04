using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A000_Platform___.B025_Machine____;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C400____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_Masu_______.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Generic;

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



            Sky position_Sky = Util_SkyCreator.New_Hirate();

            // 盤面をログ出力したいぜ☆
            logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(position_Sky)));
            logger.Flush(LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();


            List<Move> pv = new List<Move>();
            pv.Add(Move.Empty);// 「同」（※同歩など）を調べるために１つ前を見にくるので、空を入れておく。

            //────────────────────────────────────────
            // 一手指す☆
            //────────────────────────────────────────
            // ▲７六歩
            string commandLine = "7g7f 3c3d 8h2b+ 3a2b B*8h";

            for (int iPly=1; iPly<6; iPly++)
            {
                string rest;
                Move move = Conv_StringMove.ToMove(out rest, commandLine, pv[pv.Count - 1], position_Sky, logger);
                commandLine = rest;

                IttesasuResult syuryoResult;
                Util_IttesasuRoutine.DoMove(out syuryoResult, move, position_Sky, logger);
                pv.Add(move);
                position_Sky = syuryoResult.SyuryoKyokumenW.Kyokumen;

                // 盤面をログ出力したいぜ☆
                logger.AppendLine("sfen=[" + Conv_Move.ToSfen(move) + "]");
                logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(position_Sky)));
                logger.Flush(LogTypes.Plain);
                MachineImpl.GetInstance().ReadKey();
            }




            //────────────────────────────────────────
            // 一手戻す☆
            //────────────────────────────────────────
            for (int iPly = 5; 0<iPly; iPly--)
            {
                IttemodosuResult syuryoResult2;
                Util_IttemodosuRoutine.UndoMove(
                    out syuryoResult2,
                    position_Sky.Temezumi,
                    pv[pv.Count - 1],
                    position_Sky,
                    logger
                    );
                position_Sky = syuryoResult2.SyuryoSky;

                string sfen2 = Conv_Move.ToSfen(pv[pv.Count - 1]);

                // 盤面をログ出力したいぜ☆
                logger.AppendLine("back sfen2=[" + sfen2 + "]");
                logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(position_Sky)));
                logger.Flush(LogTypes.Plain);
                MachineImpl.GetInstance().ReadKey();
            }

        }
    }
}
