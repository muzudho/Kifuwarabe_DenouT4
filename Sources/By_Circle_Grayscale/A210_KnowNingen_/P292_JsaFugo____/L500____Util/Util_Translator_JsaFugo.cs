using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.P211_WordShogi__.L250____Masu;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P212_ConvPside__.C500____Converter;
using Grayscale.P213_Komasyurui_.C500____Util;
using Grayscale.P214_Masu_______.C500____Util;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P226_Tree_______.L___500_Struct;
using Grayscale.P239_ConvWords__.C500____Converter;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P292_JsaFugo____.C250____Struct;
using Grayscale.P339_ConvKyokume.C500____Converter;
using System.Text;

namespace Grayscale.P292_JsaFugo____.C500____Util
{
    public abstract class Util_Translator_JsaFugo
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜用の符号テキスト(*1)を作ります。
        /// ************************************************************************************************************************
        /// 
        ///         *1…「▲５五銀上」など。
        ///         
        ///         “同”表記に「置き換えない」バージョンです。
        /// 
        /// </summary>
        /// <param name="sasite"></param>
        /// <param name="previousKomaP"></param>
        /// <returns></returns>
        public static string ToString_NoUseDou(
            JsaFugoImpl jsaFugo,
            Move move
            )
        {
            StringBuilder sb = new StringBuilder();

            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);

            sb.Append(Conv_Playerside.ToSankaku(pside));

            //------------------------------
            // “同”に変換せず、“筋・段”をそのまま出します。
            //------------------------------
            int suji;
            int dan;
            Util_MasuNum.TryMasuToSuji(dstMasu, out suji);
            Util_MasuNum.TryMasuToDan(dstMasu, out dan);

            sb.Append(Conv_Int.ToArabiaSuji(suji));
            sb.Append(Conv_Int.ToKanSuji(dan));

            //------------------------------
            // “歩”とか。“全”ではなく“成銀”    ＜符号用＞
            //------------------------------
            sb.Append(Util_Komasyurui14.Fugo[(int)jsaFugo.Syurui]);

            //------------------------------
            // “右”とか
            //------------------------------
            sb.Append(Conv_MigiHidari.ToStr(jsaFugo.MigiHidari));

            //------------------------------
            // “寄”とか
            //------------------------------
            sb.Append(Conv_AgaruHiku.ToStr(jsaFugo.AgaruHiku));

            //------------------------------
            // “成”とか
            //------------------------------
            sb.Append(Conv_NariNarazu.Nari_ToStr(jsaFugo.Nari));

            //------------------------------
            // “打”とか
            //------------------------------
            sb.Append(Conv_DaHyoji.ToBool(jsaFugo.DaHyoji));

            return sb.ToString();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜用の符号テキスト(*1)を作ります。
        /// ************************************************************************************************************************
        /// 
        ///         *1…「▲５五銀上」など。
        /// 
        /// </summary>
        /// <param name="douExpr">“同”表記に置き換えるなら真。</param>
        /// <param name="previousKomaP"></param>
        /// <returns></returns>
        public static string ToString_UseDou(
            JsaFugoImpl jsaFugo,
            Node<Move, KyokumenWrapper> siteiNode
            )
        {
            StringBuilder sb = new StringBuilder();

            

            SyElement dstMasu = Conv_Move.ToDstMasu(siteiNode.Key);
            Playerside pside = Conv_Move.ToPlayerside(siteiNode.Key);

            sb.Append(Conv_Playerside.ToSankaku(pside));

            //------------------------------
            // “同”で表記できるところは、“同”で表記します。それ以外は“筋・段”で表記します。
            //------------------------------
            if (!siteiNode.IsRoot())
            {
                SyElement preDstMasu = Conv_Move.ToDstMasu(siteiNode.GetParentNode().Key);
                if (Masu_Honshogi.Query_ErrorMasu() != preDstMasu)
                {
                    if (Conv_SyElement.ToMasuNumber(preDstMasu) ==Conv_SyElement.ToMasuNumber(dstMasu))
                    {
                        // “同”
                        sb.Append("同");
                        goto gt_Next1;
                    }
                }
            }

            {
                // “筋・段”
                int suji;
                int dan;
                Util_MasuNum.TryMasuToSuji(dstMasu, out suji);
                Util_MasuNum.TryMasuToDan(dstMasu, out dan);

                sb.Append(Conv_Int.ToArabiaSuji(suji));
                sb.Append(Conv_Int.ToKanSuji(dan));
            }
        gt_Next1:
            ;

            //------------------------------
            // “歩”とか。“全”ではなく“成銀”    ＜符号用＞
            //------------------------------
            sb.Append(Util_Komasyurui14.Fugo[(int)jsaFugo.Syurui]);

            //------------------------------
            // “右”とか
            //------------------------------
            sb.Append(Conv_MigiHidari.ToStr(jsaFugo.MigiHidari));

            //------------------------------
            // “寄”とか
            //------------------------------
            sb.Append(Conv_AgaruHiku.ToStr(jsaFugo.AgaruHiku));

            //------------------------------
            // “成”とか
            //------------------------------
            sb.Append(Conv_NariNarazu.Nari_ToStr(jsaFugo.Nari));

            //------------------------------
            // “打”とか
            //------------------------------
            sb.Append(Conv_DaHyoji.ToBool(jsaFugo.DaHyoji));


            return sb.ToString();
        }

    }
}
