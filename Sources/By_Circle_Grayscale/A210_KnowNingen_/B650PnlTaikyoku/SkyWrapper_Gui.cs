using Grayscale.A210KnowNingen.B270Sky.C500Struct;

namespace Grayscale.A210KnowNingen.B650PnlTaikyoku.C250Struct
{
    public interface SkyWrapper_Gui
    {
        void SetGuiSky(ISky sky);

        ISky GuiSky { get; }
    }
}
