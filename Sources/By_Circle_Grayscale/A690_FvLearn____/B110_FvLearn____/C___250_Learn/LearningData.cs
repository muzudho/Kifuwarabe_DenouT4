using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C___250_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C___250_Learn
{

    /// <summary>
    /// 学習用データ。
    /// </summary>
    public interface LearningData
    {
        CsaKifu CsaKifu { get; set; }

        KifuTree Kifu { get; set; }

        /// <summary>
        /// フィーチャー・ベクター。
        /// </summary>
        FeatureVector Fv { get; set; }

        /// <summary>
        /// 初期設定。
        /// </summary>
        void AtBegin(Uc_Main uc_Main);

        /// <summary>
        /// 棋譜読込み。
        /// </summary>
        void ReadKifu(Uc_Main uc_Main);

        /// <summary>
        /// 局面PNG画像を更新。
        /// </summary>
        void ChangeKyokumenPng(Uc_Main uc_Main);


        /// <summary>
        /// 局面PNG画像書き出し。
        /// </summary>
        void WritePng(KwErrorHandler errH);


        /// <summary>
        /// 合法手を一覧します。
        /// </summary>
        /// <param name="uc_Main"></param>
        void Aaa_CreateNextNodes_Gohosyu(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            EvaluationArgs args, KwErrorHandler errH);

        /// <summary>
        /// 全合法手をダンプ。デバッグ用途。
        /// </summary>
        /// <returns></returns>
        string DumpToAllGohosyu(SkyConst src_Sky);

        
        /// <summary>
        /// 評価値を算出します。
        /// </summary>
        void DoScoreing_ForLearning(
            KifuNode node
#if DEBUG || LEARN
,
            out KyHyokaMeisai_Koumoku komawariMeisai,
            out KyHyokaMeisai_Koumoku ppMeisai
#endif
            );

        
        /// <summary>
        /// 合法手一覧を作成したい。
        /// </summary>
        void Aa_Yomi(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            KwErrorHandler errH);

    }
}
