using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P341_Ittesasu___.L___250_OperationA
{
    public interface IttesasuResult
    {
        /// <summary>
        /// 指した駒の番号。
        /// </summary>
        Finger FigMovedKoma{get;set;}

        /// <summary>
        /// 取った駒があれば、取った駒の番号。
        /// </summary>
        Finger FigFoodKoma{get;set;}

        /// <summary>
        /// 取った駒があれば、取った駒の種類。
        /// </summary>
        Komasyurui14 FoodKomaSyurui { get; set; }

        /// <summary>
        /// 終了ノード。
        /// 「進む」ときは、一手指す局面の「指した後」のツリー・ノード。
        /// 「巻き戻す」のときは、ヌル。
        /// </summary>
        Node<Move, KyokumenWrapper> Get_SyuryoNode_OrNull { get; }
        Node<Move, KyokumenWrapper> Set_SyuryoNode_OrNull { set; }

        /// <summary>
        /// 終了ノードの局面データ。
        /// </summary>
        SkyConst Susunda_Sky_orNull { get; set; }

    }
}
