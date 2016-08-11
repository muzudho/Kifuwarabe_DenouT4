using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA
{
    public class IttesasuArgImpl : IttesasuArg
    {
        /// <summary>
        /// 一手指し、開始局面。
        /// </summary>
        public　KyokumenWrapper KaisiKyokumen { get; set; }

        /// <summary>
        /// 一手指し、開始局面、手番。
        /// </summary>
        public Playerside KaisiTebanside { get; set; }

        /// <summary>
        /// これから指されるはずの、指し手。
        /// </summary>
        public Move KorekaranoMove { get { return this.m_korekaranoMove_; } }// set;
        private Move m_korekaranoMove_;

        /// <summary>
        /// これから作る局面の、手目済み。
        /// </summary>
        public int KorekaranoTemezumi_orMinus1 { get { return this.korekaranoTemezumi_orMinus1; } }
        private int korekaranoTemezumi_orMinus1;


        public IttesasuArgImpl(
            KyokumenWrapper kaisiKyokumen,
            Playerside tebanside,
            Move korekaranoMove,
            int korekaranoTemezumi_orMinus1
            )
        {
            this.KaisiKyokumen = kaisiKyokumen;
            this.KaisiTebanside = tebanside;
            this.m_korekaranoMove_ = korekaranoMove;
            this.korekaranoTemezumi_orMinus1 = korekaranoTemezumi_orMinus1;
        }
    }
}
