﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.A150_LogKyokuPng.B100_KyokumenPng.C___500_Struct;
using Grayscale.A150_LogKyokuPng.B100_KyokumenPng.C500____Struct;
using Grayscale.A150_LogKyokuPng.B200_LogKyokuPng.C500____UtilWriter;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if DEBUG
using System;
using System.Diagnostics;
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B810_LogGraphiEx.C500____Util;
#endif

namespace Grayscale.A240_KifuTreeLog.B110_KifuTreeLog.C500____Struct
{

    /// <summary>
    /// 棋譜ツリー・ログ・ライター
    /// </summary>
    public abstract class Util_KifuTreeLogWriter
    {

        public static KyokumenPngEnvironment REPORT_ENVIRONMENT;
        static Util_KifuTreeLogWriter()
        {
            Util_KifuTreeLogWriter.REPORT_ENVIRONMENT = new KyokumenPngEnvironmentImpl(
                        Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_KifuTreeLog/"),//argsDic["outFolder"],
                        "../../Engine01_Config/img/gkLog/",//argsDic["imgFolder"],
                        "koma1.png",//argsDic["kmFile"],
                        "suji1.png",//argsDic["sjFile"],
                        "20",//argsDic["kmW"],
                        "20",//argsDic["kmH"],
                        "8",//argsDic["sjW"],
                        "12"//argsDic["sjH"]
                        );
        }


        /// <summary>
        /// 棋譜ツリー・ログの書出し
        /// 
        /// TODO: フォルダーパスが長く成りすぎるのを、なんとかしたい。折り返すとか、～中略～にするとか、rootから始めないとか。
        /// </summary>
        public static void A_Write_KifuTreeLog(
            KaisetuBoards logF_kiki,
            KifuTree kifu,
            KwErrorHandler errH
            )
        {
#if DEBUG
            int logFileCounter = 0;

            try
            {
                //----------------------------------------
                // 既存の棋譜ツリー・ログを空に。
                //----------------------------------------
                {
                    string rootFolder = Path.Combine(Util_KifuTreeLogWriter.REPORT_ENVIRONMENT.OutFolder, Conv_Move.KIFU_TREE_LOG_ROOT_FOLDER);
                    if (Directory.Exists(rootFolder))
                    {
                        try
                        {
                            Directory.Delete(rootFolder, true);
                        }
                        catch (IOException)
                        {
                            // ディレクトリーが空でなくて、ディレクトリーを削除できなかったときに
                            // ここにくるが、
                            // ディレクトリーの中は空っぽにできていたりする。
                            //
                            // とりあえず続行。
                        }
                    }
                }

                //----------------------------------------
                // カレントノードまでの符号を使って、フォルダーパスを作成。
                //----------------------------------------
                StringBuilder sb_folder = new StringBuilder();
                kifu.ForeachHonpu(kifu.CurNode, (int temezumi2, KyokumenWrapper kWrap, Node<Move, KyokumenWrapper> node, ref bool toBreak) =>
                {
                    sb_folder.Append(Conv_Move.ToSfen_ForFilename(node.Key) + "/");
                });
                //sb_folder.Append( Conv_SasiteStr_Sfen.ToSasiteStr_Sfen_ForFilename(kifu.CurNode.Key) + "/");

                string sasiteText1 = Conv_Move.ToSfen(kifu.CurNode.Key);
                KifuNode kifuNode1 = (KifuNode)kifu.CurNode;

                // 評価明細のログ出力。
                Util_KifuTreeLogWriter.AA_Write_ForeachLeafs_ForDebug(
                    ref logFileCounter,
                    sasiteText1,
                    kifuNode1,
                    kifu,
                    sb_folder.ToString(),
                    Util_KifuTreeLogWriter.REPORT_ENVIRONMENT,
                    errH
                    );
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。
                string message = ex.GetType().Name + " " + ex.Message + "：評価明細付きのログ出力をしていたときです。：";
                Debug.Fail(message);

                // どうにもできないので  ログだけ取って、上に投げます。
                errH.Logger.WriteLine_Error(message);
                throw ex;
            }

            try
            {
                if (0 < logF_kiki.boards.Count)//ﾛｸﾞが残っているなら
                {
                    bool enableLog = true;// false;
                    //
                    // ログの書き出し
                    //
                    Util_GraphicalLog.WriteHtml5(
                        enableLog,
                        "#評価ログ",
                        "[" + Conv_KaisetuBoards.ToJsonStr(logF_kiki) + "]"
                    );

                    // 書き出した分はクリアーします。
                    logF_kiki.boards.Clear();
                }
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "局面評価明細を出力しようとしたときです。"); throw ex; }
#endif
        }

