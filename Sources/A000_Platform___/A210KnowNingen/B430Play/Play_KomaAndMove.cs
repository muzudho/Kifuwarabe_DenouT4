using System.Collections.Generic;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B410Collection.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B110GraphicLog.C500Util;
using Grayscale.A210KnowNingen.B170WordShogi.C260Operator;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C510UtilLogJson;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B430Play.C250Calc
{


    public abstract class Play_KomaAndMove
    {


        /// <summary>
        /// a - b = c
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logTag"></param>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> MinusMasus(
            ISky src_Sky_forLog,
            Maps_OneAndOne<Finger, SySet<SyElement>> a1,
            SySet<SyElement> b,
            ILogger errH_orNull
            )
        {
            //GraphicalLogUtil.Log(enableLog, "Thought_KomaAndMove#MinusMasus",
            //    "["+
            //    GraphicalLogUtil.JsonKyokumens_MultiKomabetuMasus(enableLog, siteiSky_forLog, a1, "a1") +
            //    "]"
            //    );


            Maps_OneAndOne<Finger, SySet<SyElement>> c = new Maps_OneAndOne<Finger, SySet<SyElement>>(a1);

            List<Finger> list_koma = c.ToKeyList();//調べたい側の全駒


            foreach (Finger selfKoma in list_koma)
            {
                SySet<SyElement> srcMasus = c.ElementAt(selfKoma);

                SySet<SyElement> minusedMasus = srcMasus.Minus_Closed(b, Util_SyElement_BinaryOperator.Dlgt_Equals_MasuNumber);

                // 差替え
                c.AddReplace(selfKoma, minusedMasus, false);//差分に差替えます。もともと無い駒なら何もしません。
            }

            return c;
        }

        /// <summary>
        /// a - b = c
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logTag"></param>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> Minus_OverThereMasus(
            ISky src_srcSky_forLog,
            Maps_OneAndOne<Finger, SySet<SyElement>> a,
            SySet<SyElement> b,
            ILogger errH_orNull
        )
        {
            Maps_OneAndOne<Finger, SySet<SyElement>> c = new Maps_OneAndOne<Finger, SySet<SyElement>>(a);

            bool enableLog = false;
            //if (null != errH_orNull)
            //{
            //    enableLog = errH_orNull.Logger.Enable;
            //}

            foreach (Finger selfKoma in c.ToKeyList())//調べたい側の全駒
            {
                SySet<SyElement> srcMasus = c.ElementAt(selfKoma);

                // a -overThere b するぜ☆
                Util_GraphicalLog.WriteHtml5(enableLog, "Thought_KomaAndMove Minus_OverThereMasus1",
                    "[\n" +
                    "    [\n" +
                    Util_FormatJson_LogGraphicEx.JsonElements_Masus(enableLog, srcMasus, "(1)引く前") +
                    "    ],\n" +
                    "],\n"
                    );
                SySet<SyElement> minusedMasus = srcMasus.Clone();
                minusedMasus.MinusMe_Opened(b, Util_SyElement_BinaryOperator.Dlgt_Equals_MasuNumber);

                // 差替え
                c.AddReplace(selfKoma, minusedMasus, false);//差分に差替えます。もともと無い駒なら何もしません。
            }

            Util_GraphicalLog.WriteHtml5(enableLog, "Thought_KomaAndMove Minus_OverThereMasus2",
                "[\n" +
                "    [\n" +
                Util_FormatJson_LogGraphicEx.JsonKyokumens_MultiKomabetuMasus(enableLog, src_srcSky_forLog, a, "(1)a") +
                Util_FormatJson_LogGraphicEx.JsonElements_Masus(enableLog, b, "(2)-overThere_b") +
                Util_FormatJson_LogGraphicEx.JsonKyokumens_MultiKomabetuMasus(enableLog, src_srcSky_forLog, c, "(3)＝c") +
                "    ],\n" +
                "],\n"
                );

            return c;
        }


    }


}
