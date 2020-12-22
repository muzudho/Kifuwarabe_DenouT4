using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class Conv_Movelist1
    {
        /// <summary>
        /// 成らない手☆
        /// </summary>
        /// <param name="komabetuSusumuMasus"></param>
        /// <param name="positionA"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static List<Move> ToMovelist_NonPromotion(
            Maps_OneAndOne<Finger, SySet<SyElement>> komabetuSusumuMasus,
            Playerside psideA,
            ISky positionA,
            ILogTag logTag
        )
        {
            List<Move> result_movelist = new List<Move>();

            komabetuSusumuMasus.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                positionA.AssertFinger(key);
                Busstop koma = positionA.BusstopIndexOf(key);

                foreach (SyElement dstMasu in value.Elements)
                {
                    Move move = ConvMove.ToMove(
                        Conv_Busstop.ToMasu(koma),
                        dstMasu,
                        Conv_Busstop.ToKomasyurui(koma),
                        Komasyurui14.H00_Null___,
                        false,//成らない
                        false,//ドロップしない
                        psideA,// positionA.GetKaisiPside(),
                        false
                        );

                    if (!result_movelist.Contains(move))
                    {
                        result_movelist.Add(
                            move//成らない手
                            );
                    }
                }
            });

            return result_movelist;
        }


        public static List<Move> ToMovelist_NonPromotion(
            List_OneAndMulti<Finger, SySet<SyElement>> komaMasus,
            Playerside psideA,
            ISky positionA,
            ILogTag logTag
            )
        {
            List<Move> movelist = new List<Move>();


            komaMasus.Foreach_Entry((Finger figKoma, SySet<SyElement> dstMasus, ref bool toBreak) =>
            {
                positionA.AssertFinger(figKoma);
                Busstop koma = positionA.BusstopIndexOf(figKoma);


                foreach (SyElement dstMasu in dstMasus.Elements)
                {
                    Move move = ConvMove.ToMove(
                        Conv_Busstop.ToMasu(koma),
                        dstMasu,
                        Conv_Busstop.ToKomasyurui(koma),
                        Komasyurui14.H00_Null___,
                        false,//成らない
                        false,//多分打たない
                        psideA,// positionA.GetKaisiPside(),
                        false
                        );

                    if (!movelist.Contains(move))
                    {
                        movelist.Add(
                            move//成らない手
                            );
                    }
                }
            });

            return movelist;
        }

    }
}
