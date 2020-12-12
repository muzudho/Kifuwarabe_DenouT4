﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B310Settei.C500Struct;
using Grayscale.A180KifuCsa.B120KifuCsa.C250Struct;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B523UtilFv.C480UtilFvEdit;
using Grayscale.A500ShogiEngine.B523UtilFv.C490UtilFvFormat;
using Grayscale.A500ShogiEngine.B523UtilFv.C491UtilFvIo;
using Nett;

namespace Grayscale.P720FvWriter
{
    public partial class UcMain : UserControl
    {

        public UcMain()
        {
            InitializeComponent();
        }

        private void btnWriter_Click(object sender, EventArgs e)
        {
        }

        private void btnMakeRandom_Click(object sender, EventArgs e)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            string filepath = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Komawari"));

            FeatureVector fv = new FeatureVectorImpl();
            Util_FeatureVectorEdit.Make_Random(fv);

            File.WriteAllText(filepath, Format_FeatureVector_Komawari.Format_Text(fv));
            MessageBox.Show("サンプルファイルを書き出しました。\n" +
                "filepath=[" + filepath + "]");
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            ILogger errH = ErrorControllerReference.ProcessTestProgramDefault;

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            string filepathR = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Komawari"));
            string filepathR_KK = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv01KK"));
            string filepathW = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv2Sample"));

            FeatureVector fv = new FeatureVectorImpl();

            if (Util_FeatureVectorInput.Make_FromFile_Komawari(fv, filepathR))
            {
            }

            if (Util_FeatureVectorInput.Make_FromFile_KK(fv, filepathR_KK, errH))
            {
            }

            File.WriteAllText(filepathW, Format_FeatureVector_Komawari.Format_Text(fv));

#if DEBUG
            MessageBox.Show("FVファイルを読み込んで、書き出しました。\n" +
                "readFilepath=["+filepathR+"]"+
                "writeFilepath=[" + filepathW + "]");
#endif
        }

        /// <summary>
        /// 棋譜読込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadKifu_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.txtKifuFilepath.Text))
            {
                goto gt_EndMethod;
            }


            CsaKifu csaKifu = Util_Csa.ReadFile(this.txtKifuFilepath.Text);

            StringBuilder sb = new StringBuilder();
            List<CsaKifuSasite> sasiteList = csaKifu.SasiteList;
            foreach (CsaKifuSasite csaSasite in sasiteList)
            {
                sb.Append(csaSasite.OptionTemezumi);
                sb.Append("手目 ");
                sb.Append(csaSasite.DestinationMasu);
                sb.Append(" ");
                sb.Append(csaSasite.Second);
                sb.Append(" ");
                sb.Append(csaSasite.Sengo);
                sb.Append(" ");
                sb.Append(csaSasite.SourceMasu);
                sb.Append(" ");
                sb.Append(csaSasite.Syurui);
                sb.AppendLine();
            }
            this.txtSasiteList.Text = sb.ToString();

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// 「ゼロクリアーでファイル作成」（タイアド・ベクター）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMakeZero_Tv_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// リザルト・ベクターを０クリアーで作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateRv0Clear_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 表変形KK 書き出し（旧Fvから）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_HyoHenkeiFvKK_Click(object sender, EventArgs e)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            string filepathR = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Komawari"));

            FeatureVector fv = new FeatureVectorImpl();
            Util_FeatureVectorInput.Make_FromFile_Komawari(fv, filepathR);

            UtilFeatureVectorOutput.Write_KK(fv, Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("DataDirectory")));
        }

        /// <summary>
        /// 1P玉KP書出しボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_1pKP_Write_Click(object sender, EventArgs e)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            string filepathR = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Komawari"));
            FeatureVector fv = new FeatureVectorImpl();
            Util_FeatureVectorInput.Make_FromFile_Komawari(fv, filepathR);

            UtilFeatureVectorOutput.Write_KP(fv, Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("DataDirectory")));
        }

        /// <summary>
        /// [PP書出し]ボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteFvPp_Click(object sender, EventArgs e)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            string filepathR = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Komawari"));
            FeatureVector fv = new FeatureVectorImpl();
            Util_FeatureVectorInput.Make_FromFile_Komawari(fv, filepathR);

            UtilFeatureVectorOutput.Write_PP_Banjo(fv, Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("DataDirectory")));
            UtilFeatureVectorOutput.Write_PP_19Mai(fv, Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("DataDirectory")));
            UtilFeatureVectorOutput.Write_PP_5Mai(fv, Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("DataDirectory")));
            UtilFeatureVectorOutput.Write_PP_3Mai(fv, Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("DataDirectory")));
        }

        /// <summary>
        /// fv_00_Scale.csv書き出し。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteFvScale_Click(object sender, EventArgs e)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            string filepathW = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Scale"));
            MessageBox.Show("filepathW=[" + filepathW + "]", "fv_00_Scale.csv書き出し。");

            FeatureVector fv = new FeatureVectorImpl();
            fv.SetBairitu_NikomaKankeiPp(0.002f);//仮の初期値。

            File.WriteAllText(filepathW, Format_FeatureVector_Scale.Format_Text(fv));
        }


    }
}
