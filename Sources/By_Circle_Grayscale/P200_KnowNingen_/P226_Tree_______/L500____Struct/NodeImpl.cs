using Grayscale.P226_Tree_______.L___500_Struct;
using System.Collections.Generic;
using Grayscale.P219_Move_______.L___500_Struct;

namespace Grayscale.P226_Tree_______.L500____Struct
{

    /// <summary>
    /// ノードです。
    /// </summary>
    public class NodeImpl<T1, T2> : Node<T1, T2>
    {



        /// <summary>
        /// 配列型。[0]平手局面、[1]１手目の局面……。リンクリスト→ツリー構造の順に移行を進めたい。
        /// </summary>
        public T2 Value { get; set; }

        public Node<T1, T2> GetParentNode()
        {
            return this.parentNode;
        }
        public void SetParentNode(Node<T1, T2> parent)
        {
            this.parentNode = parent;
        }
        private Node<T1, T2> parentNode;


        /// <summary>
        /// ルート・ノードなら真。
        /// </summary>
        /// <returns></returns>
        public bool IsRoot()
        {
            return this.GetParentNode() == null;
        }


        public T1 Key
        {
            get
            {
                return this.teSasite;
            }
        }
        private T1 teSasite;

        /// <summary>
        /// 次の局面への全ての候補手
        /// </summary>
        protected Dictionary<Move, Node<T1, T2>> NextNodes { get; set; }
        public delegate void DELEGATE_NextNodes(Move key, Node<T1, T2> node, ref bool toBreak);

        public bool HasChildNode(Move key)
        {
            return this.NextNodes.ContainsKey(key);
        }

        public Node<T1, T2> GetChildNode(Move key)
        {
            return this.NextNodes[key];
        }


        public void Foreach_ChildNodes(NodeImpl<T1, T2>.DELEGATE_NextNodes delegate_NextNodes)
        {
            bool toBreak = false;

            foreach (KeyValuePair<Move, Node<T1, T2>> entry in this.NextNodes)
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

        public void PutAdd_ChildNode(Move key,Node<T1, T2> newNode)
        {
            this.NextNodes.Add(key,newNode);
            newNode.SetParentNode( this);
        }

        public void PutSet_ChildNodes(Dictionary<Move, Node<T1, T2>> newNextNodes)
        {
            this.NextNodes = newNextNodes;
            foreach(Node<T1,T2> child in this.NextNodes.Values)
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


        public NodeImpl(T1 teSasite, T2 sky)
        {
            this.SetParentNode( null);
            this.teSasite = teSasite;
            this.Value = sky;
            this.NextNodes = new Dictionary<Move, Node<T1, T2>>();
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

            foreach(Node<T1,T2> child in this.NextNodes.Values)
            {
                result += child.CountAllNodes();
            }

            return result;
        }

    }


}
