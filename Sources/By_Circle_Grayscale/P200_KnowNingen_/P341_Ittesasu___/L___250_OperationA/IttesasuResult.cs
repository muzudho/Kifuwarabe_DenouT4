﻿using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P226_Tree_______.L___500_Struct;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P335_Move_______.L___500_Struct;

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
