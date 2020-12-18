﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B640_KifuTree___.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B523UtilFv.C491UtilFvIo;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Nett;

namespace Grayscale.A500ShogiEngine.B523UtilFv.C510UtilFvLoad
{
    public abstract class Util_FvLoad
    {
        private class PP_P1Item
        {
            public string Filepath { get; set; }
            public int P1_base { get; set; }
            public PP_P1Item(string filepath, int p1_base)
            {
                this.Filepath = filepath;
                this.P1_base = p1_base;
            }
        }

        /// <summary>
        /// 棋譜ツリーを、平手初期局面 で準備します。
        /// </summary>
        public static void CreateKifuTree(
            out Earth out_earth1,
            out ISky out_positionA,
            out Tree out_kifu1
            )
        {


            // 棋譜
            out_earth1 = new EarthImpl();
            out_positionA = UtilSkyCreator.New_Hirate();
            out_kifu1 = new TreeImpl(out_positionA);
            out_earth1.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");// 平手


            out_positionA.AssertFinger((Finger)0);
            Debug.Assert(!Conv_Masu.OnKomabukuro(
                Conv_Masu.ToMasuHandle(
                    Conv_Busstop.ToMasu(out_positionA.BusstopIndexOf((Finger)0))
                    )
                ), "駒が駒袋にあった。");
        }


        /// <summary>
        /// フィーチャー・ベクター関連のファイルを全て開きます。
        /// </summary>
        /// <param name="fv"></param>
        /// <param name="tv_orNull">学習でしか使いません。</param>
        /// <param name="rv_orNull">学習でしか使いません。</param>
        /// <param name="fv_komawari_filepath"></param>
        /// <returns></returns>
        public static string OpenFv(FeatureVector fv, string fv_komawari_filepath, ILogTag errH)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            StringBuilder sb_result = new StringBuilder();

            {//駒割
                string filepath = fv_komawari_filepath;
                if (!Util_FeatureVectorInput.Make_FromFile_Komawari(fv, filepath))
                {
                    sb_result.Append("ファイルオープン失敗 Fv[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append("開fv。");
            }

            string fvDirectory = Path.GetDirectoryName(fv_komawari_filepath);

            {//スケール
                string filepath = Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv00ScaleInFvDir"));//komawari.csvと同じフォルダー
                if (!Util_FeatureVectorInput.Make_FromFile_Scale(fv, filepath))
                {
                    sb_result.Append("ファイルオープン失敗 Fv[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append("開Sc。");
            }

            {//KK
                string filepath = Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv01KKInFvDir"));//komawari.csvと同じフォルダー
                if (!Util_FeatureVectorInput.Make_FromFile_KK(fv, filepath, errH))
                {
                    sb_result.Append("ファイルオープン失敗 KK[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append("開KK。");
            }

            {//1pKP
                string filepath = Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv02n1pKPInFvDir"));
                if (!Util_FeatureVectorInput.Make_FromFile_KP(fv, filepath, Playerside.P1, errH))
                {
                    sb_result.Append("ファイルオープン失敗 1pKP[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append("開1pKP。");
            }

            {//2pKP
                string filepath = Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv03n2pKPInFvDir"));
                if (!Util_FeatureVectorInput.Make_FromFile_KP(fv, filepath, Playerside.P2, errH))
                {
                    sb_result.Append("ファイルオープン失敗 2pKP[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append("開2pKP。");
            }

            {//盤上の駒
                List<PP_P1Item> p1List = new List<PP_P1Item>()
                {
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv04PP1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____FU_____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv05PP1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____KYO____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv06pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____KEI____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv07pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____GIN____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv08pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____KIN____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv09pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____HISYA__),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv10pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____KAKU___),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv18pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____FU_____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv19pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____KYO____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv20pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____KEI____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv21pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____GIN____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv22pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____KIN____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv23pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____HISYA__),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv24pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____KAKU___),
                };

                foreach (PP_P1Item p1Item in p1List)
                {
                    if (!Util_FeatureVectorInput.Make_FromFile_PP_Banjo(fv, p1Item.Filepath, p1Item.P1_base, errH))
                    {
                        sb_result.Append("ファイルオープン失敗 PP_Banjo[" + p1Item.Filepath + "]。");
                        goto gt_EndMethod;
                    }
                    sb_result.Append("開" + Path.GetFileName(p1Item.Filepath) + "。");
                }
            }

            {//１９枚の持ち駒
                List<PP_P1Item> p1Items = new List<PP_P1Item>()
                {
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv11PP1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIFU_____),
                    new PP_P1Item( Path.Combine(fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv25pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIFU_____)
                };

                foreach (PP_P1Item ppItem in p1Items)
                {
                    if (!Util_FeatureVectorInput.Make_FromFile_PP_Moti19Mai(fv, ppItem.Filepath, ppItem.P1_base, errH))
                    {
                        sb_result.Append("ファイルオープン失敗 PP_Banjo[" + ppItem.Filepath + "]。");
                        goto gt_EndMethod;
                    }
                    sb_result.Append("開" + Path.GetFileName(ppItem.Filepath) + "。");
                }
            }

            {//３枚の持駒
                List<PP_P1Item> p1Items = new List<PP_P1Item>()
                {
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv12PP1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKYO____),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv13pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKEI____),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv14pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIGIN____),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv15pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKIN____),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv26pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKYO____),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv27pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKEI____),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv28pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIGIN____),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv29pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKIN____),
                };

                foreach (PP_P1Item ppItem in p1Items)
                {
                    if (!Util_FeatureVectorInput.Make_FromFile_PP_Moti3or5Mai(fv, ppItem.Filepath, ppItem.P1_base, 5, errH))
                    {
                        sb_result.Append("ファイルオープン失敗 PP_Banjo[" + ppItem.Filepath + "]。");
                        goto gt_EndMethod;
                    }
                    sb_result.Append("開" + Path.GetFileName(ppItem.Filepath) + "。");
                }
            }

            {//２枚の持駒
                List<PP_P1Item> p1Items = new List<PP_P1Item>()
                {
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv16pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIHISYA__),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv17pp1pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKAKU___),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv30pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIHISYA__),
                    new PP_P1Item( Path.Combine( fvDirectory, toml.Get<TomlTable>("Resources").Get<string>("Fv31pp2pInFvDir")),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKAKU___),
                };

                foreach (PP_P1Item ppItem in p1Items)
                {
                    if (!Util_FeatureVectorInput.Make_FromFile_PP_Moti3or5Mai(fv, ppItem.Filepath, ppItem.P1_base, 3, errH))
                    {
                        sb_result.Append("ファイルオープン失敗 PP_Banjo[" + ppItem.Filepath + "]。");
                        goto gt_EndMethod;
                    }
                    sb_result.Append("開" + Path.GetFileName(ppItem.Filepath) + "。");
                }
            }

        gt_EndMethod:
            ;
            return sb_result.ToString();
        }



    }
}