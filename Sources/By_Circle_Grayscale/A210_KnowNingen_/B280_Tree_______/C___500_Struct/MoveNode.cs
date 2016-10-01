using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
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



        
        bool Child_Exists { get; }
        void Child_Clear(Tree kifu1, KwLogger logger);
        void Child_SetChild(Move key, MoveNode newNode, Tree kifu1, KwLogger logger);

        /// <summary>
        /// 学習で使ってるだけ。
        /// </summary>
        /// <returns></returns>
        Move Child_GetItem(Tree kifu1);

        List<Move> ToPvList();
    }
}
