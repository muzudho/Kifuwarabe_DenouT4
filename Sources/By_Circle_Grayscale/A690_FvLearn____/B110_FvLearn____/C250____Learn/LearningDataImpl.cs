using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;
using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C500____usiFrame___;
using Grayscale.A060_Application.B510_Conv_Sy____.C500____Converter;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A150_LogKyokuPng.B100_KyokumenPng.C___500_Struct;
using Grayscale.A150_LogKyokuPng.B100_KyokumenPng.C500____Struct;
using Grayscale.A150_LogKyokuPng.B200_LogKyokuPng.C500____UtilWriter;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C___250_Struct;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C250____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B300_KomahaiyaTr.C500____Table;
using Grayscale.A210_KnowNingen_.B380_Michi______.C500____Word;
using Grayscale.A210_KnowNingen_.B390_KomahaiyaEx.C500____Util;
using Grayscale.A210_KnowNingen_.B490_ForcePromot.C250____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C250____Args;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C100____Shogisasi;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C500____KifuWarabe;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___250_Learn;
using System;
using System.IO;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C500____Util;

#if DEBUG
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;
using System.Diagnostics;
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C250____Learn
{
    /// <summary>
    /// 学習用データ。
    /// </summary>
    public class LearningDataImpl : LearningData
    {
        public LearningDataImpl()
        {
            this.Fv = new FeatureVectorImpl();
        }

        /// <summary>
        /// 読み用。
        /// </summary>
        private FeatureVector featureVector_ForYomi = new FeatureVectorImpl();
        private Shogisasi shogisasi_ForYomi = new ShogisasiImpl(new KifuWarabeImpl(new UsiFrameworkImpl()));

        public static KyokumenPngEnvironment REPORT_ENVIRONMENT;
        static LearningDataImpl()
        {
            LearningDataImpl.REPORT_ENVIRONMENT = new KyokumenPngEnvironmentImpl(
                        Const_Filepath.m_EXE_TO_LOGGINGS,
                        Const_Filepath.m_EXE_TO_CONFIG + "img/gkLog/",
                        "koma1.png",//argsDic["kmFile"],
                        "suji1.png",//argsDic["sjFile"],
                        "20",//argsDic["kmW"],
                        "20",//argsDic["kmH"],
                        "8",//argsDic["sjW"],
                        "12"//argsDic["sjH"]
                        );
        }

        public CsaKifu CsaKifu { get; set; }

        public Earth Earth { get; set; }
        public Tree Kifu { get; set; }
        public Sky GetSky()
        {
            return this.Kifu.CurNode.GetValue();
        }

        /// <summary>
        /// フィーチャー・ベクター。
        /// </summary>
        public FeatureVector Fv { get; set; }


        /// <summary>
        /// 初期設定。
        /// </summary>
        public void AtBegin(Uc_Main uc_Main)
        {
            // データの読取「道」
            if (Michi187Array.Load(Const_Filepath.m_EXE_TO_CONFIG + "data_michi187.csv"))
            {
            }

            // データの読取「配役」
            Util_Array_KomahaiyakuEx184.Load(Const_Filepath.m_EXE_TO_CONFIG + "data_haiyaku185_UTF-8.csv", Encoding.UTF8);

            // データの読取「強制転成表」　※駒配役を生成した後で。
            Array_ForcePromotion.Load(Const_Filepath.m_EXE_TO_CONFIG + "data_forcePromotion_UTF-8.csv", Encoding.UTF8);
#if DEBUG
            {
                File.WriteAllText(Const_Filepath.m_EXE_TO_LOGGINGS + "_log_強制転成表.html", Array_ForcePromotion.LogHtml());
            }
#endif

            // データの読取「配役転換表」
            Data_KomahaiyakuTransition.Load(Const_Filepath.m_EXE_TO_CONFIG + "data_syuruiToHaiyaku.csv", Encoding.UTF8);
#if DEBUG
            {
                File.WriteAllText(Const_Filepath.m_EXE_TO_LOGGINGS + "_log_配役転換表.html", Data_KomahaiyakuTransition.Format_LogHtml());
            }
#endif

            // ファイルへのパス。
            uc_Main.TxtFvFilepath.Text = Path.GetFullPath(Const_Filepath.m_EXE_TO_CONFIG + "fv/fv_00_Komawari.csv");
            uc_Main.TxtStatus1.Text = "開くボタンで開いてください。";
        }
        /// <summary>
        /// 局面PNG画像を更新。
        /// </summary>
        public void ChangeKyokumenPng(Uc_Main uc_Main)
        {
            uc_Main.PctKyokumen.Image = null;//掴んでいる画像ファイルを放します。
            this.WritePng(Util_Loggers.ProcessLearner_DEFAULT);
            uc_Main.PctKyokumen.ImageLocation = Const_Filepath.m_EXE_TO_LOGGINGS + "_log_学習局面.png";
        }

        /// <summary>
        /// 棋譜読込み。
        /// </summary>
        public void ReadKifu(Uc_Main uc_Main)
        {

            if (!File.Exists(uc_Main.TxtKifuFilepath.Text))
            {
                goto gt_EndMethod;
            }


            // CSA棋譜テキスト→対訳→データ
            this.CsaKifu = Util_Csa.ReadFile(uc_Main.TxtKifuFilepath.Text);



            //----------------------------------------
            // 読み用。
            //----------------------------------------
            this.featureVector_ForYomi = new FeatureVectorImpl();


        gt_EndMethod:
            ;
        }


        public void WriteFv()
        {
        }


        /// <summary>
        /// 合法手一覧を作成したい。
        /// </summary>
        public void Aa_Yomi(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            KwLogger errH)
        {
            //----------------------------------------
            // 合法手のNextNodesを作成します。
            //----------------------------------------

#if DEBUG
            KaisetuBoards logF_kiki_orNull = null;// デバッグ用。
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            logF_kiki_orNull = new KaisetuBoards();
#endif
            EvaluationArgs args;
            {
                args = new EvaluationArgsImpl(
                    this.Earth.GetSennititeCounter(),
                    this.featureVector_ForYomi,
                    this.shogisasi_ForYomi,
                    LearningDataImpl.REPORT_ENVIRONMENT
#if DEBUG
                    ,
                    logF_kiki_orNull
#endif
                    );
            }
            this.Aaa_CreateNextNodes_Gohosyu(
                ref searchedMaxDepth,
                ref searchedNodes,
                searchedPv,
                args, errH);
#if DEBUG
            sw2.Stop();
            Console.WriteLine("合法手作成　　　 　= {0}", sw2.Elapsed);
            Console.WriteLine("────────────────────────────────────────");
#endif


            ////
            //// 内部データ
            ////
            //{
            //    if (null != errH.Dlgt_OnNaibuDataClear_or_Null)
            //    {
            //        errH.Dlgt_OnNaibuDataClear_or_Null();
            //        errH.Dlgt_OnNaibuDataAppend_or_Null(this.DumpToAllGohosyu(this.Kifu.CurNode.Value.ToKyokumenConst));
            //    }
            //}
        }

        /// <summary>
        /// 全合法手をダンプ。
        /// </summary>
        /// <returns></returns>
        public string DumpToAllGohosyu(Sky src_Sky)
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("--------------------");
            //sb.AppendLine("カレントノード内部データ");
            //sb.AppendLine("--------------------");
            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                // 番号
                sb.Append("Fig.");
                sb.Append(finger);
                sb.Append("　");

                // P1,P2
                sb.Append(Conv_Busstop.ToPlayerside( koma));
                sb.Append("　");

                // 升00
                sb.Append(Conv_Sy.Query_Word(Conv_Busstop.ToMasu( koma).Bitfield));
                sb.Append("　");

                // 歩、香…
                sb.Append(Util_Komasyurui14.ToIchimoji(Conv_Busstop.ToKomasyurui( koma)));

                sb.AppendLine();
            });

            return sb.ToString();
        }

        /// <summary>
        /// 局面PNG画像書き出し。
        /// </summary>
        public void WritePng(KwLogger errH)
        {
            int srcMasu_orMinusOne = -1;
            int dstMasu_orMinusOne = -1;

            SyElement srcMasu = Conv_Move.ToSrcMasu(this.Kifu.CurNode.Key);
            SyElement dstMasu = Conv_Move.ToSrcMasu(this.Kifu.CurNode.Key);
            Komasyurui14 captured = Conv_Move.ToCaptured(this.Kifu.CurNode.Key);
            bool errorCheck = Conv_Move.ToErrorCheck(this.Kifu.CurNode.Key);

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

            // 学習フォーム
            Util_KyokumenPng_Writer.Write1(
                Conv_KifuNode.ToRO_Kyokumen1(this.GetSky(), errH),
                srcMasu_orMinusOne,
                dstMasu_orMinusOne,
                foodKoma,
                Conv_Move.ToSfen( this.Kifu.CurNode.Key),
                "",
                "_log_学習局面.png",
                LearningDataImpl.REPORT_ENVIRONMENT,
                errH
                );
        }

        /// <summary>
        /// 合法手を一覧します。
        /// </summary>
        /// <param name="uc_Main"></param>
        public void Aaa_CreateNextNodes_Gohosyu(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            EvaluationArgs args,
            KwLogger errH)
        {
            try
            {
                //
                // 指し手生成ルーチンで、棋譜ツリーを作ります。
                //
                bool isHonshogi = true;
                MoveEx bestNode = Tansaku_FukasaYusen_Routine.WAA_Yomu_Start(
                    ref searchedMaxDepth,
                    ref searchedNodes,
                    searchedPv,

                    this.GetSky().Temezumi,
                    this.GetSky().KaisiPside,
                    this.Kifu.CurNode,
                    this.GetSky(),


                    isHonshogi, Mode_Tansaku.Learning,
                    args, errH);
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "棋譜ツリーを作っていたときです。"); throw ex; }

        }

        /// <summary>
        /// 二駒関係の評価値を算出します。
        /// </summary>
        public void DoScoreing_ForLearning(
            Sky position
#if DEBUG || LEARN
            ,
            out KyHyokaMeisai_Koumoku out_komawariMeisai,
            out KyHyokaMeisai_Koumoku out_ppMeisai
#endif
        )
        {
            //----------------------------------------
            // Komawari
            //----------------------------------------
            {
                Hyokakansu_Komawari handan = new Hyokakansu_Komawari();
                float score;
                handan.Evaluate(
                    out score,
#if DEBUG || LEARN
                    out out_komawariMeisai,
#endif
                    position,
                    this.Fv, //参照してもらうだけ。
                    Util_Loggers.ProcessLearner_DEFAULT
                );
            }
            //----------------------------------------
            // PP
            //----------------------------------------
            {
                Hyokakansu_NikomaKankeiPp handan_pp = new Hyokakansu_NikomaKankeiPp();
                float score;
                handan_pp.Evaluate(
                    out score,
#if DEBUG || LEARN
                    out out_ppMeisai,
#endif
                    position,
                    this.Fv, //参照してもらうだけ。
                    Util_Loggers.ProcessLearner_DEFAULT
                );
            }
        }

    }
}
