using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C250____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using System;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter
{
    public abstract class Conv_KifuNode
    {
        /// <summary>
        /// 表形式の局面データを出力します。SFENとの親和性高め。
        /// </summary>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static RO_Kyokumen1_ForFormat ToRO_Kyokumen1(Sky src_Sky, KwLogger errH)
        {
            RO_Kyokumen1_ForFormat ro_Kyokumen1 = new RO_Kyokumen1_ForFormatImpl();

            // 将棋盤
            for (int suji = 1; suji < 10; suji++)
            {
                for (int dan = 1; dan < 10; dan++)
                {
                    Finger koma0 = Util_Sky_FingersQuery.InMasuNow_Old(
                        src_Sky, Conv_Masu.ToMasu_FromBanjoSujiDan( suji, dan)
                        ).ToFirst();

                    if (Fingers.Error_1 != koma0)
                    {
                        src_Sky.AssertFinger(koma0);
                        Busstop koma1 = src_Sky.BusstopIndexOf(koma0);

                        ro_Kyokumen1.Ban[suji,dan] = Util_Komasyurui14.SfenText(
                            Conv_Busstop.ToKomasyurui(koma1),
                            Conv_Busstop.ToPlayerside( koma1)
                            );
                    }
                }
            }

            // 持ち駒の枚数
            int[] motiSu;
            Util_Sky_CountQuery.CountMoti(
                src_Sky,
                out motiSu,
                errH
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
        /// <param name="errH"></param>
        /// <returns></returns>
        public static string ToSfenstring(Sky src_Sky, Playerside pside, KwLogger errH)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("sfen ");

            for (int dan = 1; dan <= 9; dan++)
            {
                int spaceCount = 0;

                for (int suji = 9; suji >= 1; suji--)
                {
                    // 将棋盤上のどこかにある駒？
                    Finger koma0 = Util_Sky_FingersQuery.InMasuNow_Old(
                        src_Sky, Conv_Masu.ToMasu_FromBanjoSujiDan( suji, dan)
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
                            Conv_Busstop.ToPlayerside( koma1)
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
                Util_Sky_CountQuery.CountMoti(
                    src_Sky,
                    out motiSu,
                    errH
                    );

                if (0 == motiSu[(int)Pieces.K] +
                    motiSu[(int)Pieces.R] +
                    motiSu[(int)Pieces.B] +
                    motiSu[(int)Pieces.G] +
                    motiSu[(int)Pieces.S] +
                    motiSu[(int)Pieces.N] +
                    motiSu[(int)Pieces.L] +
                    motiSu[(int)Pieces.P] +
                    motiSu[(int)Pieces.k] +
                    motiSu[(int)Pieces.r] +
                    motiSu[(int)Pieces.b] +
                    motiSu[(int)Pieces.g] +
                    motiSu[(int)Pieces.s] +
                    motiSu[(int)Pieces.n] +
                    motiSu[(int)Pieces.l] +
                    motiSu[(int)Pieces.p]
                    )
                {
                    sb.Append("-");
                }
                else
                {
                    if (0 < motiSu[(int)Pieces.K])
                    {
                        if (1 < motiSu[(int)Pieces.K])
                        {
                            sb.Append(motiSu[(int)Pieces.K]);
                        }
                        sb.Append("K");
                    }

                    if (0 < motiSu[(int)Pieces.R])
                    {
                        if (1 < motiSu[(int)Pieces.R])
                        {
                            sb.Append(motiSu[(int)Pieces.R]);
                        }
                        sb.Append("R");
                    }

                    if (0 < motiSu[(int)Pieces.B])
                    {
                        if (1 < motiSu[(int)Pieces.B])
                        {
                            sb.Append(motiSu[(int)Pieces.B]);
                        }
                        sb.Append("B");
                    }

                    if (0 < motiSu[(int)Pieces.G])
                    {
                        if (1 < motiSu[(int)Pieces.G])
                        {
                            sb.Append(motiSu[(int)Pieces.G]);
                        }
                        sb.Append("G");
                    }

                    if (0 < motiSu[(int)Pieces.S])
                    {
                        if (1 < motiSu[(int)Pieces.S])
                        {
                            sb.Append(motiSu[(int)Pieces.S]);
                        }
                        sb.Append("S");
                    }

                    if (0 < motiSu[(int)Pieces.N])
                    {
                        if (1 < motiSu[(int)Pieces.N])
                        {
                            sb.Append(motiSu[(int)Pieces.N]);
                        }
                        sb.Append("N");
                    }

                    if (0 < motiSu[(int)Pieces.L])
                    {
                        if (1 < motiSu[(int)Pieces.L])
                        {
                            sb.Append(motiSu[(int)Pieces.L]);
                        }
                        sb.Append("L");
                    }

                    if (0 < motiSu[(int)Pieces.P])
                    {
                        if (1 < motiSu[(int)Pieces.P])
                        {
                            sb.Append(motiSu[(int)Pieces.P]);
                        }
                        sb.Append("P");
                    }

                    if (0 < motiSu[(int)Pieces.k])
                    {
                        if (1 < motiSu[(int)Pieces.k])
                        {
                            sb.Append(motiSu[(int)Pieces.k]);
                        }
                        sb.Append("k");
                    }

                    if (0 < motiSu[(int)Pieces.r])
                    {
                        if (1 < motiSu[(int)Pieces.r])
                        {
                            sb.Append(motiSu[(int)Pieces.r]);
                        }
                        sb.Append("r");
                    }

                    if (0 < motiSu[(int)Pieces.b])
                    {
                        if (1 < motiSu[(int)Pieces.b])
                        {
                            sb.Append(motiSu[(int)Pieces.b]);
                        }
                        sb.Append("b");
                    }

                    if (0 < motiSu[(int)Pieces.g])
                    {
                        if (1 < motiSu[(int)Pieces.g])
                        {
                            sb.Append(motiSu[(int)Pieces.g]);
                        }
                        sb.Append("g");
                    }

                    if (0 < motiSu[(int)Pieces.s])
                    {
                        if (1 < motiSu[(int)Pieces.s])
                        {
                            sb.Append(motiSu[(int)Pieces.s]);
                        }
                        sb.Append("s");
                    }

                    if (0 < motiSu[(int)Pieces.n])
                    {
                        if (1 < motiSu[(int)Pieces.n])
                        {
                            sb.Append(motiSu[(int)Pieces.n]);
                        }
                        sb.Append("n");
                    }

                    if (0 < motiSu[(int)Pieces.l])
                    {
                        if (1 < motiSu[(int)Pieces.l])
                        {
                            sb.Append(motiSu[(int)Pieces.l]);
                        }
                        sb.Append("l");
                    }

                    if (0 < motiSu[(int)Pieces.p])
                    {
                        if (1 < motiSu[(int)Pieces.p])
                        {
                            sb.Append(motiSu[(int)Pieces.p]);
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
