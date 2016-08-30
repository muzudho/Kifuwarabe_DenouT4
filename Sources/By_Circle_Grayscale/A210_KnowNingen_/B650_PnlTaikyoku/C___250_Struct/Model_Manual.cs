using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;

namespace Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct
{
    public interface Model_Manual
    {
        void SetGuiSky(SkyImpl sky);

        int GuiTemezumi { get; set; }

        SkyImpl GuiSkyConst { get; }

        Playerside GuiPside { get; set; }
    }
}
