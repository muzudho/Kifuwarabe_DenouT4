using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using Grayscale.P269_Util_Sasu__.L500____Util;
using Grayscale.P339_ConvKyokume.L500____Converter;
using Grayscale.P353_Conv_SasuEx.L500____Converter;
using System;
using System.Collections.Generic;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P335_Move_______.L___500_Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P211_WordShogi__.L500____Word;

namespace Grayscale.P354_Util_SasuEx.L500____Util
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
        public static Dictionary<string, SasuEntry> CreateNariSasite(
            SkyConst src_Sky,
            Dictionary<string, SasuEntry> a_sasitebetuEntry,
            KwErrorHandler errH
            )
        {
            //----------------------------------------
            // 『進める駒』と、『移動先升』
            //----------------------------------------
            Dictionary<string, SasuEntry> result_komabetuEntry = new Dictionary<string, SasuEntry>();

            try
            {
                Dictionary<string, Move> newSasiteList = new Dictionary<string, Move>();

                foreach(KeyValuePair<string, SasuEntry> entry in a_sasitebetuEntry)
                {
                    // 
                    // ・移動元の駒
                    // ・移動先の駒
                    //
                    Move move1 = entry.Value.NewMove;

                    SyElement srcMasu = Conv_Move.ToSrcMasu(move1);
                    SyElement dstMasu = Conv_Move.ToDstMasu(move1);
                    Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move1);
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
                        Move move = Conv_SasiteStr_Sfen.ToMove(
                            new RO_Star(// 移動元はそのまま
                                pside,
                                srcMasu,
                                srcKs
                                ),
                            new RO_Star(
                                pside,
                                dstMasu,
                                Util_Komasyurui14.ToNariCase(dstKs)//強制的に【成り】に駒の種類を変更
                            ),// 移動先
                            Komasyurui14.H00_Null___//取った駒不明
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

                //hubNode.Foreach_ChildNodes((string key, Node<Move, KyokumenWrapper> nextNode, ref bool toBreak) =>
                //{
                //});


                // 新しく作った【成り】の指し手を追加します。
                foreach (Move newMove in newSasiteList.Values)
                {
                    // 指す前の駒
                    SyElement srcMasu = Conv_Move.ToSrcMasu(newMove);

                    // 指した駒
                    SyElement dstMasu = Conv_Move.ToDstMasu(newMove);

                    // 指す前の駒を、盤上のマス目で指定
                    Finger figSasumaenoKoma = Util_Sky_FingersQuery.InMasuNow_Old(src_Sky, srcMasu).ToFirst();

                    try
                    {
                        string sasiteStr = Conv_Move.ToSfen(newMove);

                        if (!result_komabetuEntry.ContainsKey(sasiteStr))
                        {
                            // 指し手が既存でない局面だけを追加します。

                            // 『進める駒』と、『移動先升』
                            result_komabetuEntry.Add(sasiteStr, new SasuEntry(newMove, figSasumaenoKoma, dstMasu, true));
                        }

                    }
                    catch (Exception ex)
                    {
                        // 既存の指し手
                        StringBuilder sb = new StringBuilder();
                        {
                            foreach (KeyValuePair<string, SasuEntry> entry in a_sasitebetuEntry)
                            {
                                sb.Append("「");
                                sb.Append(Conv_Move.ToSfen(entry.Value.NewMove));
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
        //    KwErrorHandler errH
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
        //    KwErrorHandler errH
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
