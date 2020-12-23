using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public class IttesasuResultImpl : IIttesasuResult
    {
        public Finger FigMovedKoma { get; set; }

        public Finger FigFoodKoma { get; set; }

        public IPosition SyuryoKyokumenW { get; set; }

        public Komasyurui14 FoodKomaSyurui { get; set; }


        public IttesasuResultImpl(
            Finger figMovedKoma,
            Finger figFoodKoma,
            IPosition syuryoKyokumenW,
            Komasyurui14 foodKomaSyurui
            )
        {
            this.FigMovedKoma = figMovedKoma;
            this.FigFoodKoma = figFoodKoma;
            this.SyuryoKyokumenW = syuryoKyokumenW;
            this.FoodKomaSyurui = foodKomaSyurui;
        }

    }
}
