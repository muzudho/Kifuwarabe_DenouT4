using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A120KifuSfen;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky
{

    /// <summary>
    /// 棋譜ノードのユーティリティー。
    /// </summary>
    public abstract class UtilSkyCountQuery
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
            ISky src_Sky,
            out int[] motiSu,
            ILogger errH
        )
        {
            motiSu = new int[(int)Piece.Num];

            Fingers komas_moti1p;// 先手の持駒
            Fingers komas_moti2p;// 後手の持駒
            UtilSkyFingersQueryFx.Split_Moti1p_Moti2p(out komas_moti1p, out komas_moti2p, src_Sky, errH);

            foreach (Finger figKoma in komas_moti1p.Items)
            {
                src_Sky.AssertFinger(figKoma);
                Busstop busstop = src_Sky.BusstopIndexOf(figKoma);

                Komasyurui14 syurui = Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui(busstop));
                if (Komasyurui14.H06_Gyoku__ == syurui)
                {
                    motiSu[(int)Piece.K]++;
                }
                else if (Komasyurui14.H07_Hisya__ == syurui)
                {
                    motiSu[(int)Piece.R]++;
                }
                else if (Komasyurui14.H08_Kaku___ == syurui)
                {
                    motiSu[(int)Piece.B]++;
                }
                else if (Komasyurui14.H05_Kin____ == syurui)
                {
                    motiSu[(int)Piece.G]++;
                }
                else if (Komasyurui14.H04_Gin____ == syurui)
                {
                    motiSu[(int)Piece.S]++;
                }
                else if (Komasyurui14.H03_Kei____ == syurui)
                {
                    motiSu[(int)Piece.N]++;
                }
                else if (Komasyurui14.H02_Kyo____ == syurui)
                {
                    motiSu[(int)Piece.L]++;
                }
                else if (Komasyurui14.H01_Fu_____ == syurui)
                {
                    motiSu[(int)Piece.P]++;
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
                    motiSu[(int)Piece.k]++;
                }
                else if (Komasyurui14.H07_Hisya__ == syurui)
                {
                    motiSu[(int)Piece.r]++;
                }
                else if (Komasyurui14.H08_Kaku___ == syurui)
                {
                    motiSu[(int)Piece.b]++;
                }
                else if (Komasyurui14.H05_Kin____ == syurui)
                {
                    motiSu[(int)Piece.g]++;
                }
                else if (Komasyurui14.H04_Gin____ == syurui)
                {
                    motiSu[(int)Piece.s]++;
                }
                else if (Komasyurui14.H03_Kei____ == syurui)
                {
                    motiSu[(int)Piece.n]++;
                }
                else if (Komasyurui14.H02_Kyo____ == syurui)
                {
                    motiSu[(int)Piece.l]++;
                }
                else if (Komasyurui14.H01_Fu_____ == syurui)
                {
                    motiSu[(int)Piece.p]++;
                }
                else
                {
                }
            }

        }




    }
}
