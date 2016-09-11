﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C510____UtilFvLoad;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C___500_Hyokakansu;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

namespace Grayscale.P910_SpeedKeisok
{

    public partial class Uc_Main : UserControl
    {
        private class KeisokuResult
        {
            public string Name { get; set; }

#if DEBUG || LEARN
            public string Utiwake { get; set; }
#endif
            public TimeSpan Time { get; set; }

            public KeisokuResult(string name,
#if DEBUG || LEARN
 string utiwake,
#endif
                TimeSpan time)
            {
                this.Name = name;
#if DEBUG || LEARN
                this.Utiwake = utiwake;
#endif
                this.Time = time;
            }
        }

        public Sky Src_Sky { get; set; }
        public FeatureVector FeatureVector { get; set; }

        public Tree Kifu { get; set; }


        public Uc_Main()
        {
            this.FeatureVector = new FeatureVectorImpl();
            {
                Tree kifu_newHirate;
                Util_FvLoad.CreateKifuTree(out kifu_newHirate);
                this.Kifu = kifu_newHirate;
            }
            this.Src_Sky = this.Kifu.CurNode.Value;
            InitializeComponent();
        }

        private KeisokuResult Keisoku(Hyokakansu handan1)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            float score;
#if DEBUG || LEARN
            KyHyokaMeisai_Koumoku meisai;
#endif
            handan1.Evaluate(
                out score,
#if DEBUG || LEARN
                out meisai,
#endif
                this.Src_Sky,
                this.FeatureVector,
                Util_Loggers.ProcessSpeedTest_KEISOKU
                );

            watch.Stop();

            KeisokuResult result = new KeisokuResult(
                handan1.Name.ToString(),
#if DEBUG || LEARN
                meisai.Utiwake,
#endif
                watch.Elapsed);
            return result;
        }

        /// <summary>
        /// 計測ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKeisoku_Click(object sender, EventArgs e)
        {

            List<KeisokuResult> list = new List<KeisokuResult>();
            list.Add(this.Keisoku(new Hyokakansu_Komawari()));
            list.Add(this.Keisoku(new Hyokakansu_NikomaKankeiPp()));

            TimeSpan total = new TimeSpan();

            StringBuilder sb = new StringBuilder();
            foreach (KeisokuResult result in list)
            {
                sb.AppendLine("----------------------------------------");
                sb.AppendLine(result.Name.ToString());
                sb.Append("    ");
                sb.AppendLine(result.Time.ToString());
                sb.Append("    ");
#if DEBUG
                sb.AppendLine(result.Utiwake);
#else
                sb.AppendLine("Debugモードで実行してください。");
#endif

                total += result.Time;
            }

            {
                sb.AppendLine("----------------------------------------");
                sb.AppendLine("トータル時間");
                sb.Append("    ");
                sb.AppendLine(total.ToString());
            }

            this.txtResult.Text = sb.ToString();
        }

        /// <summary>
        /// 開くボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFv_Click(object sender, EventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessSpeedTest_KEISOKU;
            

            if ("" != this.txtFvFilepath.Text)
            {
                this.openFileDialog1.InitialDirectory = Path.GetDirectoryName(this.txtFvFilepath.Text);
                this.openFileDialog1.FileName = Path.GetFileName(this.txtFvFilepath.Text);
            }
            else
            {
                this.openFileDialog1.InitialDirectory = Application.StartupPath;
            }

            DialogResult result = this.openFileDialog1.ShowDialog();

            switch (result)
            {
                case DialogResult.OK:

                    this.txtFvFilepath.Text = this.openFileDialog1.FileName;
                    string filepath_base = this.txtFvFilepath.Text;

                    StringBuilder sb_result = new StringBuilder();
                    // フィーチャー・ベクターの外部ファイルを開きます。
                    sb_result.Append(Util_FvLoad.OpenFv(this.FeatureVector, filepath_base,errH));

                    this.txtResult.Text = sb_result.ToString();

                    break;
                default:
                    break;
            }

        //gt_EndMethod:
        //    ;

        }
    }
}
