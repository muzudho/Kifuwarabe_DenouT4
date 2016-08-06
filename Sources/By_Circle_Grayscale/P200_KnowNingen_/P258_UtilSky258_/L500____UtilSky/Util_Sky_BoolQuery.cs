﻿using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P212_ConvPside__.L500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P214_Masu_______.L500____Util;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;
using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P258_UtilSky258_.L500____UtilSky
{
    public abstract class Util_Sky_BoolQuery
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 含まれるか判定。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masus"></param>
        /// <returns></returns>
        public static bool ExistsIn(Move move, SySet<SyElement> masus, SkyConst src_Sky, KwErrorHandler errH)
        {
            bool matched = false;

            SyElement srcMasu = Conv_Move.ToSrcMasu(move);
            Playerside pside = Conv_Move.ToPlayerside(move);

            foreach (SyElement masu in masus.Elements)
            {
                Finger finger = Util_Sky_FingerQuery.InShogibanMasuNow(src_Sky, pside, masu, errH);

                if (
                    finger != Fingers.Error_1  //2014-07-21 先後も見るように追記。
                    && srcMasu == masu
                    )
                {
                    matched = true;
                    break;
                }
            }

            return matched;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 相手陣に入っていれば真。
        /// ************************************************************************************************************************
        /// 
        ///         後手は 7,8,9 段。
        ///         先手は 1,2,3 段。
        /// </summary>
        /// <returns></returns>
        public static bool InAitejin(Move move)
        {
            int dstDan = Conv_Move.ToDstDan(move);

            return (Util_Sky_BoolQuery.IsGote(move) && 7 <= dstDan) || (Util_Sky_BoolQuery.IsSente(move) && dstDan <= 3);
        }
        public static bool InAitejin(SyElement dstMasu, Playerside pside)
        {
            return Util_Masu10.InAitejin(dstMasu, pside);
        }

        /// <summary>
        /// 成り
        /// </summary>
        public static bool IsNari(Move move)
        {
            Komasyurui14 ks = Conv_Move.ToDstKomasyurui(move);

            return Util_Komasyurui14.FlagNari[(int)ks];
        }

        /// <summary>
        /// 不成
        /// </summary>
        public static bool IsFunari(Move move)
        {
            Komasyurui14 ks = Conv_Move.ToDstKomasyurui(move);

            return !Util_Komasyurui14.FlagNari[(int)ks];
        }

        public static bool IsNareruKoma(Move move)
        {
            Komasyurui14 ks = Conv_Move.ToDstKomasyurui(move);

            return Util_Komasyurui14.FlagNareruKoma[(int)ks];
        }
        public static bool IsNareruKoma(Komasyurui14 ks)
        {
            return Util_Komasyurui14.FlagNareruKoma[(int)ks];
        }

        /// <summary>
        /// 不一致判定：　先後、駒種類  が、自分と同じものが　＜ひとつもない＞
        /// </summary>
        /// <returns></returns>
        public static bool NeverOnaji(Move move, SkyConst src_Sky, params Fingers[] komaGroupArgs)
        {
            bool unmatched = true;

            Playerside pside1 = Conv_Move.ToPlayerside(move);
            Komasyurui14 ks1 = Conv_Move.ToDstKomasyurui(move);

            foreach (Fingers komaGroup in komaGroupArgs)
            {
                foreach (Finger figKoma in komaGroup.Items)
                {
                    src_Sky.AssertFinger(figKoma);
                    Busstop busstop = src_Sky.BusstopIndexOf(figKoma);

                    if (
                            pside1 == Conv_Busstop.ToPlayerside(busstop) // 誰のものか
                        && ks1 == Conv_Busstop.ToKomasyurui(busstop) // 駒の種類は
                        )
                    {
                        // １つでも一致するものがあれば、終了します。
                        unmatched = false;
                        goto gt_EndLoop;
                    }
                }

            }
        gt_EndLoop:

            return unmatched;
        }

        /*
        /// <summary>
        /// ************************************************************************************************************************
        /// FIXME: 駒台の上にあれば真。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public static bool OnKomadai(Move move)
        {
            bool result;

            SyElement dstMasu = Conv_Move.ToDstMasu(move);

            result = (Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Conv_SyElement.ToOkiba(dstMasu)//FIXME: 駒台の筋段は設定できないのでは？
                );

            return result;
        }
        */

        /// <summary>
        /// ************************************************************************************************************************
        /// 先後一致判定。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="ms2"></param>
        /// <returns></returns>
        public static bool MatchPside(Move move1, Move move2)
        {
            Playerside pside1 = Conv_Move.ToPlayerside(move1);
            Playerside pside2 = Conv_Move.ToPlayerside(move2);

            return pside1 == pside2;
        }

        /// <summary>
        /// 先手
        /// </summary>
        /// <returns></returns>
        public static bool IsSente(Move move)
        {
            return Playerside.P1 == Conv_Move.ToPlayerside(move);
        }
        /// <summary>
        /// 先手
        /// </summary>
        /// <returns></returns>
        public static bool IsSente(Busstop ms)
        {
            return Playerside.P1 == Conv_Busstop.ToPlayerside(ms);
        }

        /// <summary>
        /// 後手
        /// </summary>
        /// <returns></returns>
        public static bool IsGote(Move move)
        {
            return Playerside.P2 == Conv_Move.ToPlayerside(move);
        }
        /// <summary>
        /// 後手
        /// </summary>
        /// <returns></returns>
        public static bool IsGote(Busstop ms)
        {
            return Playerside.P2 == Conv_Busstop.ToPlayerside(ms);
        }

        /// <summary>
        /// “打” ＜アクション時＞
        /// </summary>
        /// <returns></returns>
        public static bool IsDaAction(Move move)
        {
            bool result;

            try
            {
                SyElement srcMasu = Conv_Move.ToSrcMasu(move);
                result = Okiba.ShogiBan != Conv_SyElement.ToOkiba(srcMasu)//駒台（駒袋）から打ったとき。
                    && Okiba.Empty != Conv_SyElement.ToOkiba(srcMasu);//初期配置から移動しても、打にはしません。
            }
            catch (Exception ex)
            {
                Util_OwataMinister.ERROR.DonimoNaranAkirameta(ex, "IsDaAction:");// exceptionArea=" + exceptionArea
                throw ex;
            }

            return result;
        }

        public static bool isEnableSfen(Move move)
        {
            bool enable = true;

            SyElement srcMasu = Conv_Move.ToSrcMasu(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);

            int srcDan;
            if (!Util_MasuNum.TryMasuToDan(srcMasu, out srcDan))
            {
                enable = false;
            }

            int dan;
            if (!Util_MasuNum.TryMasuToDan(dstMasu, out dan))
            {
                enable = false;
            }

            return enable;
        }

        /// <summary>
        /// 成った
        /// </summary>
        /// <returns></returns>
        public static bool IsNatta_Sasite(Move move)
        {
            // 元種類が不成、現種類が成　の場合のみ真。
            bool natta = true;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Komasyurui14 dstKs = Conv_Move.ToDstKomasyurui(move);


            // 成立しない条件を１つでも満たしていれば、偽　確定。
            if (
                Komasyurui14.H00_Null___ == srcKs
                ||
                Komasyurui14.H00_Null___ == dstKs
                ||
                Util_Komasyurui14.FlagNari[(int)srcKs]
                ||
                !Util_Komasyurui14.FlagNari[(int)dstKs]
                )
            {
                natta = false;
            }

            return natta;
        }

        /// <summary>
        /// 移動前と、移動後の場所が異なっていれば真。
        /// </summary>
        /// <returns></returns>
        public static bool DoneMove(Move move)
        {
            SyElement srcMasu = Conv_Move.ToSrcMasu(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);

            return Conv_SyElement.ToMasuNumber(dstMasu) != Conv_SyElement.ToMasuNumber(srcMasu);
        }

    }
}
