﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P212_ConvPside__.C500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.C500____Util;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using Grayscale.P339_ConvKyokume.C500____Converter;
using Grayscale.P521_FeatureVect.L500____Struct;
using Grayscale.P525_ConvFv_____.C500____Converter;
using Grayscale.P743_FvLearn____.L___400_54List;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.P743_FvLearn____.L400____54List
{
    /// <summary>
    /// 52要素のリスト。
    /// </summary>
    public abstract class Util_40t14List
    {

        private static void Error1(Busstop busstop, KwErrorHandler errH)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Util_40t14List#Error1：２駒関係FVの配列添え字がわからないぜ☆！処理は続けられない。");
            sb.AppendLine("koma1.Pside=[" + Conv_Busstop.ToPlayerside(busstop) + "]");
            sb.AppendLine("koma1.Komasyurui=[" + Conv_Busstop.ToKomasyurui(busstop) + "]");
            sb.AppendLine("koma1.Masu=[" + Conv_Busstop.ToMasu(busstop) + "]");
            sb.AppendLine("Conv_SyElement.ToOkiba(koma1.Masu)=[" + Conv_Busstop.ToOkiba(busstop) + "]");
            errH.DonimoNaranAkirameta(sb.ToString());
        }

        /// <summary>
        /// 54駒のリスト。
        /// 
        /// 盤上の40駒リスト。
        /// 駒台の14駒リスト。
        /// </summary>
        public static N40t14List Calc_40t14List(SkyConst src_Sky, KwErrorHandler errH)
        {
            N40t14List result_n40t14List = new N40t14ListImpl();


            //----------------------------------------
            // インナー・メソッド用 集計変数
            //----------------------------------------
            int p40Next = 0;
            int[] p40List = new int[40];

            int p14Next = 0;
            int[] p14List = new int[14];

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                //----------------------------------------
                // まず、p を調べます。
                //----------------------------------------
                if (Conv_Busstop.ToOkiba(koma) == Okiba.ShogiBan)
                {
                    int pIndex = FeatureVectorImpl.CHOSA_KOMOKU_ERROR;// 調査項目Ｐ１
                    //----------------------------------------
                    // 盤上の駒
                    //----------------------------------------
                    Conv_FvKoumoku525.ToPIndex_FromBanjo_PsideKomasyuruiMasu(
                        Conv_Busstop.ToPlayerside( koma),
                        Conv_Busstop.ToKomasyurui( koma),
                        Conv_Busstop.ToMasu( koma), out pIndex);

                    if (FeatureVectorImpl.CHOSA_KOMOKU_ERROR == pIndex)
                    {
                        // p1 がエラーでは、処理は続けられない。
                        Util_40t14List.Error1(koma, errH);
                        goto gt_NextLoop_player1;
                    }

                    //----------------------------------------
                    // 盤上の駒だぜ☆！
                    //----------------------------------------
                    p40List[p40Next] = pIndex;
                    p40Next++;
                }
                else if (
                    Conv_Busstop.ToOkiba(koma) == Okiba.Sente_Komadai
                    || Conv_Busstop.ToOkiba(koma) == Okiba.Gote_Komadai)
                {
                    int pIndex = FeatureVectorImpl.CHOSA_KOMOKU_ERROR;// 調査項目Ｐ１
                    //----------------------------------------
                    // 持ち駒
                    //----------------------------------------
                    Komasyurui14 motiKomasyurui = Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui(koma));//例：駒台に馬はない。角の数を数える。
                    // 駒の枚数
                    int maisu = Util_Sky_FingersQuery.InOkibaKomasyuruiNow(src_Sky,
                        Conv_Playerside.ToKomadai(Conv_Busstop.ToPlayerside( koma)), motiKomasyurui).Items.Count;
                    Conv_FvKoumoku525.ToPIndex_FromMoti_PsideKomasyuruiMaisu(Conv_Busstop.ToPlayerside(koma), motiKomasyurui, maisu, out pIndex);

                    if (FeatureVectorImpl.CHOSA_KOMOKU_ERROR == pIndex)
                    {
                        // p1 がエラーでは、処理は続けられない。
                        Util_40t14List.Error1(koma, errH);
                        goto gt_NextLoop_player1;
                    }

                    //----------------------------------------
                    // 駒台の駒だぜ☆！
                    //----------------------------------------
                    p14List[p14Next] = pIndex;
                    p14Next++;
                }

            gt_NextLoop_player1:
                ;
            });


            result_n40t14List.SetP40List_Unsorted( p40List);
            result_n40t14List.SetP40Next( p40Next);

            result_n40t14List.SetP14List_Unsorted( p14List);
            result_n40t14List.SetP14Next( p14Next);

            return result_n40t14List;
        }


    }
}
