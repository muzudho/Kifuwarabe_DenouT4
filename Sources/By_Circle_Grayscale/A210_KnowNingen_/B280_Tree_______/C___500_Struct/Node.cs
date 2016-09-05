using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{

    /// <summary>
    /// ツリー構造です。
    /// 
    /// 局面を入れるのに利用します。
    /// 根ノードに平手局面、最初の子ノードに１手目の局面、を入れるような使い方を想定しています。
    /// </summary>
    public interface Node<T1,T2>
    {

        /// <summary>
        /// このノードのキー。インスタンスを作った後では変更できません。
        /// </summary>
        T1 Key { get; }

        /// <summary>
        /// このノードの値。
        /// </summary>
        T2 Value { get; set; }

        /// <summary>
        /// 親ノード。変更可能。
        /// </summary>
        Node<T1, T2> GetParentNode();
        void SetParentNode(Node<T1, T2> parent);

        /// <summary>
        /// ルート・ノードなら真。
        /// </summary>
        /// <returns></returns>
        bool IsRoot();

        bool HasChildNode(Move key);
        /// <summary>
        /// sfenがキーなのは暫定。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Node<T1, T2> GetChildNode(Move key);

        int Count_ChildNodes{get;}
        void Clear_ChildNodes();
        bool ContainsKey_ChildNodes(Move key);
        void PutAdd_ChildNode(Move key, Node<T1, T2> newNode);
        void Set_ChildNodes(Dictionary<Move, Node<T1, T2>> newNextNodes);

        void Foreach_ChildNodes(NodeImpl<T1, T2>.DELEGATE_NextNodes d);

        /// <summary>
        /// 子ノードの削除。
        /// </summary>
        /// <param name="key"></param>
        bool RemoveChild(Move key);

        /// <summary>
        /// この木の、全てのノード数を数えます。
        /// </summary>
        /// <returns></returns>
        int CountAllNodes();

    }
}
