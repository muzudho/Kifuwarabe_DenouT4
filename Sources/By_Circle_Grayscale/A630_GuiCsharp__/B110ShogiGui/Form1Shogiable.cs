using System;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C510Form
{
    public delegate void DELEGATE_Form1_Load(MainGui_Csharp shogiGui, object sender, EventArgs e);

    public interface Form1Shogiable
    {
        Uc_Form1Mainable Uc_Form1Main { get; }

        DELEGATE_Form1_Load Delegate_Form1_Load { get; set; }
    }
}
