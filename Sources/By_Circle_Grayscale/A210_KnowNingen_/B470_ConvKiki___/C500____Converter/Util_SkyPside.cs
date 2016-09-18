using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C510____Komanokiki;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C500____Util;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B470_ConvKiki___.C500____Converter
{
    public abstract class Util_SkyPside
    {
        /// <summary>
        /// 駒の利きを調べます。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <returns></returns>
        public static MasubetuKikisuImpl ToMasubetuKikisu(
            Sky src_Sky,
            Playerside tebanside
            )
        {

            // ①現手番の駒の移動可能場所_被王手含む
            List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;

            Util_KyokumenMoves.LA_Split_KomaBETUSusumeruMasus(
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

                result.HMasu_PlayersideList[Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu( koma))] = Conv_Busstop.ToPlayerside( koma);
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
                        else if (Playerside.P1 == Conv_Busstop.ToPlayerside( koma))
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
