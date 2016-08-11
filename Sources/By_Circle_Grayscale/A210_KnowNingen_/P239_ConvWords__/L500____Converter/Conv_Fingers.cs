using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C250____Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P238_Seiza______.C250____Struct;
using Grayscale.P238_Seiza______.C500____Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P339_ConvKyokume.C500____Converter;

namespace Grayscale.P239_ConvWords__.C500____Converter
{
    public abstract class Conv_Fingers
    {

        /// <summary>
        /// フィンガー番号→駒→駒のある升の集合
        /// </summary>
        /// <param name="fingers"></param>
        /// <param name="src_Sky"></param>
        /// <returns></returns>
        public static SySet<SyElement> ToMasus(Fingers fingers, SkyConst src_Sky)
        {
            SySet<SyElement> masus = new SySet_Default<SyElement>("何かの升");

            foreach (Finger finger in fingers.Items)
            {
                src_Sky.AssertFinger(finger);
                Busstop koma = src_Sky.BusstopIndexOf(finger);


                masus.AddElement(Conv_Busstop.ToMasu( koma));
            }

            return masus;
        }
    }
}
