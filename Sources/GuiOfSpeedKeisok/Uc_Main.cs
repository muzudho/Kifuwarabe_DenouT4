using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B180Hyokakansu.C500Hyokakansu;
using Grayscale.A500ShogiEngine.B523UtilFv.C510UtilFvLoad;

#if DEBUG || LEARN
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
#endif

namespace Grayscale.P910SpeedKeisok
{

    public partial class UcMain : UserControl
    {
        private class KeisokuResult
        {
            public string Name { get; set; }

            public TimeSpan Time { get; set; }

            public KeisokuResult(string name,
                TimeSpan time)
            {
                this.Name = name;
                this.Time = time;
            }
        }

        //public Sky Src_Sky { get; set; }
        public FeatureVector FeatureVector { get; set; }

        public Earth Earth { get; set; }
        public ISky PositionA { get; set; }
        public Tree Kifu { get; set; }


        public UcMain()
        {
            this.FeatureVector = new FeatureVectorImpl();
            {
                Earth newEarth1;
                Tree newKifu1_Hirate;
                ISky positionA;
                Util_FvLoad.CreateKifuTree(
                    out newEarth1, out positionA, out newKifu1_Hirate);

                this.Earth = newEarth1;
                this.PositionA = positionA;
                this.Kifu = newKifu1_Hirate;
            }
            InitializeComponent();
        }

        private KeisokuResult Keisoku(IHyokakansu handan1, ISky positionA)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            float score_notUse = handan1.Evaluate(
                positionA.GetKaisiPside(),
                positionA,
                this.FeatureVector,
                ErrorControllerReference.ProcessSpeedTestKeisoku
                );

            watch.Stop();

            KeisokuResult result = new KeisokuResult(
                handan1.Name.ToString(),
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
            ISky positionA = this.Kifu.PositionA;//.CurNode2ok.GetNodeValue()
            list.Add(this.Keisoku(new Hyokakansu_Komawari(), positionA));
            list.Add(this.Keisoku(new Hyokakansu_NikomaKankeiPp(), positionA));

            TimeSpan total = new TimeSpan();

            StringBuilder sb = new StringBuilder();
            foreach (KeisokuResult result in list)
            {
                sb.AppendLine("----------------------------------------");
                sb.AppendLine(result.Name.ToString());
                sb.Append("    ");
                sb.AppendLine(result.Time.ToString());
                sb.Append("    ");

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
            ILogger errH = ErrorControllerReference.ProcessSpeedTestKeisoku;


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
                    sb_result.Append(Util_FvLoad.OpenFv(this.FeatureVector, filepath_base, errH));

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
