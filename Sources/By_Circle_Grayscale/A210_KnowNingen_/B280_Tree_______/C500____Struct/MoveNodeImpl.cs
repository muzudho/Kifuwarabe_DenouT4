using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{

    /// <summary>
    /// ノードです。
    /// </summary>
    public class MoveNodeImpl : MoveNode
    {
        public MoveNodeImpl()
        {
            this.Score = 0.0f;

            this.m_key_ = Move.Empty;
        }
        public MoveNodeImpl(Move move)
        {
            this.Score = 0.0f;

            this.m_key_ = move;
        }



        public Move Key
        {
            get
            {
                return this.m_key_;
            }
        }
        private Move m_key_;
        public float Score { get; set; }







        public void Child_RemoveThis(Tree kifu1, KwLogger logger)
        {
            kifu1.RemoveLastPv(logger);
        }
        public void Child_SetChild(Move move, MoveNode newChildNode, Tree kifu1, KwLogger logger)
        {
            kifu1.AppendPv(move,logger);
        }
    }
}
