﻿using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A450Server.B110Server.C250Util;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;
using Grayscale.A630GuiCsharp.B110ShogiGui.C080Shape;
using Grayscale.A630GuiCsharp.B110ShogiGui.C249Function;

#if DEBUG
// using Grayscale.Kifuwaragyoku.Entities.Logging;
#endif

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C250Timed
{


    /// <summary>
    /// ▲人間vs△コンピューター対局のやりとりです。
    /// </summary>
    public class TimedA_EngineCapture : TimedAbstract
    {


        private MainGui_Csharp mainGui;


        public TimedA_EngineCapture(MainGui_Csharp shogibanGui)
        {
            this.mainGui = shogibanGui;
        }


        public override void Step(ILogTag logTag)
        {
            // 将棋エンジンからの入力が、input99 に溜まるものとします。
            if (0 < this.mainGui.ConsoleWindowGui.InputString99.Length)
            {

#if DEBUG
                string message = "(^o^)timer入力 input99=[" + this.mainGui.ConsoleWindowGui.InputString99 + "]";
                logTag.AppendLine(message);
                logTag.Flush(LogTypes.Plain);
#endif

                //
                // 棋譜入力テキストボックスに、指し手「（例）6a6b」を入力するための一連の流れです。
                //
                {
                    this.mainGui.RepaintRequest = new RepaintRequestImpl();
                    this.mainGui.RepaintRequest.SetNyuryokuTextTail(this.mainGui.ConsoleWindowGui.InputString99);// 受信文字列を、上部テキストボックスに入れるよう、依頼します。
                    this.mainGui.Response("Timer", logTag);// テキストボックスに、受信文字列を入れます。
                    this.mainGui.ConsoleWindowGui.ClearInputString99();// 受信文字列の要求を空っぽにします。
                }

                //
                // コマ送り
                //
                {
                    string restText = Util_Function_Csharp.ReadLine_FromTextbox();

                    //if ("noop" == restText)
                    //{
                    //    this.mainGui.Link_Server.EngineClient.ShogiEngineProcessWrapper.Send_Ok();
                    //    restText = "";
                    //}

                    Util_Functions_Server.Komaokuri_Srv(
                        ref restText,

                        this.mainGui.Link_Server.Earth,
                        this.mainGui.Link_Server.KifuTree,

                        this.mainGui.SkyWrapper_Gui,
                        logTag
                        );// 棋譜の[コマ送り]を実行します。
                    Util_Function_Csharp.Komaokuri_Gui(
                        restText,
                        this.mainGui.Link_Server.KifuTree.MoveEx_Current,
                        this.mainGui.Link_Server.KifuTree.PositionA,//.CurNode2ok.GetNodeValue()
                        this.mainGui,
                        this.mainGui.Link_Server.KifuTree,
                        logTag);//追加
                    // ↑チェンジターン済み
                    Util_Menace.Menace((MainGui_Csharp)this.mainGui, logTag);// メナス
                }

                //
                // ここで、テキストボックスには「（例）6a6b」が入っています。
                //

                //
                // 駒を動かす一連の流れです。
                //
                {
                    //this.ShogiGui.ResponseData.InputTextString = "";//空っぽにすることを要求する。
                    this.mainGui.Response("Timer", logTag);// GUIに反映させます。
                }

            }
        }

    }
}
