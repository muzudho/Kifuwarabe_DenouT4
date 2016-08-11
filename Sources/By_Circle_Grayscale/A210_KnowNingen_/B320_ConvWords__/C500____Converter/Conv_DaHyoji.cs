using Grayscale.A210_KnowNingen_.B150_KifuJsa____.C500____Word;

namespace Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter
{
    public abstract class Conv_DaHyoji
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 打。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static string ToBool(DaHyoji da)
        {
            string daStr = "";

            if (DaHyoji.Visible == da)
            {
                daStr = "打";
            }

            return daStr;
        }

    }
}
