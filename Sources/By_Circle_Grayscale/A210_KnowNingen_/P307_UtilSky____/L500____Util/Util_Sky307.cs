﻿using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C250____Struct;
using Grayscale.A210_KnowNingen_.B130_Json_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B130_Json_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C250____Struct;
using Grayscale.A210_KnowNingen_.B350_SfenTransla.C500____Util;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C505____ConvLogJson;
using Grayscale.P339_ConvKyokume.C500____Converter;
using System.Diagnostics;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.P307_UtilSky____.C500____Util
{
    public abstract class Util_Sky307
    {

        public static SfenstringImpl ExportSfen(SkyConst src_Sky)
        {
            Debug.Assert(src_Sky.Count == 40, "sky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            StartposExporterImpl se = new StartposExporterImpl(src_Sky);
            return new SfenstringImpl("sfen " + Util_StartposExporter.ToSfenstring(se, false));
        }

        public static SfenstringImpl ExportSfen_ForDebug(SkyConst src_Sky, bool psideIsBlack)
        {
            StartposExporterImpl se = new StartposExporterImpl(src_Sky);
            return new SfenstringImpl("sfen " + Util_StartposExporter.ToSfenstring(se, true));
        }

        /// <summary>
        /// ログが多くなるので、１行で出力されるようにします。
        /// </summary>
        /// <returns></returns>
        public static Json_Val ToJsonVal(SkyConst src_Sky)
        {
            Json_Obj obj = new Json_Obj();

            Json_Arr arr = new Json_Arr();
            src_Sky.Foreach_Busstops((Finger finger, Busstop light, ref bool toBreak) =>
            {
                if (Busstop.Empty != light)
                {
                    arr.Add(Conv_Starlight.ToJsonVal(light));
                }
            });

            obj.Add(new Json_Prop("sprite", arr));

            return obj;
        }





        /// <summary>
        /// 「グラフィカル局面ログ」出力用だぜ☆
        /// </summary>
        public static string Json_1Sky(
            SkyConst src_Sky,
            string memo,
            string hint,
            int temezumi_yomiGenTeban_forLog//読み進めている現在の手目済

            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {

            //...(^▽^)さて、局面は☆？
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[");

            // コメント
            string comment;
            {
                StringBuilder cmt = new StringBuilder();

                // メモ
                cmt.Append(memo);

                comment = cmt.ToString();
            }

            sb.AppendLine("    { act:\"drawText\", text:\"" + comment + "\", x: 20, y:20 },");//FIXME: \記号が入ってなければいいが☆

            int hKoma = 0;
            int hMasu_sente = 81;
            int hMasu_gote = 121;

            // 全駒
            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToOkiba(koma) == Okiba.Gote_Komadai)
                {
                    // 後手持ち駒
                    sb.AppendLine("    { act:\"drawImg\", img:\"" + Util_Converter_LogGraphicEx.PsideKs14_ToString(Conv_Busstop.ToPlayerside( koma), Conv_Busstop.ToKomasyurui( koma), "") + "\", masu: " + hMasu_gote + " },");//FIXME: \記号が入ってなければいいが☆
                    hMasu_gote++;
                }
                else if (Conv_Busstop.ToOkiba(koma) == Okiba.Sente_Komadai)
                {
                    // 先手持ち駒
                    sb.AppendLine("    { act:\"drawImg\", img:\"" + Util_Converter_LogGraphicEx.PsideKs14_ToString(Conv_Busstop.ToPlayerside(koma), Conv_Busstop.ToKomasyurui(koma), "") + "\", masu: " + hMasu_sente + " },");//FIXME: \記号が入ってなければいいが☆
                    hMasu_sente++;
                }
                else if (Conv_Busstop.ToOkiba(koma) == Okiba.ShogiBan)
                {
                    // 盤上
                    sb.AppendLine("    { act:\"drawImg\", img:\"" + Util_Converter_LogGraphicEx.PsideKs14_ToString(Conv_Busstop.ToPlayerside(koma), Conv_Busstop.ToKomasyurui(koma), "") + "\", masu: " + Conv_SyElement.ToMasuNumber( Conv_Busstop.ToMasu( koma)) + " },");//FIXME: \記号が入ってなければいいが☆
                }

                hKoma++;
            });

            sb.AppendLine("],");

            // ...(^▽^)ﾄﾞｳﾀﾞｯﾀｶﾅ～☆
            return sb.ToString();
        }

    }
}