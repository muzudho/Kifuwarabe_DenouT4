﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Engine.Configuration;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.Kifuwaragyoku.Entities.Configuration;
using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Grayscale.Kifuwaragyoku.UseCases;
using Grayscale.Kifuwaragyoku.UseCases.Features;

namespace Grayscale.Kifuwaragyoku.GuiOfSpeedKeisok
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

        public UcMain()
        {
            EngineConf = new EngineConf();
            EntitiesLayer.Implement(EngineConf);

            IPlaying playing = new Playing(EngineConf);

            this.FeatureVector = new FeatureVector();
            {
                ITree newKifu1_Hirate;
                Util_FvLoad.CreateKifuTree(
                    playing, out newKifu1_Hirate);
                this.Kifu = newKifu1_Hirate;
            }
            InitializeComponent();
        }
        public IEngineConf EngineConf { get; private set; }

        //public Sky Src_Sky { get; set; }
        public IFeatureVector FeatureVector { get; set; }
        public ITree Kifu { get; set; }

        private KeisokuResult Keisoku(IHyokakansu handan1, IPosition positionA)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            float score_notUse = handan1.Evaluate(
                positionA.GetKaisiPside(),
                positionA,
                this.FeatureVector
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
            IPosition positionA = this.Kifu.PositionA;//.CurNode2ok.GetNodeValue()
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
                    sb_result.Append(Util_FvLoad.OpenFv(EngineConf, this.FeatureVector, filepath_base));

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
