using System;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public delegate void DELEGATE_Form1_Load(MainGui_Csharp shogiGui, object sender, EventArgs e);

    public interface Form1Shogiable
    {
        UcForm1Mainable Uc_Form1Main { get; }

        DELEGATE_Form1_Load Delegate_Form1_Load { get; set; }
    }
}
