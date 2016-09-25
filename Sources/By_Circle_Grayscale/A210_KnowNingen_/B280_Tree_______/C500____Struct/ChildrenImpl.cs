using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B600_UtilSky____.C500____Util;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using System.Text;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;

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
        public delegate void DELEGATE_ChildNodes3(Move key, ref bool toBreak);
        public delegate void DELEGATE_ChildNodes4(MoveEx key, ref bool toBreak);
        public delegate void DELEGATE_ChildNodes5(Move key, ref bool toBreak);

        public bool HasChildNode(Move key)
        {
            return this.Items.ContainsKey(key);
        }

        public void Foreach_ChildNodes2(ChildrenImpl.DELEGATE_ChildNodes2 delegate_NextNodes)
        {
            bool toBreak = false;

            foreach (KeyValuePair<Move, MoveNode> entry in this.Items)
            {
                List<Move> pvList = Util_Tree.CreatePv2List(entry.Value);

                delegate_NextNodes(entry.Key, pvList, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }
        public void Foreach_ChildNodes3(ChildrenImpl.DELEGATE_ChildNodes3 delegate_NextNodes)
        {
            bool toBreak = false;

            foreach (KeyValuePair<Move, MoveNode> entry in this.Items)
            {
                delegate_NextNodes(entry.Key, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }
        public void Foreach_ChildNodes4(ChildrenImpl.DELEGATE_ChildNodes4 delegate_NextNodes)
        {
            bool toBreak = false;

            foreach (KeyValuePair<Move, MoveNode> entry in this.Items)
            {
                delegate_NextNodes(entry.Value.MoveEx, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }
        public void Foreach_ChildNodes5(ChildrenImpl.DELEGATE_ChildNodes5 delegate_NextNodes)
        {
            bool toBreak = false;

            foreach (KeyValuePair<Move, MoveNode> entry in this.Items)
            {
                delegate_NextNodes(entry.Key, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
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

    }
}
