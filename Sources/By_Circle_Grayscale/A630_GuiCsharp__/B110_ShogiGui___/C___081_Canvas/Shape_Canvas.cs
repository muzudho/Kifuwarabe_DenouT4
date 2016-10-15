using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using System.Windows.Forms;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

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
            Sky positionA,
            MainGui_Csharp shogiGui,
            string windowName,
            KwLogger errH
        );

    }
}
