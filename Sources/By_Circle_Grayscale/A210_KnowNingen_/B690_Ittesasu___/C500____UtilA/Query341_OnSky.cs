using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B290_Komahaiyaku.C500____Util;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C500____Util;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA
{
    public class Query341_OnSky
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
            Sky src_Sky,//KifuTree kifu,
            out Finger foundKoma,
            KwLogger errH
            )
        {
            //SkyConst src_Sky = kifu.CurNode.Value.ToKyokumenConst;

            bool hit = false;
            foundKoma = Fingers.Error_1;


            foreach (New_Basho masu1 in srcAll.Elements)//筋・段。（先後、種類は入っていません）
            {
                foreach (Finger koma1 in Finger_Honshogi.Items_KomaOnly)
                {
                    src_Sky.AssertFinger(koma1);
                    Busstop koma2 = src_Sky.BusstopIndexOf(koma1);

                    if (pside == Conv_Busstop.ToPlayerside( koma2)
                        && Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma2)
                        && Util_Komasyurui14.Matches(syurui, Conv_Busstop.ToKomasyurui(koma2))
                        && masu1 == Conv_Busstop.ToMasu( koma2)
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
