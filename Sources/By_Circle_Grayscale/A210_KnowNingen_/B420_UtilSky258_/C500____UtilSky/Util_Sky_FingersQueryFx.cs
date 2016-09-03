using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky
{

    /// <summary>
    /// 指定局面から、『指差し番号』を問い合わせます。
    /// 
    /// 特殊なもの。
    /// </summary>
    public abstract class Util_Sky_FingersQueryFx
    {

        /// <summary>
        /// ４分割します。
        /// </summary>
        /// <param name="fs_banjoSeme">fingers</param>
        /// <param name="fs_banjoKurau"></param>
        /// <param name="fs_motiSeme"></param>
        /// <param name="fs_motiKurau"></param>
        /// <param name="src_Sky"></param>
        /// <param name="tebanSeme"></param>
        /// <param name="tebanKurau"></param>
        /// <param name="errH_OrNull"></param>
        public static void Split_BanjoSeme_BanjoKurau_MotiSeme_MotiKurau(
            out Fingers fs_banjoSeme,//戦駒（利きを調べる側）
            out Fingers fs_banjoKurau,//戦駒（喰らう側）
            out Fingers fs_motiSeme,// 持駒（利きを調べる側）
            out Fingers fs_motiKurau,// 持駒（喰らう側）
            Sky src_Sky,
            Playerside tebanSeme,
            Playerside tebanKurau,
            KwLogger errH_OrNull
        )
        {
            Fingers fs_banjoSeme_temp = new Fingers();// （１）盤上駒_攻め手
            Fingers fs_banjoKurau_temp = new Fingers();// （２）盤上駒_食らう側
            Fingers fs_motiSeme_temp = new Fingers();// （３）持ち駒_攻め手
            Fingers fs_motiKurau_temp = new Fingers();// （４）持ち駒_食らう側

            src_Sky.Foreach_Busstops((Finger finger, Busstop star, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToOkiba(star) == Okiba.ShogiBan)
                {
                    //
                    // 盤上
                    //
                    if (tebanSeme == Conv_Busstop.ToPlayerside( star))
                    {
                        fs_banjoSeme_temp.Add(finger);// （１）盤上駒_攻め手
                    }
                    else if (tebanKurau == Conv_Busstop.ToPlayerside( star))
                    {
                        fs_banjoKurau_temp.Add(finger);// （２）盤上駒_食らう側
                    }
                }
                else if (Conv_Busstop.ToOkiba(star) == Okiba.Sente_Komadai)
                {
                    //
                    // P1駒台
                    //
                    if (tebanSeme == Playerside.P1)
                    {
                        // 攻手がP1のとき、P1駒台は。
                        fs_motiSeme_temp.Add(finger);// （３）持ち駒_攻め手
                    }
                    else if (tebanSeme == Playerside.P2)
                    {
                        // 攻手がP2のとき、P1駒台は。
                        fs_motiKurau_temp.Add(finger);// （４）持ち駒_食らう側
                    }
                    else
                    {
                        throw new Exception("駒台の持ち駒を調べようとしましたが、先手でも、後手でもない指定でした。");
                    }
                }
                else if (Conv_Busstop.ToOkiba(star) == Okiba.Gote_Komadai)
                {
                    //
                    // P2駒台
                    //
                    if (tebanSeme == Playerside.P1)
                    {
                        // 攻手がP1のとき、P2駒台は。
                        fs_motiKurau_temp.Add(finger);// （３）持ち駒_攻め手
                    }
                    else if (tebanSeme == Playerside.P2)
                    {
                        // 攻手がP2のとき、P2駒台は。
                        fs_motiSeme_temp.Add(finger);// （４）持ち駒_食らう側
                    }
                    else
                    {
                        throw new Exception("駒台の持ち駒を調べようとしましたが、先手でも、後手でもない指定でした。");
                    }
                }
                else
                {
                    throw new Exception("駒台の持ち駒を調べようとしましたが、盤上でも、駒台でもない指定でした。");
                }
            });
            fs_banjoSeme = fs_banjoSeme_temp;// （１）盤上駒_攻め手
            fs_banjoKurau = fs_banjoKurau_temp;// （２）盤上駒_食らう側
            fs_motiSeme = fs_motiSeme_temp;// （３）持ち駒_攻め手
            fs_motiKurau = fs_motiKurau_temp;// （４）持ち駒_食らう側
        }


        /// <summary>
        /// 持駒を取得。
        /// </summary>
        /// <param name="fingers_banjoSeme"></param>
        /// <param name="fingers_banjoKurau"></param>
        /// <param name="fingers_motiSeme"></param>
        /// <param name="fingers_motiKurau"></param>
        /// <param name="src_Sky"></param>
        /// <param name="tebanSeme"></param>
        /// <param name="tebanKurau"></param>
        /// <param name="errH_OrNull"></param>
        public static void Split_Moti1p_Moti2p(
            out Fingers fingers_moti1p,// 持駒 1P
            out Fingers fingers_moti2p,// 持駒 2=
            Sky src_Sky,
            KwLogger errH_OrNull
        )
        {
            Fingers fingers_moti1p_temp = new Fingers();// （３）持ち駒_攻め手
            Fingers fingers_moti2p_temp = new Fingers();// （４）持ち駒_食らう側

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToOkiba(koma) == Okiba.Sente_Komadai)
                {
                    //
                    // 1P 駒台
                    //
                    fingers_moti1p_temp.Add(finger);
                }
                else if (Conv_Busstop.ToOkiba(koma) == Okiba.Gote_Komadai)
                {
                    //
                    // 2P 駒台
                    //
                    fingers_moti2p_temp.Add(finger);
                }
            });
            fingers_moti1p = fingers_moti1p_temp;
            fingers_moti2p = fingers_moti2p_temp;
        }


        public static void Split_Jigyoku_Aitegyoku(
            out Busstop koma_Jigyoku_orNull,
            out Busstop koma_Aitegyoku_orNull,
            Sky src_Sky,
            Playerside jiPside,
            Playerside aitePside
            )
        {
            Busstop koma_Jigyoku_temp = Busstop.Empty;
            Busstop koma_Aitegyoku_temp = Busstop.Empty;

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (
                    Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma)
                    && jiPside == Conv_Busstop.ToPlayerside( koma)
                    && Komasyurui14.H06_Gyoku__ == Conv_Busstop.ToKomasyurui( koma)
                    )
                {
                    //
                    // 自玉の位置
                    //
                    koma_Jigyoku_temp = koma;
                }
                else if (
                    Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma)
                    && aitePside == Conv_Busstop.ToPlayerside(koma)
                    && Komasyurui14.H06_Gyoku__ == Conv_Busstop.ToKomasyurui(koma)
                    )
                {
                    //
                    // 相手玉の位置
                    //
                    koma_Aitegyoku_temp = koma;
                }
            });
            koma_Jigyoku_orNull = koma_Jigyoku_temp;
            koma_Aitegyoku_orNull = koma_Aitegyoku_temp;
        }

        /// <summary>
        /// 1P玉と、2P玉を取得します。
        /// </summary>
        /// <param name="koma_1PGyoku_orNull"></param>
        /// <param name="koma_2PGyoku_orNull"></param>
        /// <param name="src_Sky"></param>
        public static void Split_1PGyoku_2PGyoku(
            out Busstop koma_1PGyoku_orNull,
            out Busstop koma_2PGyoku_orNull,
            Sky src_Sky
        )
        {
            Busstop koma_1PGyoku_temp = Busstop.Empty;
            Busstop koma_2PGyoku_temp = Busstop.Empty;

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if(
                    Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma)
                    && Komasyurui14.H06_Gyoku__ == Conv_Busstop.ToKomasyurui( koma)
                    )
                {
                    if (Playerside.P1 == Conv_Busstop.ToPlayerside( koma))
                    {
                        koma_1PGyoku_temp = koma;// 1P玉の位置
                    }
                    else if (Playerside.P2 == Conv_Busstop.ToPlayerside(koma))
                    {
                        koma_2PGyoku_temp = koma;// 2P玉の位置
                    }
                }
            });
            koma_1PGyoku_orNull = koma_1PGyoku_temp;
            koma_2PGyoku_orNull = koma_2PGyoku_temp;
        }

    }
}
