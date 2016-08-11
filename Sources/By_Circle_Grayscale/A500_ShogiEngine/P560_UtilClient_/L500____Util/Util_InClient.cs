﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.P145_SfenStruct_.C___250_Struct;
using Grayscale.P145_SfenStruct_.C250____Struct;
using Grayscale.P146_ConvSfen___.C500____Converter;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P307_UtilSky____.C500____Util;
using Grayscale.P324_KifuTree___.C250____Struct;
using Grayscale.P325_PnlTaikyoku.C___250_Struct;
using Grayscale.P355_KifuParserA.L___500_Parser;

namespace Grayscale.P560_UtilClient_.C500____Util
{
    public abstract class Util_InClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="restText"></param>
        /// <param name="startposImporter"></param>
        /// <param name="logTag"></param>
        public static void OnChangeSky_Im_Client(
            Model_Taikyoku model_Taikyoku,
            KifuParserA_Genjo genjo,
            KwErrorHandler errH
            )
        {
            errH.Logger.WriteLine_Error("（＾△＾）「" + genjo.InputLine + "」Util_InClient　：　クライアントの委譲メソッドｷﾀｰ☆");


            string old_inputLine = genjo.InputLine;//退避
            string rest;
            RO_Kyokumen2_ForTokenize ro_Kyokumen2_ForTokenize;
            Conv_Sfenstring146.ToKyokumen2(
                genjo.InputLine,
                out rest,
                out ro_Kyokumen2_ForTokenize
                );

            errH.Logger.WriteLine_Error("（＾△＾）old_inputLine=「" + old_inputLine + "」 rest=「" + rest + "」 Util_InClient　：　ﾊﾊｯ☆");

            //string old_inputLine = genjo.InputLine;
            //genjo.InputLine = "";


            //----------------------------------------
            // 棋譜を空っぽにし、指定の局面を与えます。
            //----------------------------------------
            {
                model_Taikyoku.Kifu.Clear();// 棋譜を空っぽにします。

                // 文字列から、指定局面を作成します。
                Playerside pside = Playerside.P1;
                int temezumi = 0;
                model_Taikyoku.Kifu.GetRoot().Value.SetKyokumen(
                    SkyConst.NewInstance(
                        Conv_Sfenstring307.ToSkyConst(new SfenstringImpl(old_inputLine), pside, temezumi),
                        temezumi//初期配置は 0手目済み。
                    )
                );//SFENのstartpos解析時
                model_Taikyoku.Kifu.SetProperty(Word_KifuTree.PropName_Startpos, old_inputLine);//指定の初期局面
            }


        }

    }
}
