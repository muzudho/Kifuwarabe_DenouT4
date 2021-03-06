﻿using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class UtilSkyBoolQuery
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 含まれるか判定。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masus"></param>
        /// <returns></returns>
        public static bool ExistsIn(Move move, SySet<SyElement> masus, IPosition positionA)
        {
            bool matched = false;

            SyElement srcMasu = ConvMove.ToSrcMasu(move, positionA);
            Playerside pside = ConvMove.ToPlayerside(move);

            foreach (SyElement masu in masus.Elements)
            {
                Finger finger = UtilSkyFingerQuery.InMasuNow_FilteringBanjo(positionA, pside, masu);

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
            int dstDan = ConvMove.ToDstDan(move);

            return (UtilSkyBoolQuery.IsGote(move) && 7 <= dstDan) || (UtilSkyBoolQuery.IsSente(move) && dstDan <= 3);
        }
        public static bool InBanjoAitejin(SyElement dstMasu, Playerside pside)
        {
            return Conv_Masu.InBanjoAitejin(dstMasu, pside);
        }

        /// <summary>
        /// 成り
        /// </summary>
        public static bool IsNari(Move move)
        {
            Komasyurui14 ks = ConvMove.ToDstKomasyurui(move);

            return Util_Komasyurui14.FlagNari[(int)ks];
        }

        /// <summary>
        /// 不成
        /// </summary>
        public static bool IsFunari(Move move)
        {
            Komasyurui14 ks = ConvMove.ToDstKomasyurui(move);

            return !Util_Komasyurui14.FlagNari[(int)ks];
        }

        public static bool IsNareruKoma(Move move)
        {
            Komasyurui14 ks = ConvMove.ToDstKomasyurui(move);

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
        public static bool NeverOnaji(Move move, IPosition src_Sky, params Fingers[] komaGroupArgs)
        {
            bool unmatched = true;

            Playerside pside1 = ConvMove.ToPlayerside(move);
            Komasyurui14 ks1 = ConvMove.ToDstKomasyurui(move);

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

            SyElement dstMasu = ConvMove.ToDstMasu(move);

            result = (Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Conv_Masu.ToOkiba(dstMasu)//FIXME: 駒台の筋段は設定できないのでは？
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
            Playerside pside1 = ConvMove.ToPlayerside(move1);
            Playerside pside2 = ConvMove.ToPlayerside(move2);

            return pside1 == pside2;
        }

        /// <summary>
        /// 先手
        /// </summary>
        /// <returns></returns>
        public static bool IsSente(Move move)
        {
            return Playerside.P1 == ConvMove.ToPlayerside(move);
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
            return Playerside.P2 == ConvMove.ToPlayerside(move);
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

            SyElement srcMasu = ConvMove.ToSrcMasu(move);
            result = Okiba.ShogiBan != Conv_Masu.ToOkiba(srcMasu)//駒台（駒袋）から打ったとき。
                && Okiba.Empty != Conv_Masu.ToOkiba(srcMasu);//初期配置から移動しても、打にはしません。

            return result;
        }

        public static bool isEnableSfen(Move move)
        {
            bool enable = true;

            SyElement srcMasu = ConvMove.ToSrcMasu(move);
            SyElement dstMasu = ConvMove.ToDstMasu(move);

            Okiba okiba;

            // src
            okiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(srcMasu));
            if (okiba == Okiba.ShogiBan)
            {
                int srcDan;
                if (!Conv_Masu.ToDan_FromBanjoMasu(srcMasu, out srcDan))
                {
                    enable = false;
                }
            }
            else
            {
                int srcDan;
                if (!Conv_Masu.ToDan_FromBangaiMasu(srcMasu, out srcDan))
                {
                    enable = false;
                }
            }

            // dst
            okiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(dstMasu));
            if (okiba == Okiba.ShogiBan)
            {
                int dan;
                if (!Conv_Masu.ToDan_FromBanjoMasu(dstMasu, out dan))
                {
                    enable = false;
                }
            }
            else
            {
                int dan;
                if (!Conv_Masu.ToDan_FromBangaiMasu(dstMasu, out dan))
                {
                    enable = false;
                }
            }

            return enable;
        }

        /// <summary>
        /// 成った
        /// </summary>
        /// <returns></returns>
        public static bool IsNattaMove(Move move)
        {
            // 元種類が不成、現種類が成　の場合のみ真。
            bool natta = true;

            Komasyurui14 srcKs = ConvMove.ToSrcKomasyurui(move);
            Komasyurui14 dstKs = ConvMove.ToDstKomasyurui(move);


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
            SyElement srcMasu = ConvMove.ToSrcMasu(move);
            SyElement dstMasu = ConvMove.ToDstMasu(move);

            return Conv_Masu.ToMasuHandle(dstMasu) != Conv_Masu.ToMasuHandle(srcMasu);
        }

    }
}
