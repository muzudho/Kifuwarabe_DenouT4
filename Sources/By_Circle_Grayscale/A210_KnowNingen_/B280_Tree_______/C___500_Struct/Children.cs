using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{
    public interface Children
    {
        bool HasChildNode(Move key);

        int Count { get; }
        void ClearAll();
        bool ContainsKey(Move key);
        void AddItem(Move key, MoveNode newNode, MoveNode parent);
        /// <summary>
        /// 既存の子要素を上書きします。
        /// </summary>
        /// <param name="existsNode"></param>
        void ChangeItem(MoveNode existsNode, MoveNode owner);

        /// <summary>
        /// 子ノードの削除。
        /// </summary>
        /// <param name="key"></param>
        bool RemoveItem(Move key);


        /// <summary>
        /// 学習で使ってるだけ。
        /// </summary>
        /// <returns></returns>
        List<Move> ToMovelist();
    }
}
