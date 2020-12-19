using System.Diagnostics;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A120KifuSfen;
using Grayscale.A210KnowNingen.B130Json.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B350SfenTransla.C500Util;
using Grayscale.A210KnowNingen.B420UtilSky258.C505ConvLogJson;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.A210KnowNingen.B600UtilSky.C500Util
{
    public abstract class UtilSky307
    {

        public static SfenStringImpl ExportSfen(Playerside psideA, ISky positionA, ILogTag logTag)
        {
            Debug.Assert(positionA.Count == 40, "sky.Starlights.Count=[" + positionA.Count + "]");//将棋の駒の数

            return new SfenStringImpl("sfen " + Util_StartposExporter.ToSfenstring(
                Conv_Sky.ToShogiban(psideA, positionA, logTag), false));
        }

        public static SfenStringImpl ExportSfen_ForDebug(
            Playerside psideA, ISky positionA, bool psideIsBlack, ILogTag logger)
        {
            return new SfenStringImpl("sfen " + Util_StartposExporter.ToSfenstring(
                Conv_Sky.ToShogiban(psideA, positionA, logger), true));
        }

        /// <summary>
        /// ログが多くなるので、１行で出力されるようにします。
        /// </summary>
        /// <returns></returns>
        public static IJsonVal ToJsonVal(ISky src_Sky)
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
            ISky src_Sky,
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
                    sb.AppendLine("    { act:\"drawImg\", img:\"" + Util_Converter_LogGraphicEx.PsideKs14_ToString(Conv_Busstop.ToPlayerside(koma), Conv_Busstop.ToKomasyurui(koma), "") + "\", masu: " + hMasu_gote + " },");//FIXME: \記号が入ってなければいいが☆
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
                    sb.AppendLine("    { act:\"drawImg\", img:\"" + Util_Converter_LogGraphicEx.PsideKs14_ToString(Conv_Busstop.ToPlayerside(koma), Conv_Busstop.ToKomasyurui(koma), "") + "\", masu: " + Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma)) + " },");//FIXME: \記号が入ってなければいいが☆
                }

                hKoma++;
            });

            sb.AppendLine("],");

            // ...(^▽^)ﾄﾞｳﾀﾞｯﾀｶﾅ～☆
            return sb.ToString();
        }

    }
}
