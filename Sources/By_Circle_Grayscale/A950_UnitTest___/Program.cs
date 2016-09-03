using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A000_Platform___.B025_Machine____;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;

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


        }
    }
}