        /// <summary>
        /// 棋譜ツリーの、ノードに格納されている、局面評価明細を、出力していきます。
        /// </summary>
        public static void AA_Write_ForeachLeafs_ForDebug(
            ref int logFileCounter,
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            string relFolder,
            KyokumenPngEnvironment reportEnvironment,
            KwErrorHandler errH
            )
        {

            // 次ノードの有無
            if (0 < node.Count_ChildNodes)
            {
                // 葉ノードではないなら

                int logFileCounter_temp = logFileCounter;
                // 先に奥の枝から。
                node.Foreach_ChildNodes((Move key, Node<Move, KyokumenWrapper> nextNode, ref bool toBreak) =>
                {
                    float score = ((KifuNode)nextNode).Score;

                    // 再帰
                    Util_KifuTreeLogWriter.AA_Write_ForeachLeafs_ForDebug(
                        ref logFileCounter_temp,
                        nodePath + " " + Conv_Move.ToSfen_ForFilename(nextNode.Key),
                        (KifuNode)nextNode,
                        kifu,
                        relFolder + ((int)score).ToString() + "点_" + Conv_Move.ToSfen(nextNode.Key) + "/",
                        reportEnvironment,
                        errH
                        );

                });
                logFileCounter = logFileCounter_temp;
            }

            // 盤１個分の png 画像ログ出力
            Util_KifuTreeLogWriter.AAA_Write_Node(
                ref logFileCounter,
                nodePath,
                node,
                kifu,
                relFolder,
                reportEnvironment,
                errH
            );

        }

