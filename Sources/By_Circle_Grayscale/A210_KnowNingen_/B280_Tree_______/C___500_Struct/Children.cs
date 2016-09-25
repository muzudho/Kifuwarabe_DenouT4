using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
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

        void Foreach_ChildNodes2(ChildrenImpl.DELEGATE_ChildNodes2 delegate_NextNodes);
        void Foreach_ChildNodes3(ChildrenImpl.DELEGATE_ChildNodes3 delegate_NextNodes);
        void Foreach_ChildNodes4(ChildrenImpl.DELEGATE_ChildNodes4 delegate_NextNodes);
        void Foreach_ChildNodes5(ChildrenImpl.DELEGATE_ChildNodes5 delegate_NextNodes);

        /// <summary>
        /// 子ノードの削除。
        /// </summary>
        /// <param name="key"></param>
        bool RemoveItem(Move key);




        bool IsLeaf { get; }

    }
}
