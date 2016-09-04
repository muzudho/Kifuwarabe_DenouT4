using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;

namespace Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv
{
    /// <summary>
    /// 足し算、引き算をしたいときなどに使います。
    /// </summary>
    public abstract class Conv_MasuNum
    {
        /// <summary>
        /// 将棋盤、駒台に筋があります。番号は1～。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static string ToLog_FromBanjoMasu(SyElement masu)
        {
            int suji;
            if(!Conv_MasuNum.ToSuji_FromBanjoMasu(masu,out suji))
            {
                suji = -1;
            }

            int dan;
            if(!Conv_MasuNum.ToDan_FromBanjoMasu(masu, out dan))
            {
                dan = -1;
            }

            return suji + "," + dan;
        }

        #region 整数変換(基礎)

        /// <summary>
        /// 将棋盤、駒台に筋があります。番号は1～。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool ToSuji_FromBanjoMasu(SyElement masu, out int result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Conv_MasuNum.ToSuji_FromBanjoMasu(masuNumber, out result);
        }
        /// <summary>
        /// TODO: 廃止予定
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ToSuji_FromBangaiMasu(SyElement masu, out int result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Conv_MasuNum.ToSuji_FromBangaiMasu(masuNumber, out result);
        }
        /// <summary>
        /// TODO: 今後こちらに置き換えていく予定。まだ使えない。
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ToPiece_FromBangaiMasu(SyElement masu, out Pieces result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Conv_MasuNum.ToPiece_FromBangaiMasu(masuNumber, out result);
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
        public static bool ToSuji_FromBanjoMasu(int masuNumber, out int result)
        {
            result = (masuNumber - Conv_MasuNum.FirstMasu_Shogiban) / 9 + 1;
            return true;
        }
        /// <summary>
        /// TODO: 廃止予定。
        /// </summary>
        /// <param name="masuNumber"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ToSuji_FromBangaiMasu(int masuNumber, out int result)
        {
            bool successful = true;

            Okiba okiba = Conv_SyElement.ToOkiba(masuNumber);

            switch (okiba)
            {
                /*case Okiba.ShogiBan: result = (masuNumber - Util_MasuNum.FirstMasu_Shogiban) / 9 + 1; break;*/
                case Okiba.Sente_Komadai: result = (masuNumber - Conv_MasuNum.FirstMasu_SenteKomadai) / 10 + 1; break;
                case Okiba.Gote_Komadai: result = (masuNumber - Conv_MasuNum.FirstMasu_GoteKomadai) / 10 + 1; break;
                case Okiba.KomaBukuro: result = (masuNumber - Conv_MasuNum.FirstMasu_Komabukuro) / 10 + 1; break;
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
        /// TODO: これに置き換え予定。
        /// </summary>
        /// <param name="masuNumber"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ToPiece_FromBangaiMasu(int masuNumber, out Pieces result)
        {
            bool successful = true;

            Okiba okiba = Conv_SyElement.ToOkiba(masuNumber);

            switch (okiba)
            {
                // TODO: まだ使えない☆
                case Okiba.Sente_Komadai: result = (Pieces)(masuNumber - Conv_MasuNum.FirstMasu_SenteKomadai); break;
                case Okiba.Gote_Komadai: result = (Pieces)(masuNumber - Conv_MasuNum.FirstMasu_GoteKomadai); break;
                case Okiba.KomaBukuro: result = (Pieces)(masuNumber - Conv_MasuNum.FirstMasu_Komabukuro); break;
                default:
                    // エラー
                    result = Pieces.None;
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
        public static bool ToDan_FromBanjoMasu(SyElement masu, out int result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Conv_MasuNum.ToDan_FromBanjoMasu(masuNumber, out result);
        }
        public static bool ToDan_FromBangaiMasu(SyElement masu, out int result)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Conv_MasuNum.ToDan_FromBangaiMasu(masuNumber, out result);
        }

        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool ToDan_FromBanjoMasu(int masuNumber, out int result)
        {
            result = (masuNumber - Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.ShogiBan))) % 9 + 1;
            return true;
        }
        public static bool ToDan_FromBangaiMasu(int masuNumber, out int result)
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
