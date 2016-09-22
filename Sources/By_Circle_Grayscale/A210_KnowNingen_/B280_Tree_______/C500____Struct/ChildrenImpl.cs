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

        public void AddItem(Move key, MoveNode newNode, KifuNode parent)
        {
            this.Items.Add(key, newNode);
            newNode.SetParentNode(parent);
        }

        public void SetItems_Old(Dictionary<Move, MoveNode> newItems, KifuNode parent)
        {
            this.Items = newItems;
            foreach (KifuNode child in this.Items.Values)
            {
                child.SetParentNode(parent);
            }
        }
        /// <summary>
        /// 棋譜ノードのValueは廃止方針☆
        /// </summary>
        /// <param name="moves"></param>
        /// <param name="parent"></param>
        public void SetItems_New(List<Move> moves, KifuNode parent)
        {
            this.Items.Clear();
            foreach (Move move in moves)
            {
                KifuNode newNode = new MoveNodeImpl(move);
                newNode.SetParentNode(parent);
                this.Items.Add(move,newNode);
            }
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
        public void PutTuginoitte_New(
            KifuNode newNode,
            KifuNode owner
            )
        {
            // 同じ指し手があれば追加してはいけない？
#if DEBUG
            System.Diagnostics.Debug.Assert(
                !this.ContainsKey(newNode.Key),
                "指し手[" + Conv_Move.ToSfen(newNode.Key) + "]は既に指されていました。"
                );
#endif
            // SFENをキーに、次ノードを増やします。

            this.AddItem(newNode.Key, newNode, owner);
            //手番はここでは変更できない。

            newNode.SetParentNode(owner);
        }
        /// <summary>
        /// 既存の子要素を上書きします。
        /// </summary>
        /// <param name="existsNode"></param>
        public void PutTuginoitte_Override(
            KifuNode existsNode,
            KifuNode owner
            )
        {
            // SFENをキーに、次ノードを増やします。
            this.Items[existsNode.Key] = existsNode;
            existsNode.SetParentNode(owner);
        }
    }
}
