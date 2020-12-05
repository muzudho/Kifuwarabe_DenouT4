using Grayscale.A210KnowNingen.B150KifuJsa.C500Word;

namespace Grayscale.A210KnowNingen.B320ConvWords.C500Converter
{
    public abstract class Conv_NariNarazu
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 成り
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="nari"></param>
        /// <returns></returns>
        public static string Nari_ToStr(NariNarazu nari)
        {
            string nariStr = "";

            switch (nari)
            {
                case NariNarazu.Nari:
                    nariStr = "成";
                    break;
                case NariNarazu.Narazu:
                    nariStr = "不成";
                    break;
                default:
                    break;
            }

            return nariStr;
        }

    }
}
