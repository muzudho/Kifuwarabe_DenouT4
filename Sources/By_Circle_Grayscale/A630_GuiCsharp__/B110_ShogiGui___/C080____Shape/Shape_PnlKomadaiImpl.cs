using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___492_Widgets;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using System.Drawing;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C080____Shape
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。駒台、駒袋を描きます。
    /// ************************************************************************************************************************
    /// </summary>
    public class Shape_PnlKomadaiImpl : Shape_Abstract, Shape_PnlKomadai
    {


        #region プロパティー

        /// <summary>
        /// ウィジェットの一覧にアクセスするために利用。
        /// </summary>
        private MainGui_Csharp ShogibanGui { get; set; }

        public Okiba Okiba { get; set; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// １升の横幅。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int MasuWidth
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// １升の縦幅。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int MasuHeight
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// このエリアの、最初の升番号。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int FirstMasuHandle { get; set; }

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Shape_PnlKomadaiImpl(string widgetName, Okiba okiba, int x, int y, int firstMasuHandle, MainGui_Csharp shogibanGui)
            : base(widgetName, x, y, 1, 1)
        {
            this.ShogibanGui = shogibanGui;
            this.Okiba = okiba;
            this.MasuWidth = 40;
            this.MasuHeight = 40;
            this.FirstMasuHandle = firstMasuHandle;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 筋を指定すると、ｘ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        public int SujiToX( int suji)
        {
            return (Conv_Masu.KOMADAI_LAST_SUJI - suji) * this.MasuWidth + this.Bounds.X;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 段を指定すると、ｙ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="dan"></param>
        /// <returns></returns>
        public int DanToY(int dan)
        {
            return (dan-1) * this.MasuHeight + this.Bounds.Y;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒台の描画は、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="g"></param>
        public void Paint( Graphics g)
        {
            if (!this.Visible)
            {
                goto gt_EndMethod;
            }

            //----------
            // 背景色
            //----------
            g.FillRectangle(
                new SolidBrush(Color.Beige),
                new Rectangle(this.Bounds.X, this.Bounds.Y, Conv_Masu.KOMADAI_LAST_SUJI * this.MasuWidth, Conv_Masu.KOMADAI_LAST_DAN * this.MasuHeight)
                );

            //----------
            // 水平線
            //----------
            for (int i = 0; i <= Conv_Masu.KOMADAI_LAST_DAN; i++)
            {
                g.DrawLine(Pens.Black,
                                0 * this.MasuWidth + this.Bounds.X,
                                i * this.MasuHeight + this.Bounds.Y,
                    Conv_Masu.KOMADAI_LAST_SUJI * this.MasuWidth + this.Bounds.X,
                                i * this.MasuHeight + this.Bounds.Y
                    );
            }

            //----------
            // 垂直線
            //----------
            for (int i = 0; i <= Conv_Masu.KOMADAI_LAST_SUJI; i++)
            {
                g.DrawLine(Pens.Black,
                                i * this.MasuWidth + this.Bounds.X,
                                0 * this.MasuHeight + this.Bounds.Y,
                                i * this.MasuHeight + this.Bounds.X,
                    Conv_Masu.KOMADAI_LAST_DAN * this.MasuHeight + this.Bounds.Y
                    );
            }

            //----------
            // 升目
            //----------
            foreach (UserWidget widget in this.ShogibanGui.Widgets.Values)
            {
                if ("Masu" == widget.Type && widget.Okiba == this.Okiba)
                {
                    Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                    cell.Kiki = false;
                    cell.KikiSu = 0;
                    cell.Paint(g);
                }
            }

        gt_EndMethod:
            ;
        }


    }
}
