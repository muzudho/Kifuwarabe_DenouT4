using Codeplex.Data;//DynamicJson
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;
using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A210_KnowNingen_.B300_KomahaiyaTr.C500____Table;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C500____Util;
using Grayscale.A210_KnowNingen_.B380_Michi______.C500____Word;
using Grayscale.A210_KnowNingen_.B390_KomahaiyaEx.C500____Util;
using Grayscale.A210_KnowNingen_.B490_ForcePromot.C250____Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C250____Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;
using Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient;
using Grayscale.A450_Server_____.B110_Server_____.C498____Server;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___081_Canvas;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___125_Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___492_Widgets;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___499_Repaint;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C080____Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C081____Canvas;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C125____Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C249____Function;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C250____Timed;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C492____Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#endif

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C500____GUI
{

    /// <summary>
    /// 将棋盤ウィンドウ（Ｃ＃）に対応。
    /// 
    /// コンソール・ウィンドウを持っている。
    /// </summary>
    public class MainGui_CsharpImpl : MainGui_Csharp
    {

        #region プロパティー

        /// <summary>
        /// 将棋サーバー。
        /// </summary>
        public Server Link_Server { get { return this.server; } }
        protected Server server;

        public SkyWrapper_Gui SkyWrapper_Gui { get { return this.m_skyWrapper_Gui_; } }
        private SkyWrapper_Gui m_skyWrapper_Gui_;

        /// <summary>
        /// コンソール・ウィンドウ。
        /// </summary>
        public SubGui ConsoleWindowGui { get { return this.consoleWindowGui; } }
        private SubGui consoleWindowGui;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, UserWidget> Widgets { get; set; }
        public void SetWidget(string name, UserWidget widget)
        {
            this.Widgets[name] = widget;
        }
        public UserWidget GetWidget(string name)
        {
            UserWidget widget;

            if (this.Widgets.ContainsKey(name))
            {
                widget = this.Widgets[name];
            }
            else
            {
                widget = UserButtonImpl.NULL_OBJECT;
            }

            return widget;
        }

        public Timed TimedA { get; set; }
        public Timed TimedB_MouseCapture { get; set; }
        public Timed TimedC { get; set; }

        public RepaintRequest RepaintRequest { get; set; }

        /// <summary>
        /// 使い方：((Form1_Shogiable)this.OwnerForm)
        /// </summary>
        public Form OwnerForm { get; set; }

        /// <summary>
        /// ウィジェット読込みクラス。
        /// </summary>
        public List<WidgetsLoader> WidgetLoaders { get; set; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// グラフィックを描くツールは全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_PnlTaikyoku Shape_PnlTaikyoku
        {
            get
            {
                return this.shape_PnlTaikyoku;
            }
        }
        private Shape_PnlTaikyoku shape_PnlTaikyoku;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// ゲームの流れの状態遷移図はこれです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public SceneName Scene
        {
            get
            {
                return this.scene1;
            }
        }

        public void SetScene(SceneName scene)
        {
            if (SceneName.Ignore != scene)
            {
                this.scene1 = scene;
            }
        }
        private SceneName scene1;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒を動かす状態遷移図はこれです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public SceneName FlowB
        {
            get
            {
                return this.flowB;
            }
        }
        public void SetFlowB(SceneName name1, KwLogger errH)
        {
            this.flowB = name1;

            //アライブ
            {
                TimedB_MouseCapture timeB = ((TimedB_MouseCapture)this.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(name1, Shape_CanvasImpl.WINDOW_NAME_SHOGIBAN, MouseEventStateName.Arive, Point.Empty, errH));
            }
        }
        private SceneName flowB;


        /// <summary>
        /// 設定データCSV
        /// </summary>
        public Data_Settei_Csv Data_Settei_Csv { get; set; }


        #endregion



        #region コンストラクター

        /// <summary>
        /// 生成後、OwnerFormをセットしてください。
        /// </summary>
        public MainGui_CsharpImpl()
        {
            this.m_skyWrapper_Gui_ = new SkyWrapper_GuiImpl();
            this.server = new Server_Impl(this.m_skyWrapper_Gui_.GuiSky, new Receiver_ForCsharpVsImpl());

            this.Widgets = new Dictionary<string, UserWidget>();

            this.consoleWindowGui = new SubGuiImpl(this);

            this.TimedA = new TimedA_EngineCapture(this);
            this.TimedB_MouseCapture = new TimedB_MouseCapture(this);
            this.TimedC = new TimedC_SaiseiCapture(this);

            this.Data_Settei_Csv = new Data_Settei_Csv();
            this.WidgetLoaders = new List<WidgetsLoader>();
            this.RepaintRequest = new RepaintRequestImpl();

            //----------
            // ビュー
            //----------
            //
            //      ボタンや将棋盤などを描画するツールを、事前準備しておきます。
            //
            this.shape_PnlTaikyoku = new Shape_PnlTaikyokuImpl("#TaikyokuPanel", this);

            //System.C onsole.WriteLine("つまんでいる駒を放します。(1)");
            this.SetFigTumandeiruKoma(-1);

            //----------
            // [出力切替]初期値
            //----------
            this.syuturyokuKirikae = SyuturyokuKirikae.Japanese;
        }

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンを起動します。
        /// ************************************************************************************************************************
        /// </summary>
        public virtual void Start_ShogiEngine(string shogiEngineFilePath, KwLogger errH)
        {
        }

        /// <summary>
        /// コンピューターの先手
        /// </summary>
        public virtual void Do_ComputerSente(KwLogger errH)
        {
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 手番が替わったときの挙動を、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        public virtual void ChangedTurn(
            //MoveEx endNode,
            Tree kifu1,

            Playerside pside,
            KwLogger errH)
        {
        }


        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public virtual void Shutdown(KwLogger errH)
        {
        }


        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public virtual void Logdase(KwLogger errH)
        {
        }



        private int noopSend_counter;
        public void Timer_Tick( KwLogger errH)
        {
            if (this.server.EngineClient.ShogiEngineProcessWrapper.IsLive_ShogiEngine())
            {
                // だいたい 1tick 50ms と考えて、20倍で 1秒。
                if ( 20 * 3 < this.noopSend_counter) // 3秒に 1 回ぐらい ok を送れば？
                {
                    // noop
                    this.server.EngineClient.ShogiEngineProcessWrapper.Send_Noop_from_server(errH);
                    this.noopSend_counter = 0;
                }
                else
                {
                    this.noopSend_counter++;
                }
            }

            this.TimedA.Step(errH);
            this.TimedB_MouseCapture.Step(errH);
            this.TimedC.Step(errH);
        }




        /// <summary>
        /// 見た目の設定を読み込みます。
        /// </summary>
        public void ReadStyle_ToForm(Form1_Shogiable ui_Form1)
        {
            try
            {
                string filepath2 = Path.Combine( Path.Combine( Application.StartupPath, Const_Filepath.m_EXE_TO_CONFIG), "data_style.txt");
#if DEBUG
                MessageBox.Show("独自スタイルシート　filepath2=" + filepath2);
#endif

                if (File.Exists(filepath2))
                {
                    string styleText = System.IO.File.ReadAllText(filepath2, Encoding.UTF8);

                    try
                    {
                        var jsonMousou_arr = DynamicJson.Parse(styleText);

                        var bodyElm = jsonMousou_arr["body"];

                        if (null != bodyElm)
                        {
                            var backColor = bodyElm["backColor"];

                            if (null != backColor)
                            {
                                var var_alpha = backColor["alpha"];

                                int red = (int)backColor["red"];

                                int green = (int)backColor["green"];

                                int blue = (int)backColor["blue"];

                                if (null != var_alpha)
                                {
                                    ui_Form1.Uc_Form1Main.BackColor = Color.FromArgb((int)var_alpha, red, green, blue);
                                }
                                else
                                {
                                    ui_Form1.Uc_Form1Main.BackColor = Color.FromArgb(red, green, blue);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("JSONのパース時にエラーか？：" + ex.GetType().Name + "：" + ex.Message);
                        throw ex;
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("JSONのパース時にエラーか？：" + ex.GetType().Name + "：" + ex.Message);
            }
        }


        /// <summary>
        /// このアプリケーションソフトの開始時の処理。
        /// </summary>
        public virtual void Load_AsStart(KwLogger errH)
        {
            //
            // 既存のログファイルを削除したい。
            //
            {

            }

            {
#if DEBUG
                errH.AppendLine("(^o^)乱数のたね＝[" + KwRandom.Seed + "]");
                errH.Flush(LogTypes.Plain);
#endif

                this.Data_Settei_Csv.Read_Add(Const_Filepath.m_EXE_TO_CONFIG + "data_settei.csv", Encoding.UTF8);
                this.Data_Settei_Csv.DebugOut();

                //----------
                // 道１８７
                //----------
                string filepath_Michi = Path.Combine(Application.StartupPath, this.Data_Settei_Csv.Get("data_michi187"));
                if (Michi187Array.Load(filepath_Michi))
                {
                }

#if DEBUG
                {
                    string filepath_LogMichi = Path.Combine(Application.StartupPath, this.Data_Settei_Csv.Get("_log_道表"));
                    File.WriteAllText(filepath_LogMichi, Michi187Array.LogHtml());
                }
#endif

                //----------
                // 駒の配役１８１
                //----------
                string filepath_Haiyaku = Path.Combine(Application.StartupPath, this.Data_Settei_Csv.Get("data_haiyaku185_UTF-8"));
                Util_Array_KomahaiyakuEx184.Load(filepath_Haiyaku, Encoding.UTF8);

                {
                    string filepath_ForcePromotion = Path.Combine(Application.StartupPath, this.Data_Settei_Csv.Get("data_forcePromotion_UTF-8"));
                    List<List<string>> rows = Array_ForcePromotion.Load(filepath_ForcePromotion, Encoding.UTF8);
                    File.WriteAllText(this.Data_Settei_Csv.Get("_log_強制転成表"), Array_ForcePromotion.LogHtml());
                }

                //----------
                // 配役転換表
                //----------
                {
                    string filepath_syuruiToHaiyaku = Path.Combine(Application.StartupPath, this.Data_Settei_Csv.Get("data_syuruiToHaiyaku"));
                    List<List<string>> rows = Data_KomahaiyakuTransition.Load(filepath_syuruiToHaiyaku, Encoding.UTF8);

                    string filepath_LogHaiyakuTenkan = Path.Combine(Application.StartupPath, this.Data_Settei_Csv.Get("_log_配役転換表"));
                    File.WriteAllText(filepath_LogHaiyakuTenkan, Data_KomahaiyakuTransition.Format_LogHtml());
                }
            }


            string filepath_widgets01 = Path.Combine(Application.StartupPath, this.Data_Settei_Csv.Get("data_widgets_01_shogiban"));
            this.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(filepath_widgets01, this));
            string filepath_widgets02 = Path.Combine(Application.StartupPath, this.Data_Settei_Csv.Get("data_widgets_02_console"));
            this.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(filepath_widgets02, this));
        }

        public void LaunchForm_AsBody(KwLogger errH)
        {
            ((Form1_Shogiable)this.OwnerForm).Delegate_Form1_Load = (MainGui_Csharp shogiGui, object sender, EventArgs e) =>
            {

                //
                // ボタンのプロパティを外部ファイルから設定
                //
                foreach (WidgetsLoader widgetsLoader in this.WidgetLoaders)
                {
                    widgetsLoader.Step1_ReadFile();//shogiGui.Shape_PnlTaikyoku
                }

                foreach (WidgetsLoader widgetsLoader in this.WidgetLoaders)
                {
                    widgetsLoader.Step2_Compile_AllWidget(shogiGui);
                }

                foreach (WidgetsLoader widgetsLoader in this.WidgetLoaders)
                {
                    widgetsLoader.Step3_SetEvent(shogiGui);
                }

            };

            this.ReadStyle_ToForm((Form1_Shogiable)this.OwnerForm);

            //
            // FIXME: [初期配置]を１回やっておかないと、[コマ送り]ボタン等で不具合が出てしまう。
            //
            {
                Util_Function_Csharp.Perform_SyokiHaichi_CurrentMutable(
                    ((Form1_Shogiable)this.OwnerForm).Uc_Form1Main.MainGui,
                    errH
                );
            }


            Application.Run(this.OwnerForm);
        }


        public void Response( string mutexString, KwLogger errH)
        {
            Uc_Form1Mainable uc_Form1Main = ((Form1_Shogiable)this.OwnerForm).Uc_Form1Main;

            // enum型
            Form1_Mutex mutex2;
            switch (mutexString)
            {
                case "Timer": mutex2 = Form1_Mutex.Timer; break;
                case "MouseOperation": mutex2 = Form1_Mutex.MouseOperation; break;
                case "Saisei": mutex2 = Form1_Mutex.Saisei; break;
                case "Launch": mutex2 = Form1_Mutex.Launch; break;
                default: mutex2 = Form1_Mutex.Empty; break;
            }


            switch (uc_Form1Main.MutexOwner)
            {
                case Form1_Mutex.Launch:   // 他全部無視
                    goto gt_EndMethod;
                case Form1_Mutex.Saisei:   // マウスとタイマーは無視
                    switch (mutex2)
                    {
                        case Form1_Mutex.MouseOperation:
                        case Form1_Mutex.Timer:
                            goto gt_EndMethod;
                    }
                    break;
                case Form1_Mutex.MouseOperation:
                case Form1_Mutex.Timer:   // タイマーは無視
                    switch (mutex2)
                    {
                        case Form1_Mutex.Timer:
                            goto gt_EndMethod;
                    }
                    break;
                default: break;
            }

            uc_Form1Main.Solute_RepaintRequest(mutex2, this, errH);// 再描画

        gt_EndMethod:
            ;
        }




        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [出力切替]
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public SyuturyokuKirikae SyuturyokuKirikae
        {
            get
            {
                return this.syuturyokuKirikae;
            }
        }
        public void SetSyuturyokuKirikae(SyuturyokuKirikae value)
        {
            this.syuturyokuKirikae = value;
        }
        private SyuturyokuKirikae syuturyokuKirikae;





        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// つまんでいる駒
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public virtual int FigTumandeiruKoma
        {
            get
            {
                return this.figTumandeiruKoma;
            }
        }
        public virtual void SetFigTumandeiruKoma(int value)
        {
            this.figTumandeiruKoma = value;
        }
        private int figTumandeiruKoma;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成るフラグ
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         マウスボタン押下時にセットされ、
        ///         マウスボタンを放したときに読み取られます。
        /// 
        /// </summary>
        public virtual bool Naru
        {
            get
            {
                return this.naruFlag;
            }
        }
        public virtual void SetNaruFlag(bool naru)
        {
            this.naruFlag = naru;
        }
        private bool naruFlag;



        public virtual Busstop GetKoma(Finger finger)
        {
            this.SkyWrapper_Gui.GuiSky.AssertFinger(finger);
            return this.SkyWrapper_Gui.GuiSky.BusstopIndexOf(finger);
        }

    }

}
