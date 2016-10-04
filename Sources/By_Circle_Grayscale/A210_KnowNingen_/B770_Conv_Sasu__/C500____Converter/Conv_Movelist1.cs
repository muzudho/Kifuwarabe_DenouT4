using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B770_Conv_Sasu__.C500____Converter
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
            Sky positionA,
            KwLogger errH
        )
        {
            List<Move> result_movelist = new List<Move>();

            komabetuSusumuMasus.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                positionA.AssertFinger(key);
                Busstop koma = positionA.BusstopIndexOf(key);

                foreach (SyElement dstMasu in value.Elements)
                {
                    Move move = Conv_Move.ToMove(
                        Conv_Busstop.ToMasu( koma),
                        dstMasu,
                        Conv_Busstop.ToKomasyurui( koma),
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
            Sky positionA,
            KwLogger errH
            )
        {
            List<Move> movelist = new List<Move>();


            komaMasus.Foreach_Entry((Finger figKoma, SySet<SyElement> dstMasus, ref bool toBreak) =>
            {
                positionA.AssertFinger(figKoma);
                Busstop koma = positionA.BusstopIndexOf(figKoma);


                foreach (SyElement dstMasu in dstMasus.Elements)
                {
                    Move move = Conv_Move.ToMove(
                        Conv_Busstop.ToMasu( koma),
                        dstMasu,
                        Conv_Busstop.ToKomasyurui( koma),
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
