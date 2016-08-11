using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape
{
    public interface Shape_BtnKoma : Shape
    {
        string WidgetName { get; }

        Finger Finger { get; set; }
        Finger Koma { get; set; }

    }
}
