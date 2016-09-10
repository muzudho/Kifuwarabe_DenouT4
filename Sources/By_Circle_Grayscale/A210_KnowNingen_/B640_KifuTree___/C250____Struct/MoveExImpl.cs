using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C250____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct
{
    public class MoveExImpl : MoveEx
    {
        public MoveExImpl(Move move)
        {
            this.Move = move;
            this.kyHyokaSheet = new KyHyokaSheetImpl();
        }



        public Move Move { get; set; }



        /// <summary>
        /// スコア
        /// </summary>
        public float Score { get { return this.m_score_; } }
        public void AddScore(float offset) { this.m_score_ += offset; }
        public void SetScore(float score) { this.m_score_ = score; }
        private float m_score_;




        /// <summary>
        /// 局面評価明細。Mutable なので、SkyConst には入れられない。
        /// </summary>
        public KyHyokaSheet KyHyokaSheet_Mutable { get { return this.kyHyokaSheet; } }
        /// <summary>
        /// 枝専用。
        /// </summary>
        /// <param name="branchKyHyoka"></param>
        public void SetBranchKyHyokaSheet(KyHyokaSheet branchKyHyoka)
        {
            this.kyHyokaSheet = branchKyHyoka;
        }
        private KyHyokaSheet kyHyokaSheet;

    }
}
