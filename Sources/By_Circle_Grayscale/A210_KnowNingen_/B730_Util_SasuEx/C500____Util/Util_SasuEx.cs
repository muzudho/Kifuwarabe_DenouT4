using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B480_Util_Sasu__.C500____Util;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B720_Conv_SasuEx.C500____Converter;
using System;
using System.Collections.Generic;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B730_Util_SasuEx.C500____Util
{

    /// <summary>
    /// ************************************************************************************************************************
    /// あるデータを、別のデータに変換します。
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class Util_SasuEx
    {

        /// <summary>
        /// これが通称【水際のいんちきプログラム】なんだぜ☆
        /// 必要により、【成り】の指し手を追加するぜ☆
        /// </summary>
        public static List<Move> CreateNariSasite(
            Sky src_Sky,
            List<Move> a_sasitebetuEntry,
            KwLogger errH
            )
        {
            //----------------------------------------
            // 『進める駒』と、『移動先升』
            //----------------------------------------
            List<Move> result_komabetuEntry = new List<Move>();

            try
            {
                Dictionary<string, Move> newSasiteList = new Dictionary<string, Move>();

                foreach(Move move1 in a_sasitebetuEntry)
                {
                    // ・移動元の駒
                    SyElement srcMasu = Conv_Move.ToSrcMasu(move1);
                    Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move1);

                    // ・移動先の駒
                    SyElement dstMasu = Conv_Move.ToDstMasu(move1);
                    Komasyurui14 dstKs = Conv_Move.ToDstKomasyurui(move1);
                    Playerside pside = Conv_Move.ToPlayerside(move1);

                    // 成りができる動きなら真。
                    bool isPromotionable;
                    if (!Util_Sasu269.IsPromotionable(out isPromotionable, srcMasu, dstMasu, srcKs, pside))
                    {
                        // ｴﾗｰ
                        goto gt_Next1;
                    }

                    if (isPromotionable)
                    {
                        Move move = Conv_Move.ToMove(
                            srcMasu,
                            dstMasu,
                            srcKs,
                            Komasyurui14.H00_Null___,//取った駒不明
                            true,//強制的に【成り】に駒の種類を変更
                            false,//成りなのでドロップは無いぜ☆（＾▽＾）
                            pside,
                            false                            
                        );

                        // TODO: 一段目の香車のように、既に駒は成っている場合があります。無い指し手だけ追加するようにします。
                        string sasiteStr = Conv_Move.ToSfen(move);//重複防止用のキー
                        if (!newSasiteList.ContainsKey(sasiteStr))
                        {
                            newSasiteList.Add(sasiteStr, move);
                        }
                    }

                gt_Next1:
                    ;
                }


                // 新しく作った【成り】の指し手を追加します。
                foreach (Move newMove in newSasiteList.Values)
                {
                    // 指す前の駒
                    SyElement srcMasu = Conv_Move.ToSrcMasu(newMove);

                    try
                    {
                        if (!result_komabetuEntry.Contains(newMove))
                        {
                            // 指し手が既存でない局面だけを追加します。

                            // 『進める駒』と、『移動先升』
                            result_komabetuEntry.Add( 
                                newMove//成りの手
                                );
                        }

                    }
                    catch (Exception ex)
                    {
                        // 既存の指し手
                        StringBuilder sb = new StringBuilder();
                        {
                            foreach (Move entry in a_sasitebetuEntry)
                            {
                                sb.Append("「");
                                sb.Append(Conv_Move.ToSfen(entry));
                                sb.Append("」");
                            }
                        }

                        //>>>>> エラーが起こりました。
                        errH.DonimoNaranAkirameta(ex, "新しく作った「成りの指し手」を既存ノードに追加していた時です。：追加したい指し手=「" + Conv_Move.ToSfen(newMove) + "」既存の手=" + sb.ToString());
                        throw ex;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Convert04.cs#AddNariSasiteでｴﾗｰ。:" + ex.GetType().Name + ":" + ex.Message);
            }

            return result_komabetuEntry;
        }

        //public static void PutAddAll_ToHubNode(
        //    Dictionary<string, SasuEntry> result_komabetuEntry,
        //    SkyConst src_Sky,
        //    KifuNode hubNode_mutable,
        //    KwLogger errH
        //    )
        //{
        //    foreach (KeyValuePair<string, SasuEntry> entry in result_komabetuEntry)
        //    {
        //        Util_SasuEx.PutAdd_ToHubNode(
        //            entry.Key,
        //            entry.Value,
        //            src_Sky,
        //            hubNode_mutable,
        //            errH
        //            );
        //    }
        //}

        //public static void PutAdd_ToHubNode(
        //    string sasiteStr_sfen,
        //    SasuEntry sasuEntry,
        //    SkyConst src_Sky,
        //    KifuNode hubNode_mutable,
        //    KwLogger errH
        //    )
        //{
        //    if (!hubNode_mutable.ContainsKey_ChildNodes(sasiteStr_sfen))//チェックを追加
        //    {
        //        hubNode_mutable.PutAdd_ChildNode(
        //            sasiteStr_sfen,
        //            Conv_SasuEntry.ToKifuNode(sasuEntry, src_Sky, errH)
        //        );
        //    }
        //}

    }
}
