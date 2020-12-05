using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA
{
    public interface IIttemodosuResult
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
        ISky SyuryoSky { get; set; }
    }
}
