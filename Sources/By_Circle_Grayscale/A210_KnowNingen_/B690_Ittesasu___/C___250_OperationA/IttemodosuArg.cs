﻿using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;


namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA
{
    public interface IttemodosuArg
    {
        /// <summary>
        /// 指し手。棋譜に記録するために「指す前／指した後」を含めた手。
        /// </summary>
        Move Move { get; set; }


        /// <summary>
        /// これから作る局面の、手目済み。
        /// </summary>
        int KorekaranoTemezumi { get; }

    }
}
