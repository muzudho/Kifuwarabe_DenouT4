using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;

using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C505____ConvLogJson;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C510____UtilLogJson
{
    public abstract class Util_FormatJson_LogGraphicEx
    {
        /// <summary>
        /// 駒別マスをJSON化します。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="src_Sky_base"></param>
        /// <param name="km_sasite"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static string JsonKyokumens_MultiKomabetuMasus(bool enableLog, Sky src_Sky_base, Maps_OneAndOne<Finger, SySet<SyElement>> km_sasite, string comment)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            km_sasite.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                // 駒１つ
                src_Sky_base.AssertFinger(key);
                Busstop koma = src_Sky_base.BusstopIndexOf(key);

                Komasyurui14 ks14 = Conv_Busstop.ToKomasyurui(koma);

                sb.AppendLine("            [");

                // マスの色
                sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },");

                // 全マス
                foreach (New_Basho masu in value.Elements)
                {
                    sb.AppendLine("                { act:\"drawMasu\" , masu:" + masu.MasuNumber + " },");
                }


                string komaImg = Util_Converter_LogGraphicEx.Finger_ToString(src_Sky_base, key, "");
                sb.AppendLine("                { act:\"drawImg\", img:\"" + komaImg + "\", masu: " + Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu( koma)) + " },");//FIXME:おかしい？

                // コメント
                sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },");

                sb.AppendLine("            ],");

            });

        gt_EndMethod:
            return sb.ToString();
        }


        /// <summary>
        /// ノードをJSON化します。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="positionA_base"></param>
        /// <param name="moveNode"></param>
        /// <param name="comment"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static string JsonElements_Node(
            bool enableLog, Sky positionA_base, MoveNode moveNode, string comment, KwLogger logger)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            SyElement dstMasu = Conv_Move.ToDstMasu(moveNode.Key);
            Komasyurui14 ks14 = Conv_Move.ToDstKomasyurui(moveNode.Key);

            Finger finger = Util_Sky_FingersQuery.InMasuNow_Old(positionA_base,
                Conv_Move.ToSrcMasu(moveNode.Key, positionA_base)
                ).ToFirst();



            //sb.AppendLine("            [");

            // マスの色
            sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },");

            // マス
            sb.AppendLine("                { act:\"drawMasu\" , masu:" + Conv_Masu.ToMasuHandle(dstMasu) + " },");


            string komaImg = Util_Converter_LogGraphicEx.Finger_ToString(positionA_base, finger, "");
            sb.AppendLine("                { act:\"drawImg\", img:\"" + komaImg + "\", masu: " + Conv_Masu.ToMasuHandle(dstMasu) + " },");//FIXME:おかしい？

            // コメント
            sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },");

            //sb.AppendLine("            ],");

        gt_EndMethod:
            return sb.ToString();
        }


        /// <summary>
        /// ハブ･ノードの次ノード・リストをJSON化します。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="positionA_base"></param>
        /// <param name="hubMoveNode"></param>
        /// <param name="comment"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static string JsonKyokumens_NextNodes(bool enableLog, Sky positionA_base, MoveNode hubMoveNode, string comment, KwLogger logger)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            hubMoveNode.Children1.Foreach_ChildNodes5((Move move, ref bool toBreak) =>
            {

                SyElement srcMasu = Conv_Move.ToSrcMasu(move, positionA_base);
                SyElement dstMasu = Conv_Move.ToDstMasu(move);

                Finger srcKoma2 = Util_Sky_FingersQuery.InMasuNow_Old(positionA_base, srcMasu).ToFirst();

                Komasyurui14 dstKs14 = Conv_Move.ToDstKomasyurui(move);

                sb.AppendLine("            [");

                // マスの色
                sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },");

                // マス
                sb.AppendLine("                { act:\"drawMasu\" , masu:" + Conv_Masu.ToMasuHandle(dstMasu) + " },");


                string komaImg = Util_Converter_LogGraphicEx.Finger_ToString(positionA_base, srcKoma2, "");
                sb.AppendLine("                { act:\"drawImg\", img:\"" + komaImg + "\", masu: " + Conv_Masu.ToMasuHandle(dstMasu) + " },");//FIXME:おかしい？

                // コメント
                sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },");

                sb.AppendLine("            ],");
            });

        gt_EndMethod:
            return sb.ToString();
        }

        /// <summary>
        /// 用途例：持ち駒を確認するために使います。
        /// </summary>
        /// <param name="hkomas_gen_MOTI"></param>
        /// <returns></returns>
        public static string JsonElements_KomaHandles(bool enableLog, Sky src_Sky, List<int> hKomas, string comment)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            //sb.AppendLine("            [");
            sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },");


            foreach (int hKoma in hKomas)
            {
                src_Sky.AssertFinger(hKoma);
                Busstop koma = src_Sky.BusstopIndexOf(hKoma);


                string komaImg = Util_Converter_LogGraphicEx.Finger_ToString(src_Sky, hKoma, "");
                sb.AppendLine("                { act:\"drawImg\", img:\"" + komaImg + "\", masu: " + Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu( koma)) + " },");//FIXME:おかしい？
            }



            sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },");

            //sb.AppendLine("            ],");

        gt_EndMethod:
            return sb.ToString();
        }

        public static string JsonElements_Masus(bool enableLog, SySet<SyElement> masus, string comment)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },\n");

            foreach (New_Basho masu in masus.Elements)
            {
                sb.AppendLine("                { act:\"drawMasu\" , masu:" + ((int)masu.MasuNumber) + " },\n");
            }



            sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },\n");

        gt_EndMethod:
            return sb.ToString();
        }


    }
}
