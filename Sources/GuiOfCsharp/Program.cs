// 進行が停止するテストを含むデバッグモードです。
#define DEBUG_STOPPABLE

using System;
using System.IO;
using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B310Settei.C500Struct;
using Grayscale.A630GuiCsharp.B110ShogiGui.C492Widgets;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500GUI;
using Nett;

namespace Grayscale.P699Form
{

    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ILogTag logTag = LogTags.ProcessGuiDefault;
            MainGui_CsharpImpl mainGui = new MainGui_CsharpImpl();//new ShogiEngineVsClientImpl(this)

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainGui.OwnerForm = new Form1Shogi(mainGui);
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            mainGui.Load_AsStart(logTag);
            mainGui.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Shogiban01Widgets")), mainGui));
            mainGui.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Console02Widgets")), mainGui));
            mainGui.LaunchForm_AsBody(logTag);
        }

    }
}
