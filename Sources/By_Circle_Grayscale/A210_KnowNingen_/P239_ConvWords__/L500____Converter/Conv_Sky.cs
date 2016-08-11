using Grayscale.P212_ConvPside__.L500____Converter;
using Grayscale.P216_ZobrishHash.L500____Struct;
using Grayscale.P224_Sky________.L___500_Struct;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;

namespace Grayscale.P239_ConvWords__.L500____Converter
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
