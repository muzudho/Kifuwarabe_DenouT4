﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser
{
    public interface KifuParserA_State
    {

        string Execute(
            ref KifuParserA_Result result,
            Model_Taikyoku model_Taikyoku,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KwLogger errH,
            KwDisplayer kd
            );

    }
}
