﻿using System.IO;
using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.UseCases.Evaluation;
using Nett;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public abstract class UtilFeatureVectorOutput
    {

        private class PpItem_P1
        {
            public string Filepath { get; set; }
            public string Title { get; set; }
            public int P1_base { get; set; }
            public PpItem_P1(string filepath, string title, int p1_base)
            {
                this.Filepath = filepath;
                this.Title = title;
                this.P1_base = p1_base;
            }
        }

        public static void Write_Scale(IFeatureVector fv, string fvDirectory)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(fvDirectory, "Engine.toml"));

            string filepathW = Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv00ScaleInFvDir));
            File.WriteAllText(filepathW, Format_FeatureVector_Scale.Format_Text(fv));
        }

        public static void Write_KK(IFeatureVector fv, string fvDirectory)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            string filepathW = Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv01KKInFvDir));
            File.WriteAllText(filepathW, Format_FeatureVector_KK.Format_KK(fv));

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine(filepathW);
            //MessageBox.Show("ファイルを書き出しました。\n" + sb.ToString());
        }

        public static void Write_KP(IFeatureVector fv, string fvDirectory)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            //StringBuilder sb = new StringBuilder();

            string filepathW1 = Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv02n1pKPInFvDir));
            string filepathW2 = Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv03n2pKPInFvDir));
            //----------------------------------------
            // 1P玉
            //----------------------------------------
            {
                File.WriteAllText(filepathW1, FormatFeatureVectorKP.Format_KP(fv, Playerside.P1));
                //sb.AppendLine(filepathW1);
            }

            //----------------------------------------
            // 2p玉
            //----------------------------------------
            {
                File.WriteAllText(filepathW2, FormatFeatureVectorKP.Format_KP(fv, Playerside.P2));
                //sb.AppendLine(filepathW2);
            }

            //MessageBox.Show("ファイルを書き出しました。\n" + sb.ToString());
        }

        /// <summary>
        /// PP 盤上の駒
        /// </summary>
        /// <param name="fv"></param>
        /// <param name="fvDirectory"></param>
        public static void Write_PP_Banjo(IFeatureVector fv, string fvDirectory)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            //StringBuilder sb = new StringBuilder();

            // P1が盤上の駒
            {
                PpItem_P1[] p1Items = new PpItem_P1[]{
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv04PP1pInFvDir)),"1P歩",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_____FU_____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv05PP1pInFvDir)),"1P香",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_____KYO____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv06pp1pInFvDir)),"1P桂",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_____KEI____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv07pp1pInFvDir)),"1P銀",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_____GIN____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv08pp1pInFvDir)),"1P金",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_____KIN____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv09pp1pInFvDir)),"1P飛",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_____HISYA__),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv10pp1pInFvDir)),"1P角",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_____KAKU___),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv18pp2pInFvDir)),"2P歩",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_____FU_____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv19pp2pInFvDir)),"2P香",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_____KYO____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv20pp2pInFvDir)),"2P桂",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_____KEI____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv21pp2pInFvDir)),"2P銀",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_____GIN____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv22pp2pInFvDir)),"2P金",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_____KIN____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv23pp2pInFvDir)),"2P飛",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_____HISYA__),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv24pp2pInFvDir)),"2P角",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_____KAKU___),
                };
                foreach (PpItem_P1 item in p1Items)
                {
                    File.WriteAllText(item.Filepath, FormatFeatureVectorPpP1Banjo.Format_PP_P1Banjo(fv, item.Title, item.P1_base));
                    //sb.AppendLine(item.Filepath);
                }
            }

            //MessageBox.Show("ファイルを書き出しました。\n" + sb.ToString());
        }

        /// <summary>
        /// PP １９枚の持駒
        /// </summary>
        /// <param name="fv"></param>
        /// <param name="fvDirectory"></param>
        public static void Write_PP_19Mai(IFeatureVector fv, string fvDirectory)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            //StringBuilder sb_result = new StringBuilder();

            {
                PpItem_P1[] p1Items = new PpItem_P1[]{
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv11PP1pInFvDir)),"1P歩",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_MOTIFU_____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv25pp2pInFvDir)),"2P歩",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_MOTIFU_____),
                };
                foreach (PpItem_P1 item in p1Items)
                {
                    File.WriteAllText(item.Filepath, FormatFeatureVectorPpP1Moti.Format_PP_P1_Moti19Mai(fv, item.Title, item.P1_base));

                    //sb_result.AppendLine(item.Filepath);
                }
            }

            //MessageBox.Show("ファイルを書き出しました。\n" + sb_result.ToString());
        }


        /// <summary>
        /// PP 5枚の持駒
        /// </summary>
        /// <param name="fv"></param>
        /// <param name="fvDirectory"></param>
        public static void Write_PP_5Mai(IFeatureVector fv, string fvDirectory)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            //StringBuilder sb = new StringBuilder();

            {
                PpItem_P1[] p1Items = new PpItem_P1[]{
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv12PP1pInFvDir)),"1P香",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_MOTIKYO____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv13pp1pInFvDir)),"1P桂",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_MOTIKEI____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv14pp1pInFvDir)),"1P銀",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_MOTIGIN____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv15pp1pInFvDir)),"1P金",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_MOTIKIN____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv26pp2pInFvDir)),"2P香",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_MOTIKYO____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv27pp2pInFvDir)),"2P桂",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_MOTIKEI____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv28pp2pInFvDir)),"2P銀",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_MOTIGIN____),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv29pp2pInFvDir)),"2P金",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_MOTIKIN____),
                };
                foreach (PpItem_P1 item in p1Items)
                {
                    File.WriteAllText(item.Filepath, FormatFeatureVectorPpP1Moti.Format_PP_P1Moti_5Mai(fv, item.Title, item.P1_base));
                    //sb.AppendLine(item.Filepath);
                }
            }

            //MessageBox.Show("ファイルを書き出しました。\n" + sb.ToString());
        }

        /// <summary>
        /// PP ３枚の持駒
        /// </summary>
        /// <param name="fv"></param>
        /// <param name="fvDirectory"></param>
        public static void Write_PP_3Mai(IFeatureVector fv, string fvDirectory)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            //StringBuilder sb = new StringBuilder();

            {
                PpItem_P1[] p1Items = new PpItem_P1[]{
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv16pp1pInFvDir)),"1P飛",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_MOTIHISYA__),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv17pp1pInFvDir)),"1P角",FeatureVector.CHOSA_KOMOKU_1P + FeatureVector.CHOSA_KOMOKU_MOTIKAKU___),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv30pp2pInFvDir)),"2P飛",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_MOTIHISYA__),
                    new PpItem_P1( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Fv31pp2pInFvDir)),"2P角",FeatureVector.CHOSA_KOMOKU_2P + FeatureVector.CHOSA_KOMOKU_MOTIKAKU___),
                };
                foreach (PpItem_P1 item in p1Items)
                {
                    File.WriteAllText(item.Filepath, FormatFeatureVectorPpP1Moti.Format_PP_P1Moti_3Mai(fv, item.Title, item.P1_base));
                    //sb.AppendLine(item.Filepath);
                }
            }

            //MessageBox.Show("ファイルを書き出しました。\n" + sb.ToString());
        }


    }
}
