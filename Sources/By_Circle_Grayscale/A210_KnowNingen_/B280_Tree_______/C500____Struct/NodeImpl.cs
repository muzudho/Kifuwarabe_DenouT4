using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B600_UtilSky____.C500____Util;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{

    /// <summary>
    /// ノードです。
    /// </summary>
    public class NodeImpl : Node
    {
        public NodeImpl(Move move, Sky sky)
        {
            this.MoveEx = new MoveExImpl(move);

            this.SetParentNode(null);
            this.teSasite = move;
            this.Value = sky;
            this.NextNodes = new Dictionary<Move, Node>();
        }

        public MoveEx MoveEx { get; set; }



        /// <summary>
        /// 配列型。[0]平手局面、[1]１手目の局面……。リンクリスト→ツリー構造の順に移行を進めたい。
        /// </summary>
        public Sky Value { get; set; }

        public Node GetParentNode()
        {
            return this.parentNode;
        }
        public void SetParentNode(Node parent)
        {
            this.parentNode = parent;
        }
        private Node parentNode;


        /// <summary>
        /// ルート・ノードなら真。
        /// </summary>
        /// <returns></returns>
        public bool IsRoot()
        {
            return this.GetParentNode() == null;
        }


        public Move Key
        {
            get
            {
                return this.teSasite;
            }
        }
        private Move teSasite;

        /// <summary>
        /// 次の局面への全ての候補手
        /// </summary>
        protected Dictionary<Move, Node> NextNodes { get; set; }
        public delegate void DELEGATE_NextNodes(Move key, Node node, ref bool toBreak);

        public bool HasChildNode(Move key)
        {
            return this.NextNodes.ContainsKey(key);
        }

        public Node GetChildNode(Move key)
        {
            return this.NextNodes[key];
        }


        public void Foreach_ChildNodes(NodeImpl.DELEGATE_NextNodes delegate_NextNodes)
        {
            bool toBreak = false;

            foreach (KeyValuePair<Move, Node> entry in this.NextNodes)
            {
                delegate_NextNodes(entry.Key, entry.Value, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }

        public void Clear_ChildNodes()
        {
            this.NextNodes.Clear();
        }

        public bool ContainsKey_ChildNodes(Move key)
        {
            return this.NextNodes.ContainsKey(key);
        }

        public void PutAdd_ChildNode(Move key,Node newNode)
        {
            this.NextNodes.Add(key,newNode);
            newNode.SetParentNode( this);
        }

        public void Set_ChildNodes(Dictionary<Move, Node> newNextNodes)
        {
            this.NextNodes = newNextNodes;
            foreach(Node child in this.NextNodes.Values)
            {
                child.SetParentNode( this);
            }
        }

        public int Count_ChildNodes
        {
            get
            {
                return this.NextNodes.Count;
            }
        }



        /// <summary>
        /// 子ノードの削除。
        /// </summary>
        /// <param name="key"></param>
        public bool RemoveChild(Move key)
        {
            return this.NextNodes.Remove(key);
        }

        /// <summary>
        /// この木の、全てのノード数を数えます。
        /// </summary>
        /// <returns></returns>
        public int CountAllNodes()
        {
            int result = 0;

            result += 1;

            foreach(Node child in this.NextNodes.Values)
            {
                result += child.CountAllNodes();
            }

            return result;
        }






        public bool IsLeaf { get { return 0 == this.Count_ChildNodes; } }

        public bool HasTuginoitte(
    Move key
    )
        {
            return this.ContainsKey_ChildNodes(key);
        }

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
            Node newNode
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

            this.PutAdd_ChildNode(newNode.Key, newNode);
            //手番はここでは変更できない。

            newNode.SetParentNode(this);
        }
        /// <summary>
        /// 既存の子要素を上書きします。
        /// </summary>
        /// <param name="existsNode"></param>
        public void PutTuginoitte_Override(
            Node existsNode
            )
        {
            // SFENをキーに、次ノードを増やします。
            this.NextNodes[existsNode.Key] = existsNode;
            existsNode.SetParentNode(this);
        }


        public string Json_NextNodes_MultiSky(
    string memo,
    string hint,
    int temezumi_yomiGenTeban_forLog,//読み進めている現在の手目済
    KwLogger errH)
        {
            StringBuilder sb = new StringBuilder();

            this.Foreach_ChildNodes((Move move, Node node, ref bool toBreak) =>
            {
                sb.AppendLine(Util_Sky307.Json_1Sky(
                    node.Value,
                    memo + "：" + Conv_Move.ToSfen(move),
                    hint + "_SF解1",
                    temezumi_yomiGenTeban_forLog
                    ));// 局面をテキストで作成
            });

            return sb.ToString();
        }


        ///// <summary>
        ///// この木の、全てのノードを、フォルダーとして作成します。
        ///// </summary>
        ///// <returns></returns>
        //public void CreateAllFolders(string folderpath, int currentDeep, int limitDeep)
        //{
        //    try
        //    {
        //        if (null == this.GetParentNode())
        //        {
        //            // ルートノードはスルー。
        //        }
        //        else
        //        {
        //            folderpath = folderpath + "/" + Conv_SasiteStr_Sfen.ToSasiteStr_Sfen_ForFilename(this.Key);
        //            Directory.CreateDirectory(folderpath);
        //        }

        //        if (currentDeep <= limitDeep)
        //        {
        //            this.Foreach_ChildNodes((string key, Node<Move, Sky> node, ref bool toBreak) =>
        //            {
        //                ((KifuNode)node).CreateAllFolders(
        //                    folderpath,
        //                    currentDeep + 1,
        //                    limitDeep);
        //            });
        //        }
        //    }
        //    catch(PathTooLongException ex)
        //    {
        //        System.Console.WriteLine("パスが長すぎた☆無視して続行☆：" + ex.Message + "\n folderpath=[" + folderpath + "]");
        //    }
        //}

    }
}
