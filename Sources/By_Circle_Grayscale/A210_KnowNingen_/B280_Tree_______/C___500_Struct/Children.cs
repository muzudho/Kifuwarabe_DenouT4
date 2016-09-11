using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{
    public interface Children
    {
        bool HasChildNode(Move key);
        Node GetFirst();
        /// <summary>
        /// sfenがキーなのは暫定。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Node GetChildNode(Move key);

        int Count { get; }
        void ClearAll();
        bool ContainsKey(Move key);
        void AddItem(Move key, Node newNode, Node parent);
        void SetItem(Dictionary<Move, Node> newNextNodes, Node parent);

        void Foreach_ChildNodes(ChildrenImpl.DELEGATE_NextNodes d);

        /// <summary>
        /// 子ノードの削除。
        /// </summary>
        /// <param name="key"></param>
        bool RemoveItem(Move key);




        bool IsLeaf { get; }

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
        void PutTuginoitte_New(Node newNode, Node owner);
        /// <summary>
        /// 既存の子要素を上書きします。
        /// </summary>
        /// <param name="existsNode"></param>
        void PutTuginoitte_Override(Node existsNode, Node owner);

        string Json_NextNodes_MultiSky(
    string memo,
    string hint,
    int temezumi_yomiGenTeban_forLog,//読み進めている現在の手目済
    KwLogger errH
    );

    }
}
