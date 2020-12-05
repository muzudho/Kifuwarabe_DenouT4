﻿using System.Collections.Generic;
using Grayscale.A060Application.B410Collection.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B320_ConvWords__.C500Converter
{
    /// <summary>
    /// 使ってない？
    /// </summary>
    public abstract class Conv_KomabetuMasus268
    {
        /// <summary>
        /// FIXME: 使ってない？
        /// 
        /// 変換『「駒→手」のコレクション』→『「駒、指し手」のペアのリスト』
        /// </summary>
        public static List<Couple<Finger, SyElement>> ToList(
            Maps_OneAndOne<Finger, SySet<SyElement>> km
            )
        {
            List<Couple<Finger, SyElement>> kmList = new List<Couple<Finger, SyElement>>();

            foreach (Finger koma in km.ToKeyList())
            {
                SySet<SyElement> masus = km.ElementAt(koma);

                foreach (SyElement masu in masus.Elements)
                {
                    // セットとして作っているので、重複エレメントは無いはず……☆
                    kmList.Add(new Couple<Finger, SyElement>(koma, masu));
                }
            }

            return kmList;
        }

    }
}
