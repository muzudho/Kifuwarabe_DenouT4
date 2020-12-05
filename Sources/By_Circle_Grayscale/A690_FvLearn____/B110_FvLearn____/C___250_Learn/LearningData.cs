using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A180KifuCsa.B120KifuCsa.C250Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C500Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C250Struct;
#endif

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C___250_Learn
{

    /// <summary>
    /// 学習用データ。
    /// </summary>
    public interface LearningData
    {
        CsaKifu CsaKifu { get; set; }

        Earth Earth { get; set; }
        Tree KifuA { get; set; }
        Sky PositionA { get; set; }
        Move GetMove();
        Move ToCurChildItem();

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
        void ChangeKyokumenPng(
            Uc_Main uc_Main,
            Move move,
            Sky positionA
            );


        /// <summary>
        /// 局面PNG画像書き出し。
        /// </summary>
        void WritePng(
            Move move,
            Sky positionA,
            KwLogger errH);


        /// <summary>
        /// 合法手を一覧します。
        /// </summary>
        /// <param name="uc_Main"></param>
        void Aaa_CreateNextNodes_Gohosyu(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            Tree kifu1,
            Playerside psideA,
            Sky positionA,
            string[] searchedPv,
            EvaluationArgs args,
            KwLogger errH);

        /// <summary>
        /// 全合法手をダンプ。デバッグ用途。
        /// </summary>
        /// <returns></returns>
        string DumpToAllGohosyu(Sky src_Sky);


        /// <summary>
        /// 評価値を算出します。
        /// </summary>
        void DoScoreing_ForLearning(
            Playerside psideA,
            Sky positionA
            );


        /// <summary>
        /// 合法手一覧を作成したい。
        /// </summary>
        void Aa_Yomi(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            Tree kifu1,
            Sky positionA,
            string[] searchedPv,
            KwLogger errH);

    }
}
