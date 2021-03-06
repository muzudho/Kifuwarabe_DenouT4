﻿using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class Conv_KomabetuSusumeruMasus
    {
        /// <summary>
        /// 変換「各（自駒が動ける升）」→「各（自駒が動ける手）」
        /// </summary>
        /// <param name="komaBETUSusumeruMasus">駒別の進める升</param>
        /// <param name="siteiNode">指定ノード</param>
        /// <returns></returns>
        public static Maps_OneAndMulti<Finger, Move> ToKomaBETUAllMoves(
            List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus,
            IPosition positionA
            )
        {
            Maps_OneAndMulti<Finger, Move> result_komabetuAllMoves = new Maps_OneAndMulti<Finger, Move>();

            komaBETUSusumeruMasus.Foreach_Entry((Finger figKoma, SySet<SyElement> susumuMasuSet, ref bool toBreak) =>
            {
                // 動かす星。
                positionA.AssertFinger(figKoma);
                Busstop srcStar = positionA.BusstopIndexOf(figKoma);

                foreach (SyElement susumuMasu in susumuMasuSet.Elements)// 星が進める升。
                {
                    // 移動先の星（升の変更）
                    Busstop dstStar = Conv_Busstop.ToBusstop(
                        Conv_Busstop.ToPlayerside(srcStar),
                        susumuMasu,
                        Conv_Busstop.ToKomasyurui(srcStar)
                    );

                    // 打かどうかは元位置（駒台）から判定してくれだぜ☆（＾▽＾）
                    Move move = ConvMove.ToMove(
                        Conv_Busstop.ToMasu(srcStar),
                        Conv_Busstop.ToMasu(dstStar),
                        Conv_Busstop.ToKomasyurui(srcStar),
                        Conv_Busstop.ToKomasyurui(dstStar),//これで成りかどうか判定
                        Komasyurui14.H00_Null___,//取った駒不明
                        Conv_Busstop.ToPlayerside(srcStar),
                        false
                        );
                    result_komabetuAllMoves.Put_NewOrOverwrite(figKoma, move);//FIXME: １つの駒に指し手は１つ？？




                    // これが通称【水際のいんちきプログラム】なんだぜ☆
                    // 必要により、【成り】の指し手を追加します。
                    // FIXME: ここ以外で、成りの指し手を追加している☆？（＾～＾）？

                    Okiba srcOkiba = Conv_Busstop.ToOkiba(srcStar);//Moveは置き場情報を欠損している。
                    if (Okiba.ShogiBan == srcOkiba)
                    //if(ConvMove.ToDrop(move))
                    {
                        // 将棋盤上の駒だけが、「成り」ができるぜ☆（＾～＾）
                        SyElement srcMasu = ConvMove.ToSrcMasu(move, positionA);
                        SyElement dstMasu = ConvMove.ToDstMasu(move);
                        Komasyurui14 srcKs = ConvMove.ToSrcKomasyurui(move);
                        Komasyurui14 dstKs = ConvMove.ToDstKomasyurui(move);
                        Playerside pside = ConvMove.ToPlayerside(move);
                        Util_Sasu269.AddKomaBETUAllNariMoves(
                            result_komabetuAllMoves,
                            figKoma,//動かす駒
                            srcMasu,
                            dstMasu,
                            srcKs,
                            dstKs,
                            pside
                            );
                    }
                }
            });

            return result_komabetuAllMoves;
        }

    }
}
