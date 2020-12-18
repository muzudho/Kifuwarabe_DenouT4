using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA
{
    public class IttemodosuResultImpl : IIttemodosuResult
    {
        public Finger FigMovedKoma { get; set; }

        public Finger FigFoodKoma { get; set; }

        public ISky SyuryoSky { get; set; }

        public Komasyurui14 FoodKomaSyurui { get; set; }

        public IttemodosuResultImpl(
            Finger figMovedKoma,
            Finger figFoodKoma,
            ISky syuryoSky,
            Komasyurui14 foodKomaSyurui
            )
        {
            this.FigMovedKoma = figMovedKoma;
            this.FigFoodKoma = figFoodKoma;
            this.SyuryoSky = syuryoSky;
            this.FoodKomaSyurui = foodKomaSyurui;
        }
    }
}
