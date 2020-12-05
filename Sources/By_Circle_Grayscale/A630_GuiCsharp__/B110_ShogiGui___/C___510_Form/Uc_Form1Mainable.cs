using System.Drawing;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B310Settei.L510Xml;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form
{
    public interface Uc_Form1Mainable
    {
        Color BackColor { get; set; }

        Form1_Mutex MutexOwner { get; set; }

        void Solute_RepaintRequest(
            Form1_Mutex mutex, MainGui_Csharp mainGui, KwLogger errH);

        MainGui_Csharp MainGui { get; }

        SetteiXmlFile SetteiXmlFile { get; }
    }
}
