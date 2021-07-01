namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    using System;
    using System.Text;
    using Grayscale.Kifuwaragyoku.Entities.Logging;
    using Grayscale.Kifuwaragyoku.Entities.Positioning;
    using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
    using Grayscale.Kifuwaragyoku.Entities.Take1Base;

    public abstract class ConvKifuNode
    {
        /// <summary>
        /// 表形式の局面データを出力します。SFENとの親和性高め。
        /// </summary>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static ISfenPosition1 ToRO_Kyokumen1(IPosition src_Sky)
        {
            ISfenPosition1 ro_Kyokumen1 = new SfenPosition1Impl();

            // 将棋盤
            for (int suji = 1; suji < 10; suji++)
            {
                for (int dan = 1; dan < 10; dan++)
                {
                    Finger koma0 = UtilSkyFingersQuery.InMasuNow_Old(
                        src_Sky, Conv_Masu.ToMasu_FromBanjoSujiDan(suji, dan)
                        ).ToFirst();

                    if (Fingers.Error_1 != koma0)
                    {
                        src_Sky.AssertFinger(koma0);
                        Busstop koma1 = src_Sky.BusstopIndexOf(koma0);

                        ro_Kyokumen1.Ban[suji, dan] = Util_Komasyurui14.SfenText(
                            Conv_Busstop.ToKomasyurui(koma1),
                            Conv_Busstop.ToPlayerside(koma1)
                            );
                    }
                }
            }

            // 持ち駒の枚数
            int[] motiSu;
            UtilSkyCountQuery.CountMoti(
                src_Sky,
                out motiSu);

            Array.Copy(motiSu, ro_Kyokumen1.MotiSu, motiSu.Length);

            // 手目済み
            ro_Kyokumen1.Temezumi = src_Sky.Temezumi;

            return ro_Kyokumen1;
        }


        /// <summary>
        /// 局面データから、SFEN文字列を作ります。
        /// </summary>
        /// <param name="pside"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static string ToSfenstring(IPosition src_Sky, Playerside pside)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("sfen ");

            for (int dan = 1; dan <= 9; dan++)
            {
                int spaceCount = 0;

                for (int suji = 9; suji >= 1; suji--)
                {
                    // 将棋盤上のどこかにある駒？
                    Finger koma0 = UtilSkyFingersQuery.InMasuNow_Old(
                        src_Sky, Conv_Masu.ToMasu_FromBanjoSujiDan(suji, dan)
                        ).ToFirst();

                    if (Fingers.Error_1 != koma0)
                    {
                        if (0 < spaceCount)
                        {
                            sb.Append(spaceCount);
                            spaceCount = 0;
                        }


                        src_Sky.AssertFinger(koma0);
                        Busstop koma1 = src_Sky.BusstopIndexOf(koma0);

                        sb.Append(Util_Komasyurui14.SfenText(
                            Conv_Busstop.ToKomasyurui(koma1),
                            Conv_Busstop.ToPlayerside(koma1)
                            ));
                    }
                    else
                    {
                        spaceCount++;
                    }

                }

                if (0 < spaceCount)
                {
                    sb.Append(spaceCount);
                    spaceCount = 0;
                }

                if (dan != 9)
                {
                    sb.Append("/");
                }
            }

            sb.Append(" ");

            //------------------------------------------------------------
            // 先後
            //------------------------------------------------------------
            switch (pside)
            {
                case Playerside.P2:
                    sb.Append("w");
                    break;
                default:
                    sb.Append("b");
                    break;
            }

            sb.Append(" ");

            //------------------------------------------------------------
            // 持ち駒の枚数
            //------------------------------------------------------------
            {
                int[] motiSu;
                UtilSkyCountQuery.CountMoti(
                    src_Sky,
                    out motiSu);

                if (0 == motiSu[(int)Piece.K1] +
                    motiSu[(int)Piece.R1] +
                    motiSu[(int)Piece.B1] +
                    motiSu[(int)Piece.G1] +
                    motiSu[(int)Piece.S1] +
                    motiSu[(int)Piece.N1] +
                    motiSu[(int)Piece.L1] +
                    motiSu[(int)Piece.P1] +
                    motiSu[(int)Piece.K2] +
                    motiSu[(int)Piece.R2] +
                    motiSu[(int)Piece.B2] +
                    motiSu[(int)Piece.G2] +
                    motiSu[(int)Piece.S2] +
                    motiSu[(int)Piece.N2] +
                    motiSu[(int)Piece.L2] +
                    motiSu[(int)Piece.P2]
                    )
                {
                    sb.Append("-");
                }
                else
                {
                    if (0 < motiSu[(int)Piece.K1])
                    {
                        if (1 < motiSu[(int)Piece.K1])
                        {
                            sb.Append(motiSu[(int)Piece.K1]);
                        }
                        sb.Append("K");
                    }

                    if (0 < motiSu[(int)Piece.R1])
                    {
                        if (1 < motiSu[(int)Piece.R1])
                        {
                            sb.Append(motiSu[(int)Piece.R1]);
                        }
                        sb.Append("R");
                    }

                    if (0 < motiSu[(int)Piece.B1])
                    {
                        if (1 < motiSu[(int)Piece.B1])
                        {
                            sb.Append(motiSu[(int)Piece.B1]);
                        }
                        sb.Append("B");
                    }

                    if (0 < motiSu[(int)Piece.G1])
                    {
                        if (1 < motiSu[(int)Piece.G1])
                        {
                            sb.Append(motiSu[(int)Piece.G1]);
                        }
                        sb.Append("G");
                    }

                    if (0 < motiSu[(int)Piece.S1])
                    {
                        if (1 < motiSu[(int)Piece.S1])
                        {
                            sb.Append(motiSu[(int)Piece.S1]);
                        }
                        sb.Append("S");
                    }

                    if (0 < motiSu[(int)Piece.N1])
                    {
                        if (1 < motiSu[(int)Piece.N1])
                        {
                            sb.Append(motiSu[(int)Piece.N1]);
                        }
                        sb.Append("N");
                    }

                    if (0 < motiSu[(int)Piece.L1])
                    {
                        if (1 < motiSu[(int)Piece.L1])
                        {
                            sb.Append(motiSu[(int)Piece.L1]);
                        }
                        sb.Append("L");
                    }

                    if (0 < motiSu[(int)Piece.P1])
                    {
                        if (1 < motiSu[(int)Piece.P1])
                        {
                            sb.Append(motiSu[(int)Piece.P1]);
                        }
                        sb.Append("P");
                    }

                    if (0 < motiSu[(int)Piece.K2])
                    {
                        if (1 < motiSu[(int)Piece.K2])
                        {
                            sb.Append(motiSu[(int)Piece.K2]);
                        }
                        sb.Append("k");
                    }

                    if (0 < motiSu[(int)Piece.R2])
                    {
                        if (1 < motiSu[(int)Piece.R2])
                        {
                            sb.Append(motiSu[(int)Piece.R2]);
                        }
                        sb.Append("r");
                    }

                    if (0 < motiSu[(int)Piece.B2])
                    {
                        if (1 < motiSu[(int)Piece.B2])
                        {
                            sb.Append(motiSu[(int)Piece.B2]);
                        }
                        sb.Append("b");
                    }

                    if (0 < motiSu[(int)Piece.G2])
                    {
                        if (1 < motiSu[(int)Piece.G2])
                        {
                            sb.Append(motiSu[(int)Piece.G2]);
                        }
                        sb.Append("g");
                    }

                    if (0 < motiSu[(int)Piece.S2])
                    {
                        if (1 < motiSu[(int)Piece.S2])
                        {
                            sb.Append(motiSu[(int)Piece.S2]);
                        }
                        sb.Append("s");
                    }

                    if (0 < motiSu[(int)Piece.N2])
                    {
                        if (1 < motiSu[(int)Piece.N2])
                        {
                            sb.Append(motiSu[(int)Piece.N2]);
                        }
                        sb.Append("n");
                    }

                    if (0 < motiSu[(int)Piece.L2])
                    {
                        if (1 < motiSu[(int)Piece.L2])
                        {
                            sb.Append(motiSu[(int)Piece.L2]);
                        }
                        sb.Append("l");
                    }

                    if (0 < motiSu[(int)Piece.P2])
                    {
                        if (1 < motiSu[(int)Piece.P2])
                        {
                            sb.Append(motiSu[(int)Piece.P2]);
                        }
                        sb.Append("p");
                    }
                }

            }

            // 手目
            sb.Append(" 1");

            return sb.ToString();
        }
    }
}
