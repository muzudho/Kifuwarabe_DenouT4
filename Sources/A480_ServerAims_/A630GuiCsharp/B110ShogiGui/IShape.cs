using System.Drawing;

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C080Shape
{
    public interface IShape
    {

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 位置とサイズ
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Rectangle Bounds { get; }

        void SetBounds(Rectangle rect);

    }
}
