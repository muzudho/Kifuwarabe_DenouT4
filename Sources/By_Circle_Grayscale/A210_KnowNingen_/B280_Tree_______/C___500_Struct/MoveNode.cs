using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

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



        
        void Child_RemoveThis(Tree kifu1, KwLogger logger);
        void Child_SetChild(Move key, MoveNode newNode, Tree kifu1, KwLogger logger);
    }
}
