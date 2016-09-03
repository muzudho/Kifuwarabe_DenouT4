using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A000_Platform___.B025_Machine____;

namespace Grayscale.A950_UnitTest___
{
    class Program
    {
        static void Main(string[] args)
        {
            KwLogger logger = Util_Loggers.ProcessUnitTest_DEFAULT;

            logger.Append("テストＡ");
            logger.Flush(LogTypes.Plain);

            MachineImpl.GetInstance().ReadKey();
        }
    }
}
