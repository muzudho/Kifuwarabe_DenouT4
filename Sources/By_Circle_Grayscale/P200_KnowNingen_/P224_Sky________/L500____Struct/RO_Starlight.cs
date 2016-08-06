using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P219_Move_______.L___500_Struct;

namespace Grayscale.P224_Sky________.L500____Struct
{

    /// <summary>
    /// リードオンリー駒位置
    /// 
    /// 動かない星の光。
    /// </summary>
    public class SingleBusstop : DoubleBusstopable
    {
        #region プロパティー類

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後、升、配役
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Busstop Now { get { return this.now; } }
        protected Busstop now;

        #endregion



        /// <summary>
        /// ************************************************************************************************************************
        /// 駒用。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="syurui"></param>
        public SingleBusstop(Busstop nowStar)
        {
            this.now = nowStar;
        }
    }
}
