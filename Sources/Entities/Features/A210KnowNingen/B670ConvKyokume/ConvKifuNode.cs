using System;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class ConvKifuNode
    {
        /// <summary>
        /// 表形式の局面データを出力します。SFENとの親和性高め。
        /// </summary>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static ISfenPosition1 ToRO_Kyokumen1(ISky src_Sky, ILogTag logTag)
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
                out motiSu,
                logTag
                );

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
        public static string ToSfenstring(ISky src_Sky, Playerside pside, ILogTag logTag)
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
                    out motiSu,
                    logTag
                    );

                if (0 == motiSu[(int)Piece.K] +
                    motiSu[(int)Piece.R] +
                    motiSu[(int)Piece.B] +
                    motiSu[(int)Piece.G] +
                    motiSu[(int)Piece.S] +
                    motiSu[(int)Piece.N] +
                    motiSu[(int)Piece.L] +
                    motiSu[(int)Piece.P] +
                    motiSu[(int)Piece.k] +
                    motiSu[(int)Piece.r] +
                    motiSu[(int)Piece.b] +
                    motiSu[(int)Piece.g] +
                    motiSu[(int)Piece.s] +
                    motiSu[(int)Piece.n] +
                    motiSu[(int)Piece.l] +
                    motiSu[(int)Piece.p]
                    )
                {
                    sb.Append("-");
                }
                else
                {
                    if (0 < motiSu[(int)Piece.K])
                    {
                        if (1 < motiSu[(int)Piece.K])
                        {
                            sb.Append(motiSu[(int)Piece.K]);
                        }
                        sb.Append("K");
                    }

                    if (0 < motiSu[(int)Piece.R])
                    {
                        if (1 < motiSu[(int)Piece.R])
                        {
                            sb.Append(motiSu[(int)Piece.R]);
                        }
                        sb.Append("R");
                    }

                    if (0 < motiSu[(int)Piece.B])
                    {
                        if (1 < motiSu[(int)Piece.B])
                        {
                            sb.Append(motiSu[(int)Piece.B]);
                        }
                        sb.Append("B");
                    }

                    if (0 < motiSu[(int)Piece.G])
                    {
                        if (1 < motiSu[(int)Piece.G])
                        {
                            sb.Append(motiSu[(int)Piece.G]);
                        }
                        sb.Append("G");
                    }

                    if (0 < motiSu[(int)Piece.S])
                    {
                        if (1 < motiSu[(int)Piece.S])
                        {
                            sb.Append(motiSu[(int)Piece.S]);
                        }
                        sb.Append("S");
                    }

                    if (0 < motiSu[(int)Piece.N])
                    {
                        if (1 < motiSu[(int)Piece.N])
                        {
                            sb.Append(motiSu[(int)Piece.N]);
                        }
                        sb.Append("N");
                    }

                    if (0 < motiSu[(int)Piece.L])
                    {
                        if (1 < motiSu[(int)Piece.L])
                        {
                            sb.Append(motiSu[(int)Piece.L]);
                        }
                        sb.Append("L");
                    }

                    if (0 < motiSu[(int)Piece.P])
                    {
                        if (1 < motiSu[(int)Piece.P])
                        {
                            sb.Append(motiSu[(int)Piece.P]);
                        }
                        sb.Append("P");
                    }

                    if (0 < motiSu[(int)Piece.k])
                    {
                        if (1 < motiSu[(int)Piece.k])
                        {
                            sb.Append(motiSu[(int)Piece.k]);
                        }
                        sb.Append("k");
                    }

                    if (0 < motiSu[(int)Piece.r])
                    {
                        if (1 < motiSu[(int)Piece.r])
                        {
                            sb.Append(motiSu[(int)Piece.r]);
                        }
                        sb.Append("r");
                    }

                    if (0 < motiSu[(int)Piece.b])
                    {
                        if (1 < motiSu[(int)Piece.b])
                        {
                            sb.Append(motiSu[(int)Piece.b]);
                        }
                        sb.Append("b");
                    }

                    if (0 < motiSu[(int)Piece.g])
                    {
                        if (1 < motiSu[(int)Piece.g])
                        {
                            sb.Append(motiSu[(int)Piece.g]);
                        }
                        sb.Append("g");
                    }

                    if (0 < motiSu[(int)Piece.s])
                    {
                        if (1 < motiSu[(int)Piece.s])
                        {
                            sb.Append(motiSu[(int)Piece.s]);
                        }
                        sb.Append("s");
                    }

                    if (0 < motiSu[(int)Piece.n])
                    {
                        if (1 < motiSu[(int)Piece.n])
                        {
                            sb.Append(motiSu[(int)Piece.n]);
                        }
                        sb.Append("n");
                    }

                    if (0 < motiSu[(int)Piece.l])
                    {
                        if (1 < motiSu[(int)Piece.l])
                        {
                            sb.Append(motiSu[(int)Piece.l]);
                        }
                        sb.Append("l");
                    }

                    if (0 < motiSu[(int)Piece.p])
                    {
                        if (1 < motiSu[(int)Piece.p])
                        {
                            sb.Append(motiSu[(int)Piece.p]);
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
