using Grayscale.A210_KnowNingen_.B150_KifuJsa____.C500____Word;

namespace Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter
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
