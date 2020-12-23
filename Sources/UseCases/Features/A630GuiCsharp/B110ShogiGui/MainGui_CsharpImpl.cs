using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Codeplex.Data;//DynamicJson
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Nett;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
// using Grayscale.Kifuwaragyoku.Entities.Logging;
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
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
        public ShapePnlTaikyoku Shape_PnlTaikyoku
        {
            get
            {
                return this.shape_PnlTaikyoku;
            }
        }
        private ShapePnlTaikyoku shape_PnlTaikyoku;

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
        public void SetFlowB(SceneName name1, ILogTag logTag)
        {
            this.flowB = name1;

            //アライブ
            {
                TimedBMouseCapture timeB = ((TimedBMouseCapture)this.TimedB_MouseCapture);
                timeB.MouseEventQueue.Enqueue(
                    new MouseEventState(name1, ShapeCanvasImpl.WINDOW_NAME_SHOGIBAN, MouseEventStateName.Arive, Point.Empty, logTag));
            }
        }
        private SceneName flowB;
        #endregion



        #region コンストラクター

        /// <summary>
        /// 生成後、OwnerFormをセットしてください。
        /// </summary>
        public MainGui_CsharpImpl()
        {
            this.m_skyWrapper_Gui_ = new SkyWrapper_GuiImpl();
            this.server = new Server_Impl(this.m_skyWrapper_Gui_.GuiSky, new ReceiverForCsharpVsImpl());

            this.Widgets = new Dictionary<string, UserWidget>();

            this.consoleWindowGui = new SubGuiImpl(this);

            this.TimedA = new TimedA_EngineCapture(this);
            this.TimedB_MouseCapture = new TimedBMouseCapture(this);
            this.TimedC = new TimedC_SaiseiCapture(this);

            this.WidgetLoaders = new List<WidgetsLoader>();
            this.RepaintRequest = new RepaintRequestImpl();

            //----------
            // ビュー
            //----------
            //
            //      ボタンや将棋盤などを描画するツールを、事前準備しておきます。
            //
            this.shape_PnlTaikyoku = new ShapePnlTaikyokuImpl("#TaikyokuPanel", this);

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
        public virtual void Start_ShogiEngine(string shogiEngineFilePath, ILogTag logTag)
        {
        }

        /// <summary>
        /// コンピューターの先手
        /// </summary>
        public virtual void Do_ComputerSente(ILogTag logTag)
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
            ILogTag logTag)
        {
        }


        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public virtual void Shutdown(ILogTag logTag)
        {
        }


        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public virtual void Logdase(ILogTag logTag)
        {
        }



        private int noopSend_counter;
        public void Timer_Tick(ILogTag logTag)
        {
            if (this.server.EngineClient.ShogiEngineProcessWrapper.IsLive_ShogiEngine())
            {
                // だいたい 1tick 50ms と考えて、20倍で 1秒。
                if (20 * 3 < this.noopSend_counter) // 3秒に 1 回ぐらい ok を送れば？
                {
                    // noop
                    this.server.EngineClient.ShogiEngineProcessWrapper.Send_Noop_from_server(logTag);
                    this.noopSend_counter = 0;
                }
                else
                {
                    this.noopSend_counter++;
                }
            }

            this.TimedA.Step(logTag);
            this.TimedB_MouseCapture.Step(logTag);
            this.TimedC.Step(logTag);
        }




        /// <summary>
        /// 見た目の設定を読み込みます。
        /// </summary>
        public void ReadStyle_ToForm(Form1Shogiable ui_Form1)
        {
            try
            {
                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                string filepath2 = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("DataStyleText"));
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
                        throw;
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
        public virtual void Load_AsStart(ILogTag logTag)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            //
            // 既存のログファイルを削除したい。
            //
            {

            }

            {
#if DEBUG
                logTag.AppendLine("(^o^)乱数のたね＝[" + KwRandom.Seed + "]");
                logTag.Flush(LogTypes.Plain);
#endif

                //----------
                // 道１８７
                //----------
                {
                    var filepath_Michi = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.DataMichi187Csv));
                    if (Michi187Array.Load(filepath_Michi))
                    {
                    }
                }

#if DEBUG
                {
                    var filepath_LogMichi = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.MichiHyoLogHtml));
                    File.WriteAllText(filepath_LogMichi, Michi187Array.LogHtml());
                }
#endif

                //----------
                // 駒の配役１８１
                //----------
                {
                    var filepath_Haiyaku = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.DataHaiyaku185UTF8Csv));
                    Util_Array_KomahaiyakuEx184.Load(filepath_Haiyaku, Encoding.UTF8);
                }

                {
                    var filepath_ForcePromotion = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.DataForcePromotionUTF8Csv));
                    List<List<string>> rows = Array_ForcePromotion.Load(filepath_ForcePromotion, Encoding.UTF8);
                }
                {
                    var filepath_forcePromotionLogHtml = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.ForcePromotionLogHtml));
                    File.WriteAllText(filepath_forcePromotionLogHtml, Array_ForcePromotion.LogHtml());
                }

                //----------
                // 配役転換表
                //----------
                {
                    var filepath_syuruiToHaiyaku = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.DataSyuruiToHaiyakuCsv));
                    List<List<string>> rows = Data_KomahaiyakuTransition.Load(filepath_syuruiToHaiyaku, Encoding.UTF8);
                }
                {
                    var filepath_LogHaiyakuTenkan = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.HaiyakuTenkanHyoLogHtml));
                    File.WriteAllText(filepath_LogHaiyakuTenkan, Data_KomahaiyakuTransition.Format_LogHtml());
                }
            }

            {
                var filepath_widgets01 = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.DataWidgets01ShogibanCsv));
                this.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(filepath_widgets01, this));
            }

            {
                var filepath_widgets02 = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.DataWidgets02ConsoleCsv));
                this.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(filepath_widgets02, this));
            }
        }

        public void LaunchForm_AsBody(ILogTag logTag)
        {
            ((Form1Shogiable)this.OwnerForm).Delegate_Form1_Load = (MainGui_Csharp shogiGui, object sender, EventArgs e) =>
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

            this.ReadStyle_ToForm((Form1Shogiable)this.OwnerForm);

            //
            // FIXME: [初期配置]を１回やっておかないと、[コマ送り]ボタン等で不具合が出てしまう。
            //
            {
                Util_Function_Csharp.Perform_SyokiHaichi_CurrentMutable(
                    ((Form1Shogiable)this.OwnerForm).Uc_Form1Main.MainGui,
                    logTag
                );
            }


            Application.Run(this.OwnerForm);
        }


        public void Response(string mutexString, ILogTag logTag)
        {
            UcForm1Mainable uc_Form1Main = ((Form1Shogiable)this.OwnerForm).Uc_Form1Main;

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

            uc_Form1Main.Solute_RepaintRequest(mutex2, this, logTag);// 再描画

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
