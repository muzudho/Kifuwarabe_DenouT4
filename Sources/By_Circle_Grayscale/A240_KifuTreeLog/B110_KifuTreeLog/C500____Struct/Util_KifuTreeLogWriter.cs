using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.A150_LogKyokuPng.B100_KyokumenPng.C___500_Struct;
using Grayscale.A150_LogKyokuPng.B100_KyokumenPng.C500____Struct;
using Grayscale.A150_LogKyokuPng.B200_LogKyokuPng.C500____UtilWriter;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.IO;

#if DEBUG
using System;
using System.Diagnostics;
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B810_LogGraphiEx.C500____Util;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using System.Text;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
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
            Tree kifu,
            KwLogger errH
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
                Util_Tree.ForeachHonpu2(kifu.CurNode, (int temezumi2, Move move, ref bool toBreak) =>
                {
                    sb_folder.Append(Conv_Move.ToSfen_ForFilename(move) + "/");
                });
                //sb_folder.Append( Conv_SasiteStr_Sfen.ToSasiteStr_Sfen_ForFilename(kifu.CurNode.Key) + "/");

                string sasiteText1 = Conv_Move.ToSfen(kifu.CurNode.Key);
                MoveEx kifuNode1 = kifu.CurNode;

                /*
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
                */
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。
                string message = ex.GetType().Name + " " + ex.Message + "：評価明細付きのログ出力をしていたときです。：";
                Debug.Fail(message);

                // どうにもできないので  ログだけ取って、上に投げます。
                errH.AppendLine(message);
                errH.Flush(LogTypes.Error);
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
            catch (Exception ex) {
                errH.DonimoNaranAkirameta(ex, "局面評価明細を出力しようとしたときです。");
                throw ex;
            }
#endif
        }

        /// <summary>
        /// 盤１個分のログ。
        /// </summary>
        private static void AAA_Write_Node(
            ref int logFileCounter,
            string nodePath,
            MoveEx moveEx,
            Sky positionA,
            Tree kifu,
            string relFolder,
            KyokumenPngEnvironment reportEnvironment,
            KwLogger errH
            )
        {
            string fileName = "";

            try
            {

                // 出力先
                fileName = Conv_Filepath.ToEscape("_log_" + ((int)moveEx.Score) + "点_" + logFileCounter + "_" + nodePath + ".png");
                relFolder = Conv_Filepath.ToEscape(relFolder);
                //
                // 画像ﾛｸﾞ
                //
                if (true)
                {
                    int srcMasu_orMinusOne = -1;
                    int dstMasu_orMinusOne = -1;

                    SyElement srcMasu = Conv_Move.ToSrcMasu(moveEx.Move, positionA);
                    SyElement dstMasu = Conv_Move.ToDstMasu(moveEx.Move);
                    bool errorCheck = Conv_Move.ToErrorCheck(moveEx.Move);
                    Komasyurui14 captured = Conv_Move.ToCaptured(moveEx.Move);

                    if (!errorCheck)
                    {
                        srcMasu_orMinusOne = Conv_Masu.ToMasuHandle(srcMasu);
                        dstMasu_orMinusOne = Conv_Masu.ToMasuHandle(dstMasu);
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
                        Conv_KifuNode.ToRO_Kyokumen1(positionA, errH),
                        srcMasu_orMinusOne,
                        dstMasu_orMinusOne,
                        foodKoma,
                        Conv_Move.ToSfen(moveEx.Move),
                        relFolder,
                        fileName,
                        reportEnvironment,
                        errH
                        );
                    logFileCounter++;
                }
            }
            catch (System.Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "盤１個分のログを出力しようとしていたときです。\n fileName=[" + fileName + "]\n relFolder=[" + relFolder + "]");
                throw ex;
            }
        }
    }
}
