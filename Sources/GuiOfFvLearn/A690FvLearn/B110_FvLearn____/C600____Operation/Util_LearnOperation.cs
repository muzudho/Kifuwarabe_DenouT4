﻿#if DEBUG
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
// using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A500ShogiEngine.B523UtilFv.C480UtilFvEdit;
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
#elif LEARN
using Grayscale.A500ShogiEngine.B523UtilFv.C480UtilFvEdit;
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
#else
using System.IO;
using System.Text;
using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A240_KifuTreeLog.B110KifuTreeLog.C500Struct;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B200Scoreing.C250Args;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C100Shogisasi;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe;
using Grayscale.A500ShogiEngine.B523UtilFv.C510UtilFvLoad;
using Grayscale.A690FvLearn.B110FvLearn.C250Learn;
using Grayscale.A690FvLearn.B110FvLearn.C260View;
using Grayscale.A690FvLearn.B110FvLearn.C420Inspection;
using Grayscale.A690FvLearn.B110FvLearn.C470____StartZero;
using Grayscale.Kifuwaragyoku.UseCases;
#endif

namespace Grayscale.A690FvLearn.B110FvLearn.C600Operation
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
        public static void ARankUpSelectedMove(UcMain uc_Main, float tyoseiryo, ILogTag logger)
        {
            //----------------------------------------
            // 選択したノードを参考に、減点を行う。
            //----------------------------------------
            foreach (GohosyuListItem item in uc_Main.LstGohosyu.SelectedItems)
            {
                Move move1 = item.Move;
#if DEBUG
                string sfen = ConvMove.ToSfen(item.Move);
                logger.AppendLine("sfen=" + sfen);
                logger.Flush(LogTypes.Plain);
#endif
                /*
                if (uc_Main.LearningData.ContainsKeyCurChildNode(move1, uc_Main.LearningData.KifuA, logger))
                {
#if DEBUG
                    logTag.AppendLine("----------------------------------------");
                    logTag.AppendLine("FV 総合点（読込前）1");
                    logTag.AppendLine("      PP =" + Util_FeatureVectorEdit.GetTotal_PP(uc_Main.LearningData.Fv));
                    logTag.AppendLine("----------------------------------------");
                    logTag.Flush(LogTypes.Plain);
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
                    logTag.AppendLine("----------------------------------------");
                    logTag.AppendLine("FV 総合点（読込後）6");
                    logTag.AppendLine("      PP =" + Util_FeatureVectorEdit.GetTotal_PP(uc_Main.LearningData.Fv));
                    logTag.AppendLine("----------------------------------------");
                    logTag.Flush(LogTypes.Plain);
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
            string[] searchedPv = new string[ProgramSupport.SEARCHED_PV_LENGTH];
            uc_Main.LearningData.Aa_Yomi(
                ref searchedMaxDepth,
                ref searchedNodes,
                uc_Main.LearningData.KifuA,
                uc_Main.LearningData.PositionA,
                searchedPv,
                LogTags.ProcessLearnerDefault);
        }


        /// <summary>
        /// 初期局面の評価値を 0 点にするようにFVを調整します。
        /// </summary>
        public static void Do_ZeroStart(ref bool isRequest_ShowGohosyu, UcMain uc_Main, ILogTag logTag)
        {
            bool isRequestDoEvents = false;
            Util_StartZero.Adjust_HirateSyokiKyokumen_0ten_AndFvParamRange(ref isRequestDoEvents, uc_Main.LearningData.Fv, logTag);

            //// 合法手一覧を作成
            //uc_Main.LearningData.Aa_Yomi(uc_Main.LearningData.Kifu.CurNode.Key, logTag);

            // 局面の合法手表示の更新を要求
            isRequest_ShowGohosyu = true;
        }

        /// <summary>
        /// FIXME: 未実装
        /// 指し手の順位上げ。
        /// </summary>
        public static void DoRankUpMove(
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            UcMain uc_Main, ILogTag logTag)
        {
            // 評価値変化量
            float chosei_bairitu;
            float.TryParse(uc_Main.TxtChoseiBairituB.Text, out chosei_bairitu);

            if (Playerside.P2 == uc_Main.LearningData.PositionA.GetKaisiPside())
            {
                chosei_bairitu *= -1; //後手はマイナスの方が有利。
            }

            Util_LearnOperation.ARankUpSelectedMove(uc_Main, chosei_bairitu, logTag);

            // 現局面の合法手表示の更新を要求
            isRequest_ShowGohosyu = true;
            // 局面PNG画像更新を要求
            isRequest_ChangeKyokumenPng = true;
        }

        /// <summary>
        /// FIXME: 未実装
        /// 指し手の順位下げ。
        /// </summary>
        public static void DoRankDownMove(
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            UcMain uc_Main, ILogTag logTag)
        {
            // 評価値変化量
            float badScore;
            float.TryParse(uc_Main.TxtChoseiBairituB.Text, out badScore);
            badScore *= -1.0f;

            if (Playerside.P2 == uc_Main.LearningData.PositionA.GetKaisiPside())
            {
                badScore *= -1; //後手はプラスの方が不利。
            }

            Util_LearnOperation.ARankUpSelectedMove(uc_Main, badScore, logTag);

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
        public static void Do_ShowNikomaHyokati(UcMain uc_Main)
        {
            uc_Main.LearningData.DoScoreing_ForLearning(
                uc_Main.LearningData.PositionA.GetKaisiPside(),
                uc_Main.LearningData.PositionA
                );

            uc_Main.TxtNikomaHyokati.Text = "";
        }

        public static void Do_OpenFvCsv(UcMain uc_Main)
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
                    sb_result.Append(Util_FvLoad.OpenFv(uc_Main.LearningData.Fv, filepath_base, LogTags.ProcessLearnerDefault));
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
                        float paramRange = UtilInspection.FvParamRange(uc_Main.LearningData.Fv);
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



        public static void Load_CsaKifu(UcMain uc_Main, ILogTag logTag)
        {
            uc_Main.LearningData.ReadKifu(uc_Main);

            UtilLearningView.ShowMoveList(uc_Main.LearningData, uc_Main, logTag);
        }


        public static void Do_OpenCsaKifu(
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            string kifuFilepath,
            UcMain uc_Main, ILogTag logTag)
        {
            uc_Main.TxtKifuFilepath.Text = kifuFilepath;

            // 平手初期局面の棋譜ツリーを準備します。
            Util_LearnOperation.Setup_KifuTree(
                ref isRequest_ShowGohosyu,
                ref isRequest_ChangeKyokumenPng,
                uc_Main, logTag);

            // 処理が重いので。
            Application.DoEvents();

            // CSA棋譜を読み込みます。
            Util_LearnOperation.Load_CsaKifu(uc_Main, logTag);

            // 合法手を調べます。
            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[ProgramSupport.SEARCHED_PV_LENGTH];
            uc_Main.LearningData.Aa_Yomi(
                ref searchedMaxDepth,
                ref searchedNodes,
                uc_Main.LearningData.KifuA,
                uc_Main.LearningData.PositionA,
                searchedPv,
                logTag);
            // ノード情報の表示
            UtilLearningView.Aa_ShowNode2(
                uc_Main.LearningData,
                uc_Main.LearningData.PositionA,
                uc_Main, LogTags.ProcessLearnerDefault);

            //gt_EndMethod:
            //    ;
        }

        /// <summary>
        /// 棋譜ツリーをセットアップします。
        /// </summary>
        public static void Setup_KifuTree(
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            UcMain uc_Main,
            ILogTag logTag)
        {
            ISky positionA;
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
                    new ShogisasiImpl(new Playing(), new ProgramSupport(new UsiFrameworkImpl())),
                    UtilKifuTreeLogWriter.REPORT_ENVIRONMENT
#if DEBUG
                    ,
                    logF_kiki
#endif
);
            }

            // 合法手を数えたい。
            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[ProgramSupport.SEARCHED_PV_LENGTH];
            uc_Main.LearningData.Aaa_CreateNextNodes_Gohosyu(
                ref searchedMaxDepth,
                ref searchedNodes,
                newKifu1_Hirate,
                positionA.GetKaisiPside(),
                positionA,
                searchedPv,
                args, logTag);

            // 現局面の合法手表示の更新を要求
            isRequest_ShowGohosyu = true;

            // 局面PNG画像更新を要求
            isRequest_ChangeKyokumenPng = true;
        }


    }
}
