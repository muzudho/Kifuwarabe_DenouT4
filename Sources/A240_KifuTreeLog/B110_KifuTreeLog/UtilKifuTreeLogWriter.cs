using System.IO;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B310Settei.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A060Application.B620ConvText.C500Converter;
using Grayscale.A150LogKyokuPng.B100KyokumenPng.C500Struct;
using Grayscale.A150LogKyokuPng.B200LogKyokuPng.C500UtilWriter;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A120KifuSfen;
using Nett;

#if DEBUG
using System;
using System.Diagnostics;
using Grayscale.A210KnowNingen.B110GraphicLog.C500Util;
using Grayscale.A210KnowNingen.B810LogGraphiEx.C500Util;
using Grayscale.A060Application.B110Log.C500Struct;
using System.Text;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
#endif

namespace Grayscale.A240_KifuTreeLog.B110KifuTreeLog.C500Struct
{

    /// <summary>
    /// 棋譜ツリー・ログ・ライター
    /// </summary>
    public abstract class UtilKifuTreeLogWriter
    {
        public static KyokumenPngEnvironment REPORT_ENVIRONMENT;
        static UtilKifuTreeLogWriter()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            UtilKifuTreeLogWriter.REPORT_ENVIRONMENT = new KyokumenPngEnvironmentImpl(
                        Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("KifuTreeLogDirectory")),//argsDic["outFolder"],
                        Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("ImgGkLogDirectory")),//argsDic["imgFolder"],
                        toml.Get<TomlTable>("Resources").Get<string>("Koma1PngFilename"),//argsDic["kmFile"],
                        toml.Get<TomlTable>("Resources").Get<string>("Suji1PngFilename"),//argsDic["sjFile"],
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
            ILogger errH
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
                    string rootFolder = Path.Combine(Util_KifuTreeLogWriter.REPORT_ENVIRONMENT.OutFolder, ConvMove.KIFU_TREE_LOG_ROOT_FOLDER);
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
                    sb_folder.Append(ConvMove.ToSfen_ForFilename(move) + "/");
                });
                //sb_folder.Append( Conv_SasiteStr_Sfen.ToSasiteStr_Sfen_ForFilename(kifu.CurNode.Key) + "/");

                string sasiteText1 = ConvMove.ToSfen(kifu.CurNode.Key);
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
                throw;
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
                throw;
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
            ISky positionA,
            Tree kifu,
            string relFolder,
            KyokumenPngEnvironment reportEnvironment,
            ILogger errH
            )
        {
            string fileName = "";

            try
            {

                // 出力先
                fileName = Conv_Filepath.ToEscape($"_log_{(int)moveEx.Score}点_{logFileCounter}_{nodePath}.png");
                relFolder = Conv_Filepath.ToEscape(relFolder);
                //
                // 画像ﾛｸﾞ
                //
                if (true)
                {
                    int srcMasu_orMinusOne = -1;
                    int dstMasu_orMinusOne = -1;

                    SyElement srcMasu = ConvMove.ToSrcMasu(moveEx.Move, positionA);
                    SyElement dstMasu = ConvMove.ToDstMasu(moveEx.Move);
                    bool errorCheck = ConvMove.ToErrorCheck(moveEx.Move);
                    Komasyurui14 captured = ConvMove.ToCaptured(moveEx.Move);

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
                        ConvKifuNode.ToRO_Kyokumen1(positionA, errH),
                        srcMasu_orMinusOne,
                        dstMasu_orMinusOne,
                        foodKoma,
                        ConvMove.ToSfen(moveEx.Move),
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
                throw;
            }
        }
    }
}
