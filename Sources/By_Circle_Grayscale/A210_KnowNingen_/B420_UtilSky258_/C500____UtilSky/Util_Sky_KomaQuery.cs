using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C500____Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky
{
    public abstract class Util_Sky_KomaQuery
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 指定のマスにある駒を返します。（本将棋用）
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static Busstop InMasuNow(Sky src_Sky, SyElement masu)
        {
            Busstop koma = Busstop.Empty;

            Finger fig = Util_Sky_FingersQuery.InMasuNow_Old(src_Sky, masu).ToFirst();

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
        public static Busstop InMasuPsideNow(Sky src_Sky, SyElement masu, Playerside pside)
        {
            Busstop koma = Busstop.Empty;

            Finger fig = Util_Sky_FingersQuery.InMasuNow_Old(src_Sky, masu).ToFirst();

            if (Fingers.Error_1 == fig)
            {
                // 指定の升には駒がない。
                goto gt_EndMethod;
            }

            koma = Util_Koma.FromFinger(src_Sky, fig);
            if (Conv_Busstop.ToPlayerside( koma) != pside)
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
        public static Busstop InMasuPsideKomasyuruiNow(Sky src_Sky, SyElement masu, Playerside pside, Komasyurui14 syurui)
        {
            Busstop koma = Busstop.Empty;

            Finger fig = Util_Sky_FingersQuery.InMasuNow_Old(src_Sky, masu).ToFirst();

            if (Fingers.Error_1 == fig)
            {
                // 指定の升には駒がない。
                goto gt_EndMethod;
            }

            koma = Util_Koma.FromFinger(src_Sky, fig);
            if (Conv_Busstop.ToPlayerside( koma) != pside || Conv_Busstop.ToKomasyurui( koma) != syurui)
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
