using System.Drawing;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape
{
    public interface Shape
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
