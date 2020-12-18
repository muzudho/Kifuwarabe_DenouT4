using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C250Masu;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B410SeizaFinger.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA
{
    public class Query341OnSky
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋盤上での検索
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="srcAll">候補マス</param>
        /// <param name="komas"></param>
        /// <returns></returns>
        public static bool Query_Koma(
            Playerside pside,
            Komasyurui14 syurui,
            SySet<SyElement> srcAll,
            ISky src_Sky,//KifuTree kifu,
            out Finger foundKoma,
            ILogTag errH
            )
        {
            //SkyConst src_Sky = kifu.CurNode.Value.ToKyokumenConst;

            bool hit = false;
            foundKoma = Fingers.Error_1;


            foreach (INewBasho masu1 in srcAll.Elements)//筋・段。（先後、種類は入っていません）
            {
                foreach (Finger koma1 in Finger_Honshogi.Items_KomaOnly)
                {
                    src_Sky.AssertFinger(koma1);
                    Busstop koma2 = src_Sky.BusstopIndexOf(koma1);

                    if (pside == Conv_Busstop.ToPlayerside(koma2)
                        && Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma2)
                        && Util_Komasyurui14.Matches(syurui, Conv_Busstop.ToKomasyurui(koma2))
                        && masu1 == Conv_Busstop.ToMasu(koma2)
                        )
                    {
                        // 候補マスにいた
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        hit = true;
                        foundKoma = koma1;
                        break;
                    }
                }
            }

            return hit;
        }





    }
}
