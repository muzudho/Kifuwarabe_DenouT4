using Grayscale.A210KnowNingen.B150KifuJsa.C500Word;

namespace Grayscale.A210KnowNingen.B320ConvWords.C500Converter
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
