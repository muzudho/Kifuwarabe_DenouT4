﻿using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

namespace Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter
{
    public abstract class Conv_SyElement
    {

        public static int ToMasuNumber(SyElement syElm)
        {
            int result;

            if (syElm is New_Basho)
            {
                result = ((New_Basho)syElm).MasuNumber;
            }
            else
            {
                result = Masu_Honshogi.nError;
            }

            return result;
        }

        public static Okiba ToOkiba(SyElement masu)
        {
            int masuNumber = Conv_SyElement.ToMasuNumber(masu);
            return Conv_SyElement.ToOkiba(masuNumber);
        }

        public static Okiba ToOkiba(int masuNumber)
        {
            Okiba result;

            if ((int)Masu_Honshogi.nban11_１一 <= masuNumber && masuNumber <= (int)Masu_Honshogi.nban99_９九)
            {
                // 将棋盤
                result = Okiba.ShogiBan;
            }
            else if ((int)Masu_Honshogi.nsen01 <= masuNumber && masuNumber <= (int)Masu_Honshogi.nsen40)
            {
                // 先手駒台
                result = Okiba.Sente_Komadai;
            }
            else if ((int)Masu_Honshogi.ngo01 <= masuNumber && masuNumber <= (int)Masu_Honshogi.ngo40)
            {
                // 後手駒台
                result = Okiba.Gote_Komadai;
            }
            else if ((int)Masu_Honshogi.nfukuro01 <= masuNumber && masuNumber <= (int)Masu_Honshogi.nfukuro40)
            {
                // 駒袋
                result = Okiba.KomaBukuro;
            }
            else
            {
                // 該当なし
                result = Okiba.Empty;
            }

            return result;
        }
    }
}
