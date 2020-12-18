﻿using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A630GuiCsharp.B110ShogiGui.C080Shape;
using Grayscale.A630GuiCsharp.B110ShogiGui.C081Canvas;
using Grayscale.A630GuiCsharp.B110ShogiGui.C100Widgets;
using Grayscale.A630GuiCsharp.B110ShogiGui.C125Scene;
using Grayscale.A630GuiCsharp.B110ShogiGui.C250Timed;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;

namespace Grayscale.P699Form
{
    public partial class UcForm2Main : UserControl
    {


        public UcForm2Main()
        {
            InitializeComponent();
        }


        public void SetInputareaText(string value)
        {
            //System.C onsole.WriteLine("☆セット：" + value);
            this.txtInputarea.Text = value;
        }

        public void AppendInputareaText(string value, [CallerMemberName] string memberName = "")
        {
            //System.C onsole.WriteLine("☆アペンド(" + memberName + ")：" + value);
            this.txtInputarea.Text += value;
        }

        public string GetOutputareaText()
        {
            return this.txtOutputarea.Text;
        }


        #region ゲームエンジンの振りをするメソッド

        /// <summary>
        /// ************************************************************************************************************************
        /// 入力欄のテキストを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public string ReadText()
        {
            return this.txtInputarea.Text;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 出力欄にテキストを出力します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public void WriteLine_Syuturyoku(string text)
        {
            this.txtOutputarea.Text = text;
        }

        #endregion

        private void txtInputarea_KeyDown(object sender, KeyEventArgs e)
        {
            // [Ctrl]+[A] で全選択します。
            AspectOriented_TextBox.KeyDown_SelectAll(sender, e);
        }

        private void txtOutputarea_KeyDown(object sender, KeyEventArgs e)
        {
            // [Ctrl]+[A] で全選択します。
            AspectOriented_TextBox.KeyDown_SelectAll(sender, e);
        }

        private void Uc_Form2Main_Paint(object sender, PaintEventArgs e)
        {
            MainGui_Csharp shogibanGui = ((Form2_Console)this.ParentForm).Form1_Shogi.Uc_Form1Main.MainGui;

            if (null == shogibanGui.ConsoleWindowGui.Shape_Canvas)
            {
                goto gt_EndMethod;
            }

            //------------------------------
            // 画面の描画です。
            //------------------------------
            shogibanGui.ConsoleWindowGui.Shape_Canvas.Paint(
                sender, e,
                shogibanGui.Link_Server.KifuTree.PositionA.GetKaisiPside(),
                shogibanGui.Link_Server.KifuTree.PositionA,
                shogibanGui, ShapeCanvasImpl.WINDOW_NAME_CONSOLE, LogTags.ProcessGuiPaint);

        gt_EndMethod:
            ;

        }

        private void Uc_Form2Main_MouseDown(object sender, MouseEventArgs e)
        {
            ILogTag logTag = LogTags.ProcessGuiDefault;
            MainGui_Csharp shogibanGui = ((Form2_Console)this.ParentForm).Form1_Shogi.Uc_Form1Main.MainGui;

            if (null == shogibanGui.Shape_PnlTaikyoku)
            {
                goto gt_EndMethod;
            }

            // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
            shogibanGui.RepaintRequest = new RepaintRequestImpl();


            if (e.Button == MouseButtons.Left)
            {
                //------------------------------------------------------------
                // 左ボタン
                //------------------------------------------------------------
                TimedBMouseCapture timeB = ((TimedBMouseCapture)shogibanGui.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(shogibanGui.Scene, ShapeCanvasImpl.WINDOW_NAME_CONSOLE, MouseEventStateName.MouseLeftButtonDown, e.Location, logTag));
            }
            else if (e.Button == MouseButtons.Right)
            {
                //------------------------------------------------------------
                // 右ボタン
                //------------------------------------------------------------
                TimedBMouseCapture timeB = ((TimedBMouseCapture)shogibanGui.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(shogibanGui.Scene, ShapeCanvasImpl.WINDOW_NAME_CONSOLE, MouseEventStateName.MouseRightButtonDown, e.Location, logTag));


                //------------------------------
                // このメインパネルの反応
                //------------------------------
                shogibanGui.Response("MouseOperation", logTag);

            }
            else
            {
                //------------------------------
                // このメインパネルの反応
                //------------------------------
                shogibanGui.Response("MouseOperation", logTag);
            }

        gt_EndMethod:
            ;

        }

        private void Uc_Form2Main_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ILogTag logTag = LogTags.ProcessGuiDefault;
            MainGui_Csharp mainGui = ((Form2_Console)this.ParentForm).Form1_Shogi.Uc_Form1Main.MainGui;

            // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
            mainGui.RepaintRequest = new RepaintRequestImpl();

            //------------------------------
            // マウスボタンが放されたときの、表示物の反応や、将棋データの変更がこの中に書かれています。
            //------------------------------
            if (e.Button == MouseButtons.Left)
            {
                //------------------------------------------------------------
                // 左ボタン
                //------------------------------------------------------------
                TimedBMouseCapture timeB = ((TimedBMouseCapture)mainGui.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(mainGui.Scene, ShapeCanvasImpl.WINDOW_NAME_CONSOLE, MouseEventStateName.MouseLeftButtonUp, e.Location, logTag));
            }
            else if (e.Button == MouseButtons.Right)
            {
                //------------------------------------------------------------
                // 右ボタン
                //------------------------------------------------------------
                TimedBMouseCapture timeB = ((TimedBMouseCapture)mainGui.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(mainGui.Scene, ShapeCanvasImpl.WINDOW_NAME_CONSOLE, MouseEventStateName.MouseRightButtonUp, e.Location, logTag));
            }
        }

    }
}
