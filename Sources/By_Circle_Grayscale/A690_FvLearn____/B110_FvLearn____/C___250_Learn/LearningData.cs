using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A180KifuCsa.B120KifuCsa.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B200Scoreing.C250Args;

#if DEBUG || LEARN
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
#endif

namespace Grayscale.A690FvLearn.B110_FvLearn____.C___250_Learn
{

    /// <summary>
    /// 学習用データ。
    /// </summary>
    public interface LearningData
    {
        CsaKifu CsaKifu { get; set; }

        Earth Earth { get; set; }
        Tree KifuA { get; set; }
        ISky PositionA { get; set; }
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
            ISky positionA
            );


        /// <summary>
        /// 局面PNG画像書き出し。
        /// </summary>
        void WritePng(
            Move move,
            ISky positionA,
            ILogger errH);


        /// <summary>
        /// 合法手を一覧します。
        /// </summary>
        /// <param name="uc_Main"></param>
        void Aaa_CreateNextNodes_Gohosyu(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            Tree kifu1,
            Playerside psideA,
            ISky positionA,
            string[] searchedPv,
            EvaluationArgs args,
            ILogger errH);

        /// <summary>
        /// 全合法手をダンプ。デバッグ用途。
        /// </summary>
        /// <returns></returns>
        string DumpToAllGohosyu(ISky src_Sky);


        /// <summary>
        /// 評価値を算出します。
        /// </summary>
        void DoScoreing_ForLearning(
            Playerside psideA,
            ISky positionA
            );


        /// <summary>
        /// 合法手一覧を作成したい。
        /// </summary>
        void Aa_Yomi(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            Tree kifu1,
            ISky positionA,
            string[] searchedPv,
            ILogger errH);

    }
}
