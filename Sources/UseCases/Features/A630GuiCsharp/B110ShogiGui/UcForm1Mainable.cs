using System.Drawing;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public interface UcForm1Mainable
    {
        Color BackColor { get; set; }

        Form1_Mutex MutexOwner { get; set; }

        void Solute_RepaintRequest(
            Form1_Mutex mutex, MainGui_Csharp mainGui);

        MainGui_Csharp MainGui { get; }
    }
}
