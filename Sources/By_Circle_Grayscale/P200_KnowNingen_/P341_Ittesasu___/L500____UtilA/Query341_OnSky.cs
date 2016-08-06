using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P211_WordShogi__.L___250_Masu;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P212_ConvPside__.L500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P234_Komahaiyaku.L500____Util;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P256_SeizaFinger.L250____Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;

namespace Grayscale.P341_Ittesasu___.L500____UtilA
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
            SkyConst src_Sky,//KifuTree kifu,
            out Finger foundKoma,
            KwErrorHandler errH
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
                    Busstop koma2 = src_Sky.StarlightIndexOf(koma1).Now;


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
