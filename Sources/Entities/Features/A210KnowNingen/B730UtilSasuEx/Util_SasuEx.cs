using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.Entities.Features
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
        public static List<Move> CreateNariMove(
            ISky positionA,
            List<Move> a_moveBetuEntry)
        {
            //----------------------------------------
            // 『進める駒』と、『移動先升』
            //----------------------------------------
            List<Move> result_komabetuEntry = new List<Move>();

            Dictionary<string, Move> newMoveList = new Dictionary<string, Move>();

            foreach (Move move1 in a_moveBetuEntry)
            {
                // ・移動元の駒
                SyElement srcMasu = ConvMove.ToSrcMasu(move1, positionA);
                Komasyurui14 srcKs = ConvMove.ToSrcKomasyurui(move1);

                // ・移動先の駒
                SyElement dstMasu = ConvMove.ToDstMasu(move1);
                Komasyurui14 dstKs = ConvMove.ToDstKomasyurui(move1);
                Playerside pside = ConvMove.ToPlayerside(move1);

                // 成りができる動きなら真。
                bool isPromotionable;
                if (!Util_Sasu269.IsPromotionable(out isPromotionable, srcMasu, dstMasu, srcKs, pside))
                {
                    // ｴﾗｰ
                    goto gt_Next1;
                }

                if (isPromotionable)
                {
                    Move move = ConvMove.ToMove(
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
                    string moveStr = ConvMove.ToSfen(move);//重複防止用のキー
                    if (!newMoveList.ContainsKey(moveStr))
                    {
                        newMoveList.Add(moveStr, move);
                    }
                }

            gt_Next1:
                ;
            }


            // 新しく作った【成り】の指し手を追加します。
            foreach (Move newMove in newMoveList.Values)
            {
                // 指す前の駒
                SyElement srcMasu = ConvMove.ToSrcMasu(newMove, positionA);

                if (!result_komabetuEntry.Contains(newMove))
                {
                    // 指し手が既存でない局面だけを追加します。

                    // 『進める駒』と、『移動先升』
                    result_komabetuEntry.Add(
                        newMove//成りの手
                        );
                }
            }

            return result_komabetuEntry;
        }

    }
}
