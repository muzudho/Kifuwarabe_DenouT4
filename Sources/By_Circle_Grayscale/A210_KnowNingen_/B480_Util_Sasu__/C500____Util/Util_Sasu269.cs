using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B480_Util_Sasu__.C500____Util
{
    public abstract class Util_Sasu269
    {

        ///// <summary>
        ///// ************************************************************************************************************************
        ///// 先後の交代
        ///// ************************************************************************************************************************
        ///// </summary>
        ///// <param name="pside">先後</param>
        ///// <returns>ひっくりかえった先後</returns>
        //public static Playerside AlternatePside(Playerside pside)
        //{
        //    Playerside result;

        //    switch (pside)
        //    {
        //        case Playerside.P1:
        //            result = Playerside.P2;
        //            break;

        //        case Playerside.P2:
        //            result = Playerside.P1;
        //            break;

        //        default:
        //            result = pside;
        //            break;
        //    }

        //    return result;
        //}




        ///// <summary>
        ///// 変換『「指し手→局面」のコレクション』→『「駒、指し手」のペアのリスト』
        ///// </summary>
        //public static List<Couple<Finger,Masu>> SasitebetuSky_ToKamList(
        //    SkyConst src_Sky_genzai,
        //    Dictionary<ShootingStarlightable, SkyBuffer> ss,
        //    LarabeLoggerTag logTag
        //    )
        //{
        //    List<Couple<Finger, Masu>> kmList = new List<Couple<Finger, Masu>>();

        //    // TODO:
        //    foreach(KeyValuePair<ShootingStarlightable,SkyBuffer> entry in ss)
        //    {
        //        RO_Star_Koma srcKoma = Util_Starlightable.AsKoma(entry.Key.LongTimeAgo);
        //        RO_Star_Koma dstKoma = Util_Starlightable.AsKoma(entry.Key);


        //            Masu srcMasu = srcKoma.Masu;
        //            Masu dstMasu = dstKoma.Masu;

        //            Finger figKoma = Util_Sky.Fingers_AtMasuNow(src_Sky_genzai,srcMasu).ToFirst();

        //            kmList.Add(new Couple<Finger, Masu>(figKoma, dstMasu));
        //    }

        //    return kmList;
        //}






        /// <summary>
        /// 「成り」ができる動きなら真。
        /// </summary>
        /// <returns></returns>
        public static bool IsPromotionable(
            out bool isPromotionable,
            SyElement srcMasu,
            SyElement dstMasu,
            Komasyurui14 srcKs,
            Playerside pside
            )
        {
            bool successful = true;
            isPromotionable = false;

            if (Okiba.ShogiBan != Conv_Masu.ToOkiba(srcMasu))//srcKoma.Masu
            {
                successful = false;
                goto gt_EndMethod;
            }

            if (Util_Komasyurui14.IsNari(srcKs))//srcKoma.Komasyurui
            {
                // 既に成っている駒は、「成り」の指し手を追加すると重複エラーになります。
                // 成りになれない、で正常終了します。
                goto gt_EndMethod;
            }

            int srcDan;
            Okiba srcOkiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(srcMasu));
            if (srcOkiba==Okiba.ShogiBan)
            {
                if (!Conv_Masu.ToDan_FromBanjoMasu(srcMasu, out srcDan))
                {
                    throw new Exception("段に変換失敗");
                }
            }
            else
            {
                if (!Conv_Masu.ToDan_FromBangaiMasu(srcMasu, out srcDan))
                {
                    throw new Exception("段に変換失敗");
                }
            }

            int dstDan;
            Okiba dstOkiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(dstMasu));
            if (dstOkiba == Okiba.ShogiBan)
            {
                if (!Conv_Masu.ToDan_FromBanjoMasu(dstMasu, out dstDan))
                {
                    throw new Exception("段に変換失敗");
                }
            }
            else
            {
                if (!Conv_Masu.ToDan_FromBangaiMasu(dstMasu, out dstDan))
                {
                    throw new Exception("段に変換失敗");
                }
            }


            // 先手か、後手かで大きく処理を分けます。
            switch (pside)
            {
                case Playerside.P1:
                    {
                        if (srcDan <= 3)
                        {
                            // 3段目から上にあった駒が移動したのなら、成りの資格ありです。
                            isPromotionable = true;
                            goto gt_EndMethod;
                        }

                        if (dstDan <= 3)
                        {
                            // 3段目から上に駒が移動したのなら、成りの資格ありです。
                            isPromotionable = true;
                            goto gt_EndMethod;
                        }
                    }
                    break;
                case Playerside.P2:
                    {
                        if (7 <= srcDan)
                        {
                            // 7段目から下にあった駒が移動したのなら、成りの資格ありです。
                            isPromotionable = true;
                            goto gt_EndMethod;
                        }

                        if (7 <= dstDan)
                        {
                            // 7段目から下に駒が移動したのなら、成りの資格ありです。
                            isPromotionable = true;
                            goto gt_EndMethod;
                        }
                    }
                    break;
                default: throw new Exception("未定義のプレイヤーサイドです。");
            }
        gt_EndMethod:
            ;
            return successful;
        }
        /// <summary>
        /// これが通称【水際のいんちきプログラム】なんだぜ☆
        /// 必要により、【成り】の指し手を追加します。
        /// </summary>
        public static void Add_KomaBETUAllNariSasites(
            Maps_OneAndMulti<Finger, Move> komaBETUAllMoves,
            Finger figKoma,
            SyElement srcMasu,
            SyElement dstMasu,
            Komasyurui14 srcKs,
            Komasyurui14 dstKs,
            Playerside pside
            )
        {
            try
            {
                bool isPromotionable;
                if (!Util_Sasu269.IsPromotionable(out isPromotionable,
                    srcMasu,
                    dstMasu,
                    srcKs,
                    pside
                    ))
                {
                    goto gt_EndMethod;
                }

                // 成りの資格があれば、成りの指し手を作ります。
                if (isPromotionable)
                {
                    //MessageBox.Show("成りの資格がある駒がありました。 src=["+srcKoma.Masu.Word+"]["+srcKoma.Syurui+"]");

                    Move move = Conv_Move.ToMove(
                        srcMasu,
                        dstMasu,
                        srcKs,
                        Komasyurui14.H00_Null___,//取った駒不明
                        true,//Util_Komasyurui14.ToNariCase(dstKs)//強制的に【成り】に駒の種類を変更
                        false,//成り駒を作るので、ドロップの可能性は無いぜ☆（＾▽＾）
                        pside,
                        false
                        );

                    // TODO: 一段目の香車のように、既に駒は成っている場合があります。無い指し手だけ追加するようにします。
                    komaBETUAllMoves.AddNotOverwrite(figKoma, move);
                }

            gt_EndMethod:
                ;
            }
            catch (Exception ex)
            {
                throw new Exception("Convert04.cs#AddNariSasiteでｴﾗｰ。:" + ex.GetType().Name + ":" + ex.Message);
            }
        }




    }
}
