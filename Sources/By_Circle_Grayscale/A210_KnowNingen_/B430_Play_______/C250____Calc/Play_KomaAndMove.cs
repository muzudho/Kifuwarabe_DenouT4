using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C260____Operator;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C510____UtilLogJson;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B430_Play_______.C250____Calc
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
            Sky src_Sky_forLog,
            Maps_OneAndOne<Finger, SySet<SyElement>> a1,
            SySet<SyElement> b,
            KwLogger errH_orNull
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

                SySet<SyElement> minusedMasus = srcMasus.Minus_Closed( b, Util_SyElement_BinaryOperator.Dlgt_Equals_MasuNumber);

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
            Sky src_srcSky_forLog,
            Maps_OneAndOne<Finger, SySet<SyElement>> a,
            SySet<SyElement> b,
            KwLogger errH_orNull
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
                    "    ],\n"+
                    "],\n"
                    );
                SySet<SyElement> minusedMasus = srcMasus.Clone();
                minusedMasus.MinusMe_Opened(b, Util_SyElement_BinaryOperator.Dlgt_Equals_MasuNumber);

                // 差替え
                c.AddReplace(selfKoma, minusedMasus, false);//差分に差替えます。もともと無い駒なら何もしません。
            }

            Util_GraphicalLog.WriteHtml5(enableLog, "Thought_KomaAndMove Minus_OverThereMasus2",
                "[\n"+
                "    [\n" +
                Util_FormatJson_LogGraphicEx.JsonKyokumens_MultiKomabetuMasus(enableLog, src_srcSky_forLog, a, "(1)a") +
                Util_FormatJson_LogGraphicEx.JsonElements_Masus(enableLog, b, "(2)-overThere_b") +
                Util_FormatJson_LogGraphicEx.JsonKyokumens_MultiKomabetuMasus(enableLog, src_srcSky_forLog, c, "(3)＝c") +
                "    ],\n"+
                "],\n"
                );

            return c;
        }


    }


}
