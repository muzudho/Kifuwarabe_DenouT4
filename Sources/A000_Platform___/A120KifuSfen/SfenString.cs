using System.Text;
using Grayscale.A120KifuSfen;

namespace Grayscale.A120KifuSfen
{
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
                    pos1.MotiSu[(int)Piece.K] +
                    pos1.MotiSu[(int)Piece.R] +
                    pos1.MotiSu[(int)Piece.B] +
                    pos1.MotiSu[(int)Piece.G] +
                    pos1.MotiSu[(int)Piece.S] +
                    pos1.MotiSu[(int)Piece.N] +
                    pos1.MotiSu[(int)Piece.L] +
                    pos1.MotiSu[(int)Piece.P] +
                    pos1.MotiSu[(int)Piece.k] +
                    pos1.MotiSu[(int)Piece.r] +
                    pos1.MotiSu[(int)Piece.b] +
                    pos1.MotiSu[(int)Piece.g] +
                    pos1.MotiSu[(int)Piece.s] +
                    pos1.MotiSu[(int)Piece.n] +
                    pos1.MotiSu[(int)Piece.l] +
                    pos1.MotiSu[(int)Piece.p]
                    )
                {
                    sb.Append("-");
                }
                else
                {
                    if (0 < pos1.MotiSu[(int)Piece.K])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.K])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.K]);
                        }
                        sb.Append("K");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.R])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.R])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.R]);
                        }
                        sb.Append("R");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.B])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.B])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.B]);
                        }
                        sb.Append("B");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.G])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.G])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.G]);
                        }
                        sb.Append("G");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.S])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.S])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.S]);
                        }
                        sb.Append("S");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.N])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.N])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.N]);
                        }
                        sb.Append("N");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.L])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.L])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.L]);
                        }
                        sb.Append("L");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.P])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.P])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.P]);
                        }
                        sb.Append("P");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.k])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.k])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.k]);
                        }
                        sb.Append("k");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.r])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.r])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.r]);
                        }
                        sb.Append("r");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.b])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.b])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.b]);
                        }
                        sb.Append("b");
                    }

                    if (0 < pos1.MotiSu[(int)Piece.g])
                    {
                        if (1 < pos1.MotiSu[(int)Piece.g])
                        {
                            sb.Append(pos1.MotiSu[(int)Piece.g]);
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
