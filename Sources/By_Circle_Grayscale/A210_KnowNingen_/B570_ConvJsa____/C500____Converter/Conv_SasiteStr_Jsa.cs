﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B550_JsaFugo____.C250____Struct;
using Grayscale.A210_KnowNingen_.B550_JsaFugo____.C500____Util;
using Grayscale.A210_KnowNingen_.B560_JsaFugoWrit.C500____Writer;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter
{
    public abstract class Conv_SasiteStr_Jsa
    {
        /// <summary>
        /// 「▲７六歩」といった符号にして返します。
        /// </summary>
        /// <param name="node">keyで指し手の指定、かつ、１つ前のノードに移動するのに使います。</param>
        /// <param name="kyokumenWrapper">現局面です。</param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static string ToSasiteStr_Jsa(
            Node<Move, KyokumenWrapper> node,
            KwErrorHandler errH
            )
        {
            Komasyurui14 ks = Conv_Move.ToSrcKomasyurui(node.Key);

            JsaFugoImpl jsaFugo = Array_JsaFugoCreator15.ItemMethods[(int)ks](
                node.Key,
                node.Value,// kyokumenWrapper,
                errH);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

            return Util_Translator_JsaFugo.ToString_UseDou(jsaFugo, node);
        }

    }
}