using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___250_Learn;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___450_Tyoseiryo;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___490_StopLearning;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C250____Learn;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C260____View;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C450____Tyoseiryo;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C480____Functions;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C490____StopLearning;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C508____AutoSasiteRush;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C510____AutoKifuRead;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C600____Operation;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Grayscale.A690_FvLearn____
{
    public partial class Uc_Main : UserControl
    {

        public LearningData LearningData{get;set;}

        public Button BtnUpdateKyokumenHyoka { get { return this.btnUpdateKyokumenHyoka; } }

        public CheckBox ChkTyoseiryoAuto { get { return this.chkTyoseiryoAuto; } }

        public CheckBox ChkStartZero { get { return this.chkStartZero; } }

        public CheckBox ChkAutoParamRange { get { return this.chkAutoParamRange; } }

        public CheckBox ChkRenzokuAutoRun { get { return this.chkRenzokuAutoRun; } }

        public CheckBox ChkHyakuretuken { get { return this.chkHyakuretuken; } }

        public CheckBox ChkAutosave { get { return this.chkAutosave; } }

        public CheckBox ChkAutoKifuNext { get { return this.chkAutoKifuNext; } }

        public CheckBox ChkIgnoreThink { get { return this.chkIgnoreThink; } }

        public CheckBox ChkEndLearnTesu { get { return this.chkEndLearnTesu; } }



        public ListBox LstSasite { get { return this.lstSasite; } }

        public ListBox LstGohosyu { get { return this.lstGohosyu; } }

        public OpenFileDialog OpenFvFileDialog2 { get { return this.openFvFileDialog2; } }

        public OpenFileDialog OpenCsaFileDialog1 { get { return this.openCsaFileDialog1; } }

        

        public PictureBox PctKyokumen { get { return this.pctKyokumen; } }

        public TextBox TxtTyoseiryo { get { return this.txtTyoseiryo; } }

        public TextBox TxtStatus1 { get { return this.txtStatus1; } }

        public TextBox TxtTemezumi { get { return this.txtTemezumi; } }

        public TextBox TxtGohosyuTe { get { return this.txtGohosyuTe; } }

        public TextBox TxtAllNodesCount { get { return this.txtAllNodesCount; } }

        public TextBox TxtKifuFilepath { get { return this.txtKifuFilepath; } }

        public TextBox TxtFvFilepath { get { return this.txtFvFilepath; } }

        public TextBox TxtChoseiBairituB { get { return this.txtChoseiBairituB; } }

        public TextBox TxtNikomaHyokati { get { return this.txtNikomaHyokati; } }

        public TextBox TxtAutosaveYMD { get { return this.txtAutosaveYMD; } }

        public TextBox TxtAutosaveHMS { get { return this.txtAutosaveHMS; } }

        public TextBox TxtRenzokuTe { get { return this.txtRenzokuTe; } }

        public TextBox TxtIgnoreLearn { get { return this.txtIgnoreLearn; } }

        public TextBox TxtEndLearnTesu { get { return this.txtEndLearnTesu; } }


        /// <summary>
        /// 調整量の設定
        /// </summary>
        public TyoseiryoSettings TyoseiryoSettings { get { return this.tyoseiryoSettings; } }
        private TyoseiryoSettings tyoseiryoSettings;

        /// <summary>
        /// 自動学習を中断させるためのもの。
        /// </summary>
        public StopLearning StopLearning { get { return this.stopLearning; } }
        private StopLearning stopLearning;

        public Uc_Main()
        {
            this.LearningData = new LearningDataImpl();
            this.stopLearning = new StopLearningImpl(Path.Combine(Application.StartupPath, "Stop_learning.txt"));
            this.tyoseiryoSettings = new TyoseiryoSettingsImpl();

            //
            // イベントハンドラー登録
            //
            Util_Loggers.ProcessLearner_DEFAULT.KwDisplayer_OrNull.Dlgt_OnLog1Append_or_Null = (string log) =>
            {
                this.txtIttesasuLog.Text += log;
            };
            Util_Loggers.ProcessLearner_DEFAULT.KwDisplayer_OrNull.Dlgt_OnLog1Clear_or_Null = () =>
            {
                this.txtIttesasuLog.Text = "";
            };
            //Util_OwataMinister.LEARNER.Dlgt_OnNaibuDataAppend_or_Null = (string log) =>
            //{
            //    this.txtNaibuData.Text += log;
            //};
            //Util_OwataMinister.LEARNER.Dlgt_OnNaibuDataClear_or_Null = () =>
            //{
            //    this.txtNaibuData.Text = "";
            //};

            //
            // 調整量
            //
            Util_Tyoseiryo.Init(this.tyoseiryoSettings, Util_AutoSasiteRush.RENZOKU_KAISU);

            InitializeComponent();
        }

        /// <summary>
        /// [一手指す]ボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSasu_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;
            bool isRequest_ShowGohosyu = false;
            bool isRequest_ChangeKyokumenPng = false;

            Util_LearningView.Ittesasu_ByBtnClick(
                ref isRequest_ShowGohosyu,
                ref isRequest_ChangeKyokumenPng,
                this.LearningData,
                this,
                errH
            );

            if (isRequest_ShowGohosyu)
            {
                // 合法手一覧を更新
                Util_LearnFunctions.FvParamRange_PP(this.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                Util_LearningView.Aa_ShowGohosyu2(this.LearningData, this, errH);
                isRequest_ShowGohosyu = false;
            }

            if (isRequest_ChangeKyokumenPng)
            {
                this.LearningData.ChangeKyokumenPng(
                    this,
                    this.LearningData.GetMove(),
                    this.LearningData.PositionA
                    );
                isRequest_ChangeKyokumenPng = false;
            }
        }

        private string[] search_kifu_folder_lines = new string[]{};

        /// <summary>
        /// フォームの初期化が終わったあとに。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uc_Main_Load(object sender, EventArgs e)
        {
            this.LearningData.AtBegin(this);

            // フォームの初期化が終わった後で。
            this.lblExplainStopLearning.Text = "自動学習の止め方：　次のファイルがあれば、そのうち止まります。「" + this.StopLearning.StopLearningFilePath + "」";

            // ラベル
            this.chkHyakuretuken.Text = "1位になるまで繰返す(最大" + Util_AutoSasiteRush.RENZOKU_KAISU + "回)";

            // ファイル読込
            {
                string path = Path.Combine(Application.StartupPath, "Search_kifu_folder.txt");
                if(File.Exists(path))
                {
                    this.search_kifu_folder_lines = File.ReadAllLines(path, Encoding.UTF8);
                }
            }
        }

        private void btnOpenCsa_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;

            bool isRequest_ShowGohosyu = false;
            bool isRequest_ChangeKyokumenPng = false;

            string kifuFilepath = "";
            {
                this.OpenCsaFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.StartupPath);
                DialogResult result = this.OpenCsaFileDialog1.ShowDialog();
                switch (result)
                {
                    case DialogResult.OK:
                        kifuFilepath = this.OpenCsaFileDialog1.FileName;
                        break;
                    default:
                        break;
                }
            }

            if(""!=kifuFilepath)
            {
                Util_LearnOperation.Do_OpenCsaKifu(
                    ref isRequest_ShowGohosyu,
                    ref isRequest_ChangeKyokumenPng,
                    kifuFilepath,
                    this,errH);
            }

            if (isRequest_ShowGohosyu)
            {
                // 合法手一覧を更新
                Util_LearnFunctions.FvParamRange_PP(this.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                Util_LearningView.Aa_ShowGohosyu2(this.LearningData, this, Util_Loggers.ProcessLearner_DEFAULT);
                isRequest_ShowGohosyu = false;
            }

            if (isRequest_ChangeKyokumenPng)
            {
                this.LearningData.ChangeKyokumenPng(
                    this,
                    this.LearningData.GetMove(),
                    this.LearningData.PositionA
                );
                isRequest_ChangeKyokumenPng = false;
            }
        }
        

        /// <summary>
        /// fv.csvを開くボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFvCsv_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;

            Util_LearnOperation.Do_OpenFvCsv(this,errH);
        }

        /// <summary>
        /// [FV上書き]ボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteFv_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;

            Util_LearnFunctions.Do_Save(this, errH);
        }

        /// <summary>
        /// １手指す前に。[局面評価更新]ボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateKyokumenHyoka_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;
            bool isRequest_ShowGohosyu = false;
            bool isRequest_ChangeKyokumenPng = false;

            Util_AutoKifuRead.Do_UpdateKyokumenHyoka(
                ref isRequest_ShowGohosyu,
                ref isRequest_ChangeKyokumenPng,
                this, errH
                );

            if (isRequest_ShowGohosyu)
            {
                // 合法手一覧を更新
                Util_LearnFunctions.FvParamRange_PP(this.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                Util_LearningView.Aa_ShowGohosyu2(this.LearningData, this, errH);
                isRequest_ShowGohosyu = false;
            }

            if (isRequest_ChangeKyokumenPng)
            {
                this.LearningData.ChangeKyokumenPng(
                    this,
                    this.LearningData.GetMove(),
                    this.LearningData.PositionA
                    );
                isRequest_ChangeKyokumenPng = false;
            }
        }

        /// <summary>
        /// [成功移動]
        /// </summary>
        public void SeikoIdo()
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;

            // 今読んだ棋譜を移す先（成功時）
            if (2 <= this.search_kifu_folder_lines.Length)
            {
                string src = this.TxtKifuFilepath.Text;
                string dst = Path.Combine(this.search_kifu_folder_lines[1], Path.GetFileName(this.TxtKifuFilepath.Text));

                //MessageBox.Show("棋譜移動\n" +
                //    "src=[" + src + "]\n" +
                //    "dst=[" + dst + "]");

                try
                {
                    File.Move(src, dst);
                    this.TxtKifuFilepath.Text = "";
                }
                catch(Exception ex)
                {
                    errH.DonimoNaranAkirameta("Uc_Main#SeikoIdo: "+ex.GetType().Name+"："+ ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// [失敗移動]
        /// </summary>
        private void SippaiIdo()
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;

            // 今読んだ棋譜を移す先（失敗時）
            if (3 <= this.search_kifu_folder_lines.Length)
            {
                string src = this.TxtKifuFilepath.Text;
                string dst = Path.Combine(this.search_kifu_folder_lines[2], Path.GetFileName(this.TxtKifuFilepath.Text));

                MessageBox.Show("棋譜移動\n" +
                    "src=[" + src + "]\n" +
                    "dst=[" + dst + "]");

                try
                {
                    File.Move(src, dst);
                    this.TxtKifuFilepath.Text = "";
                }
                catch (Exception ex)
                {
                    errH.DonimoNaranAkirameta("Uc_Main#SippaiIdo: " + ex.GetType().Name + "：" + ex.Message);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// FVを0～999に矯正。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFv999_999_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;
            bool isRequest_ShowGohosyu = false;

            isRequest_ShowGohosyu = true;

            if (isRequest_ShowGohosyu)
            {
                // 合法手一覧を更新
                Util_LearnFunctions.FvParamRange_PP(this.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                isRequest_ShowGohosyu = false;
            }
        }

        /// <summary>
        /// 二駒評価値を調べるボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSiraberuHyokatiNikoma_Click(object sender, EventArgs e)
        {
            Util_LearnOperation.Do_ShowNikomaHyokati(this);
        }

        /// <summary>
        /// [指し手の順位下げ]ボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSasiteRankDown_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;
            bool isRequest_ShowGohosyu = false;
            bool isRequest_ChangeKyokumenPng = false;

            Util_LearnOperation.Do_RankDownSasite(
                ref isRequest_ShowGohosyu,
                ref isRequest_ChangeKyokumenPng,
                this, errH);

            if (isRequest_ShowGohosyu)
            {
                // 合法手一覧を更新
                Util_LearnFunctions.FvParamRange_PP(this.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                Util_LearningView.Aa_ShowGohosyu2(this.LearningData, this, errH);
                isRequest_ShowGohosyu = false;
            }

            if (isRequest_ChangeKyokumenPng)
            {
                this.LearningData.ChangeKyokumenPng(
                    this,
                    this.LearningData.GetMove(),
                    this.LearningData.PositionA
                    );
                isRequest_ChangeKyokumenPng = false;
            }
        }

        /// <summary>
        /// [指し手の順位上げ]ボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSasiteRankUp_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;
            bool isRequest_ShowGohosyu = false;
            bool isRequest_ChangeKyokumenPng = false;

            Util_LearnOperation.Do_RankUpSasite(
                ref isRequest_ShowGohosyu,
                ref isRequest_ChangeKyokumenPng,
                this, errH);

            if (isRequest_ShowGohosyu)
            {
                // 合法手一覧を更新
                Util_LearnFunctions.FvParamRange_PP(this.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                Util_LearningView.Aa_ShowGohosyu2(this.LearningData, this, errH);
                isRequest_ShowGohosyu = false;
            }

            if (isRequest_ChangeKyokumenPng)
            {
                this.LearningData.ChangeKyokumenPng(
                    this,
                    this.LearningData.GetMove(),
                    this.LearningData.PositionA
                    );
                isRequest_ChangeKyokumenPng = false;
            }

        }

        /// <summary>
        /// 初期局面の評価値を 0 点にするようにFVを調整します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartZero_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;
            bool isRequest_ShowGohosyu = false;

            Util_LearnOperation.Do_ZeroStart( ref isRequest_ShowGohosyu, this, errH);

            if (isRequest_ShowGohosyu)
            {
                // 合法手一覧を更新
                Util_LearnFunctions.FvParamRange_PP(this.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                Util_LearningView.Aa_ShowGohosyu2(this.LearningData, this, errH);
                isRequest_ShowGohosyu = false;
            }
        }

        private void btnSeikoIdo_Click(object sender, EventArgs e)
        {
            this.SeikoIdo();
        }

        private void btnSippaiIdo_Click(object sender, EventArgs e)
        {
            this.SippaiIdo();
        }

        /// <summary>
        /// 次の棋譜をランダムに選んでセット。
        /// </summary>
        public void Do_NextKifuSet(
            out bool out_isEmptyKifu,
            ref bool ref_isRequest_ShowGohosyu,
            ref bool ref_isRequest_ChangeKyokumenPng,
            KwLogger errH)
        {
            //
            // 前対局の情報のクリアー
            //
            {
                this.txtIttesasuLog.Text = "";
            }
            //
            // ランダム・棋譜選択
            //
            string kifuFilepath = this.GetRandamKifuFilepath_OrEmpty();
            if ("" != kifuFilepath)
            {
                Util_LearnOperation.Do_OpenCsaKifu(
                    ref ref_isRequest_ShowGohosyu,
                    ref ref_isRequest_ChangeKyokumenPng,
                    kifuFilepath,
                    this, errH);
                out_isEmptyKifu = false;
            }
            else
            {
                out_isEmptyKifu = true;
            }
        }

        /// <summary>
        /// [次棋譜セット]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextKifuSet_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessLearner_DEFAULT;

            bool isRequest_ShowGohosyu = false;
            bool isRequest_ChangeKyokumenPng = false;

            bool isEmptyKifu;
            this.Do_NextKifuSet(out isEmptyKifu, ref isRequest_ShowGohosyu, ref isRequest_ChangeKyokumenPng, errH);


            if (isRequest_ShowGohosyu)
            {
                // 合法手一覧を更新
                Util_LearnFunctions.FvParamRange_PP(this.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                Util_LearningView.Aa_ShowGohosyu2(this.LearningData, this, Util_Loggers.ProcessLearner_DEFAULT);
                isRequest_ShowGohosyu = false;
            }

            if (isRequest_ChangeKyokumenPng)
            {
                this.LearningData.ChangeKyokumenPng(
                    this,
                    this.LearningData.GetMove(),
                    this.LearningData.PositionA
                );
                isRequest_ChangeKyokumenPng = false;
            }
        }

        /// <summary>
        /// 指定フォルダーから、CSA棋譜をランダムに選びます。
        /// </summary>
        /// <returns></returns>
        private string GetRandamKifuFilepath_OrEmpty()
        {
            string result_path = "";

            if (1 <= this.search_kifu_folder_lines.Length)
            {
                string path_doFolder = this.search_kifu_folder_lines[0];

                string[] files = null;
                if (Directory.Exists(path_doFolder))
                {
                    files = Directory.GetFiles(path_doFolder);
                }

                string selectedFile = "";
                if (null != files && 0 < files.Length)
                {
                    selectedFile = files[KwRandom.Random.Next(files.Length)];
                }

                if ("" != selectedFile)
                {
                    result_path = Path.Combine(path_doFolder, selectedFile);

                    //MessageBox.Show("ランダムチョイス棋譜\n" +
                    //    "result_path=[" + result_path + "]");
                }
            }

            return result_path;
        }

    }
}
