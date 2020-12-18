using System.Drawing;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B310Settei.L510Xml;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C510Form
{
    public interface UcForm1Mainable
    {
        Color BackColor { get; set; }

        Form1_Mutex MutexOwner { get; set; }

        void Solute_RepaintRequest(
            Form1_Mutex mutex, MainGui_Csharp mainGui, ILogTag errH);

        MainGui_Csharp MainGui { get; }

        SetteiXmlFile SetteiXmlFile { get; }
    }
}
