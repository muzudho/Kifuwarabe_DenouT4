using System.Collections.Generic;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B550JsaFugo.C250Struct;
using Grayscale.A210KnowNingen.B550JsaFugo.C500Util;
using Grayscale.A210KnowNingen.B560_JsaFugoWrit.C500Writer;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;

namespace Grayscale.A210KnowNingen.B570ConvJsa.C500Converter
{
    public abstract class ConvMoveStrJsa
    {
        /// <summary>
        /// 「▲７六歩」といった符号にして返します。
        /// </summary>
        /// <param name="node">keyで指し手の指定、かつ、１つ前のノードに移動するのに使います。</param>
        /// <param name="ISky">現局面です。</param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static string ToMoveStrJsa(
            Move move,
            List<Move> honpuList,
            ISky positionA,
            ILogger errH
            )
        {
            Komasyurui14 ks = ConvMove.ToSrcKomasyurui(move);

            JsaFugoImpl jsaFugo = ArrayJsaFugoCreator15.ItemMethods[(int)ks](
                move,
                positionA,
                errH);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

            return Util_Translator_JsaFugo.ToString_UseDou(jsaFugo, move, honpuList);
        }

    }
}
