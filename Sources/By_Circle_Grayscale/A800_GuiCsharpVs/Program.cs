using System;
using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.P699_Form_______;

namespace Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C500____Gui
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            KwLogger errH = Util_Loggers.ProcessGui_DEFAULT;
            MainGui_CsharpVsImpl mainGuiVs = new MainGui_CsharpVsImpl();

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainGuiVs.OwnerForm = new Form1_Shogi(mainGuiVs);
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            mainGuiVs.Load_AsStart(errH);
            mainGuiVs.LaunchForm_AsBody(errH);

        }
    }
}
