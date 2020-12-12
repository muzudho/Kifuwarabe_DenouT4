using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;

namespace Grayscale.A210KnowNingen.B320ConvWords.C500Converter
{
    public abstract class Conv_Sasite
    {
        public static string Sasite_To_KsString_ForLog(Move move, Playerside pside_genTeban)
        {
            string result;

            bool errorCheck = ConvMove.ToErrorCheck(move);
            if (errorCheck)
            {
                result = "指し手が未設定か、エラー？";// "合法手はありません。";
                goto gt_EndMethod;
            }

            Komasyurui14 ks = ConvMove.ToDstKomasyurui(move);

            // 指し手を「△歩」といった形で。
            result = Util_Komasyurui14.ToNimoji(ks, pside_genTeban);

        gt_EndMethod:
            return result;
        }

    }
}
