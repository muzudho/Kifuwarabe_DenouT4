using System;
using System.IO;
using System.Text;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B310Settei.C500Struct;
using Grayscale.A060Application.B510ConvSy.C500Converter;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___;
using Grayscale.A150LogKyokuPng.B100KyokumenPng.C500Struct;
using Grayscale.A150LogKyokuPng.B100KyokumenPng.C500Struct;
using Grayscale.A150LogKyokuPng.B200LogKyokuPng.C500UtilWriter;
using Grayscale.A180KifuCsa.B120KifuCsa.C250Struct;
using Grayscale.A180KifuCsa.B120KifuCsa.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B300_KomahaiyaTr.C500Table;
using Grayscale.A210KnowNingen.B380Michi.C500Word;
using Grayscale.A210KnowNingen.B390KomahaiyaEx.C500Util;
using Grayscale.A210KnowNingen.B490ForcePromot.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B180Hyokakansu.C500Hyokakansu;
using Grayscale.A500ShogiEngine.B200Scoreing.C240Shogisasi;
using Grayscale.A500ShogiEngine.B200Scoreing.C250Args;
using Grayscale.A500ShogiEngine.B200Scoreing.C250Args;
using Grayscale.A500ShogiEngine.B240_TansaFukasa.C500Struct;
using Grayscale.A500ShogiEngine.B240_TansaFukasa.C500Struct;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C100Shogisasi;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe;
using Grayscale.A690FvLearn.B110_FvLearn____.C___250_Learn;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
// using Grayscale.A060Application.B310Settei.C500Struct;
using System.Diagnostics;
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
// using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
#endif

#if DEBUG || LEARN
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
#endif

namespace Grayscale.A690FvLearn.B110_FvLearn____.C250____Learn
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
        public Tree KifuA { get; set; }
        public ISky PositionA { get; set; }// FIXME: できればカレントノードの局面。
        public Move GetMove()
        {
            return this.KifuA.MoveEx_Current.Move;
        }
        public Move ToCurChildItem()
        {
            return this.KifuA.MoveEx_Current.Move;//.Child_GetItem(this.KifuA);
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
        public void ChangeKyokumenPng(
            Uc_Main uc_Main,
            Move move,
            ISky positionA
            )
        {
            uc_Main.PctKyokumen.Image = null;//掴んでいる画像ファイルを放します。
            this.WritePng(
                move,
                positionA,
                ErrorControllerReference.ProcessLearnerDefault
                );
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
            Tree kifu1,
            ISky positionA,
            string[] searchedPv,
            ILogger errH)
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
                kifu1,
                positionA.GetKaisiPside(),
                positionA,
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
        public string DumpToAllGohosyu(ISky src_Sky)
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
                sb.Append(Conv_Busstop.ToPlayerside(koma));
                sb.Append("　");

                // 升00
                sb.Append(Conv_Sy.Query_Word(Conv_Busstop.ToMasu(koma).Bitfield));
                sb.Append("　");

                // 歩、香…
                sb.Append(Util_Komasyurui14.ToIchimoji(Conv_Busstop.ToKomasyurui(koma)));

                sb.AppendLine();
            });

            return sb.ToString();
        }

        /// <summary>
        /// 局面PNG画像書き出し。
        /// </summary>
        public void WritePng(
            Move move,
            ISky positionA,
            ILogger errH
            )
        {
            int srcMasu_orMinusOne = -1;
            int dstMasu_orMinusOne = -1;

            SyElement srcMasu = ConvMove.ToSrcMasu(move);
            SyElement dstMasu = ConvMove.ToSrcMasu(move);
            Komasyurui14 captured = ConvMove.ToCaptured(move);
            bool errorCheck = ConvMove.ToErrorCheck(move);

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

            // 学習フォーム
            Util_KyokumenPng_Writer.Write1(
                ConvKifuNode.ToRO_Kyokumen1(positionA, errH),
                srcMasu_orMinusOne,
                dstMasu_orMinusOne,
                foodKoma,
                ConvMove.ToSfen(move),
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
            Tree kifu1,
            Playerside psideA,
            ISky positionA,
            string[] searchedPv,
            EvaluationArgs args,
            ILogger logger)
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

                    kifu1,
                    psideA,//positionA.GetKaisiPside(),
                    positionA,


                    isHonshogi, Mode_Tansaku.Learning,
                    args, logger);
            }
            catch (Exception ex)
            {
                logger.DonimoNaranAkirameta(ex, "棋譜ツリーを作っていたときです。");
                throw;
            }
        }

        /// <summary>
        /// 二駒関係の評価値を算出します。
        /// </summary>
        public void DoScoreing_ForLearning(
            Playerside psideA,
            ISky positionA
        )
        {
            //----------------------------------------
            // Komawari
            //----------------------------------------
            {
                Hyokakansu_Komawari handan = new Hyokakansu_Komawari();
                float score_notUse = handan.Evaluate(
                    psideA,
                    positionA,
                    this.Fv, //参照してもらうだけ。
                    ErrorControllerReference.ProcessLearnerDefault
                );
            }
            //----------------------------------------
            // PP
            //----------------------------------------
            {
                Hyokakansu_NikomaKankeiPp handan_pp = new Hyokakansu_NikomaKankeiPp();
                float score_notUse = handan_pp.Evaluate(
                    psideA,
                    positionA,
                    this.Fv, //参照してもらうだけ。
                    ErrorControllerReference.ProcessLearnerDefault
                );
            }
        }

    }
}
