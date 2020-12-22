namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface SkyWrapper_Gui
    {
        void SetGuiSky(ISky sky);

        ISky GuiSky { get; }
    }
}
