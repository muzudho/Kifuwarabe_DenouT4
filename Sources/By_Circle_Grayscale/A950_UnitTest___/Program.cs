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



            Sky sky = Util_SkyCreator.New_Hirate();

            // 盤面をログ出力したいぜ☆
            logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(sky)));
            logger.Flush(LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();

            // ７六歩
            bool abnormal;
            string rest;
            Move move = Conv_StringMove.ToMove(
                out abnormal,
                out rest,
                "7g7f",
                Move.Empty,
                sky,
                logger
                );

            string sfen = Conv_Move.ToSfen(move);

            logger.AppendLine("sfen=["+ sfen+"]");
            logger.Flush(LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();

            // 一手指す☆
            IttesasuResult syuryoResult;
            Util_IttesasuRoutine.DoMove(
                out syuryoResult,
                move,
                sky,
                logger);

            // 盤面をログ出力したいぜ☆
            logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(syuryoResult.SyuryoKyokumenW.Kyokumen)));
            logger.Flush(LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();

        }
    }
}