        /// <summary>
        /// 盤１個分のログ。
        /// </summary>
        private static void AAA_Write_Node(
            ref int logFileCounter,
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            string relFolder,
            KyokumenPngEnvironment reportEnvironment,
            KwErrorHandler errH
            )
        {
            string fileName = "";

            try
            {

                // 出力先
                fileName = Conv_Filepath.ToEscape("_log_" + ((int)node.Score) + "点_" + logFileCounter + "_" + nodePath + ".png");
                relFolder = Conv_Filepath.ToEscape(relFolder);
                //
                // 画像ﾛｸﾞ
                //
                if (true)
                {
                    int srcMasu_orMinusOne = -1;
                    int dstMasu_orMinusOne = -1;

                    SyElement srcMasu = Conv_Move.ToSrcMasu(node.Key);
                    SyElement dstMasu = Conv_Move.ToDstMasu(node.Key);
                    bool errorCheck = Conv_Move.ToErrorCheck(node.Key);
                    Komasyurui14 captured = Conv_Move.ToCaptured(node.Key);

                    if (!errorCheck)
                    {
                        srcMasu_orMinusOne = Conv_SyElement.ToMasuNumber(srcMasu);
                        dstMasu_orMinusOne = Conv_SyElement.ToMasuNumber(dstMasu);
                    }

                    KyokumenPngArgs_FoodOrDropKoma foodKoma;
                    if (Komasyurui14.H00_Null___ != captured)
                    {
                        switch (Util_Komasyurui14.NarazuCaseHandle(captured))
                        {
                            case Komasyurui14.H00_Null___: foodKoma = KyokumenPngArgs_FoodOrDropKoma.NONE; break;
                            case Komasyurui14.H01_Fu_____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.FU__; break;
                            case Komasyurui14.H02_Kyo____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.KYO_; break;
                            case Komasyurui14.H03_Kei____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.KEI_; break;
                            case Komasyurui14.H04_Gin____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.GIN_; break;
                            case Komasyurui14.H05_Kin____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.KIN_; break;
                            case Komasyurui14.H07_Hisya__: foodKoma = KyokumenPngArgs_FoodOrDropKoma.HI__; break;
                            case Komasyurui14.H08_Kaku___: foodKoma = KyokumenPngArgs_FoodOrDropKoma.KAKU; break;
                            default: foodKoma = KyokumenPngArgs_FoodOrDropKoma.UNKNOWN; break;
                        }
                    }
                    else
                    {
                        foodKoma = KyokumenPngArgs_FoodOrDropKoma.NONE;
                    }


                    // 評価明細に添付
                    Util_KyokumenPng_Writer.Write1(
                        Conv_KifuNode.ToRO_Kyokumen1(node, errH),
                        srcMasu_orMinusOne,
                        dstMasu_orMinusOne,
                        foodKoma,
                        Conv_Move.ToSfen(node.Key),
                        relFolder,
                        fileName,
                        reportEnvironment,
                        errH
                        );
                    logFileCounter++;
                }

                //
                // 評価明細
                //
                {
                    Util_KifuTreeLogWriter.AAAA_Write_HyokaMeisai(fileName, node, relFolder, reportEnvironment);
                }
            }
            catch (System.Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "盤１個分のログを出力しようとしていたときです。\n fileName=[" + fileName + "]\n relFolder=[" + relFolder + "]"); throw ex;
            }
        }


        /// <summary>
        /// 評価明細の書き出し。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="node"></param>
        /// <param name="relFolder"></param>
        /// <param name="env"></param>
        public static void AAAA_Write_HyokaMeisai(
            string id,
            KifuNode node,
            string relFolder,
            KyokumenPngEnvironment env
            )
        {

            StringBuilder sb = new StringBuilder();

            // 見出し
            sb.Append(id);
            sb.Append("    ");
            sb.Append(((int)node.Score).ToString());
            sb.Append("    ");
            switch (node.Value.Kyokumen.KaisiPside)
            {
                case Playerside.P1: sb.Append("P2が指し終えた局面。手番P1"); break;
                case Playerside.P2: sb.Append("P1が指し終えた局面。手番P2"); break;
                case Playerside.Empty: sb.Append("手番Empty"); break;
            }
            sb.AppendLine();

            foreach (KeyValuePair<string, KyHyokaMeisai_Koumoku> entry in node.KyHyokaSheet_Mutable.Items)
            {
                KyHyokaMeisai_Koumoku koumoku = ((KyHyokaMeisai_Koumoku)entry.Value);

                sb.Append("    ");

                sb.Append(entry.Key);//項目名
                sb.Append("  ");
                sb.Append(koumoku.UtiwakeValue);//評価値
                sb.Append("  ");

                sb.Append(koumoku.Utiwake);//内訳
                sb.AppendLine();
            }
            sb.AppendLine();

            ////------------------------------
            //// TODO: 局面ハッシュ
            ////------------------------------
            //sb.Append("hash:");
            //sb.AppendLine(Conv_Sky.ToKyokumenHash(node.Value.ToKyokumenConst).ToString());
            //sb.AppendLine();

            File.AppendAllText(env.OutFolder + relFolder + "/_log_評価明細.txt", sb.ToString());
        }









//        /// <summary>
//        /// 評価明細ログの書出し
//        /// </summary>
//        private static void B_Write_Html5(
//            //Shogisasi shogisasi,
//            KaisetuBoards logF_kiki,
//            KifuTree kifu,
//            KwErrorHandler errH
//            )
//        {
//#if DEBUG
//            try
//            {
//                if (0 < logF_kiki.boards.Count)//ﾛｸﾞが残っているなら
//                {
//                    bool enableLog = true;// false;
//                    //
//                    // ログの書き出し
//                    //
//                    Util_GraphicalLog.WriteHtml5(
//                        enableLog,
//                        "#評価ログ",
//                        "[" + Conv_KaisetuBoards.ToJsonStr(logF_kiki) + "]"
//                    );

//                    // 書き出した分はクリアーします。
//                    logF_kiki.boards.Clear();
//                }
//            }
//            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "HTML5ログを出力しようとしたときです。"); throw ex; }
//#endif
//        }




    }
}
