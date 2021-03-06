﻿namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    using System.Collections.Generic;
    using System.Text;
    using Grayscale.Kifuwaragyoku.Entities.Take1Base;

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
        /// <param name="move"></param>
        /// <param name="previousKomaP"></param>
        /// <returns></returns>
        public static string ToString_NoUseDou(
            JsaFugoImpl jsaFugo,
            Move move
            )
        {
            StringBuilder sb = new StringBuilder();

            Playerside pside = ConvMove.ToPlayerside(move);
            SyElement dstMasu = ConvMove.ToDstMasu(move);

            sb.Append(Conv_Playerside.ToSankaku(pside));

            //------------------------------
            // “同”に変換せず、“筋・段”をそのまま出します。
            //------------------------------
            Okiba okiba2 = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(dstMasu));
            if (okiba2 == Okiba.ShogiBan)
            {
                // 将棋盤☆
                int suji;
                int dan;
                Conv_Masu.ToSuji_FromBanjoMasu(dstMasu, out suji);
                Conv_Masu.ToDan_FromBanjoMasu(dstMasu, out dan);

                sb.Append(Conv_Int.ToArabiaSuji(suji));
                sb.Append(Conv_Int.ToKanSuji(dan));
            }
            else
            {
                // 盤外に指すことはないぜ☆（＾～＾）エラーの代わりに適当に文字を出そうぜ☆（＾▽＾）
                Piece pieces;
                Conv_Masu.ToPiece_FromBangaiMasu(dstMasu, out pieces);

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
            Move move,
            List<Move> honpuList
            )
        {
            StringBuilder sb = new StringBuilder();



            SyElement dstMasu = ConvMove.ToDstMasu(move);
            Playerside pside = ConvMove.ToPlayerside(move);

            sb.Append(Conv_Playerside.ToSankaku(pside));

            //------------------------------
            // “同”で表記できるところは、“同”で表記します。それ以外は“筋・段”で表記します。
            //------------------------------
            int index = honpuList.Count - 1;
            if (0 < index - 1)
            {
                index--;
                SyElement preDstMasu = ConvMove.ToDstMasu(honpuList[index]);
                if (Masu_Honshogi.Query_ErrorMasu() != preDstMasu)
                {
                    if (Conv_Masu.ToMasuHandle(preDstMasu) == Conv_Masu.ToMasuHandle(dstMasu))
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

                Okiba okiba2 = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(dstMasu));
                if (okiba2 == Okiba.ShogiBan)
                {
                    Conv_Masu.ToSuji_FromBanjoMasu(dstMasu, out suji);
                    Conv_Masu.ToDan_FromBanjoMasu(dstMasu, out dan);

                    sb.Append(Conv_Int.ToArabiaSuji(suji));
                    sb.Append(Conv_Int.ToKanSuji(dan));
                }
                else
                {
                    // 盤外に指すことはないぜ☆（＾～＾）エラーの代わりに適当に文字を出そうぜ☆（＾▽＾）
                    Piece pieces;
                    Conv_Masu.ToPiece_FromBangaiMasu(dstMasu, out pieces);

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
