using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P234_Komahaiyaku.L500____Util;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P335_Move_______.L___500_Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;

namespace Grayscale.P239_ConvWords__.L500____Converter
{
    public abstract class Conv_Sasite
    {
        /*
        /// <summary>
        /// FIXME:使ってない？
        /// </summary>
        /// <param name="sasite"></param>
        /// <returns></returns>
        public static string Sasite_To_KsString_ForLog(Move move)
        {
            return Util_Komasyurui14.ToIchimoji(Conv_Move.ToSrcKomasyurui(move));
        }
        */

        public static string Sasite_To_KsString_ForLog(Starbeamable sasite, Playerside pside_genTeban)
        {
            string result;

            if (null == sasite)
            {
                result = "合法手はありません。";
                goto gt_EndMethod;
            }

            RO_Star koma = Util_Starlightable.AsKoma(sasite.Now);

            // 指し手を「△歩」といった形で。
            result = Util_Komasyurui14.ToNimoji(Util_Komahaiyaku184.Syurui(koma.Haiyaku), pside_genTeban);

        gt_EndMethod:
            return result;
        }

    }
}
