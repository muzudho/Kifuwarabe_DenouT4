using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;


namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA
{
    public class IttemodosuArgImpl : IttemodosuArg
    {
        /// <summary>
        /// 指し手。
        /// </summary>
        public Move Move { get; set; }

        /// <summary>
        /// これから作る局面の、手目済み。
        /// </summary>
        public int KorekaranoTemezumi { get { return this.m_korekaranoTemezumi_; } }
        private int m_korekaranoTemezumi_;

        public IttemodosuArgImpl(
            Move move,
            int korekaranoTemezumi
            )
        {
            this.Move = move;
            this.m_korekaranoTemezumi_ = korekaranoTemezumi;
        }
    }
}
