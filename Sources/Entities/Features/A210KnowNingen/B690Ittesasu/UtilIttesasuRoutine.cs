﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{


    public abstract class UtilIttesasuRoutine
    {
        /// <summary>
        /// 一手指します。
        /// </summary>
        /// <param name="ittesasuArg"></param>
        /// <param name="ittesasu_mutable"></param>
        /// <param name="syuryoResult"></param>
        /// <param name="logTag"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void DoMove_Normal(
            out IIttesasuResult syuryoResult,
            ref Move move1,//このメソッド実行後、取った駒を上書きされることがあるぜ☆（＾▽＾）
            IPosition positionA,// 一手指し、開始局面。
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            //------------------------------
            // 用意
            //------------------------------
            syuryoResult = new IttesasuResultImpl(Fingers.Error_1, Fingers.Error_1, null, Komasyurui14.H00_Null___);

            //------------------------------
            // 動かす駒を移動先へ。
            //------------------------------
            //Debug.Assert(null != ittesasuArg.KorekaranoMove, "これからの指し手がヌルでした。");
            Finger figMovedKoma;
            UtilIttesasuRoutine.Do24_UgokasuKoma_IdoSakiHe(
                out figMovedKoma,
                move1,
                positionA);
            syuryoResult.FigMovedKoma = figMovedKoma; //動かした駒更新
            Debug.Assert(Fingers.Error_1 != syuryoResult.FigMovedKoma, "動かした駒がない☆！？エラーだぜ☆！");

            SyElement dstMasu = ConvMove.ToDstMasu(move1);
            Komasyurui14 dstKs = ConvMove.ToDstKomasyurui(move1);
            Busstop afterStar;
            {
                afterStar = UtilIttesasuRoutine.Do36_KomaOnDestinationMasu(
                    dstKs,
                    move1,
                    positionA
                    );
            }

            // IPosition 局面データは、この関数の途中で何回か変更されます。ローカル変数に退避しておくと、同期が取れなくなります。

            //------------------------------------------------------------
            // 駒を取る
            //------------------------------------------------------------
            Finger figFoodKoma = Fingers.Error_1;
            Busstop food_koma = Busstop.Empty;
            Playerside food_pside = Playerside.Empty;
            SyElement food_akiMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);
            {
                UtilIttesasuRoutine.Do61_KomaToru(
                    afterStar,
                    positionA,
                    out figFoodKoma,
                    out food_koma,
                    out food_pside,
                    out food_akiMasu);

                if (Fingers.Error_1 != figFoodKoma)
                {
                    //>>>>> 指した先に駒があったなら
                    syuryoResult.FoodKomaSyurui = Conv_Busstop.ToKomasyurui(food_koma);
                }
                else
                {
                    syuryoResult.FoodKomaSyurui = Komasyurui14.H00_Null___;
                }
            }
            Debug.Assert(figMovedKoma != Fingers.Error_1, "駒を動かせなかった？1");

            if (Fingers.Error_1 != figFoodKoma)
            {
                //------------------------------------------------------------
                // 指した駒と、取った駒の移動
                //------------------------------------------------------------

                //------------------------------
                // 局面データの書き換え
                //------------------------------
                positionA.AddObjects(
                    //
                    // 指した駒と、取った駒
                    //
                    new Finger[] { figMovedKoma,//指した駒番号
                            figFoodKoma// 取った駒
                    },
                    new Busstop[] { afterStar,//指した駒
                            Conv_Busstop.ToBusstop(
                            food_pside,
                            food_akiMasu,//駒台の空きマスへ
                            Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui( food_koma))// 取られた駒の種類。表面を上に向ける。
                        )// 取った駒
                    }
                    );
            }
            else
            {
                //------------------------------------------------------------
                // 指した駒の移動
                //------------------------------------------------------------

                //駒を取って変化しているかもしれない？
                positionA.AddObjects(
                    //
                    // 指した駒
                    //
                    new Finger[] { figMovedKoma }, new Busstop[] { afterStar }
                    );
            }

            syuryoResult.FigFoodKoma = figFoodKoma; //取った駒更新

            if (syuryoResult.FoodKomaSyurui != Komasyurui14.H00_Null___)
            {
                // 元のキーの、取った駒の種類だけを差替えます。
                move1 = ConvMove.SetCaptured(
                    move1,
                    syuryoResult.FoodKomaSyurui
                    );
            }


            //------------------------------
            // 駒を進めてから、先後と手目済を進めること。
            //------------------------------
            {
                positionA.ReversePlayerside();


                positionA.SetTemezumi(positionA.Temezumi + 1);
            }

            //
            // ノード
            syuryoResult.SyuryoKyokumenW = positionA;
            // この変数を返すのがポイント。棋譜とは別に、現局面。
        }

        /// <summary>
        /// 棋譜ツリーのカレントを変更します。
        /// </summary>
        public static void BeforeUpdateKifuTree(
            IPlaying playing,
            ITree kifu1,
            Move move,
            IPosition positionA)
        {
            MoveEx newNodeB = new MoveExImpl(move);

            //----------------------------------------
            // 次ノード追加
            //----------------------------------------
            playing.GetSennititeCounter().CountUp_New(
                Conv_Sky.ToKyokumenHash(positionA), "After3_ChangeCurrent(次の一手なし)");

            //次ノードを、これからのカレントとします。
            kifu1.MoveEx_SetCurrent(TreeImpl.OnDoCurrentMove(newNodeB, kifu1, positionA));
        }


        /// <summary>
        /// 動かす駒を移動先へ。
        /// </summary>
        /// <param name="figMovedKoma"></param>
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="obsoluted_kifu_mutable"></param>
        /// <param name="isMakimodosi"></param>
        private static void Do24_UgokasuKoma_IdoSakiHe(
            out Finger figMovedKoma,
            Move move,
            IPosition positionA
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            figMovedKoma = Fingers.Error_1;

            //------------------------------------------------------------
            // 選択  ：  動かす駒
            //------------------------------------------------------------
            // 進むとき
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

            //Debug.Assert(null != move, "Sasu24_UgokasuKoma_IdoSakiHe: 指し手がヌルでした。");
            if (UtilSkyBoolQuery.IsDaAction(move))// 多分、ここで move がヌルになるエラーがある☆
            {
                //----------
                // 駒台から “打”
                //----------
                Fingers fingers = UtilSkyFingersQuery.InMasuNow_New(positionA, move);
                if (fingers.Count < 1)
                {
                    throw new Exception($"Util_IttesasuRoutine#Do24:指し手に該当する駒が無かったぜ☆（＾～＾） move={ConvMove.ToLog(move)}");
                }
                figMovedKoma = fingers.ToFirst();
            }
            else
            {
                //----------
                // 将棋盤から
                //----------

                SyElement srcMasu = ConvMove.ToSrcMasu(move, positionA);
                Debug.Assert(!Masu_Honshogi.IsErrorBasho(srcMasu), "srcKoma.Masuエラー。15");
                SyElement dstMasu = ConvMove.ToDstMasu(move);
                Playerside pside = ConvMove.ToPlayerside(move);

                figMovedKoma = UtilSkyFingerQuery.InMasuNow_FilteringBanjo(
                    positionA,
                    pside,
                    srcMasu// 将棋盤上と確定している☆（＾▽＾）
                    );
                Debug.Assert(figMovedKoma != Fingers.Error_1, "駒を動かせなかった？13");
            }
        }



        /// <summary>
        /// [巻戻し]時の、駒台にもどる動きを吸収。
        /// </summary>
        /// <param name="syurui2"></param>
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        /// <returns></returns>
        private static Busstop Do36_KomaOnDestinationMasu(
            Komasyurui14 syurui2,
            Move move,
            IPosition src_Sky)
        {
            Playerside pside = ConvMove.ToPlayerside(move);
            SyElement dstMasu = ConvMove.ToDstMasu(move);

            // 次の位置
            return Conv_Busstop.ToBusstop(pside,
                dstMasu,
                syurui2);
        }



        /// <summary>
        /// 駒を取る動き。
        /// </summary>
        private static void Do61_KomaToru(
            Busstop dstKoma,
            IPosition susunda_Sky_orNull_before,//駒を取られたとき、局面を変更します。
            out Finger out_figFoodKoma,
            out Busstop out_food_koma,
            out Playerside pside,
            out SyElement akiMasu
            )
        {
            //----------
            // 将棋盤上のその場所に駒はあるか
            //----------
            out_figFoodKoma = UtilSkyFingersQuery.InMasuNow_Old(susunda_Sky_orNull_before, Conv_Busstop.ToMasu(dstKoma)).ToFirst();//盤上


            if (Fingers.Error_1 != out_figFoodKoma)
            {
                //>>>>> 指した先に駒があったなら

                //
                // 取られる駒
                //
                susunda_Sky_orNull_before.AssertFinger(out_figFoodKoma);
                out_food_koma = susunda_Sky_orNull_before.BusstopIndexOf(out_figFoodKoma);
#if DEBUG
                //if (null != logTag.KwDisplayerOrNull.OnAppendLog)
                //{
                //    logTag.KwDisplayerOrNull.OnAppendLog("駒取った=" + Conv_Busstop.ToKomasyurui(out_food_koma) + Environment.NewLine);
                //}
#endif
                //
                // 取られる駒は、駒置き場の空きマスに移動させます。
                //
                Okiba okiba;
                switch (Conv_Busstop.ToPlayerside(dstKoma))
                {
                    case Playerside.P1:
                        {
                            okiba = Okiba.Sente_Komadai;
                            pside = Playerside.P1;
                        }
                        break;
                    case Playerside.P2:
                        {
                            okiba = Okiba.Gote_Komadai;
                            pside = Playerside.P2;
                        }
                        break;
                    default:
                        {
                            //>>>>> エラー：　先後がおかしいです。
                            throw new Exception($"エラー：　先後がおかしいです。 dst.Pside={Conv_Busstop.ToPlayerside(dstKoma)}");
                        }
                }

                //
                // 駒台に駒を置く動き
                //
                {
                    // 駒台の空きスペース
                    akiMasu = UtilIttesasuRoutine.GetKomadaiKomabukuroSpace(okiba, susunda_Sky_orNull_before);

                    if (Masu_Honshogi.IsErrorBasho(akiMasu))
                    {
                        //>>>>> エラー：　駒台に空きスペースがありませんでした。
                        throw new Exception($"エラー：　駒台に空きスペースがありませんでした。 駒台={Okiba.Gote_Komadai}");
                    }
                    //>>>>> 駒台に空きスペースがありました。
                }
            }
            else
            {
                out_food_koma = Busstop.Empty;
                pside = Playerside.Empty;
                akiMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);
            }
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// 駒台の空いている升を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba">先手駒台、または後手駒台</param>
        /// <param name="uc_Main">メインパネル</param>
        /// <returns>置ける場所。無ければヌル。</returns>
        public static SyElement GetKomadaiKomabukuroSpace(Okiba okiba, IPosition src_Sky)
        {
            SyElement akiMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);

            // 先手駒台または後手駒台の、各マスの駒がある場所を調べます。
            bool[] exists = new bool[Conv_Masu.KOMADAI_KOMABUKURO_SPACE_LENGTH];//駒台スペースは40マスです。


            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToOkiba(koma) == okiba)
                {
                    exists[
                        Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma)) - Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(okiba))
                        ] = true;
                }
            });


            //駒台スペースは40マスです。
            for (int i = 0; i < Conv_Masu.KOMADAI_KOMABUKURO_SPACE_LENGTH; i++)
            {
                if (!exists[i])
                {
                    akiMasu = Masu_Honshogi.Masus_All[i + Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(okiba))];
                    goto gt_EndMethod;
                }
            }

        gt_EndMethod:

            //System.C onsole.WriteLine("ゲット駒台駒袋スペース＝" + akiMasu);

            return akiMasu;
        }


    }


}
