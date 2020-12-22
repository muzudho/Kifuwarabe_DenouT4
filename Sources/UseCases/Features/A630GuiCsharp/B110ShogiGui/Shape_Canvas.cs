using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public interface Shape_Canvas
    {

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="shogiGui"></param>
        /// <param name="logTag"></param>
        void Paint(
            object sender,
            PaintEventArgs e,
            Playerside psideA,
            ISky positionA,
            MainGui_Csharp shogiGui,
            string windowName,
            ILogTag logTag
        );

    }
}
