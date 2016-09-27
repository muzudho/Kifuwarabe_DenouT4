using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{
    public class ChildrenImpl : Children
    {
        public ChildrenImpl()
        {
            this.m_move_ = Move.Empty;
            this.m_moveNode_ = null;
        }
        /// <summary>
        /// 棋譜ノードのValueは廃止方針☆
        /// </summary>
        /// <param name="moves"></param>
        /// <param name="parent"></param>
        public ChildrenImpl(List<Move> moves, MoveNode parent)
        {
            foreach (Move move in moves)
            {
                MoveNode newNode = new MoveNodeImpl(move);
                this.AddItem(move, newNode, parent);
                break;
            }
        }



        public int Count
        {
            get
            {
                if (this.m_move_==Move.Empty)
                {
                    return 0;
                }
                return 1;
            }
        }



        /// <summary>
        /// 次の局面への全ての候補手
        /// </summary>
        private Move m_move_;
        private MoveNode m_moveNode_;

        public bool HasChildNode(Move key)
        {
            return this.m_move_ == key && key != Move.Empty;
        }
        public bool ContainsKey(Move key)
        {
            return this.HasChildNode(key);
        }
        public void ClearAll()
        {
            this.m_move_ = Move.Empty;
            this.m_moveNode_ = null;
        }
        public void AddItem(Move move, MoveNode newNode, MoveNode parent)
        {
            this.m_move_ = move;
            this.m_moveNode_ = newNode;
            newNode.SetParentNode(parent);
        }
        /// <summary>
        /// 既存の子要素を上書きします。
        /// </summary>
        /// <param name="existsNode"></param>
        public void ChangeItem(
            MoveNode existsNode,
            MoveNode parent
            )
        {
            this.m_move_ = existsNode.Key;
            this.m_moveNode_ = existsNode;
            existsNode.SetParentNode(parent);
        }
        /// <summary>
        /// 子ノードの削除。
        /// </summary>
        /// <param name="key"></param>
        public bool RemoveItem(Move key)
        {
            if (this.m_move_==key)
            {
                this.m_move_ = Move.Empty;
                this.m_moveNode_ = null;
                return true;
            }
            return false;
        }




        public List<Move> ToMovelist()
        {
            List<Move> movelist = new List<Move>();

            if(this.m_move_ != Move.Empty)
            {
                movelist.Add(this.m_move_);
            }

            return movelist;
        }
    }
}
