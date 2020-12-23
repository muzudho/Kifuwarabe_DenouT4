using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{

    /// <summary>
    /// ウィジェットを描画する土台。
    /// </summary>
    public class ShapeCanvasImpl : Shape_Abstract, Shape_Canvas
    {

        public const string WINDOW_NAME_SHOGIBAN = "Shogiban";
        public const string WINDOW_NAME_CONSOLE = "Console";


        public ShapeCanvasImpl(string widgetName, int x, int y, int width, int height)
            : base(widgetName, x, y, width, height)
        {
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 対局の描画の一式は、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Paint(
            object sender,
            PaintEventArgs e,
            Playerside psideA,
            ISky positionA,//shogiGui.Link_Server.KifuTree.CurNode.GetNodeValue()
            MainGui_Csharp shogibanGui,
            string windowName)
        {
            //----------------------------------------
            // 登録ウィジェットの描画
            //----------------------------------------
            foreach (UserWidget widget in shogibanGui.Widgets.Values)
            {
                if (widget.Window == windowName)
                {
                    widget.Paint(e.Graphics);
                }
            }
        }

    }
}
