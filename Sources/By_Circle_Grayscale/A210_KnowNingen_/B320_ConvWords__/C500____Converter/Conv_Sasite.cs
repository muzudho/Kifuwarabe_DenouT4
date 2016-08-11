using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter
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
