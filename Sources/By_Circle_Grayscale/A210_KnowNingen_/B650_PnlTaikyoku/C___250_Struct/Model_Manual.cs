﻿using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct
{
    public interface Model_Manual
    {
        void SetGuiSky(Sky sky);

        int GuiTemezumi { get; set; }

        Sky GuiSky { get; }

        Playerside GuiPside { get; set; }
    }
}
