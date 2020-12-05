using System.Collections.Generic;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C500Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500Struct;
using Grayscale.A210_KnowNingen_.B550_JsaFugo____.C250Struct;
using Grayscale.A210_KnowNingen_.B550_JsaFugo____.C500Util;
using Grayscale.A210_KnowNingen_.B560_JsaFugoWrit.C500____Writer;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500Converter;

namespace Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500Converter
{
    public abstract class Conv_SasiteStr_Jsa
    {
        /// <summary>
        /// 「▲７六歩」といった符号にして返します。
        /// </summary>
        /// <param name="node">keyで指し手の指定、かつ、１つ前のノードに移動するのに使います。</param>
        /// <param name="Sky">現局面です。</param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static string ToSasiteStr_Jsa(
            Move move,
            List<Move> honpuList,
            Sky positionA,
            KwLogger errH
            )
        {
            Komasyurui14 ks = Conv_Move.ToSrcKomasyurui(move);

            JsaFugoImpl jsaFugo = Array_JsaFugoCreator15.ItemMethods[(int)ks](
                move,
                positionA,
                errH);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

            return Util_Translator_JsaFugo.ToString_UseDou(jsaFugo, move, honpuList);
        }

    }
}
