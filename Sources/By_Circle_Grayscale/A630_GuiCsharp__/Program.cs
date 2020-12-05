﻿// 進行が停止するテストを含むデバッグモードです。
#define DEBUG_STOPPABLE

using System;
using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B310Settei.C500Struct;
using Grayscale.A630GuiCsharp.B110ShogiGui.C492Widgets;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500GUI;

namespace Grayscale.P699_Form_______
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
            MainGui_CsharpImpl mainGui = new MainGui_CsharpImpl();//new ShogiEngineVsClientImpl(this)

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainGui.OwnerForm = new Form1_Shogi(mainGui);
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            mainGui.Load_AsStart(errH);
            mainGui.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(Const_Filepath.m_EXE_TO_CONFIG + "data_widgets_01_shogiban.csv", mainGui));
            mainGui.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(Const_Filepath.m_EXE_TO_CONFIG + "data_widgets_02_console.csv", mainGui));
            mainGui.LaunchForm_AsBody(errH);
        }

    }
}
