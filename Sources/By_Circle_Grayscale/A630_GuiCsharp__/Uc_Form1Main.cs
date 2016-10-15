using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;
using Grayscale.A060_Application.B310_Settei_____.L510____Xml;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___100_Widgets;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___125_Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___499_Repaint;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C060____TextBoxListener;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C080____Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C081____Canvas;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C125____Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C249____Function;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C250____Timed;
using System;
using System.Text;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#endif

namespace Grayscale.P699_Form_______
{

    /// <summary>
    /// ************************************************************************************************************************
    /// メイン画面です。
    /// ************************************************************************************************************************
    /// </summary>
    [Serializable]
    public partial class Uc_Form1Main : UserControl, Uc_Form1Mainable
    {

        #region プロパティー類

        public MainGui_Csharp MainGui { get { return this.mainGui; } }
        public void SetMainGui(MainGui_Csharp mainGui)
        {
            this.mainGui = mainGui;
        }
        private MainGui_Csharp mainGui;

        /// <summary>
        /// 設定XMLファイル
        /// </summary>
        public SetteiXmlFile SetteiXmlFile
        {
            get
            {
                return this.setteiXmlFile;
            }
        }
        private SetteiXmlFile setteiXmlFile;


        private const int NSQUARE = 9 * 9;

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public Uc_Form1Main()
        {
            InitializeComponent();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessGui_DEFAULT;

            this.MainGui.Timer_Tick(errH);
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// 起動直後の流れです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uc_Form1Main_Load(object sender, EventArgs e)
        {
            KwLogger logger = Util_Loggers.ProcessGui_DEFAULT;

            Uc_Form2Main uc_Form2Main = ((Form1_Shogi)this.ParentForm).Form2_Console.Uc_Form2Main;

            //
            // 設定XMLファイル
            //
            {
                this.setteiXmlFile = new SetteiXmlFile(Const_Filepath.m_EXE_TO_CONFIG + "data_settei.xml");
                if (!this.SetteiXmlFile.Exists())
                {
                    // ファイルが存在しませんでした。

                    // 作ります。
                    this.SetteiXmlFile.Write();
                }

                if (!this.SetteiXmlFile.Read())
                {
                    // 読取に失敗しました。
                }

                // デバッグ
                this.SetteiXmlFile.DebugWrite();
            }


            //----------
            // 棋譜
            //----------
            //
            //      先後や駒など、対局に用いられる事柄、物を事前準備しておきます。
            //

            //----------
            // 駒の並べ方
            //----------
            //
            //      平手に並べます。
            //
            {
                Sky positionInit = Util_SkyCreator.New_Hirate();//起動直後
                this.MainGui.Link_Server.Earth.Clear();

                // 棋譜を空っぽにします。
                Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(this.MainGui.Link_Server.KifuTree, positionInit,logger);

                this.MainGui.Link_Server.Earth.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面

                this.MainGui.SkyWrapper_Gui.SetGuiSky(positionInit);
            }



            // 全駒の再描画
            this.MainGui.RepaintRequest = new RepaintRequestImpl();
            this.MainGui.RepaintRequest.SetFlag_RecalculateRequested();

            //----------
            // フェーズ
            //----------
            this.MainGui.SetScene(SceneName.SceneB_1TumamitaiKoma);

            //----------
            // 監視
            //----------
            this.gameEngineTimer1.Start();

            //----------
            // 将棋エンジンが、コンソールの振りをします。
            //----------
            //
            //      このメインパネルに、コンソールの振りをさせます。
            //      将棋エンジンがあれば、将棋エンジンの入出力を返すように内部を改造してください。
            //
            TextboxListener.SetTextboxListener(
                uc_Form2Main.ReadText, uc_Form2Main.WriteLine_Syuturyoku);


            //----------
            // 画面の出力欄
            //----------
            //
            //      出力欄（上下段）を空っぽにします。
            //
            uc_Form2Main.WriteLine_Syuturyoku("");



            this.MainGui.Response("Launch", logger);

            // これで、最初に見える画面の準備は終えました。
            // あとは、操作者の入力を待ちます。
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 描画するのはここです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uc_Form1Main_Paint(object sender, PaintEventArgs e)
        {
            if (null == this.MainGui.Shape_PnlTaikyoku)
            {
                goto gt_EndMethod;
            }

            //------------------------------
            // 画面の描画です。
            //------------------------------
            this.MainGui.Shape_PnlTaikyoku.Paint(
                sender,
                e,
                this.MainGui.Link_Server.KifuTree.PositionA.GetKaisiPside(),
                this.MainGui.Link_Server.KifuTree.PositionA,
                this.MainGui, Shape_CanvasImpl.WINDOW_NAME_SHOGIBAN, Util_Loggers.ProcessGui_PAINT);

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスが動いたときの挙動です。
        /// ************************************************************************************************************************
        /// 
        ///         マウスが重なったときの、表示物の反応や、将棋データの変更がこの中に書かれています。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uc_Form1Main_MouseMove(object sender, MouseEventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessGui_DEFAULT;

            if (null != this.MainGui.Shape_PnlTaikyoku)
            {
                // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
                this.MainGui.RepaintRequest = new RepaintRequestImpl();

                // マウスムーブ
                {
                    TimedB_MouseCapture timeB = ((TimedB_MouseCapture)this.MainGui.TimedB_MouseCapture);
                    timeB.MouseEventQueue.Enqueue(
                        new MouseEventState(this.MainGui.Scene, Shape_CanvasImpl.WINDOW_NAME_SHOGIBAN, MouseEventStateName.MouseMove, e.Location, errH));
                }

                //------------------------------
                // このメインパネルの反応
                //------------------------------
                this.MainGui.Response("MouseOperation", errH);
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスのボタンを押下したときの挙動です。
        /// ************************************************************************************************************************
        /// 
        ///         マウスボタンが押下されたときの、表示物の反応や、将棋データの変更がこの中に書かれています。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uc_Form1Main_MouseDown(object sender, MouseEventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessGui_DEFAULT;

            if (null == this.MainGui.Shape_PnlTaikyoku)
            {
                goto gt_EndMethod;
            }

            // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
            this.MainGui.RepaintRequest = new RepaintRequestImpl();


            if (e.Button == MouseButtons.Left)
            {
                //------------------------------------------------------------
                // 左ボタン
                //------------------------------------------------------------
                TimedB_MouseCapture timeB = ((TimedB_MouseCapture)this.MainGui.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(this.MainGui.Scene, Shape_CanvasImpl.WINDOW_NAME_SHOGIBAN, MouseEventStateName.MouseLeftButtonDown, e.Location, errH));
            }
            else if (e.Button == MouseButtons.Right)
            {
                //------------------------------------------------------------
                // 右ボタン
                //------------------------------------------------------------
                TimedB_MouseCapture timeB = ((TimedB_MouseCapture)this.MainGui.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(this.MainGui.Scene, Shape_CanvasImpl.WINDOW_NAME_SHOGIBAN, MouseEventStateName.MouseRightButtonDown, e.Location, errH));


                //------------------------------
                // このメインパネルの反応
                //------------------------------
                this.MainGui.Response("MouseOperation", errH);

            }
            else
            {
                //------------------------------
                // このメインパネルの反応
                //------------------------------
                this.MainGui.Response("MouseOperation", errH);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスのボタンが放されたときの挙動です。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uc_Form1Main_MouseUp(object sender, MouseEventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessGui_DEFAULT;

            // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
            this.MainGui.RepaintRequest = new RepaintRequestImpl();

            //------------------------------
            // マウスボタンが放されたときの、表示物の反応や、将棋データの変更がこの中に書かれています。
            //------------------------------
            if (e.Button == MouseButtons.Left)
            {
                //------------------------------------------------------------
                // 左ボタン
                //------------------------------------------------------------
                TimedB_MouseCapture timeB = ((TimedB_MouseCapture)this.MainGui.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(this.MainGui.Scene, Shape_CanvasImpl.WINDOW_NAME_SHOGIBAN, MouseEventStateName.MouseLeftButtonUp, e.Location, errH));
            }
            else if (e.Button == MouseButtons.Right)
            {
                //------------------------------------------------------------
                // 右ボタン
                //------------------------------------------------------------
                TimedB_MouseCapture timeB = ((TimedB_MouseCapture)this.MainGui.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(this.MainGui.Scene, Shape_CanvasImpl.WINDOW_NAME_SHOGIBAN, MouseEventStateName.MouseRightButtonUp, e.Location, errH));
            }
        }


        public Form1_Mutex MutexOwner { get; set; }

        /// <summary>
        /// ************************************************************************************************************************
        /// 入力欄の表示・出力欄の表示・再描画
        /// ************************************************************************************************************************
        /// 
        /// このメインパネルに何かして欲しいことがあれば、
        /// RequestForMain に要望を入れて、この関数を呼び出してください。
        ///
        /// 同時には処理できない項目もあります。
        /// </summary>
        /// <param name="response"></param>
        public void Solute_RepaintRequest(
            Form1_Mutex mutex, MainGui_Csharp mainGui, KwLogger errH)
        {
            Uc_Form2Main form2 = ((Form1_Shogi)this.ParentForm).Form2_Console.Uc_Form2Main;

            //------------------------------------------------------------
            // 駒の座標再計算
            //------------------------------------------------------------
            if (mainGui.RepaintRequest.Is_KomasRecalculateRequested())
            {
                this.MainGui.SkyWrapper_Gui.GuiSky.Foreach_Busstops((Finger finger, Busstop busstop, ref bool toBreak) =>
                {
                    Util_Function_Csharp.Redraw_KomaLocation(finger, this.MainGui, errH);
                });
            }
            mainGui.RepaintRequest.Clear_KomasRecalculateRequested();


            //------------------------------
            // 入力欄の表示
            //------------------------------
            if (mainGui.RepaintRequest.IsRequested_RepaintNyuryokuText)
            {
                // 指定のテキストで上書きします。
                form2.SetInputareaText(mainGui.RepaintRequest.NyuryokuText);
            }
            else if (mainGui.RepaintRequest.IsRequested_NyuryokuTextTail)
            {
                // 指定のテキストを後ろに足します。
                form2.AppendInputareaText(mainGui.RepaintRequest.NyuryokuTextTail);
                mainGui.RepaintRequest.SetNyuryokuTextTail( "");//要求の解除
            }

            //------------------------------
            // 出力欄（上・下段）の表示
            //------------------------------
            switch (mainGui.RepaintRequest.SyuturyokuRequest)
            {
                case RepaintRequestGedanTxt.Clear:
                    {
                        // 出力欄（上下段）を空っぽにします。
                        form2.WriteLine_Syuturyoku("");

                        // ログ
                        //errH.AppendLine_AddMemo( "");
                        //errH.AppendLine_AddMemo( "");
                    }
                    break;

                case RepaintRequestGedanTxt.Kifu:
                    {
                        // 出力欄（上下段）に、棋譜を出力します。
                        switch (this.MainGui.SyuturyokuKirikae)
                        {
                            case SyuturyokuKirikae.Japanese:
                                form2.WriteLine_Syuturyoku(Util_KirokuGakari.ToJsaFugoListString(
                                    this.MainGui.Link_Server.Earth,
                                    this.MainGui.Link_Server.KifuTree,//.CurrentNode,
                                    "Ui_PnlMain.Response", errH));
                                break;
                            case SyuturyokuKirikae.Sfen:
                                form2.WriteLine_Syuturyoku(
                                    Util_KirokuGakari.ToSfen_PositionCommand(
                                        this.MainGui.Link_Server.Earth,
                                        this.MainGui.Link_Server.KifuTree//.CurrentNode//エンドノード
                                        ));
                                break;
                            case SyuturyokuKirikae.Html:
                                form2.WriteLine_Syuturyoku(Uc_Form1Main.CreateHtml(this.MainGui));
                                break;
                        }

#if DEBUG
                        // ログ
                        errH.AppendLine(form2.GetOutputareaText());
                        errH.Flush(LogTypes.Plain);
#endif
                    }
                    break;

                default:
                    // スルー
                    break;
            }

            //------------------------------
            // 再描画
            //------------------------------
            if (mainGui.RepaintRequest.IsRefreshRequested())
            {
                this.Refresh();

                mainGui.RepaintRequest.ClearRefreshRequest();
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 出力欄（上段）でキーボードのキーが押されたときの挙動をここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOutput1_KeyDown(object sender, KeyEventArgs e)
        {
            AspectOriented_TextBox.KeyDown_SelectAll(sender, e);
            ////------------------------------
            //// [Ctrl]+[A] で、全選択します。
            ////------------------------------
            //if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            //{
            //    ((TextBox)sender).SelectAll();
            //} 
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 入力欄でキーボードのキーが押されたときの挙動をここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput1_KeyDown(object sender, KeyEventArgs e)
        {
            AspectOriented_TextBox.KeyDown_SelectAll(sender, e);
            ////------------------------------
            //// [Ctrl]+[A] で、全選択します。
            ////------------------------------
            //if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            //{
            //    ((TextBox)sender).SelectAll();
            //} 
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// HTML出力。（これは作者のホームページ用に書かれています）
        /// ************************************************************************************************************************
        /// </summary>
        public static string CreateHtml(MainGui_Csharp mainGui)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<div style=\"position:relative; left:0px; top:0px; border:solid 1px black; width:250px; height:180px;\">");

            // 後手の持ち駒
            sb.AppendLine("    <div style=\"position:absolute; left:0px; top:2px; width:30px;\">");
            sb.AppendLine("        △後手");
            sb.AppendLine("        <div style=\"margin-top:10px; width:30px;\">");
            sb.Append("            ");

            Sky siteiSky = mainGui.SkyWrapper_Gui.GuiSky;

            //────────────────────────────────────────
            // 持ち駒（後手）
            //────────────────────────────────────────
            siteiSky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToOkiba(koma) == Okiba.Gote_Komadai)
                {
                    sb.Append(Util_Komasyurui14.Fugo[(int)Conv_Busstop.ToKomasyurui(koma)]);
                }
            });

            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");

            //────────────────────────────────────────
            // 盤上
            //────────────────────────────────────────
            sb.AppendLine("    <div style=\"position:absolute; left:30px; top:2px; width:182px;\">");
            sb.AppendLine("        <table>");
            for (int dan = 1; dan <= 9; dan++)
            {
                sb.Append("        <tr>");
                for (int suji = 9; 1 <= suji; suji--)
                {
                    bool isSpace = true;

                    siteiSky.Foreach_Busstops((Finger finger, Busstop koma2, ref bool toBreak) =>
                    {
                        int suji2;
                        int dan2;
                        Okiba okiba = Conv_Masu.ToOkiba(Conv_Busstop.ToMasu(koma2));
                        if (okiba == Okiba.ShogiBan)
                        {
                            Conv_Masu.ToSuji_FromBanjoMasu(Conv_Busstop.ToMasu(koma2), out suji2);
                            Conv_Masu.ToDan_FromBanjoMasu(Conv_Busstop.ToMasu(koma2), out dan2);
                        }
                        else
                        {
                            Conv_Masu.ToSuji_FromBangaiMasu(Conv_Busstop.ToMasu(koma2), out suji2);
                            Conv_Masu.ToDan_FromBangaiMasu(Conv_Busstop.ToMasu(koma2), out dan2);
                        }


                        if (
                            Conv_Busstop.ToOkiba(koma2) == Okiba.ShogiBan //盤上
                            && suji2 == suji
                            && dan2 == dan
                        )
                        {
                            if (Playerside.P2 == Conv_Busstop.ToPlayerside( koma2))
                            {
                                sb.Append("<td><span class=\"koma2x\">");
                                sb.Append(Util_Komasyurui14.Fugo[(int)Conv_Busstop.ToKomasyurui(koma2)]);
                                sb.Append("</span></td>");
                                isSpace = false;
                            }
                            else
                            {
                                sb.Append("<td><span class=\"koma1x\">");
                                sb.Append(Util_Komasyurui14.Fugo[(int)Conv_Busstop.ToKomasyurui(koma2)]);
                                sb.Append("</span></td>");
                                isSpace = false;
                            }
                        }


                    });

                    if (isSpace)
                    {
                        sb.Append("<td>　</td>");
                    }
                }

                sb.AppendLine("</tr>");
            }
            sb.AppendLine("        </table>");
            sb.AppendLine("    </div>");

            //────────────────────────────────────────
            // 持ち駒（先手）
            //────────────────────────────────────────
            sb.AppendLine("    <div style=\"position:absolute; left:215px; top:2px; width:30px;\">");
            sb.AppendLine("        ▲先手");
            sb.AppendLine("        <div style=\"margin-top:10px; width:30px;\">");
            sb.Append("            ");

            siteiSky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToOkiba(koma) == Okiba.Sente_Komadai)
                {
                    sb.Append(Util_Komasyurui14.Fugo[(int)Conv_Busstop.ToKomasyurui(koma)]);
                }
            });

            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");

            //
            sb.AppendLine("</div>");

            return sb.ToString();
        }


    }
}
