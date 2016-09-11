using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{

    /// <summary>
    /// ツリー構造です。
    /// 
    /// 局面を入れるのに利用します。
    /// 根ノードに平手局面、最初の子ノードに１手目の局面、を入れるような使い方を想定しています。
    /// </summary>
    public interface Node
    {

        /// <summary>
        /// このノードのキー。インスタンスを作った後では変更できません。
        /// </summary>
        Move Key { get; }

        /// <summary>
        /// このノードの値。
        /// </summary>
        Sky Value { get; set; }

        /// <summary>
        /// 親ノード。変更可能。
        /// </summary>
        Node GetParentNode();
        void SetParentNode(Node parent);

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
        Node GetChildNode(Move key);

        int Count_ChildNodes{get;}
        void Clear_ChildNodes();
        bool ContainsKey_ChildNodes(Move key);
        void PutAdd_ChildNode(Move key, Node newNode);
        void Set_ChildNodes(Dictionary<Move, Node> newNextNodes);

        void Foreach_ChildNodes(NodeImpl.DELEGATE_NextNodes d);

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






        bool IsLeaf { get; }
        bool HasTuginoitte(Move sasiteStr);

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
        void PutTuginoitte_New(Node newNode);
        /// <summary>
        /// 既存の子要素を上書きします。
        /// </summary>
        /// <param name="existsNode"></param>
        void PutTuginoitte_Override(Node existsNode);

        string Json_NextNodes_MultiSky(
    string memo,
    string hint,
    int temezumi_yomiGenTeban_forLog,//読み進めている現在の手目済
    KwLogger errH
    );

        MoveEx MoveEx { get; set; }

    }
}
