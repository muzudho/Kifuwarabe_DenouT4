using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter
{
    /// <summary>
    /// 将棋盤上の情報を数えます。
    /// </summary>
    public abstract class Conv_Shogiban
    {
        public static string ToLog_Type2(ShogibanImpl shogiban, Sky sky, Move move)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Conv_Playerside.ToLog_Kanji(sky.GetKaisiPside(move)));
            sb.AppendLine(sky.Temezumi+"手目済");
            sb.AppendLine(Conv_Move.ToLog(move));


            sb.Append(Conv_Shogiban.ToLog(shogiban));

            return sb.ToString();
        }
        public static string ToLog(ShogibanImpl shogiban)
        {
            StringBuilder sb = new StringBuilder();

            // 後手の持ち駒
            for (int iMoti=(int)Pieces.StartGote; iMoti< (int)Pieces.NumGote; iMoti++)
            {
                if (0 < shogiban.MotiSu[iMoti])
                {
                    sb.Append(Util_Komasyurui14.IchimojiPieces[iMoti]);
                    if (1 < shogiban.MotiSu[iMoti])
                    {
                        sb.Append(shogiban.MotiSu[iMoti]);
                    }
                }
            }
            sb.AppendLine();

            // 将棋盤
            sb.AppendLine("┌──┬──┬──┬──┬──┬──┬──┬──┬──┐");
            for (int dan = 1; dan<10; dan++)
            {
                sb.Append("│");
                for (int suji = 9; 0 < suji; suji--)
                {
                    int masuNumber = Conv_Masu.ToMasuHandle_FromBanjoSujiDan(suji, dan);
                    if(shogiban.ContainsBanjoKoma(masuNumber))
                    {
                        Busstop koma = shogiban.GetBanjoKomaFromMasu(masuNumber);
                        string errorMessage = shogiban.GetErrorMessage(masuNumber);
                        Komasyurui14 ks = Conv_Busstop.ToKomasyurui(koma);
                        Playerside pside = Conv_Busstop.ToPlayerside(koma);

                        if (Playerside.P1 == pside)
                        {
                            sb.Append(Util_Komasyurui14.NimojiSente[(int)ks]);
                        }
                        else
                        {
                            sb.Append(Util_Komasyurui14.NimojiGote[(int)ks]);
                        }

                        if (""!= errorMessage)
                        {
                            sb.Append(errorMessage);
                        }
                    }
                    else
                    {
                        sb.Append("　　");
                    }
                    sb.Append("│");
                }
                sb.AppendLine();

                if (dan < 9)
                {
                    sb.AppendLine("├──┼──┼──┼──┼──┼──┼──┼──┼──┤");
                }
            }
            sb.AppendLine("└──┴──┴──┴──┴──┴──┴──┴──┴──┘");

            // 先手の持ち駒
            for (int iMoti = (int)Pieces.StartSente; iMoti < (int)Pieces.NumSente; iMoti++)
            {
                if (0 < shogiban.MotiSu[iMoti])
                {
                    sb.Append(Util_Komasyurui14.IchimojiPieces[iMoti]);
                    if (1 < shogiban.MotiSu[iMoti])
                    {
                        sb.Append(shogiban.MotiSu[iMoti]);
                    }
                }
            }
            sb.AppendLine();

            return sb.ToString();
        }

        public static string ToStartposDanString(int leftestMasu, ShogibanImpl shogiban)
        {
            //Debug.Assert(src_Sky.Count == 40, "sourceSky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            StringBuilder sb = new StringBuilder();

            List<Busstop> list = new List<Busstop>();
            for (int masuNumber = leftestMasu; masuNumber >= 0; masuNumber -= 9)
            {
                if (shogiban.ContainsBanjoKoma(masuNumber))
                {
                    list.Add(shogiban.GetBanjoKomaFromMasu(masuNumber));
                }
                else
                {
                    list.Add(Busstop.Empty);
                }
            }

            int spaceCount = 0;
            foreach (Busstop koma in list)
            {
                if (koma == Busstop.Empty)
                {
                    spaceCount++;
                }
                else
                {
                    if (0 < spaceCount)
                    {
                        sb.Append(spaceCount.ToString());
                        spaceCount = 0;
                    }

                    // 駒の種類だけだと先手ゴマになってしまう。先後も判定した。
                    switch(Conv_Busstop.ToPlayerside( koma))
                    {
                        case Playerside.P1:
                            sb.Append(Util_Komasyurui14.Sfen1P[(int)Conv_Busstop.ToKomasyurui( koma)]);
                            break;
                        case Playerside.P2:
                            sb.Append(Util_Komasyurui14.Sfen2P[(int)Conv_Busstop.ToKomasyurui(koma)]);
                            break;
                        default:
                            throw new Exception("ない手番");
                    }
                }
            }
            if (0 < spaceCount)
            {
                sb.Append(spaceCount.ToString());
                spaceCount = 0;
            }

            return sb.ToString();
        }


    }
}
