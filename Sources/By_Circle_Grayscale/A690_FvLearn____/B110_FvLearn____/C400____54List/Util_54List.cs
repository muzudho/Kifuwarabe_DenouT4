using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B160_ConvFv_____.C500____Converter;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___400_54List;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C400____54List
{
    public class Util_54List
    {

        private static void Error1(Busstop busstop, KwLogger errH)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Util_54List#Error1：２駒関係FVの配列添え字がわからないぜ☆！処理は続けられない。");
            sb.AppendLine("koma1.Pside=[" + Conv_Busstop.ToPlayerside(busstop) + "]");
            sb.AppendLine("koma1.Komasyurui=[" + Conv_Busstop.ToKomasyurui(busstop) + "]");
            sb.AppendLine("koma1.Masu=[" + Conv_Busstop.ToMasu(busstop) + "]");
            sb.AppendLine("Conv_Masu.ToOkiba(koma1.Masu)=[" + Conv_Busstop.ToOkiba(busstop) + "]");
            errH.DonimoNaranAkirameta(sb.ToString());
        }


        /// <summary>
        /// 54駒のリスト。
        /// 
        /// 盤上の40駒リスト。
        /// 駒台の14駒リスト。
        /// </summary>
        public static N54List Calc_54List(Sky src_Sky, KwLogger errH)
        {
            N54List result_n54List = new N54ListImpl();


            //----------------------------------------
            // インナー・メソッド用 集計変数
            //----------------------------------------
            int p54Next = 0;
            int[] p54List = new int[54];

            src_Sky.Foreach_Busstops((Finger finger, Busstop busstop, ref bool toBreak) =>
            {
                //----------------------------------------
                // まず、p を調べます。
                //----------------------------------------
                if (Conv_Busstop.ToOkiba(busstop) == Okiba.ShogiBan)
                {
                    int pIndex = FeatureVectorImpl.CHOSA_KOMOKU_ERROR;// 調査項目Ｐ１
                    //----------------------------------------
                    // 盤上の駒
                    //----------------------------------------
                    Conv_FvKoumoku525.ToPIndex_FromBanjo_PsideKomasyuruiMasu(
                        Conv_Busstop.ToPlayerside(busstop),
                        Conv_Busstop.ToKomasyurui(busstop),
                        Conv_Busstop.ToMasu(busstop),
                        out pIndex);

                    if (FeatureVectorImpl.CHOSA_KOMOKU_ERROR == pIndex)
                    {
                        // p1 がエラーでは、処理は続けられない。
                        Util_54List.Error1(busstop, errH);
                        goto gt_NextLoop_player1;
                    }

                    //----------------------------------------
                    // 盤上の駒だぜ☆！
                    //----------------------------------------
                    p54List[p54Next] = pIndex;
                    p54Next++;
                }
                else if (
                    Conv_Busstop.ToOkiba(busstop) == Okiba.Sente_Komadai
                    || Conv_Busstop.ToOkiba(busstop) == Okiba.Gote_Komadai)
                {
                    int pIndex = FeatureVectorImpl.CHOSA_KOMOKU_ERROR;// 調査項目Ｐ１
                    //----------------------------------------
                    // 持ち駒
                    //----------------------------------------
                    Komasyurui14 motiKomasyurui = Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui(busstop));//例：駒台に馬はない。角の数を数える。
                    // 駒の枚数
                    int maisu = Util_Sky_FingersQuery.InOkibaKomasyuruiNow(src_Sky, Conv_Playerside.ToKomadai(Conv_Busstop.ToPlayerside(busstop)), motiKomasyurui).Items.Count;
                    Conv_FvKoumoku525.ToPIndex_FromMoti_PsideKomasyuruiMaisu(Conv_Busstop.ToPlayerside(busstop), motiKomasyurui, maisu, out pIndex);

                    if (FeatureVectorImpl.CHOSA_KOMOKU_ERROR == pIndex)
                    {
                        // p1 がエラーでは、処理は続けられない。
                        Util_54List.Error1(busstop, errH);
                        goto gt_NextLoop_player1;
                    }

                    //----------------------------------------
                    // 駒台の駒だぜ☆！
                    //----------------------------------------
                    p54List[p54Next] = pIndex;
                    p54Next++;
                }

            gt_NextLoop_player1:
                ;
            });


            result_n54List.SetP54List_Unsorted(p54List);
            result_n54List.SetP54Next(p54Next);

            return result_n54List;
        }

    }
}
