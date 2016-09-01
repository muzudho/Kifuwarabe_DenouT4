using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA
{
    public class IttesasuResultImpl : IttesasuResult
    {
        public Finger FigMovedKoma { get; set; }

        public Finger FigFoodKoma { get; set; }

        public Node<Move, KyokumenWrapper> Get_SyuryoNode_OrNull { get { return this.syuryoNode_OrNull; } }
        public Node<Move, KyokumenWrapper> Set_SyuryoNode_OrNull { set { this.syuryoNode_OrNull = value; } }
        private Node<Move, KyokumenWrapper> syuryoNode_OrNull;

        public Komasyurui14 FoodKomaSyurui{ get; set; }

        public Sky Susunda_Sky_orNull{ get; set; }

        public IttesasuResultImpl(
            Finger figMovedKoma,
            Finger figFoodKoma,
            Node<Move, KyokumenWrapper> syuryoNode_OrNull,
            Komasyurui14 foodKomaSyurui,
            Sky susunda_Sky_orNull
            )
        {
            this.FigMovedKoma = figMovedKoma;
            this.FigFoodKoma = figFoodKoma;
            this.syuryoNode_OrNull = syuryoNode_OrNull;
            this.FoodKomaSyurui = foodKomaSyurui;
            this.Susunda_Sky_orNull = susunda_Sky_orNull;
        }

    }
}
