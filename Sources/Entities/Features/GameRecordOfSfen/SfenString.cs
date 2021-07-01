namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    using System.Text;
    using Grayscale.Kifuwaragyoku.Entities.Take1Base;

    /// <summary>
    /// SFEN文字列。
    /// </summary>
    public abstract class SfenString
    {
        public static string From(ISfenPosition1 pos1, bool white)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("sfen ");

            for (int dan = 1; dan <= 9; dan++)
            {
                int spaceCount = 0;

                for (int suji = 9; suji >= 1; suji--)
                {
                    // 将棋盤上のどこかにある駒？
                    string koma0 = pos1.Ban[suji, dan];

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
                    pos1.MotiSu[(int)Piece.K1] +
                    pos1.MotiSu[(int)Piece.R1] +
                    pos1.MotiSu[(int)Piece.B1] +
                    pos1.MotiSu[(int)Piece.G1] +
                    pos1.MotiSu[(int)Piece.S1] +
                    pos1.MotiSu[(int)Piece.N1] +
                    pos1.MotiSu[(int)Piece.L1] +
                    pos1.MotiSu[(int)Piece.P1] +
                    pos1.MotiSu[(int)Piece.K2] +
                    pos1.MotiSu[(int)Piece.R2] +
                    pos1.MotiSu[(int)Piece.B2] +
                    pos1.MotiSu[(int)Piece.G2] +
                    pos1.MotiSu[(int)Piece.S2] +
                    pos1.MotiSu[(int)Piece.N2] +
                    pos1.MotiSu[(int)Piece.L2] +
                    pos1.MotiSu[(int)Piece.P2]
                    )
                {
                    sb.Append("-");
                }
                else
                {
                    if (0 < pos1.MotiSu[(int)Piece.K1])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.K1])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.K1]);
                        }
                        sb.Append("K");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.R1])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.R1])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.R1]);
                        }
                        sb.Append("R");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.B1])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.B1])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.B1]);
                        }
                        sb.Append("B");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.G1])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.G1])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.G1]);
                        }
                        sb.Append("G");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.S1])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.S1])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.S1]);
                        }
                        sb.Append("S");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.N1])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.N1])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.N1]);
                        }
                        sb.Append("N");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.L1])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.L1])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.L1]);
                        }
                        sb.Append("L");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.P1])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.P1])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.P1]);
                        }
                        sb.Append("P");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.K2])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.K2])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.K2]);
                        }
                        sb.Append("k");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.R2])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.R2])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.R2]);
                        }
                        sb.Append("r");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.B2])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.B2])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.B2]);
                        }
                        sb.Append("b");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.G2])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.G2])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.G2]);
                        }
                        sb.Append("g");
                    }

                    if (0 < pos1.MotiSu[(int)PieceType.S])
                    {
                        if (1 < pos1.MotiSu[(int)PieceType.S])
                        {
                            sb.Append(pos1.MotiSu[(int)PieceType.S]);
                        }
                        sb.Append("s");
                    }

                    if (0 < pos1.MotiSu[(int)PieceType.N])
                    {
                        if (1 < pos1.MotiSu[(int)PieceType.N])
                        {
                            sb.Append(pos1.MotiSu[(int)PieceType.N]);
                        }
                        sb.Append("n");
                    }

                    if (0 < pos1.MotiSu[(int)PieceType.L])
                    {
                        if (1 < pos1.MotiSu[(int)PieceType.L])
                        {
                            sb.Append(pos1.MotiSu[(int)PieceType.L]);
                        }
                        sb.Append("l");
                    }

                    if (0 < pos1.MotiSu[(int)PieceType.P])
                    {
                        if (1 < pos1.MotiSu[(int)PieceType.P])
                        {
                            sb.Append(pos1.MotiSu[(int)PieceType.P]);
                        }
                        sb.Append("p");
                    }
                }

            }

            // 手目
            sb.Append(" ");
            sb.Append(pos1.Temezumi);

            return sb.ToString();
        }

    }
}
