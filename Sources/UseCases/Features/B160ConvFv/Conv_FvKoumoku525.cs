﻿using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{

    /// <summary>
    /// フィーチャーベクターの項目番号を算出します。
    /// </summary>
    public abstract class Conv_FvKoumoku525
    {
        /// <summary>
        /// ２駒関係[ＫＫ]用。
        /// フィーチャーベクターの調査項目インデックス。該当なければ-1。
        /// </summary>
        /// <param name="pside">プレイヤーサイド</param>
        /// <param name="komasyurui">盤上の種類</param>
        /// <param name="masu">盤上の駒の升</param>
        /// <returns></returns>
        public static int ToKIndex_From_PsideBanjoKomasyuruiMasu(IPosition src_Sky, Playerside pside)
        {
            // 調査項目番号（Ｋ１、Ｋ２等）
            int result;

            SyElement masu;
            {
                Finger figK1 = UtilSkyFingersQuery.InOkibaPsideKomasyuruiNow(src_Sky, Okiba.ShogiBan, pside, Komasyurui14.H06_Gyoku__).ToFirst();

                src_Sky.AssertFinger(figK1);
                Busstop komaK1 = src_Sky.BusstopIndexOf(figK1);
                masu = Conv_Busstop.ToMasu(komaK1);
            }

            if (Okiba.ShogiBan != Conv_Masu.ToOkiba(masu))
            {
                // 盤上でなければ。
                result = -1;
                goto gt_EndMethod;
            }

            int kSuji;
            Conv_Masu.ToSuji_FromBanjoMasu(masu, out kSuji);
            int kDan;
            Conv_Masu.ToDan_FromBanjoMasu(masu, out kDan);


            int p1;
            Conv_FvKoumoku522.Converter_K1_to_P(Playerside.P1, kDan, kSuji, out p1);


            result = p1;

        gt_EndMethod:
            ;
            return result;
        }


        /// <summary>
        /// ２駒関係[ＰＰ]用。
        /// フィーチャーベクターの調査項目インデックス。該当なければ-1。
        /// </summary>
        /// <param name="pside">プレイヤーサイド</param>
        /// <param name="komasyurui">盤上の種類</param>
        /// <param name="masu">盤上の駒の升</param>
        /// <returns></returns>
        public static int ToPIndex_FromBanjo_PsideKomasyuruiMasu(Playerside pside, Komasyurui14 komasyurui, SyElement masu, out int p_index)
        {
            p_index = 0;//ここに累計していく。

            if (Okiba.ShogiBan != Conv_Masu.ToOkiba(masu))
            {
                // 盤上でなければ。
                p_index = -1;
                goto gt_EndMethod;
            }

            switch (pside)
            {
                case Playerside.P1: break;
                case Playerside.P2: p_index += FeatureVector.CHOSA_KOMOKU_2P; break;
                default: break;
            }

            switch (komasyurui)
            {
                case Komasyurui14.H11_Tokin__: //thru
                case Komasyurui14.H01_Fu_____: p_index += FeatureVector.CHOSA_KOMOKU_____FU_____; break;
                case Komasyurui14.H12_NariKyo: //thru
                case Komasyurui14.H02_Kyo____: p_index += FeatureVector.CHOSA_KOMOKU_____KYO____; break;
                case Komasyurui14.H13_NariKei: //thru
                case Komasyurui14.H03_Kei____: p_index += FeatureVector.CHOSA_KOMOKU_____KEI____; break;
                case Komasyurui14.H14_NariGin: //thru
                case Komasyurui14.H04_Gin____: p_index += FeatureVector.CHOSA_KOMOKU_____GIN____; break;
                case Komasyurui14.H05_Kin____: p_index += FeatureVector.CHOSA_KOMOKU_____KIN____; break;
                case Komasyurui14.H06_Gyoku__: p_index += FeatureVector.CHOSA_KOMOKU_____OH_____; break;
                case Komasyurui14.H09_Ryu____: //thru
                case Komasyurui14.H07_Hisya__: p_index += FeatureVector.CHOSA_KOMOKU_____HISYA__; break;
                case Komasyurui14.H10_Uma____: //thru
                case Komasyurui14.H08_Kaku___: p_index += FeatureVector.CHOSA_KOMOKU_____KAKU___; break;
                default: p_index = -1; goto gt_EndMethod;
            }

            p_index += Conv_Masu.ToMasuHandle(masu);

        gt_EndMethod:
            ;
            return p_index;
        }

        /// <summary>
        /// ２駒関係[ＰＰ]用。
        /// フィーチャーベクターの調査項目インデックス。該当なければ-1。
        /// </summary>
        /// <param name="pside">プレイヤーサイド</param>
        /// <param name="komasyurui">持駒の種類</param>
        /// <param name="maisu">その持駒の種類の、持っている個数</param>
        /// <returns></returns>
        public static int ToPIndex_FromMoti_PsideKomasyuruiMaisu(Playerside pside, Komasyurui14 komasyurui, int maisu, out int p_index)
        {
            p_index = 0;//ここに累計していく。

            switch (pside)
            {
                case Playerside.P1: break;
                case Playerside.P2: p_index += FeatureVector.CHOSA_KOMOKU_2P; break;
                default: break;
            }

            switch (komasyurui)
            {
                //case Ks14.H11_Tokin__: //駒台にない。
                case Komasyurui14.H01_Fu_____: p_index += FeatureVector.CHOSA_KOMOKU_MOTIFU_____; break;
                //case Ks14.H12_NariKyo: //駒台にない。
                case Komasyurui14.H02_Kyo____: p_index += FeatureVector.CHOSA_KOMOKU_MOTIKYO____; break;
                //case Ks14.H13_NariKei: //駒台にない。
                case Komasyurui14.H03_Kei____: p_index += FeatureVector.CHOSA_KOMOKU_MOTIKEI____; break;
                //case Ks14.H14_NariGin: //駒台にない。
                case Komasyurui14.H04_Gin____: p_index += FeatureVector.CHOSA_KOMOKU_MOTIGIN____; break;
                case Komasyurui14.H05_Kin____: p_index += FeatureVector.CHOSA_KOMOKU_MOTIKIN____; break;
                //case Ks14.H06_Oh_____: // 駒台にない。
                //case Ks14.H09_Ryu____: //駒台にない。
                case Komasyurui14.H07_Hisya__: p_index += FeatureVector.CHOSA_KOMOKU_MOTIHISYA__; break;
                //case Ks14.H10_Uma____: //駒台にない。
                case Komasyurui14.H08_Kaku___: p_index += FeatureVector.CHOSA_KOMOKU_MOTIKAKU___; break;
                default: p_index = -1; goto gt_EndMethod;
            }

            p_index += maisu;//持ち駒の数

        gt_EndMethod:
            ;
            return p_index;
        }

    }


}
