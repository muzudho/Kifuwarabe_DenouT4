using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class UtilSkyKomaQuery
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 指定のマスにある駒を返します。（本将棋用）
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static Busstop InMasuNow(IPosition src_Sky, SyElement masu)
        {
            Busstop koma = Busstop.Empty;

            Finger fig = UtilSkyFingersQuery.InMasuNow_Old(src_Sky, masu).ToFirst();

            if (Fingers.Error_1 == fig)
            {
                // 指定の升には駒がない。
                goto gt_EndMethod;
            }

            koma = Util_Koma.FromFinger(src_Sky, fig);

        gt_EndMethod:
            return koma;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 指定のマスにある駒を返します。（本将棋用）
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static Busstop InMasuPsideNow(IPosition src_Sky, SyElement masu, Playerside pside)
        {
            Busstop koma = Busstop.Empty;

            Finger fig = UtilSkyFingersQuery.InMasuNow_Old(src_Sky, masu).ToFirst();

            if (Fingers.Error_1 == fig)
            {
                // 指定の升には駒がない。
                goto gt_EndMethod;
            }

            koma = Util_Koma.FromFinger(src_Sky, fig);
            if (Conv_Busstop.ToPlayerside(koma) != pside)
            {
                // サイドが異なる
                koma = Busstop.Empty;
                goto gt_EndMethod;
            }

        gt_EndMethod:
            return koma;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 指定のマスにある駒を返します。（本将棋用）
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static Busstop InMasuPsideKomasyuruiNow(IPosition src_Sky, SyElement masu, Playerside pside, Komasyurui14 syurui)
        {
            Busstop koma = Busstop.Empty;

            Finger fig = UtilSkyFingersQuery.InMasuNow_Old(src_Sky, masu).ToFirst();

            if (Fingers.Error_1 == fig)
            {
                // 指定の升には駒がない。
                goto gt_EndMethod;
            }

            koma = Util_Koma.FromFinger(src_Sky, fig);
            if (Conv_Busstop.ToPlayerside(koma) != pside || Conv_Busstop.ToKomasyurui(koma) != syurui)
            {
                // サイド または駒の種類が異なる
                koma = Busstop.Empty;
                goto gt_EndMethod;
            }

        gt_EndMethod:
            return koma;
        }

    }
}
