using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B290_Komahaiyaku.C500____Util;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C500____Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

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
            Sky src_Sky,
            out int[] motiSu,
            KwLogger errH
        )
        {
            motiSu = new int[(int)Pieces.Num];

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
                    motiSu[(int)Pieces.K]++;
                }
                else if (Komasyurui14.H07_Hisya__ == syurui)
                {
                    motiSu[(int)Pieces.R]++;
                }
                else if (Komasyurui14.H08_Kaku___ == syurui)
                {
                    motiSu[(int)Pieces.B]++;
                }
                else if (Komasyurui14.H05_Kin____ == syurui)
                {
                    motiSu[(int)Pieces.G]++;
                }
                else if (Komasyurui14.H04_Gin____ == syurui)
                {
                    motiSu[(int)Pieces.S]++;
                }
                else if (Komasyurui14.H03_Kei____ == syurui)
                {
                    motiSu[(int)Pieces.N]++;
                }
                else if (Komasyurui14.H02_Kyo____ == syurui)
                {
                    motiSu[(int)Pieces.L]++;
                }
                else if (Komasyurui14.H01_Fu_____ == syurui)
                {
                    motiSu[(int)Pieces.P]++;
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
                    motiSu[(int)Pieces.k]++;
                }
                else if (Komasyurui14.H07_Hisya__ == syurui)
                {
                    motiSu[(int)Pieces.r]++;
                }
                else if (Komasyurui14.H08_Kaku___ == syurui)
                {
                    motiSu[(int)Pieces.b]++;
                }
                else if (Komasyurui14.H05_Kin____ == syurui)
                {
                    motiSu[(int)Pieces.g]++;
                }
                else if (Komasyurui14.H04_Gin____ == syurui)
                {
                    motiSu[(int)Pieces.s]++;
                }
                else if (Komasyurui14.H03_Kei____ == syurui)
                {
                    motiSu[(int)Pieces.n]++;
                }
                else if (Komasyurui14.H02_Kyo____ == syurui)
                {
                    motiSu[(int)Pieces.l]++;
                }
                else if (Komasyurui14.H01_Fu_____ == syurui)
                {
                    motiSu[(int)Pieces.p]++;
                }
                else
                {
                }
            }

        }




    }
}
