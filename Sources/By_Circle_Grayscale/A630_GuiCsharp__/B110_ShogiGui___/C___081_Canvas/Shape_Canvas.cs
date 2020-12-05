using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___081_Canvas
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
            KwLogger errH
        );

    }
}
