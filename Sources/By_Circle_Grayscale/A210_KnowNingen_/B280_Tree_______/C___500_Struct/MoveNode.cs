using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{
    /// <summary>
    /// ツリー構造です。
    /// 
    /// 根ノードに平手局面、最初の子ノードに１手目の局面、を入れるような使い方を想定しています。
    /// </summary>
    public interface MoveNode
    {
        /// <summary>
        /// このノードのキー。インスタンスを作った後では変更できません。
        /// </summary>
        Move Key { get; }
        float Score { get; set; }
        /// <summary>
        /// ルート・ノードなら真。
        /// </summary>
        /// <returns></returns>
        bool IsRoot();




        /// <summary>
        /// 親ノード。変更可能。
        /// </summary>
        MoveNode GetParentNode();
        void SetParentNode(MoveNode parent);



        
        bool Child_HasItem { get; }
        void Child_Clear();
        bool Child_ContainsKey(Move key);
        void Child_SetItem(Move key, MoveNode newNode);

        /// <summary>
        /// 学習で使ってるだけ。
        /// </summary>
        /// <returns></returns>
        Move Child_GetItem();

        List<Move> ToPvList();
    }
}
