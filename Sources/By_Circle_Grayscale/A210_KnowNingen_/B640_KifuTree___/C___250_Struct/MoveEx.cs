using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct
{
    public interface MoveEx
    {
        Move Move { get; set; }



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

    }
}
