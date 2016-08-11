using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.C500____Util;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P339_ConvKyokume.C500____Converter;

namespace Grayscale.P239_ConvWords__.C500____Converter
{
    public abstract class Conv_Sasite
    {
        public static string Sasite_To_KsString_ForLog(Move move, Playerside pside_genTeban)
        {
            string result;

            bool errorCheck = Conv_Move.ToErrorCheck(move);
            if (errorCheck)
            {
                result = "指し手が未設定か、エラー？";// "合法手はありません。";
                goto gt_EndMethod;
            }

            Komasyurui14 ks = Conv_Move.ToDstKomasyurui(move);

            // 指し手を「△歩」といった形で。
            result = Util_Komasyurui14.ToNimoji(ks, pside_genTeban);

        gt_EndMethod:
            return result;
        }

    }
}
