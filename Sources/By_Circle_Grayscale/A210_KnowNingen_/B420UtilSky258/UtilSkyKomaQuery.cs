using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B310Shogiban.C500Util;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky
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
        public static Busstop InMasuNow(ISky src_Sky, SyElement masu)
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
        public static Busstop InMasuPsideNow(ISky src_Sky, SyElement masu, Playerside pside)
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
        public static Busstop InMasuPsideKomasyuruiNow(ISky src_Sky, SyElement masu, Playerside pside, Komasyurui14 syurui)
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
