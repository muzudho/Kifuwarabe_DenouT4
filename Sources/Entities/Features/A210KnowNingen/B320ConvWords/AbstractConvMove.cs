using Grayscale.Kifuwaragyoku.Entities.Features;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class AbstractConvMove
    {
        public static string ChangeMoveToKsStringForLog(Move move, Playerside pside_genTeban)
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
