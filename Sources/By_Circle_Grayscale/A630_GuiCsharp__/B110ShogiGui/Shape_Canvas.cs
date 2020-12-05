using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C081Canvas
{
    public interface Shape_Canvas
    {

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="shogiGui"></param>
        /// <param name="errH"></param>
        void Paint(
            object sender,
            PaintEventArgs e,
            Playerside psideA,
            ISky positionA,
            MainGui_Csharp shogiGui,
            string windowName,
            ILogger errH
        );

    }
}
