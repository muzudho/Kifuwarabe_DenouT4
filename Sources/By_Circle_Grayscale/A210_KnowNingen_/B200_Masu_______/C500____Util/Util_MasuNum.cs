using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B200_Masu_______.C500____Util
{
    /// <summary>
    /// 足し算、引き算をしたいときなどに使います。
    /// </summary>
    public abstract class Util_MasuNum
    {

        #region 整数変換(基礎)

        /// <summary>
        /// 将棋盤、駒台に筋があります。番号は1～。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool TryBanjoMasuToSuji(SyElement masu, out int result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Util_MasuNum.TryBanjoMasuToSuji(masuNumber, out result);
        }
        public static bool TryBangaiMasuToSuji(SyElement masu, out int result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Util_MasuNum.TryBangaiMasuToSuji(masuNumber, out result);
        }

        public static int FirstMasu_Shogiban { get { return Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.ShogiBan)); } }
        public static int FirstMasu_SenteKomadai { get { return Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.Sente_Komadai)); } }
        public static int FirstMasu_GoteKomadai { get { return Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.Gote_Komadai)); } }
        public static int FirstMasu_Komabukuro { get { return Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.KomaBukuro)); } }

        /// <summary>
        /// 将棋盤、駒台に筋があります。番号は1～。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool TryBanjoMasuToSuji(int masuNumber, out int result)
        {
            result = (masuNumber - Util_MasuNum.FirstMasu_Shogiban) / 9 + 1;
            return true;
        }
        public static bool TryBangaiMasuToSuji(int masuNumber, out int result)
        {
            bool successful = true;

            Okiba okiba = Conv_SyElement.ToOkiba(masuNumber);

            switch (okiba)
            {
                /*case Okiba.ShogiBan: result = (masuNumber - Util_MasuNum.FirstMasu_Shogiban) / 9 + 1; break;*/
                case Okiba.Sente_Komadai: result = (masuNumber - Util_MasuNum.FirstMasu_SenteKomadai) / 10 + 1; break;
                case Okiba.Gote_Komadai: result = (masuNumber - Util_MasuNum.FirstMasu_GoteKomadai) / 10 + 1; break;
                case Okiba.KomaBukuro: result = (masuNumber - Util_MasuNum.FirstMasu_Komabukuro) / 10 + 1; break;
                default:
                    // エラー
                    result = -1;
                    successful = false;
                    goto gt_EndMethod;
            }

            gt_EndMethod:
            return successful;
        }

        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool TryBanjoMasuToDan(SyElement masu, out int result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Util_MasuNum.TryBanjoMasuToDan(masuNumber, out result);
        }
        public static bool TryBangaiMasuToDan(SyElement masu, out int result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Util_MasuNum.TryBangaiMasuToDan(masuNumber, out result);
        }
        public static bool TryDokokaMasuToDan(SyElement masu, out int result)
        {
            //Okiba okiba = Conv_SyElement.ToOkiba(Conv_SyElement.ToMasuNumber(masu));

            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            Okiba okiba = Conv_SyElement.ToOkiba(masuNumber);

            if (okiba==Okiba.ShogiBan)
            {
                return Util_MasuNum.TryBanjoMasuToDan(masuNumber, out result);
            }
            return Util_MasuNum.TryBangaiMasuToDan(masuNumber, out result);
        }

        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool TryBanjoMasuToDan(int masuNumber, out int result)
        {
            result = (masuNumber - Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.ShogiBan))) % 9 + 1;
            return true;
        }
        public static bool TryBangaiMasuToDan(int masuNumber, out int result)
        {
            bool successful = true;

            Okiba okiba = Conv_SyElement.ToOkiba(masuNumber);

            switch (okiba)
            {
                /*
                case Okiba.ShogiBan:
                    result = (masuNumber - Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.ShogiBan))) % 9 + 1;
                    break;
                */
                case Okiba.Sente_Komadai:
                    result = (masuNumber - Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.Sente_Komadai))) % 10 + 1;
                    break;

                case Okiba.Gote_Komadai:
                    result = (masuNumber - Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.Gote_Komadai))) % 10 + 1;
                    break;

                case Okiba.KomaBukuro:
                    result = (masuNumber - Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.KomaBukuro))) % 10 + 1;
                    break;

                default:
                    // エラー
                    result = -1;
                    successful = false;
                    goto gt_EndMethod;
            }

            gt_EndMethod:
            return successful;
        }

        #endregion
    }
}
