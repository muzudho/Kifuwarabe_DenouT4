using System;
using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.P699Form;

namespace Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C500Gui
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ILogger errH = ErrorControllerReference.ProcessGuiDefault;
            MainGui_CsharpVsImpl mainGuiVs = new MainGui_CsharpVsImpl();

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainGuiVs.OwnerForm = new Form1Shogi(mainGuiVs);
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            mainGuiVs.Load_AsStart(errH);
            mainGuiVs.LaunchForm_AsBody(errH);

        }
    }
}
