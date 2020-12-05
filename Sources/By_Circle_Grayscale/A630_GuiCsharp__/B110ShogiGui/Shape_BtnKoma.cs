using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C080Shape
{
    public interface Shape_BtnKoma : IShape
    {
        string WidgetName { get; }

        Finger Finger { get; set; }
        Finger Koma { get; set; }

    }
}
