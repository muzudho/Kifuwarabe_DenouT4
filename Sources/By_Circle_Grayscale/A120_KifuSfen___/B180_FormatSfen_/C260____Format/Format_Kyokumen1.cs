using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using System.Text;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;

namespace Grayscale.A120_KifuSfen___.B180_FormatSfen_.C260____Format
{
    public abstract class Format_Kyokumen1
    {
        public static string ToSfenstring(RO_Kyokumen1_ForFormat ro_kyokumen1, bool white)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("sfen ");

            for (int dan = 1; dan <= 9; dan++)
            {
                int spaceCount = 0;

                for (int suji = 9; suji >= 1; suji--)
                {
                    // 将棋盤上のどこかにある駒？
                    string koma0 = ro_kyokumen1.Ban[suji,dan];

                    if ("" != koma0)
                    {
                        if (0 < spaceCount)
                        {
                            sb.Append(spaceCount);
                            spaceCount = 0;
                        }

                        sb.Append(koma0);
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
            if (white)
            {
                sb.Append("w");
            }
            else
            {
                sb.Append("b");
            }

            sb.Append(" ");

            //------------------------------------------------------------
            // 持ち駒
            //------------------------------------------------------------
            {
                /*
                int[] motiSu;
                ro_kyokumen1.GetMotiSu(
                    out motiSu
                    );
                    */

                if (0 ==
                    ro_kyokumen1.MotiSu[(int)Pieces.K] +
                    ro_kyokumen1.MotiSu[(int)Pieces.R] +
                    ro_kyokumen1.MotiSu[(int)Pieces.B] +
                    ro_kyokumen1.MotiSu[(int)Pieces.G] +
                    ro_kyokumen1.MotiSu[(int)Pieces.S] +
                    ro_kyokumen1.MotiSu[(int)Pieces.N] +
                    ro_kyokumen1.MotiSu[(int)Pieces.L] +
                    ro_kyokumen1.MotiSu[(int)Pieces.P] +
                    ro_kyokumen1.MotiSu[(int)Pieces.k] +
                    ro_kyokumen1.MotiSu[(int)Pieces.r] +
                    ro_kyokumen1.MotiSu[(int)Pieces.b] +
                    ro_kyokumen1.MotiSu[(int)Pieces.g] +
                    ro_kyokumen1.MotiSu[(int)Pieces.s] +
                    ro_kyokumen1.MotiSu[(int)Pieces.n] +
                    ro_kyokumen1.MotiSu[(int)Pieces.l] +
                    ro_kyokumen1.MotiSu[(int)Pieces.p]
                    )
                {
                    sb.Append("-");
                }
                else
                {
                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.K])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.K])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.K]);
                        }
                        sb.Append("K");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.R])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.R])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.R]);
                        }
                        sb.Append("R");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.B])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.B])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.B]);
                        }
                        sb.Append("B");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.G])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.G])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.G]);
                        }
                        sb.Append("G");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.S])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.S])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.S]);
                        }
                        sb.Append("S");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.N])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.N])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.N]);
                        }
                        sb.Append("N");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.L])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.L])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.L]);
                        }
                        sb.Append("L");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.P])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.P])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.P]);
                        }
                        sb.Append("P");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.k])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.k])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.k]);
                        }
                        sb.Append("k");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.r])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.r])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.r]);
                        }
                        sb.Append("r");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.b])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.b])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.b]);
                        }
                        sb.Append("b");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)Pieces.g])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)Pieces.g])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)Pieces.g]);
                        }
                        sb.Append("g");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)PieceTypes.S])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)PieceTypes.S])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)PieceTypes.S]);
                        }
                        sb.Append("s");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)PieceTypes.N])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)PieceTypes.N])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)PieceTypes.N]);
                        }
                        sb.Append("n");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)PieceTypes.L])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)PieceTypes.L])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)PieceTypes.L]);
                        }
                        sb.Append("l");
                    }

                    if (0 < ro_kyokumen1.MotiSu[(int)PieceTypes.P])
                    {
                        if (1 < ro_kyokumen1.MotiSu[(int)PieceTypes.P])
                        {
                            sb.Append(ro_kyokumen1.MotiSu[(int)PieceTypes.P]);
                        }
                        sb.Append("p");
                    }
                }

            }

            // 手目
            sb.Append(" ");
            sb.Append(ro_kyokumen1.Temezumi);

            return sb.ToString();
        }

    }
}
