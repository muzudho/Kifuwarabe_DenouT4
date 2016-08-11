using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;

namespace Grayscale.P341_Ittesasu___.L___250_OperationA
{
    public interface IttesasuArg
    {
        /// <summary>
        /// 一手指し、開始局面。
        /// </summary>
        KyokumenWrapper KaisiKyokumen{get;set;}

        /// <summary>
        /// 一手指し、開始局面、手番。
        /// </summary>
        Playerside KaisiTebanside { get; set; }

        /// <summary>
        /// 一手指し、終了局面。これから指されるはずの手。棋譜に記録するために「指す前／指した後」を含めた手。
        /// </summary>
        Move KorekaranoMove { get; }// set;

        /// <summary>
        /// これから作る局面の、手目済み。
        /// </summary>
        int KorekaranoTemezumi_orMinus1{get;}
    }
}
