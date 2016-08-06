using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P226_Tree_______.L___500_Struct;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P341_Ittesasu___.L___250_OperationA;


namespace Grayscale.P341_Ittesasu___.L250____OperationA
{
    public class IttemodosuArgImpl : IttemodosuArg
    {
        /// <summary>
        /// 一手指し局面開始ノード。
        /// </summary>
        public Node<Move, KyokumenWrapper> KaisiNode { get; set; }

        /// <summary>
        /// 指し手。
        /// </summary>
        public Move Move { get; set; }

        /// <summary>
        /// これから作る局面の、手目済み。
        /// </summary>
        public int KorekaranoTemezumi_orMinus1 { get { return this.korekaranoTemezumi_orMinus1; } }
        private int korekaranoTemezumi_orMinus1;

        public IttemodosuArgImpl(
            Node<Move, KyokumenWrapper> kaisiNode,
            Move move,
            int korekaranoTemezumi_orMinus1
            )
        {
            this.KaisiNode = kaisiNode;
            this.Move = move;
            this.korekaranoTemezumi_orMinus1 = korekaranoTemezumi_orMinus1;
        }
    }
}
