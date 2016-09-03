using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B350_SfenTransla.C500____Util
{
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
                sb.Append(Conv_Shogiban.ToStartposDanString(72,shogiban));
            }
            sb.Append("/");

            // 2段目
            {
                //マス番号は、73,64,55,46,37,28,19,10,1。
                sb.Append(Conv_Shogiban.ToStartposDanString(73,shogiban));
            }
            sb.Append("/");

            // 3段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(74,shogiban));
            }
            sb.Append("/");

            // 4段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(75,shogiban));
            }
            sb.Append("/");

            // 5段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(76,shogiban));
            }
            sb.Append("/");

            // 6段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(77,shogiban));
            }
            sb.Append("/");

            // 7段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(78,shogiban));
            }
            sb.Append("/");

            // 8段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(79,shogiban));
            }
            sb.Append("/");

            // 9段目
            {
                sb.Append(Conv_Shogiban.ToStartposDanString(80,shogiban));
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
                shogiban.MotiSu[(int)Pieces.P] < 1
                && shogiban.MotiSu[(int)Pieces.L] < 1
                && shogiban.MotiSu[(int)Pieces.N] < 1
                && shogiban.MotiSu[(int)Pieces.S] < 1
                && shogiban.MotiSu[(int)Pieces.G] < 1
                && shogiban.MotiSu[(int)Pieces.K] < 1
                && shogiban.MotiSu[(int)Pieces.R] < 1
                && shogiban.MotiSu[(int)Pieces.B] < 1
                && shogiban.MotiSu[(int)Pieces.p] < 1
                && shogiban.MotiSu[(int)Pieces.l] < 1
                && shogiban.MotiSu[(int)Pieces.n] < 1
                && shogiban.MotiSu[(int)Pieces.s] < 1
                && shogiban.MotiSu[(int)Pieces.g] < 1
                && shogiban.MotiSu[(int)Pieces.k] < 1
                && shogiban.MotiSu[(int)Pieces.r] < 1
                && shogiban.MotiSu[(int)Pieces.b] < 1
                )
            {
                sb.Append(" -");
            }
            else
            {
                sb.Append(" ");

                // 先手持ち駒
                //王
                if (0 < shogiban.MotiSu[(int)Pieces.K])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.K])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.K]);
                    }
                    sb.Append("K");
                }

                //飛車
                if (0 < shogiban.MotiSu[(int)Pieces.R])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.R])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.R]);
                    }
                    sb.Append("R");
                }

                //角
                if (0 < shogiban.MotiSu[(int)Pieces.B])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.B])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.B]);
                    }
                    sb.Append("B");
                }

                //金
                if (0 < shogiban.MotiSu[(int)Pieces.G])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.G])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.G]);
                    }
                    sb.Append("G");
                }

                //銀
                if (0 < shogiban.MotiSu[(int)Pieces.S])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.S])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.S]);
                    }
                    sb.Append("S");
                }

                //桂馬
                if (0 < shogiban.MotiSu[(int)Pieces.N])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.N])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.N]);
                    }
                    sb.Append("N");
                }

                //香車
                if (0 < shogiban.MotiSu[(int)Pieces.L])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.L])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.L]);
                    }
                    sb.Append("L");
                }

                //歩
                if (0 < shogiban.MotiSu[(int)Pieces.P])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.P])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.P]);
                    }
                    sb.Append("P");
                }

                // 後手持ち駒
                //王
                if (0 < shogiban.MotiSu[(int)Pieces.k])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.k])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.k]);
                    }
                    sb.Append("k");
                }

                //飛車
                if (0 < shogiban.MotiSu[(int)Pieces.r])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.r])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.r]);
                    }
                    sb.Append("r");
                }

                //角
                if (0 < shogiban.MotiSu[(int)Pieces.b])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.b])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.b]);
                    }
                    sb.Append("b");
                }

                //金
                if (0 < shogiban.MotiSu[(int)Pieces.g])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.g])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.g]);
                    }
                    sb.Append("g");
                }

                //銀
                if (0 < shogiban.MotiSu[(int)Pieces.s])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.s])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.s]);
                    }
                    sb.Append("s");
                }

                //桂馬
                if (0 < shogiban.MotiSu[(int)Pieces.n])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.n])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.n]);
                    }
                    sb.Append("n");
                }

                //香車
                if (0 < shogiban.MotiSu[(int)Pieces.l])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.l])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.l]);
                    }
                    sb.Append("l");
                }

                //歩
                if (0 < shogiban.MotiSu[(int)Pieces.p])
                {
                    if (1 < shogiban.MotiSu[(int)Pieces.p])
                    {
                        sb.Append(shogiban.MotiSu[(int)Pieces.p]);
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
                if (0 < shogiban.KomabukuroSu[(int)PieceTypes.K])
                {
                    sb.Append("K");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceTypes.K]);
                }

                //飛車
                if (0 < shogiban.KomabukuroSu[(int)PieceTypes.R])
                {
                    sb.Append("R");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceTypes.R]);
                }

                //角
                if (0 < shogiban.KomabukuroSu[(int)PieceTypes.B])
                {
                    sb.Append("B");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceTypes.B]);
                }

                //金
                if (0 < shogiban.KomabukuroSu[(int)PieceTypes.G])
                {
                    sb.Append("G");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceTypes.G]);
                }

                //銀
                if (0 < shogiban.KomabukuroSu[(int)PieceTypes.S])
                {
                    sb.Append("S");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceTypes.S]);
                }

                //桂馬
                if (0 < shogiban.KomabukuroSu[(int)PieceTypes.N])
                {
                    sb.Append("N");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceTypes.N]);
                }

                //香車
                if (0 < shogiban.KomabukuroSu[(int)PieceTypes.L])
                {
                    sb.Append("L");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceTypes.L]);
                }

                //歩
                if (0 < shogiban.KomabukuroSu[(int)PieceTypes.P])
                {
                    sb.Append("P");
                    sb.Append(shogiban.KomabukuroSu[(int)PieceTypes.P]);
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
