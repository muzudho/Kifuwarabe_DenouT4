﻿using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C500Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B320_ConvWords__.C500Converter
{
    public abstract class Conv_Fingers
    {

        /// <summary>
        /// フィンガー番号→駒→駒のある升の集合
        /// </summary>
        /// <param name="fingers"></param>
        /// <param name="src_Sky"></param>
        /// <returns></returns>
        public static SySet<SyElement> ToMasus(Fingers fingers, Sky src_Sky)
        {
            SySet<SyElement> masus = new SySet_Default<SyElement>("何かの升");

            foreach (Finger finger in fingers.Items)
            {
                src_Sky.AssertFinger(finger);
                Busstop koma = src_Sky.BusstopIndexOf(finger);


                masus.AddElement(Conv_Busstop.ToMasu(koma));
            }

            return masus;
        }
    }
}
