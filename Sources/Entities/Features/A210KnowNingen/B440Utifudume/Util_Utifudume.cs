using System;
using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG

#endif

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    /// <summary>
    /// 打ち歩詰め
    /// </summary>
    public abstract class Util_Utifudume
    {

        /// <summary>
        /// 打ち歩詰め処理。
        /// 
        /// TODO:打ち歩詰めチェック
        /// </summary>
        public static void Utifudume(
            Playerside psideA,// = positionA.GetKaisiPside();
            ISky positionA,
            SySet<SyElement> masus_mikata_onBanjo,//打ち歩詰めチェック用
            SySet<SyElement> masus_aite_onBanjo,//打ち歩詰めチェック用
            SySet<SyElement>[] aMasus//駒種類別、置こうとする升
            )
        {
            // 攻め側
            //Playerside psideA = positionA.GetKaisiPside();

            // 相手の王の位置
            Busstop king_aite;
            Finger figKing_aite;
            Playerside pside_aite;


            switch (psideA)
            {
                case Playerside.P1:
                    pside_aite = Playerside.P2;
                    figKing_aite = Finger_Honshogi.GoteOh;
                    positionA.AssertFinger(figKing_aite);
                    king_aite = positionA.BusstopIndexOf(figKing_aite);
                    break;
                case Playerside.P2:
                    pside_aite = Playerside.P1;
                    figKing_aite = Finger_Honshogi.SenteOh;
                    positionA.AssertFinger(figKing_aite);
                    king_aite = positionA.BusstopIndexOf(figKing_aite);
                    break;
                default: throw new Exception("エラー：打ち歩詰めチェック中。プレイヤー不明。");
            }

            // 相手の玉頭の升。
            SyElement masu_gyokutou = null;
            {
                SySet<SyElement> sySet = KomanoKidou.DstIppo_上(pside_aite, Conv_Busstop.ToMasu(king_aite));
                foreach (SyElement element2 in sySet.Elements)//最初の１件を取る。
                {
                    masu_gyokutou = element2;
                    break;
                }

                if (null == masu_gyokutou)
                {
                    goto gt_EndUtifudume;
                }
            }

            // 相手の玉。
            Fingers fingers_aiteKing = new Fingers();
            fingers_aiteKing.Add(figKing_aite);

            // 相手の玉頭に、自分側の利きがあるかどうか。
            bool isKiki_mikata = false;
            {
                // 利き一覧
                Maps_OneAndOne<Finger, SySet<SyElement>> kikiMap = Query_FingersMasusSky.To_KomabetuKiki_OnBanjo(
                    fingers_aiteKing,
                    masus_mikata_onBanjo,
                    masus_aite_onBanjo,
                    positionA);

                int gyokutouMasuNumber = Conv_Masu.ToMasuHandle(masu_gyokutou);
                kikiMap.Foreach_Values((SySet<SyElement> values, ref bool toBreak) =>
                {
                    foreach (SyElement element in values.Elements)
                    {
                        int masuNumber = Conv_Masu.ToMasuHandle(element);
                        if (masuNumber == gyokutouMasuNumber)
                        {
                            isKiki_mikata = true;
                            toBreak = true;
                            break;
                        }
                    }
                });
            }

            if (!isKiki_mikata)
            {
                goto gt_EndUtifudume;
            }

            // 相手の玉頭に、利きのある相手側の駒の種類の一覧。
            List<Komasyurui14> ksList = new List<Komasyurui14>();
            SySet<SyElement> aitegyokuKiki;
            {
                // 相手側の盤上の駒一覧。
                Fingers fingers_aiteKoma_Banjo = UtilSkyFingersQuery.InOkibaPsideNow(positionA, Okiba.ShogiBan, pside_aite);

                // 利き一覧
                Maps_OneAndOne<Finger, SySet<SyElement>> kikiMap = Query_FingersMasusSky.To_KomabetuKiki_OnBanjo(
                    fingers_aiteKoma_Banjo,
                    masus_aite_onBanjo,//相手の駒は、味方
                    masus_mikata_onBanjo,//味方の駒は、障害物。
                    positionA);
                aitegyokuKiki = kikiMap.ElementAt(figKing_aite);

                int gyokutouMasuNumber = Conv_Masu.ToMasuHandle(masu_gyokutou);
                kikiMap.Foreach_Entry((Finger figKoma, SySet<SyElement> values, ref bool toBreak) =>
                {
                    foreach (SyElement element in values.Elements)
                    {
                        int masuNumber = Conv_Masu.ToMasuHandle(element);
                        if (masuNumber == gyokutouMasuNumber)
                        {
                            positionA.AssertFinger(figKoma);
                            ksList.Add(Conv_Busstop.ToKomasyurui(positionA.BusstopIndexOf(figKoma)));
                            break;
                        }
                    }
                });
            }

            // 「王様でしか取れない状態」ではなければ、スルー。
            if (ksList.Count != 1)
            {
                goto gt_EndUtifudume;
            }
            else if (ksList[0] != Komasyurui14.H06_Gyoku__)
            {
                goto gt_EndUtifudume;
            }

            // 「王様に逃げ道がある」なら、スルー。
            IMasubetuKikisu masubetuKikisu_semeKoma = Util_SkyPside.ToMasubetuKikisu(positionA, psideA);
            Dictionary<int, int> nigerarenaiMap = new Dictionary<int, int>();
            switch (psideA)
            {
                case Playerside.P1: nigerarenaiMap = masubetuKikisu_semeKoma.Kikisu_AtMasu_2P; break;
                case Playerside.P2: nigerarenaiMap = masubetuKikisu_semeKoma.Kikisu_AtMasu_1P; break;
                default: throw new Exception("エラー：打ち歩詰めチェック中。プレイヤー不明。");
            }
            foreach (SyElement element in aitegyokuKiki.Elements)
            {
                // 攻撃側の利きが利いていない、空きマスがあるかどうか。
                int movableMasuNumber_king = Conv_Masu.ToMasuHandle(element);

                if (nigerarenaiMap[movableMasuNumber_king] == 0)
                {
                    // 逃げ切った☆！
                    goto gt_EndUtifudume;
                }
            }

            //----------------------------------------
            // 打ち歩詰め確定
            //----------------------------------------

            // １升（玉頭升）を、クリアーします。

            aMasus[(int)Komasyurui14.H01_Fu_____].Minus_Closed(
                masu_gyokutou, Util_SyElement_BinaryOperator.Dlgt_Equals_MasuNumber);


        gt_EndUtifudume:
            ;
        }

    }
}
