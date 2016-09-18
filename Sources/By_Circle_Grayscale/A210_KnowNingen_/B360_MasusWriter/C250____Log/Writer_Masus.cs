using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B360_MasusWriter.C250____Writer
{
    public abstract class Writer_Masus
    {

        /// <summary>
        /// デバッグ用文字列を作ります。
        /// </summary>
        /// <param name="masus"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public static string Log_Masus(SySet<SyElement> masus, string memo)
        {
            StringBuilder sb = new StringBuilder();

            int errorCount = 0;

            // フォルスクリア
            bool[] ban81 = new bool[81];

            // フラグ立て
            foreach (New_Basho basho in masus.Elements)
            {
                if (Okiba.ShogiBan == Conv_Masu.ToOkiba(Masu_Honshogi.Masus_All[basho.MasuNumber]))
                {
                    ban81[basho.MasuNumber] = true;
                }
            }



            sb.AppendLine("...(^▽^)さて、局面は☆？");

            if (null != memo && "" != memo.Trim())
            {
                sb.AppendLine(memo);
            }

            sb.AppendLine("　９　８　７　６　５　４　３　２　１");
            sb.AppendLine("┏━┯━┯━┯━┯━┯━┯━┯━┯━┓");
            for (int dan = 1; dan <= 9; dan++)
            {
                sb.Append("┃");
                for (int suji = 9; suji >= 1; suji--)// 筋は左右逆☆
                {
                    SyElement masu = Conv_Masu.ToMasu_FromBanjoSujiDan( suji, dan);
                    if (Okiba.ShogiBan == Conv_Masu.ToOkiba(masu))
                    {
                        if (ban81[Conv_Masu.ToMasuHandle(masu)])
                        {
                            sb.Append("●");
                        }
                        else
                        {
                            sb.Append("  ");
                        }
                    }
                    else
                    {
                        errorCount++;
                        sb.Append("  ");
                    }


                    if (suji == 1)//１筋が最後だぜ☆
                    {
                        sb.Append("┃");
                        sb.AppendLine(Conv_Int.ToKanSuji(dan));
                    }
                    else
                    {
                        sb.Append("│");
                    }
                }

                if (dan == 9)
                {
                    sb.AppendLine("┗━┷━┷━┷━┷━┷━┷━┷━┷━┛");
                }
                else
                {
                    sb.AppendLine("┠─┼─┼─┼─┼─┼─┼─┼─┼─┨");
                }
            }


            // 後手駒台
            sb.Append("エラー数：");
            sb.AppendLine(errorCount.ToString());
            sb.AppendLine("...(^▽^)ﾄﾞｳﾀﾞｯﾀｶﾅ～☆");


            return sb.ToString();
        }

    }
}
