﻿using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B220_ZobrishHash.C500____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C250____Struct;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C500____Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.P339_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter
{
    public abstract class Conv_Sky
    {

        /// <summary>
        /// 千日手判定用の、局面ハッシュを返します。
        /// 
        /// TODO: 持ち駒も判定したい。
        /// </summary>
        /// <returns></returns>
        public static ulong ToKyokumenHash(Sky sky)
        {
            ulong hash = 0;

            foreach (Finger fig in sky.Fingers_All().Items)
            {
                sky.AssertFinger(fig);
                Busstop koma = sky.BusstopIndexOf(fig);

                // 盤上の駒。 FIXME: 持ち駒はまだ見ていない。
                ulong value = Util_ZobristHashing.GetValue(
                    Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu(koma)),
                    Conv_Busstop.ToPlayerside(koma),
                    Conv_Busstop.ToKomasyurui(koma)
                    );

                hash ^= value;
            }

            return hash;
        }
    }
}