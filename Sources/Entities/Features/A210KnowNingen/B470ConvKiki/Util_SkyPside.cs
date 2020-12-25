using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Configuration;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class Util_SkyPside
    {
        /// <summary>
        /// 駒の利きを調べます。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <returns></returns>
        public static MasubetuKikisuImpl ToMasubetuKikisu(
            IEngineConf engineConf,
            IPosition src_Sky,
            Playerside tebanside
            )
        {

            // ①現手番の駒の移動可能場所_被王手含む
            List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;

            Util_KyokumenMoves.LA_Split_KomaBETUSusumeruMasus(
                engineConf,
                3,
                //node_forLog,
                out komaBETUSusumeruMasus,//進めるマス
                true,//本将棋か
                src_Sky,//現在の局面
                tebanside,//手番
                false//相手番か
#if DEBUG
                ,
                null
#endif
            );

            MasubetuKikisuImpl result = new MasubetuKikisuImpl();

            //
            // 「升ごとの敵味方」を調べます。
            //
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                src_Sky.AssertFinger(figKoma);
                Busstop koma = src_Sky.BusstopIndexOf(figKoma);

                result.HMasu_PlayersideList[Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma))] = Conv_Busstop.ToPlayerside(koma);
            }

            //
            // 駒のない升は無視します。
            //

            //
            // 駒のあるマスに、その駒の味方のコマが効いていれば　味方＋１
            //
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                //
                // 駒
                //
                src_Sky.AssertFinger(figKoma);
                Busstop koma = src_Sky.BusstopIndexOf(figKoma);

                // 将棋盤上の戦駒のみ判定
                if (Okiba.ShogiBan != Conv_Busstop.ToOkiba(koma))
                {
                    goto gt_Next1;
                }


                //
                // 駒の利きカウント FIXME:貫通してないか？
                //
                komaBETUSusumeruMasus.Foreach_Entry((Finger figKoma2, SySet<SyElement> kikiZukei, ref bool toBreak) =>
                {
                    IEnumerable<SyElement> kikiMasuList = kikiZukei.Elements;
                    foreach (SyElement masu in kikiMasuList)
                    {
                        // その枡に利いている駒のハンドルを追加
                        if (result.HMasu_PlayersideList[Conv_Masu.ToMasuHandle(masu)] == Playerside.Empty)
                        {
                            // 駒のないマスは無視。
                        }
                        else if (Playerside.P1 == Conv_Busstop.ToPlayerside(koma))
                        {
                            // 利きのあるマスにある駒と、この駒のプレイヤーサイドが同じ。
                            result.Kikisu_AtMasu_1P[Conv_Masu.ToMasuHandle(masu)] += 1;
                        }
                        else
                        {
                            // 反対の場合。
                            result.Kikisu_AtMasu_2P[Conv_Masu.ToMasuHandle(masu)] += 1;
                        }
                    }
                });

            gt_Next1:
                ;
            }

            return result;

        }

    }
}
