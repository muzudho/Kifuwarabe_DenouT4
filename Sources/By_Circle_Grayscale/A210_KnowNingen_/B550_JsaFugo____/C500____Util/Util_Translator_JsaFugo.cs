using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B550_JsaFugo____.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Text;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;

namespace Grayscale.A210_KnowNingen_.B550_JsaFugo____.C500____Util
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
            Okiba okiba2 = Conv_SyElement.ToOkiba(Conv_SyElement.ToMasuNumber(dstMasu));
            if (okiba2 == Okiba.ShogiBan)
            {
                // 将棋盤☆
                int suji;
                int dan;
                Conv_MasuNum.ToSuji_FromBanjoMasu(dstMasu, out suji);
                Conv_MasuNum.ToDan_FromBanjoMasu(dstMasu, out dan);

                sb.Append(Conv_Int.ToArabiaSuji(suji));
                sb.Append(Conv_Int.ToKanSuji(dan));
            }
            else
            {
                // 盤外に指すことはないぜ☆（＾～＾）エラーの代わりに適当に文字を出そうぜ☆（＾▽＾）
                Pieces pieces;
                Conv_MasuNum.ToPiece_FromBangaiMasu(dstMasu, out pieces);

                // FIXME: ほんとはこんなの表記しないぜ☆（＾～＾）
                sb.Append(Util_Komasyurui14.NimojiPieces[(int)pieces]);
            }


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

                Okiba okiba2 = Conv_SyElement.ToOkiba(Conv_SyElement.ToMasuNumber(dstMasu));
                if (okiba2 == Okiba.ShogiBan)
                {
                    Conv_MasuNum.ToSuji_FromBanjoMasu(dstMasu, out suji);
                    Conv_MasuNum.ToDan_FromBanjoMasu(dstMasu, out dan);

                    sb.Append(Conv_Int.ToArabiaSuji(suji));
                    sb.Append(Conv_Int.ToKanSuji(dan));
                }
                else
                {
                    // 盤外に指すことはないぜ☆（＾～＾）エラーの代わりに適当に文字を出そうぜ☆（＾▽＾）
                    Pieces pieces;
                    Conv_MasuNum.ToPiece_FromBangaiMasu(dstMasu, out pieces);

                    // FIXME: ほんとはこんなの表記しないぜ☆（＾～＾）
                    sb.Append(Util_Komasyurui14.NimojiPieces[(int)pieces]);
                }
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
