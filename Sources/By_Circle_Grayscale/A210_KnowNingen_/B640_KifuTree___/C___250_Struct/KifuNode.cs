using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;


namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct
{

    /// <summary>
    /// 棋譜ノード。
    /// </summary>
    public interface KifuNode : Node<Move, KyokumenWrapper>
    {

        #region プロパティー

        /// <summary>
        /// スコア
        /// </summary>
        float Score { get; }
        void AddScore(float offset);
        void SetScore(float score);

        /// <summary>
        /// 局面評価明細。Mutable なので、SkyConst には入れられない。
        /// </summary>
        KyHyokaSheet KyHyokaSheet_Mutable { get; }
        /// <summary>
        /// 枝専用。
        /// </summary>
        /// <param name="branchKyHyokaSheet"></param>
        void SetBranchKyHyokaSheet(KyHyokaSheet branchKyHyokaSheet);

        #endregion

        /// <summary>
        /// この局面データを、読み込めないようにします。（移行用）
        /// </summary>
        void Lock_Kyokumendata();


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜に　次の一手　を追加します。
        /// ************************************************************************************************************************
        /// 
        /// KifuIO を通して使ってください。
        /// 
        /// ①コマ送り用。
        /// ②「成り」フラグの更新用。
        /// ③マウス操作用
        /// 
        /// カレントノードは変更しません。
        /// </summary>
        void PutTuginoitte_New(Node<Move, KyokumenWrapper> newNode);
        /// <summary>
        /// 既存の子要素を上書きします。
        /// </summary>
        /// <param name="existsNode"></param>
        void PutTuginoitte_Override(Node<Move, KyokumenWrapper> existsNode);
        bool HasTuginoitte(Move sasiteStr);

        string Json_NextNodes_MultiSky(
            string memo,
            string hint,
            int temezumi_yomiGenTeban_forLog,//読み進めている現在の手目済
            KwLogger errH
            );

        bool IsLeaf { get; }
    }
}
