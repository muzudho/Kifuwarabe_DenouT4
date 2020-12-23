using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface SkyWrapper_Gui
    {
        void SetGuiSky(IPosition sky);

        IPosition GuiSky { get; }
    }
}
