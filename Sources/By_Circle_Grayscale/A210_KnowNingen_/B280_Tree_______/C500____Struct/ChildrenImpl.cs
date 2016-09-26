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
            this.Items = new Dictionary<Move, MoveNode>();
        }
        /// <summary>
        /// 棋譜ノードのValueは廃止方針☆
        /// </summary>
        /// <param name="moves"></param>
        /// <param name="parent"></param>
        public ChildrenImpl(List<Move> moves, MoveNode parent)
        {
            this.Items = new Dictionary<Move, MoveNode>();
            foreach (Move move in moves)
            {
                MoveNode newNode = new MoveNodeImpl(move);
                this.AddItem(move, newNode, parent);
            }
        }



        /// <summary>
        /// 次の局面への全ての候補手
        /// </summary>
        protected Dictionary<Move, MoveNode> Items { get; set; }
        public delegate void DELEGATE_ChildNodes2(Move key, List<Move> honpuList, ref bool toBreak);
        public delegate void DELEGATE_ChildNodes4(MoveEx key, ref bool toBreak);

        public bool HasChildNode(Move key)
        {
            return this.Items.ContainsKey(key);
        }


        public void ClearAll()
        {
            this.Items.Clear();
        }

        public bool ContainsKey(Move key)
        {
            return this.Items.ContainsKey(key);
        }

        public void AddItem(Move move, MoveNode newNode, MoveNode parent)
        {
            this.Items.Add(move, newNode);
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
            // SFENをキーに、次ノードを増やします。
            this.Items[existsNode.Key].SetParentNode(null);
            this.Items[existsNode.Key] = existsNode;
            existsNode.SetParentNode(parent);
        }

        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        /// <summary>
        /// 子ノードの削除。
        /// </summary>
        /// <param name="key"></param>
        public bool RemoveItem(Move key)
        {
            return this.Items.Remove(key);
        }


        public bool IsLeaf { get { return 0 == this.Count; } }


        public List<Move> ToMovelist()
        {
            List<Move> movelist = new List<Move>();

            foreach (KeyValuePair<Move, MoveNode> entry in this.Items)
            {
                movelist.Add(entry.Key);
            }

            return movelist;
        }
        public List<MoveEx> ToMoveExList()
        {
            List<MoveEx> moveExList = new List<MoveEx>();

            foreach (KeyValuePair<Move, MoveNode> entry in this.Items)
            {
                moveExList.Add(entry.Value.MoveEx);
            }

            return moveExList;
        }

    }
}
