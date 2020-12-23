using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{

    /// <summary>
    /// フィンガーを１つ求めるユーティリティーです。
    /// </summary>
    public abstract class UtilSkyFingerQuery
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
        public static Finger InBanjoMasuNow(IPosition src_Sky, Playerside pside, SyElement masu)
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
                Conv_Masu.ToSuji_FromBanjoMasu(Conv_Busstop.ToMasu(koma), out suji1);
                Conv_Masu.ToSuji_FromBanjoMasu(masu, out suji2);
                Conv_Masu.ToDan_FromBanjoMasu(Conv_Busstop.ToMasu(koma), out dan1);
                Conv_Masu.ToDan_FromBanjoMasu(masu, out dan2);

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
        public static Finger InOkibaSyuruiNow_IgnoreCase(IPosition positionA, Okiba okiba, Komasyurui14 komasyurui)
        {
            Finger found = Fingers.Error_1;

            Komasyurui14 syuruiNarazuCase = Util_Komasyurui14.NarazuCaseHandle(komasyurui);

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                positionA.AssertFinger(finger);
                Busstop koma = positionA.BusstopIndexOf(finger);

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
        public static Finger InMasuNow_FilteringBanjo(IPosition positionA, Playerside pside, SyElement masu)
        {
            Finger foundKoma = Fingers.Error_1;

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                positionA.AssertFinger(finger);
                Busstop koma = positionA.BusstopIndexOf(finger);

                // 先後は見ますが、将棋盤限定です。
                if (Conv_Busstop.ToOkiba(koma) == Okiba.ShogiBan)
                {
                    int suji1;
                    int suji2;
                    int dan1;
                    int dan2;
                    Conv_Masu.ToSuji_FromBanjoMasu(Conv_Busstop.ToMasu(koma), out suji1);
                    Conv_Masu.ToSuji_FromBanjoMasu(masu, out suji2);
                    Conv_Masu.ToDan_FromBanjoMasu(Conv_Busstop.ToMasu(koma), out dan1);
                    Conv_Masu.ToDan_FromBanjoMasu(masu, out dan2);

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
            }

            return foundKoma;
        }


    }
}
