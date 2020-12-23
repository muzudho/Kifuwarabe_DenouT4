using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public class IttemodosuResultImpl : IIttemodosuResult
    {
        public Finger FigMovedKoma { get; set; }

        public Finger FigFoodKoma { get; set; }

        public IPosition SyuryoSky { get; set; }

        public Komasyurui14 FoodKomaSyurui { get; set; }

        public IttemodosuResultImpl(
            Finger figMovedKoma,
            Finger figFoodKoma,
            IPosition syuryoSky,
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
