using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C500____usiFrame___;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A240_KifuTreeLog.B110_KifuTreeLog.C500____Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C250____Args;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C100____Shogisasi;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C500____KifuWarabe;
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C510____UtilFvLoad;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C250____Learn;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C260____View;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C420____Inspection;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C470____StartZero;
using System.IO;
using System.Text;
using System.Windows.Forms;

#if DEBUG || LEARN
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C480____UtilFvEdit;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

#if DEBUG
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#endif

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C600____Operation
{
    public abstract class Util_LearnOperation
    {

        /// <summary>
        /// 指定の指し手の順位調整を行います。
        /// 
        /// 全体が調整されてしまうような☆？
        /// </summary>
        /// <param name="uc_Main"></param>
        /// <param name="tyoseiryo"></param>
        public static void A_RankUp_SelectedSasite(Uc_Main uc_Main, float tyoseiryo, KwLogger logger)
        {
            //----------------------------------------
            // 選択したノードを参考に、減点を行う。
            //----------------------------------------
            foreach (GohosyuListItem item in uc_Main.LstGohosyu.SelectedItems)
            {
                Move move1 = item.Move;
#if DEBUG
                string sfen = Conv_Move.ToSfen(item.Move);
                errH.AppendLine("sfen=" + sfen);
                errH.Flush(LogTypes.Plain);
#endif
                /*
                if (uc_Main.LearningData.ContainsKeyCurChildNode(move1, uc_Main.LearningData.KifuA, logger))
                {
#if DEBUG
                    errH.AppendLine("----------------------------------------");
                    errH.AppendLine("FV 総合点（読込前）1");
                    errH.AppendLine("      PP =" + Util_FeatureVectorEdit.GetTotal_PP(uc_Main.LearningData.Fv));
                    errH.AppendLine("----------------------------------------");
                    errH.Flush(LogTypes.Plain);
#endif

                    Sky positionA = uc_Main.LearningData.PositionA;

                    Util_IttesasuSuperRoutine.DoMove_Super1(
                        ref positionA,//指定局面
                        ref move1,
                        "F100",
                        logger
                    );

                    // 盤上の駒、持駒を数えます。
                    N54List nextNode_n54List = Util_54List.Calc_54List(positionA, logger);

                    float real_tyoseiryo; //実際に調整した量。
                    Util_FvScoreing.UpdateKyokumenHyoka(
                        nextNode_n54List,
                        positionA,
                        uc_Main.LearningData.Fv,
                        tyoseiryo,
                        out real_tyoseiryo,
                        logger
                        );//相手が有利になる点

                    IttemodosuResult ittemodosuResult;
                    Util_IttemodosuRoutine.UndoMove(
                        out ittemodosuResult,
                        move1,
                        positionA,
                        "F900",
                        logger
                        );
                    positionA = ittemodosuResult.SyuryoSky;


#if DEBUG
                    errH.AppendLine("----------------------------------------");
                    errH.AppendLine("FV 総合点（読込後）6");
                    errH.AppendLine("      PP =" + Util_FeatureVectorEdit.GetTotal_PP(uc_Main.LearningData.Fv));
                    errH.AppendLine("----------------------------------------");
                    errH.Flush(LogTypes.Plain);
#endif
                }
                */
            }

            //----------------------------------------
            // 点数を付け直すために、ノードを一旦、全削除
            //----------------------------------------
            uc_Main.LearningData.KifuA.Pv_RemoveLast(logger);

            //----------------------------------------
            // ネクスト・ノードを再作成
            //----------------------------------------
            // TODO:本譜のネクスト・ノードは？
            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[KifuWarabeImpl.SEARCHED_PV_LENGTH];
            uc_Main.LearningData.Aa_Yomi(
                ref searchedMaxDepth,
                ref searchedNodes,
                uc_Main.LearningData.KifuA,
                uc_Main.LearningData.PositionA,
                searchedPv,
                Util_Loggers.ProcessLearner_DEFAULT);
        }


        /// <summary>
        /// 初期局面の評価値を 0 点にするようにFVを調整します。
        /// </summary>
        public static void Do_ZeroStart(ref bool isRequest_ShowGohosyu, Uc_Main uc_Main, KwLogger errH)
        {
            bool isRequestDoEvents = false;
            Util_StartZero.Adjust_HirateSyokiKyokumen_0ten_AndFvParamRange(ref isRequestDoEvents, uc_Main.LearningData.Fv, errH);

            //// 合法手一覧を作成
            //uc_Main.LearningData.Aa_Yomi(uc_Main.LearningData.Kifu.CurNode.Key, errH);

            // 局面の合法手表示の更新を要求
            isRequest_ShowGohosyu = true;
        }

        /// <summary>
        /// FIXME: 未実装
        /// 指し手の順位上げ。
        /// </summary>
        public static void Do_RankUpSasite(
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            Uc_Main uc_Main, KwLogger errH)
        {
            // 評価値変化量
            float chosei_bairitu;
            float.TryParse(uc_Main.TxtChoseiBairituB.Text, out chosei_bairitu);

            if (Playerside.P2 == uc_Main.LearningData.PositionA.GetKaisiPside())
            {
                chosei_bairitu *= -1; //後手はマイナスの方が有利。
            }

            Util_LearnOperation.A_RankUp_SelectedSasite(uc_Main, chosei_bairitu, errH);

            // 現局面の合法手表示の更新を要求
            isRequest_ShowGohosyu = true;
            // 局面PNG画像更新を要求
            isRequest_ChangeKyokumenPng = true;
        }

        /// <summary>
        /// FIXME: 未実装
        /// 指し手の順位下げ。
        /// </summary>
        public static void Do_RankDownSasite(
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            Uc_Main uc_Main, KwLogger errH)
        {
            // 評価値変化量
            float badScore;
            float.TryParse(uc_Main.TxtChoseiBairituB.Text, out badScore);
            badScore *= -1.0f;

            if (Playerside.P2 == uc_Main.LearningData.PositionA.GetKaisiPside())
            {
                badScore *= -1; //後手はプラスの方が不利。
            }

            Util_LearnOperation.A_RankUp_SelectedSasite(uc_Main, badScore, errH);

            // 現局面の合法手表示の更新を要求
            isRequest_ShowGohosyu = true;
            // 局面PNG画像更新を要求
            isRequest_ChangeKyokumenPng = true;
        }

        /// <summary>
        /// FIXME:未実装
        /// 二駒の評価値を表示。
        /// </summary>
        /// <param name="uc_Main"></param>
        public static void Do_ShowNikomaHyokati(Uc_Main uc_Main)
        {
            uc_Main.LearningData.DoScoreing_ForLearning(
                uc_Main.LearningData.PositionA.GetKaisiPside(),
                uc_Main.LearningData.PositionA
                );

            uc_Main.TxtNikomaHyokati.Text = "";
        }

        public static void Do_OpenFvCsv(Uc_Main uc_Main, KwLogger errH)
        {
            if ("" != uc_Main.TxtFvFilepath.Text)
            {
                uc_Main.OpenFvFileDialog2.InitialDirectory = Path.GetDirectoryName(uc_Main.TxtFvFilepath.Text);
                uc_Main.OpenFvFileDialog2.FileName = Path.GetFileName(uc_Main.TxtFvFilepath.Text);
            }
            else
            {
                uc_Main.OpenFvFileDialog2.InitialDirectory = Application.StartupPath;
            }

            DialogResult result = uc_Main.OpenFvFileDialog2.ShowDialog();
            switch (result)
            {
                case DialogResult.OK:
                    uc_Main.TxtFvFilepath.Text = uc_Main.OpenFvFileDialog2.FileName;
                    string filepath_base = uc_Main.TxtFvFilepath.Text;

                    StringBuilder sb_result = new StringBuilder();
                    // フィーチャー・ベクターの外部ファイルを開きます。
                    sb_result.Append(Util_FvLoad.OpenFv(uc_Main.LearningData.Fv, filepath_base, errH));
                    uc_Main.TxtStatus1.Text = sb_result.ToString();

                    // うまくいっていれば、フィーチャー・ベクターのセットアップが終わっているはず。
                    {
                        // 調整量
                        uc_Main.TyoseiryoSettings.SetSmallest(uc_Main.LearningData.Fv.TyoseiryoSmallest_NikomaKankeiPp);
                        uc_Main.TyoseiryoSettings.SetLargest(uc_Main.LearningData.Fv.TyoseiryoLargest_NikomaKankeiPp);
                        //
                        // 調整量（初期値）
                        //
                        uc_Main.TxtTyoseiryo.Text = uc_Main.LearningData.Fv.TyoseiryoInit_NikomaKankeiPp.ToString();


                        // 半径
                        float paramRange = Util_Inspection.FvParamRange( uc_Main.LearningData.Fv);
                        uc_Main.ChkAutoParamRange.Text = "評価更新毎-" + paramRange + "～" + paramRange + "矯正";
                    }

                    uc_Main.BtnUpdateKyokumenHyoka.Enabled = true;

                    break;
                default:
                    break;
            }

            //gt_EndMethod:
            //    ;
        }



        public static void Load_CsaKifu(Uc_Main uc_Main, KwLogger errH)
        {
            uc_Main.LearningData.ReadKifu(uc_Main);

            Util_LearningView.ShowSasiteList(uc_Main.LearningData, uc_Main, errH);
        }


        public static void Do_OpenCsaKifu(
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            string kifuFilepath,
            Uc_Main uc_Main, KwLogger errH)
        {
            uc_Main.TxtKifuFilepath.Text = kifuFilepath;

            // 平手初期局面の棋譜ツリーを準備します。
            Util_LearnOperation.Setup_KifuTree(
                ref isRequest_ShowGohosyu,
                ref isRequest_ChangeKyokumenPng,
                uc_Main,errH);

            // 処理が重いので。
            Application.DoEvents();

            // CSA棋譜を読み込みます。
            Util_LearnOperation.Load_CsaKifu(uc_Main,errH);

            // 合法手を調べます。
            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[KifuWarabeImpl.SEARCHED_PV_LENGTH];
            uc_Main.LearningData.Aa_Yomi(
                ref searchedMaxDepth,
                ref searchedNodes,
                uc_Main.LearningData.KifuA,
                uc_Main.LearningData.PositionA,
                searchedPv,
                errH);
            // ノード情報の表示
            Util_LearningView.Aa_ShowNode2(
                uc_Main.LearningData,
                uc_Main.LearningData.PositionA,
                uc_Main, Util_Loggers.ProcessLearner_DEFAULT);

        //gt_EndMethod:
        //    ;
        }

        /// <summary>
        /// 棋譜ツリーをセットアップします。
        /// </summary>
        public static void Setup_KifuTree(
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            Uc_Main uc_Main,
            KwLogger errH)
        {
            Sky positionA;
            Tree newKifu1_Hirate;
            {
                Earth newEarth1;
                Util_FvLoad.CreateKifuTree(
                    out newEarth1,
                    out positionA,
                    out newKifu1_Hirate
                    );

                uc_Main.LearningData.Earth = newEarth1;
                uc_Main.LearningData.KifuA = newKifu1_Hirate;
            }

            EvaluationArgs args;
            {
#if DEBUG
                KaisetuBoards logF_kiki = new KaisetuBoards();// デバッグ用だが、メソッドはこのオブジェクトを必要としてしまう。
#endif
                args = new EvaluationArgsImpl(
                    uc_Main.LearningData.Earth.GetSennititeCounter(),
                    new FeatureVectorImpl(),
                    new ShogisasiImpl(new KifuWarabeImpl(new UsiFrameworkImpl())),
                    Util_KifuTreeLogWriter.REPORT_ENVIRONMENT
#if DEBUG
                    ,
                    logF_kiki
#endif
);
            }

            // 合法手を数えたい。
            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[KifuWarabeImpl.SEARCHED_PV_LENGTH];
            uc_Main.LearningData.Aaa_CreateNextNodes_Gohosyu(
                ref searchedMaxDepth,
                ref searchedNodes,
                newKifu1_Hirate,
                positionA,
                searchedPv,
                args, errH);

            // 現局面の合法手表示の更新を要求
            isRequest_ShowGohosyu = true;
            
            // 局面PNG画像更新を要求
            isRequest_ChangeKyokumenPng = true;
        }


    }
}
