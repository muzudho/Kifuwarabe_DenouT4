using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class ConvMoveStrJsa
    {
        /// <summary>
        /// 「▲７六歩」といった符号にして返します。
        /// </summary>
        /// <param name="node">keyで指し手の指定、かつ、１つ前のノードに移動するのに使います。</param>
        /// <param name="ISky">現局面です。</param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static string ToMoveStrJsa(
            Move move,
            List<Move> honpuList,
            IPosition positionA)
        {
            Komasyurui14 ks = ConvMove.ToSrcKomasyurui(move);

            JsaFugoImpl jsaFugo = ArrayJsaFugoCreator15.ItemMethods[(int)ks](
                move,
                positionA);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

            return Util_Translator_JsaFugo.ToString_UseDou(jsaFugo, move, honpuList);
        }

    }
}
