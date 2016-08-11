using Grayscale.P035_Collection_.L500____Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P269_Util_Sasu__.L500____Util;
using Grayscale.P339_ConvKyokume.L500____Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P360_Conv_Sasu__.L500____Converter
{
    public abstract class Conv_KomabetuSusumeruMasus
    {
        /// <summary>
        /// 変換「各（自駒が動ける升）」→「各（自駒が動ける手）」
        /// </summary>
        /// <param name="komaBETUSusumeruMasus">駒別の進める升</param>
        /// <param name="siteiNode">指定ノード</param>
        /// <returns></returns>
        public static Maps_OneAndMulti<Finger, Move> ToKomaBETUAllSasites(
            List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus,
            SkyConst src_Sky
            )
        {
            Maps_OneAndMulti<Finger, Move> result_komabetuAllMoves = new Maps_OneAndMulti<Finger, Move>();

            komaBETUSusumeruMasus.Foreach_Entry((Finger figKoma, SySet<SyElement> susumuMasuSet, ref bool toBreak) =>
            {
                // 動かす星。
                src_Sky.AssertFinger(figKoma);
                Busstop srcStar = src_Sky.BusstopIndexOf(figKoma);

                foreach (SyElement susumuMasu in susumuMasuSet.Elements)// 星が進める升。
                {
                    // 移動先の星（升の変更）
                    Busstop dstStar = Conv_Busstop.ToBusstop(
                        Conv_Busstop.ToPlayerside( srcStar),
                        susumuMasu,
                        Conv_Busstop.ToKomasyurui( srcStar)
                    );

                    // 打かどうかは元位置（駒台）から判定してくれだぜ☆（＾▽＾）
                    Move move = Conv_Move.ToMove(
                        Conv_Busstop.ToMasu( srcStar),
                        Conv_Busstop.ToMasu(dstStar),
                        Conv_Busstop.ToKomasyurui( srcStar),
                        Conv_Busstop.ToKomasyurui(dstStar),//これで成りかどうか判定
                        Komasyurui14.H00_Null___,//取った駒不明
                        Conv_Busstop.ToPlayerside( srcStar),
                        false
                        );
                    result_komabetuAllMoves.Put_NewOrOverwrite(figKoma, move);//FIXME: １つの駒に指し手は１つ？？




                    // これが通称【水際のいんちきプログラム】なんだぜ☆
                    // 必要により、【成り】の指し手を追加します。
                    // FIXME: ここ以外で、成りの指し手を追加している☆？（＾～＾）？

                    Okiba srcOkiba = Conv_Busstop.ToOkiba(srcStar);//Moveは置き場情報を欠損している。
                    if (Okiba.ShogiBan == srcOkiba)
                    //if(Conv_Move.ToDrop(move))
                    {
                        // 将棋盤上の駒だけが、「成り」ができるぜ☆（＾～＾）
                        SyElement srcMasu = Conv_Move.ToSrcMasu(move);
                        SyElement dstMasu = Conv_Move.ToDstMasu(move);
                        Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
                        Komasyurui14 dstKs = Conv_Move.ToDstKomasyurui(move);
                        Playerside pside = Conv_Move.ToPlayerside(move);
                        Util_Sasu269.Add_KomaBETUAllNariSasites(
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
