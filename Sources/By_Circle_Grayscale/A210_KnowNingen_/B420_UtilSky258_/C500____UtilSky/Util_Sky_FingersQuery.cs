﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_Masu_______.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C500____Util;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky
{

    /// <summary>
    /// 指定局面から、『指差し番号』を問い合わせます。
    /// 
    /// Okiba,Playerside,Komasyurui,Suji を引数に使うシンプルなもの。
    /// </summary>
    public abstract class Util_Sky_FingersQuery
    {
        /// <summary>
        /// **********************************************************************************************************************
        /// 駒のハンドルを返します。
        /// **********************************************************************************************************************
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public static Fingers InOkibaPsideNow(SkyConst src_Sky, Okiba okiba, Playerside pside)
        {
            Fingers fingers = new Fingers();

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToOkiba(koma) == okiba
                    && Conv_Busstop.ToPlayerside(koma)== pside
                    )
                {
                    fingers.Add(finger);
                }
            });

            return fingers;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒のハンドル(*1)を返します。
        /// ************************************************************************************************************************
        /// 
        ///         *1…将棋の駒１つ１つに付けられた番号です。
        /// 
        /// </summary>
        /// <param name="syurui"></param>
        /// <param name="hKomas"></param>
        /// <returns></returns>
        public static Fingers InKomasyuruiNow(SkyConst src_Sky, Komasyurui14 syurui, KwErrorHandler errH)
        {
            Fingers figKomas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                src_Sky.AssertFinger(figKoma);
                Busstop koma = src_Sky.BusstopIndexOf(figKoma);


                if (Util_Komasyurui14.Matches(syurui, Conv_Busstop.ToKomasyurui(koma)))
                {
                    figKomas.Add(figKoma);
                }
            }

            return figKomas;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒のハンドルを返します。　：　置き場、種類
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="syurui"></param>
        /// <param name="kifu"></param>
        /// <returns></returns>
        public static Fingers InOkibaKomasyuruiNow(SkyConst src_Sky, Okiba okiba, Komasyurui14 syurui)
        {
            Fingers komas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                src_Sky.AssertFinger(figKoma);
                Busstop koma = src_Sky.BusstopIndexOf(figKoma);


                if (
                    okiba == Conv_Busstop.ToOkiba(koma)
                    && Util_Komasyurui14.Matches(syurui, Conv_Busstop.ToKomasyurui( koma))
                    )
                {
                    komas.Add(figKoma);
                }
            }

            return komas;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 局面上のオブジェクトを返します。置き場、先後サイド、駒の種類で絞りこみます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="src_Sky">局面データ。</param>
        /// <param name="okiba">置き場。</param>
        /// <param name="pside">先後サイド。</param>
        /// <param name="komaSyurui">駒の種類。</param>
        /// <returns></returns>
        public static Fingers InOkibaPsideKomasyuruiNow(SkyConst src_Sky, Okiba okiba, Playerside pside, Komasyurui14 komaSyurui)
        {
            Fingers fingers = new Fingers();

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (
                    okiba == Conv_Busstop.ToOkiba(koma)
                    && pside == Conv_Busstop.ToPlayerside( koma)
                    && komaSyurui == Conv_Busstop.ToKomasyurui( koma)
                    )
                {
                    fingers.Add(finger);
                }
            });

            return fingers;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 指定のマスにあるスプライトを返します。（本将棋用）
        /// ************************************************************************************************************************
        /// 
        /// FIXME: 打に対応したい。
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static Fingers InMasuNow_Old(SkyConst src_Sky, SyElement masu)//, KwErrorHandler errH
        {
            // １個入る。
            Fingers found = new Fingers();

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                Busstop koma = Util_Koma.FromFinger(src_Sky, finger);

                if (Masu_Honshogi.Basho_Equals(Conv_Busstop.ToMasu( koma), masu))
                {
                    found.Add(finger);
                }
            }

            return found;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 指定のマスにあるスプライトを返します。（本将棋用）
        /// ************************************************************************************************************************
        /// 
        /// FIXME: 打に対応したい。
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static Fingers InMasuNow_New(SkyConst src_Sky, Move move)
        {
            bool drop = Conv_Move.ToDrop(move);
            SyElement srcMasu = Conv_Move.ToSrcMasu(move);

            // １個入る。
            Fingers found = new Fingers();

            if (drop)
            {
                // 「打」は、駒台をサーチ☆
                Playerside pside = Conv_Move.ToPlayerside(move);

                if (Playerside.P1 == pside)//先手
                {
                    foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
                    {
                        Busstop koma = Util_Koma.FromFinger(src_Sky, finger);

                        if (Okiba.Sente_Komadai == Conv_SyElement.ToOkiba(Conv_Busstop.ToMasu( koma)))
                        {
                            found.Add(finger);
                        }
                    }
                }
                else// 後手
                {
                    foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
                    {
                        Busstop koma = Util_Koma.FromFinger(src_Sky, finger);

                        if (Okiba.Gote_Komadai == Conv_SyElement.ToOkiba(Conv_Busstop.ToMasu( koma)))
                        {
                            found.Add(finger);
                        }
                    }

                }
            }
            // 「打」でなければ。
            else
            {
                foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
                {
                    Busstop koma = Util_Koma.FromFinger(src_Sky, finger);

                    if (Masu_Honshogi.Basho_Equals(Conv_Busstop.ToMasu( koma), srcMasu))
                    {
                        found.Add(finger);
                    }
                }
            }


            return found;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 指定の筋にあるスプライトを返します。（本将棋用）二歩チェックに利用。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="okiba">置き場</param>
        /// <param name="pside">先後</param>
        /// <param name="pside">駒種類</param>
        /// <param name="suji">筋番号1～9</param>
        /// <returns></returns>
        public static Fingers InOkibaPsideKomasyuruiSujiNow(SkyConst src_Sky, Okiba okiba, Playerside pside, Komasyurui14 ks, int suji)
        {
            Fingers found = new Fingers();

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                src_Sky.AssertFinger(finger);
                Busstop koma2 = src_Sky.BusstopIndexOf(finger);

                int suji2;
                Util_MasuNum.TryMasuToSuji(Conv_Busstop.ToMasu( koma2), out suji2);

                if (
                    Conv_Busstop.ToOkiba(koma2)==okiba
                    && Conv_Busstop.ToPlayerside( koma2) == pside
                    && Conv_Busstop.ToKomasyurui( koma2) == ks
                    && suji2 == suji
                    )
                {
                    found.Add(finger);
                }
            }

            return found;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 指定した置き場にある駒のハンドルを返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="okiba"></param>
        /// <returns></returns>
        public static Fingers InOkibaNow(SkyConst src_Sky, Okiba okiba, KwErrorHandler errH)
        {
            Fingers komas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                src_Sky.AssertFinger(figKoma);
                Busstop koma = src_Sky.BusstopIndexOf(figKoma);

                if (okiba == Conv_Busstop.ToOkiba(koma))
                {
                    komas.Add(figKoma);
                }
            }

            return komas;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒のハンドルを返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="pside"></param>
        /// <param name="hKomas"></param>
        /// <returns></returns>
        public static Fingers InPsideNow(SkyConst src_Sky, Playerside pside, KwErrorHandler errH)
        {
            Fingers fingers = new Fingers();

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (pside == Conv_Busstop.ToPlayerside( koma))
                {
                    fingers.Add(finger);
                }
            });

            return fingers;
        }
    }
}
