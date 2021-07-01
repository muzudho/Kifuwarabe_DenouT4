namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    using Grayscale.Kifuwaragyoku.Entities.Logging;
    using Grayscale.Kifuwaragyoku.Entities.Positioning;
    using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
    using Grayscale.Kifuwaragyoku.Entities.Take1Base;


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
        /// <param name="logTag"></param>
        public static void CountMoti(
            IPosition src_Sky,
            out int[] motiSu)
        {
            motiSu = new int[(int)Piece.Num];

            Fingers komas_moti1p;// 先手の持駒
            Fingers komas_moti2p;// 後手の持駒
            UtilSkyFingersQueryFx.Split_Moti1p_Moti2p(out komas_moti1p, out komas_moti2p, src_Sky);

            foreach (Finger figKoma in komas_moti1p.Items)
            {
                src_Sky.AssertFinger(figKoma);
                Busstop busstop = src_Sky.BusstopIndexOf(figKoma);

                Komasyurui14 syurui = Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui(busstop));
                if (Komasyurui14.H06_Gyoku__ == syurui)
                {
                    motiSu[(int)Piece.K1]++;
                }
                else if (Komasyurui14.H07_Hisya__ == syurui)
                {
                    motiSu[(int)Piece.R1]++;
                }
                else if (Komasyurui14.H08_Kaku___ == syurui)
                {
                    motiSu[(int)Piece.B1]++;
                }
                else if (Komasyurui14.H05_Kin____ == syurui)
                {
                    motiSu[(int)Piece.G1]++;
                }
                else if (Komasyurui14.H04_Gin____ == syurui)
                {
                    motiSu[(int)Piece.S1]++;
                }
                else if (Komasyurui14.H03_Kei____ == syurui)
                {
                    motiSu[(int)Piece.N1]++;
                }
                else if (Komasyurui14.H02_Kyo____ == syurui)
                {
                    motiSu[(int)Piece.L1]++;
                }
                else if (Komasyurui14.H01_Fu_____ == syurui)
                {
                    motiSu[(int)Piece.P1]++;
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
                    motiSu[(int)Piece.K2]++;
                }
                else if (Komasyurui14.H07_Hisya__ == syurui)
                {
                    motiSu[(int)Piece.R2]++;
                }
                else if (Komasyurui14.H08_Kaku___ == syurui)
                {
                    motiSu[(int)Piece.B2]++;
                }
                else if (Komasyurui14.H05_Kin____ == syurui)
                {
                    motiSu[(int)Piece.G2]++;
                }
                else if (Komasyurui14.H04_Gin____ == syurui)
                {
                    motiSu[(int)Piece.S2]++;
                }
                else if (Komasyurui14.H03_Kei____ == syurui)
                {
                    motiSu[(int)Piece.N2]++;
                }
                else if (Komasyurui14.H02_Kyo____ == syurui)
                {
                    motiSu[(int)Piece.L2]++;
                }
                else if (Komasyurui14.H01_Fu_____ == syurui)
                {
                    motiSu[(int)Piece.P2]++;
                }
                else
                {
                }
            }

        }




    }
}
