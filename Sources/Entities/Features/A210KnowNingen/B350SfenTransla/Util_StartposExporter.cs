namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    using System.Text;
    using Grayscale.Kifuwaragyoku.Entities.Take1Base;

    public abstract class Util_StartposExporter
    {
        /// <summary>
        /// 「position [sfen ＜sfenstring＞ | startpos ] moves ＜move1＞ ... ＜movei＞」の中の、
        /// ＜sfenstring＞の部分を作成します。
        /// </summary>
        /// <returns></returns>
        public static string ToSfenstring(ShogibanImpl shogiban, bool outputKomabukuro_ForDebug)
        {
            StringBuilder sb = new StringBuilder();

            // 1段目
            {
                //マス番号は、72,63,54,45,36,27,18,9,0。
                sb.Append(Conv_Shogiban.ToStartposDanString(72, shogiban));
            }
            sb.Append("/");

            // 2段目
            {
                //マス番号は、73,64,55,46,37,28,19,10,1。
                sb.Append(Conv_Shogiban.ToStartposDanString(73, shogiban));
            }
            sb.Append("/");

            // 3段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(74, shogiban));
            }
            sb.Append("/");

            // 4段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(75, shogiban));
            }
            sb.Append("/");

            // 5段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(76, shogiban));
            }
            sb.Append("/");

            // 6段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(77, shogiban));
            }
            sb.Append("/");

            // 7段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(78, shogiban));
            }
            sb.Append("/");

            // 8段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(79, shogiban));
            }
            sb.Append("/");

            // 9段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(80, shogiban));
            }

            // 先後
            switch (shogiban.KaisiPside)
            {
                case Playerside.P1: sb.Append(" b"); break;
                case Playerside.P2: sb.Append(" w"); break;
                default: sb.Append(" ?"); break;
            }

            // 持ち駒
            if (
                shogiban.MotiSu[(int)Piece.P1] < 1
                && shogiban.MotiSu[(int)Piece.L1] < 1
                && shogiban.MotiSu[(int)Piece.N1] < 1
                && shogiban.MotiSu[(int)Piece.S1] < 1
                && shogiban.MotiSu[(int)Piece.G1] < 1
                && shogiban.MotiSu[(int)Piece.K1] < 1
                && shogiban.MotiSu[(int)Piece.R1] < 1
                && shogiban.MotiSu[(int)Piece.B1] < 1
                && shogiban.MotiSu[(int)Piece.P2] < 1
                && shogiban.MotiSu[(int)Piece.L2] < 1
                && shogiban.MotiSu[(int)Piece.N2] < 1
                && shogiban.MotiSu[(int)Piece.S2] < 1
                && shogiban.MotiSu[(int)Piece.G2] < 1
                && shogiban.MotiSu[(int)Piece.K2] < 1
                && shogiban.MotiSu[(int)Piece.R2] < 1
                && shogiban.MotiSu[(int)Piece.B2] < 1
                )
            {
                sb.Append(" -");
            }
            else
            {
                sb.Append(" ");

                // 先手持ち駒
                //王
                if (0 < shogiban.MotiSu[(int)Piece.K1])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.K1])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.K1]);
                    }
                    sb.Append("K");
                }

                //飛車
                if (0 < shogiban.MotiSu[(int)Piece.R1])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.R1])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.R1]);
                    }
                    sb.Append("R");
                }

                //角
                if (0 < shogiban.MotiSu[(int)Piece.B1])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.B1])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.B1]);
                    }
                    sb.Append("B");
                }

                //金
                if (0 < shogiban.MotiSu[(int)Piece.G1])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.G1])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.G1]);
                    }
                    sb.Append("G");
                }

                //銀
                if (0 < shogiban.MotiSu[(int)Piece.S1])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.S1])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.S1]);
                    }
                    sb.Append("S");
                }

                //桂馬
                if (0 < shogiban.MotiSu[(int)Piece.N1])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.N1])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.N1]);
                    }
                    sb.Append("N");
                }

                //香車
                if (0 < shogiban.MotiSu[(int)Piece.L1])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.L1])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.L1]);
                    }
                    sb.Append("L");
                }

                //歩
                if (0 < shogiban.MotiSu[(int)Piece.P1])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.P1])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.P1]);
                    }
                    sb.Append("P");
                }

                // 後手持ち駒
                //王
                if (0 < shogiban.MotiSu[(int)Piece.K2])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.K2])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.K2]);
                    }
                    sb.Append("k");
                }

                //飛車
                if (0 < shogiban.MotiSu[(int)Piece.R2])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.R2])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.R2]);
                    }
                    sb.Append("r");
                }

                //角
                if (0 < shogiban.MotiSu[(int)Piece.B2])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.B2])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.B2]);
                    }
                    sb.Append("b");
                }

                //金
                if (0 < shogiban.MotiSu[(int)Piece.G2])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.G2])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.G2]);
                    }
                    sb.Append("g");
                }

                //銀
                if (0 < shogiban.MotiSu[(int)Piece.S2])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.S2])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.S2]);
                    }
                    sb.Append("s");
                }

                //桂馬
                if (0 < shogiban.MotiSu[(int)Piece.N2])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.N2])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.N2]);
                    }
                    sb.Append("n");
                }

                //香車
                if (0 < shogiban.MotiSu[(int)Piece.L2])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.L2])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.L2]);
                    }
                    sb.Append("l");
                }

                //歩
                if (0 < shogiban.MotiSu[(int)Piece.P2])
                {
                    if (1 < shogiban.MotiSu[(int)Piece.P2])
                    {
                        sb.Append(shogiban.MotiSu[(int)Piece.P2]);
                    }
                    sb.Append("p");
                }
            }

            // 1固定
            sb.Append(" 1");


            // デバッグ表示用
            if (outputKomabukuro_ForDebug)
            {
                // 駒袋
                sb.Append("(");
                //王
                if (0 < shogiban.KomabukuroSu[(int)PieceType.K])
                {
                    sb.Append("K");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceType.K]);
                }

                //飛車
                if (0 < shogiban.KomabukuroSu[(int)PieceType.R])
                {
                    sb.Append("R");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceType.R]);
                }

                //角
                if (0 < shogiban.KomabukuroSu[(int)PieceType.B])
                {
                    sb.Append("B");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceType.B]);
                }

                //金
                if (0 < shogiban.KomabukuroSu[(int)PieceType.G])
                {
                    sb.Append("G");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceType.G]);
                }

                //銀
                if (0 < shogiban.KomabukuroSu[(int)PieceType.S])
                {
                    sb.Append("S");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceType.S]);
                }

                //桂馬
                if (0 < shogiban.KomabukuroSu[(int)PieceType.N])
                {
                    sb.Append("N");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceType.N]);
                }

                //香車
                if (0 < shogiban.KomabukuroSu[(int)PieceType.L])
                {
                    sb.Append("L");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceType.L]);
                }

                //歩
                if (0 < shogiban.KomabukuroSu[(int)PieceType.P])
                {
                    sb.Append("P");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceType.P]);
                }

                sb.Append(")");
            }


            string sfenstring = sb.ToString();

            // 平手初期局面
            if ("sfen lnsgkgsnl/1r5b1/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL w - 1" == sfenstring)
            {
                sfenstring = "startpos";
            }

            return sfenstring;
        }
    }
}
