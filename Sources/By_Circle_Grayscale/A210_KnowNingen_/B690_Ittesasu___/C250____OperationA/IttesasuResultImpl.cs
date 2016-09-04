using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA
{
    public class IttesasuResultImpl : IttesasuResult
    {
        public Finger FigMovedKoma { get; set; }

        public Finger FigFoodKoma { get; set; }

        public Move SyuryoMove { get; set; }
        public KyokumenWrapper SyuryoKyokumenW { get; set; }

        public Komasyurui14 FoodKomaSyurui{ get; set; }


        public IttesasuResultImpl(
            Finger figMovedKoma,
            Finger figFoodKoma,
            Move syuryoMove,
            KyokumenWrapper syuryoKyokumenW,
            Komasyurui14 foodKomaSyurui            
            )
        {
            this.FigMovedKoma = figMovedKoma;
            this.FigFoodKoma = figFoodKoma;
            this.SyuryoMove = syuryoMove;
            this.SyuryoKyokumenW = syuryoKyokumenW;
            this.FoodKomaSyurui = foodKomaSyurui;
        }

    }
}
