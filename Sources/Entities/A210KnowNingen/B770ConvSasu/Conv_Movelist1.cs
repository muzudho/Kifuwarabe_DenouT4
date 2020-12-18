using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B410Collection.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B770ConvSasu.C500Converter
{
    public abstract class Conv_Movelist1
    {
        /// <summary>
        /// 成らない手☆
        /// </summary>
        /// <param name="komabetuSusumuMasus"></param>
        /// <param name="positionA"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static List<Move> ToMovelist_NonPromotion(
            Maps_OneAndOne<Finger, SySet<SyElement>> komabetuSusumuMasus,
            Playerside psideA,
            ISky positionA,
            ILogger errH
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
            ILogger errH
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
