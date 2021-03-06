﻿using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface IIttesasuResult
    {
        /// <summary>
        /// 指した駒の番号。
        /// </summary>
        Finger FigMovedKoma { get; set; }

        /// <summary>
        /// 取った駒があれば、取った駒の番号。
        /// </summary>
        Finger FigFoodKoma { get; set; }

        /// <summary>
        /// 取った駒があれば、取った駒の種類。
        /// </summary>
        Komasyurui14 FoodKomaSyurui { get; set; }

        /// <summary>
        /// 終了ノード。
        /// 「進む」ときは、一手指す局面の「指した後」のツリー・ノード。
        /// 「巻き戻す」のときは、ヌル。
        /// </summary>
        IPosition SyuryoKyokumenW { get; set; }
    }
}
