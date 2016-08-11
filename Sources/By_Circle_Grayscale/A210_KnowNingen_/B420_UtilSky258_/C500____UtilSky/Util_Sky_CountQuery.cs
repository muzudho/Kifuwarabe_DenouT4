﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B290_Komahaiyaku.C500____Util;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C250____Struct;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C500____Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.P339_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky
{

    /// <summary>
    /// 棋譜ノードのユーティリティー。
    /// </summary>
    public abstract class Util_Sky_CountQuery
    {


        /// <summary>
        /// 持ち駒を数えます。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="mK"></param>
        /// <param name="mR"></param>
        /// <param name="mB"></param>
        /// <param name="mG"></param>
        /// <param name="mS"></param>
        /// <param name="mN"></param>
        /// <param name="mL"></param>
        /// <param name="mP"></param>
        /// <param name="mk"></param>
        /// <param name="mr"></param>
        /// <param name="mb"></param>
        /// <param name="mg"></param>
        /// <param name="ms"></param>
        /// <param name="mn"></param>
        /// <param name="ml"></param>
        /// <param name="mp"></param>
        /// <param name="errH"></param>
        public static void CountMoti(
            SkyConst src_Sky,
            out int mK,
            out int mR,
            out int mB,
            out int mG,
            out int mS,
            out int mN,
            out int mL,
            out int mP,

            out int mk,
            out int mr,
            out int mb,
            out int mg,
            out int ms,
            out int mn,
            out int ml,
            out int mp,
            KwErrorHandler errH
        )
        {
            mK = 0;
            mR = 0;
            mB = 0;
            mG = 0;
            mS = 0;
            mN = 0;
            mL = 0;
            mP = 0;

            mk = 0;
            mr = 0;
            mb = 0;
            mg = 0;
            ms = 0;
            mn = 0;
            ml = 0;
            mp = 0;

            Fingers komas_moti1p;// 先手の持駒
            Fingers komas_moti2p;// 後手の持駒
            Util_Sky_FingersQueryFx.Split_Moti1p_Moti2p(out komas_moti1p, out komas_moti2p, src_Sky, errH);

            foreach (Finger figKoma in komas_moti1p.Items)
            {
                src_Sky.AssertFinger(figKoma);
                Busstop busstop = src_Sky.BusstopIndexOf(figKoma);

                Komasyurui14 syurui = Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui(busstop));
                if (Komasyurui14.H06_Gyoku__ == syurui)
                {
                    mK++;
                }
                else if (Komasyurui14.H07_Hisya__ == syurui)
                {
                    mR++;
                }
                else if (Komasyurui14.H08_Kaku___ == syurui)
                {
                    mB++;
                }
                else if (Komasyurui14.H05_Kin____ == syurui)
                {
                    mG++;
                }
                else if (Komasyurui14.H04_Gin____ == syurui)
                {
                    mS++;
                }
                else if (Komasyurui14.H03_Kei____ == syurui)
                {
                    mN++;
                }
                else if (Komasyurui14.H02_Kyo____ == syurui)
                {
                    mL++;
                }
                else if (Komasyurui14.H01_Fu_____ == syurui)
                {
                    mP++;
                }
                else
                {
                }
            }

            // 後手の持駒
            foreach (Finger figKoma in komas_moti2p.Items)
            {
                src_Sky.AssertFinger(figKoma);
                Busstop busstop = src_Sky.BusstopIndexOf(figKoma);
                
                Komasyurui14 syurui = Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui(busstop));

                if (Komasyurui14.H06_Gyoku__ == syurui)
                {
                    mk++;
                }
                else if (Komasyurui14.H07_Hisya__ == syurui)
                {
                    mr++;
                }
                else if (Komasyurui14.H08_Kaku___ == syurui)
                {
                    mb++;
                }
                else if (Komasyurui14.H05_Kin____ == syurui)
                {
                    mg++;
                }
                else if (Komasyurui14.H04_Gin____ == syurui)
                {
                    ms++;
                }
                else if (Komasyurui14.H03_Kei____ == syurui)
                {
                    mn++;
                }
                else if (Komasyurui14.H02_Kyo____ == syurui)
                {
                    ml++;
                }
                else if (Komasyurui14.H01_Fu_____ == syurui)
                {
                    mp++;
                }
                else
                {
                }
            }

        }




    }
}