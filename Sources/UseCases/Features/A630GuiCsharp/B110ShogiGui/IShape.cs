using System.Drawing;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
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
