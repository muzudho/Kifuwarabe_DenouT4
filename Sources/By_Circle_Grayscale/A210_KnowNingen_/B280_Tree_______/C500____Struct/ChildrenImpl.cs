using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B600_UtilSky____.C500____Util;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using System.Text;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{
    public class ChildrenImpl : Children
    {
        public ChildrenImpl()
        {
            this.Items = new Dictionary<Move, Node>();
        }



        /// <summary>
        /// 次の局面への全ての候補手
        /// </summary>
        protected Dictionary<Move, Node> Items { get; set; }
        public delegate void DELEGATE_NextNodes(Move key, Node node, Sky sky, ref bool toBreak);

        public bool HasChildNode(Move key)
        {
            return this.Items.ContainsKey(key);
        }

        public Node GetFirst()
        {
            return this.Items[0];
        }
        public Node GetChildNode(Move key)
        {
            return this.Items[key];
        }

        public void Foreach_ChildNodes(ChildrenImpl.DELEGATE_NextNodes delegate_NextNodes)
        {
            bool toBreak = false;

            foreach (KeyValuePair<Move, Node> entry in this.Items)
            {
                delegate_NextNodes(entry.Key, entry.Value, entry.Value.GetValue(), ref toBreak);

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

        public void AddItem(Move key, Node newNode, Node parent)
        {
            this.Items.Add(key, newNode);
            newNode.SetParentNode(parent);
        }

        public void SetItems(Dictionary<Move, Node> newItems, Node parent)
        {
            this.Items = newItems;
            foreach (Node child in this.Items.Values)
            {
                child.SetParentNode(parent);
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
            Node newNode,
            Node owner
            )
        {
            // 同じ指し手があれば追加してはいけない？
#if DEBUG
            System.Diagnostics.Debug.Assert(
                !this.HasTuginoitte(newNode.Key),
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
            Node existsNode,
            Node owner
            )
        {
            // SFENをキーに、次ノードを増やします。
            this.Items[existsNode.Key] = existsNode;
            existsNode.SetParentNode(owner);
        }

        public string Json_NextNodes_MultiSky(
string memo,
string hint,
int temezumi_yomiGenTeban_forLog,//読み進めている現在の手目済
KwLogger errH)
        {
            StringBuilder sb = new StringBuilder();

            this.Foreach_ChildNodes((Move move, Node node, Sky sky, ref bool toBreak) =>
            {
                sb.AppendLine(Util_Sky307.Json_1Sky(
                    sky,
                    memo + "：" + Conv_Move.ToSfen(move),
                    hint + "_SF解1",
                    temezumi_yomiGenTeban_forLog
                    ));// 局面をテキストで作成
            });

            return sb.ToString();
        }

    }
}
