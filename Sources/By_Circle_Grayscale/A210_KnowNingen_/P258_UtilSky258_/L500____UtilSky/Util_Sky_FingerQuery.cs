using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P214_Masu_______.L500____Util;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P256_SeizaFinger.L250____Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P258_UtilSky258_.L500____UtilSky
{

    /// <summary>
    /// フィンガーを１つ求めるユーティリティーです。
    /// </summary>
    public abstract class Util_Sky_FingerQuery
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 指定の場所にある駒を返します。
        /// ************************************************************************************************************************
        /// 
        ///         先後は見ますが、将棋盤限定です。
        /// 
        /// </summary>
        /// <param name="okiba">置き場</param>
        /// <param name="masu">筋、段</param>
        /// <param name="uc_Main">メインパネル</param>
        /// <returns>駒。無ければヌル。</returns>
        public static Finger InMasuNow(SkyConst src_Sky, Playerside pside, SyElement masu, KwErrorHandler errH)
        {
            Finger foundKoma = Fingers.Error_1;

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                src_Sky.AssertFinger(finger);
                Busstop koma = src_Sky.BusstopIndexOf(finger);

                int suji1;
                int suji2;
                int dan1;
                int dan2;
                Util_MasuNum.TryMasuToSuji(Conv_Busstop.ToMasu( koma), out suji1);
                Util_MasuNum.TryMasuToSuji(masu, out suji2);
                Util_MasuNum.TryMasuToDan(Conv_Busstop.ToMasu(koma), out dan1);
                Util_MasuNum.TryMasuToDan(masu, out dan2);

                if (
                    Conv_Busstop.ToPlayerside(koma) == pside
                    && suji1 == suji2
                    && dan1 == dan2
                    )
                {
                    foundKoma = finger;
                    break;
                }

            }

            return foundKoma;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒の種類（不成として扱います）を指定して、駒を検索します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="komasyurui"></param>
        /// <param name="uc_Main"></param>
        /// <returns>無ければ -1</returns>
        public static Finger InOkibaSyuruiNow_IgnoreCase(SkyConst src_Sky, Okiba okiba, Komasyurui14 komasyurui, KwErrorHandler errH)
        {
            Finger found = Fingers.Error_1;

            Komasyurui14 syuruiNarazuCase = Util_Komasyurui14.NarazuCaseHandle(komasyurui);

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                src_Sky.AssertFinger(finger);
                Busstop koma = src_Sky.BusstopIndexOf(finger);

                if (Conv_Busstop.ToOkiba(koma) == okiba
                    && Util_Komasyurui14.Matches(Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui(koma)), syuruiNarazuCase))
                {
                    found = finger;
                    break;
                }
            }

            return found;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 指定の場所にある駒を返します。
        /// ************************************************************************************************************************
        /// 
        ///         先後は見ますが、将棋盤限定です。
        /// 
        /// </summary>
        /// <param name="okiba">置き場</param>
        /// <param name="masu">筋、段</param>
        /// <param name="uc_Main">メインパネル</param>
        /// <returns>駒。無ければヌル。</returns>
        public static Finger InShogibanMasuNow(SkyConst src_Sky, Playerside pside, SyElement masu, KwErrorHandler errH)
        {
            Finger foundKoma = Fingers.Error_1;

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                src_Sky.AssertFinger(finger);
                Busstop koma = src_Sky.BusstopIndexOf(finger);

                int suji1;
                int suji2;
                int dan1;
                int dan2;
                Util_MasuNum.TryMasuToSuji(Conv_Busstop.ToMasu(koma), out suji1);
                Util_MasuNum.TryMasuToSuji(masu, out suji2);
                Util_MasuNum.TryMasuToDan(Conv_Busstop.ToMasu(koma), out dan1);
                Util_MasuNum.TryMasuToDan(masu, out dan2);

                // 先後は見ますが、将棋盤限定です。
                if (
                    Conv_Busstop.ToPlayerside( koma) == pside
                    && Conv_Busstop.ToOkiba(koma) == Okiba.ShogiBan
                    && suji1 == suji2
                    && dan1 == dan2
                    )
                {
                    foundKoma = finger;
                    break;
                }

            }

            return foundKoma;
        }


    }
}
