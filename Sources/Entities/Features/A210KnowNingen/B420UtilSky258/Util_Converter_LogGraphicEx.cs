﻿using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class Util_Converter_LogGraphicEx
    {


        /// <summary>
        /// 駒画像のファイル名。
        /// </summary>
        /// <param name="pside"></param>
        /// <param name="ks14"></param>
        /// <param name="extentionWithDot"></param>
        /// <returns></returns>
        public static string PsideKs14_ToString(Playerside pside, Komasyurui14 ks14, string extentionWithDot)
        {
            string komaImg;

            if (pside == Playerside.P1)
            {
                switch (ks14)
                {
                    case Komasyurui14.H01_Fu_____: komaImg = "fu" + extentionWithDot; break;
                    case Komasyurui14.H02_Kyo____: komaImg = "kyo" + extentionWithDot; break;
                    case Komasyurui14.H03_Kei____: komaImg = "kei" + extentionWithDot; break;
                    case Komasyurui14.H04_Gin____: komaImg = "gin" + extentionWithDot; break;
                    case Komasyurui14.H05_Kin____: komaImg = "kin" + extentionWithDot; break;
                    case Komasyurui14.H06_Gyoku__: komaImg = "oh" + extentionWithDot; break;
                    case Komasyurui14.H07_Hisya__: komaImg = "hi" + extentionWithDot; break;
                    case Komasyurui14.H08_Kaku___: komaImg = "kaku" + extentionWithDot; break;
                    case Komasyurui14.H09_Ryu____: komaImg = "ryu" + extentionWithDot; break;
                    case Komasyurui14.H10_Uma____: komaImg = "uma" + extentionWithDot; break;
                    case Komasyurui14.H11_Tokin__: komaImg = "tokin" + extentionWithDot; break;
                    case Komasyurui14.H12_NariKyo: komaImg = "narikyo" + extentionWithDot; break;
                    case Komasyurui14.H13_NariKei: komaImg = "narikei" + extentionWithDot; break;
                    case Komasyurui14.H14_NariGin: komaImg = "narigin" + extentionWithDot; break;
                    default: komaImg = "batu" + extentionWithDot; break;
                }
            }
            else
            {
                switch (ks14)
                {
                    case Komasyurui14.H01_Fu_____: komaImg = "fuV" + extentionWithDot; break;
                    case Komasyurui14.H02_Kyo____: komaImg = "kyoV" + extentionWithDot; break;
                    case Komasyurui14.H03_Kei____: komaImg = "keiV" + extentionWithDot; break;
                    case Komasyurui14.H04_Gin____: komaImg = "ginV" + extentionWithDot; break;
                    case Komasyurui14.H05_Kin____: komaImg = "kinV" + extentionWithDot; break;
                    case Komasyurui14.H06_Gyoku__: komaImg = "ohV" + extentionWithDot; break;
                    case Komasyurui14.H07_Hisya__: komaImg = "hiV" + extentionWithDot; break;
                    case Komasyurui14.H08_Kaku___: komaImg = "kakuV" + extentionWithDot; break;
                    case Komasyurui14.H09_Ryu____: komaImg = "ryuV" + extentionWithDot; break;
                    case Komasyurui14.H10_Uma____: komaImg = "umaV" + extentionWithDot; break;
                    case Komasyurui14.H11_Tokin__: komaImg = "tokinV" + extentionWithDot; break;
                    case Komasyurui14.H12_NariKyo: komaImg = "narikyoV" + extentionWithDot; break;
                    case Komasyurui14.H13_NariKei: komaImg = "narikeiV" + extentionWithDot; break;
                    case Komasyurui14.H14_NariGin: komaImg = "nariginV" + extentionWithDot; break;
                    default: komaImg = "batu" + extentionWithDot; break;
                }
            }

            return komaImg;
        }


        /// <summary>
        /// 駒画像のファイル名。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="finger"></param>
        /// <param name="extentionWithDot"></param>
        /// <returns></returns>
        public static string Finger_ToString(IPosition src_Sky, Finger finger, string extentionWithDot)
        {
            string komaImg = "";

            if ((int)finger < Finger_Honshogi.Items_KomaOnly.Length)
            {
                src_Sky.AssertFinger(finger);
                Busstop koma = src_Sky.BusstopIndexOf(finger);

                Playerside pside = Conv_Busstop.ToPlayerside(koma);
                Komasyurui14 ks14 = Conv_Busstop.ToKomasyurui(koma);

                komaImg = Util_Converter_LogGraphicEx.PsideKs14_ToString(pside, ks14, extentionWithDot);
            }
            else
            {
                komaImg = Util_Converter_LogGraphicEx.PsideKs14_ToString(Playerside.Empty, Komasyurui14.H00_Null___, extentionWithDot);
            }

            return komaImg;
        }


    }
}
