﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA
{


    public abstract class Util_IttesasuRoutine
    {
        /// <summary>
        /// 一手指します。
        /// </summary>
        /// <param name="ittesasuArg"></param>
        /// <param name="ittesasu_mutable"></param>
        /// <param name="syuryoResult"></param>
        /// <param name="errH"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void DoMove(
            out IttesasuResult syuryoResult,
            Move kaisiMove,//これから指されるはずの手。棋譜に記録するために「指す前／指した後」を含めた手。
            Sky kaisiSky,// 一手指し、開始局面。
            KwLogger errH,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            // 退避☆
            Playerside kaisiTebanside = kaisiSky.KaisiPside;//一手指し、開始局面、手番。
            int syuryoTemezumi = kaisiSky.Temezumi + 1;//1手進める //これから作る局面の、手目済み。



            int exceptionArea = 0;


            // 編集対象ノード（巻き戻し時と、進む時で異なる）
            Move editNodeRef_Move;
            Sky editNodeRef_KyokumenW;
            try
            {
                //------------------------------
                // 用意
                //------------------------------
                exceptionArea = 1010;
                syuryoResult = new IttesasuResultImpl(Fingers.Error_1, Fingers.Error_1, Move.Empty, null, Komasyurui14.H00_Null___);

                exceptionArea = 1040;
                //------------------------------
                // 符号(局面)の追加
                //------------------------------
                {
                    //進むときは、必ずノードの追加と、カレントの移動がある。

                    //現局面ノードのクローンを作成します。
                    editNodeRef_Move = kaisiMove;
                    editNodeRef_KyokumenW = new SkyImpl(kaisiSky);
                    editNodeRef_KyokumenW.SetKaisiPside(
                        Conv_Playerside.Reverse(editNodeRef_KyokumenW.KaisiPside));
                    editNodeRef_KyokumenW.SetTemezumi(syuryoTemezumi);
                }


                exceptionArea = 1050;
                //------------------------------
                // 動かす駒を移動先へ。
                //------------------------------
                //Debug.Assert(null != ittesasuArg.KorekaranoSasite, "これからの指し手がヌルでした。");
                Finger figMovedKoma;
                Util_IttesasuRoutine.Do24_UgokasuKoma_IdoSakiHe(
                    out figMovedKoma,
                    kaisiMove,
                    kaisiTebanside,
                    kaisiSky,
                    errH
                    //hint
                    );
                syuryoResult.FigMovedKoma = figMovedKoma; //動かした駒更新
                Debug.Assert(Fingers.Error_1 != syuryoResult.FigMovedKoma, "動かした駒がない☆！？エラーだぜ☆！");


                exceptionArea = 1060;
                SyElement dstMasu = Conv_Move.ToDstMasu(kaisiMove);
                Komasyurui14 dstKs = Conv_Move.ToDstKomasyurui(kaisiMove);
                Busstop afterStar;
                {
                    afterStar = Util_IttesasuRoutine.Do36_KomaOnDestinationMasu(
                        dstKs,
                        kaisiMove,
                        editNodeRef_KyokumenW
                        );
                }



                exceptionArea = 1070;
                // Sky 局面データは、この関数の途中で何回か変更されます。ローカル変数に退避しておくと、同期が取れなくなります。

                //------------------------------------------------------------
                // 駒を取る
                //------------------------------------------------------------
                Finger figFoodKoma = Fingers.Error_1;
                Busstop food_koma = Busstop.Empty;
                Playerside food_pside = Playerside.Empty;
                SyElement food_akiMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);
                {
                    Util_IttesasuRoutine.Do61_KomaToru(
                        afterStar,
                        editNodeRef_KyokumenW,
                        out figFoodKoma,
                        out food_koma,
                        out food_pside,
                        out food_akiMasu,
                        errH
                        );

                    if (Fingers.Error_1 != figFoodKoma)
                    {
                        //>>>>> 指した先に駒があったなら
                        syuryoResult.FoodKomaSyurui = Conv_Busstop.ToKomasyurui( food_koma);
                    }
                    else
                    {
                        syuryoResult.FoodKomaSyurui = Komasyurui14.H00_Null___;
                    }
                }
                Debug.Assert(figMovedKoma != Fingers.Error_1, "駒を動かせなかった？1");


                exceptionArea = 1080;
                if (Fingers.Error_1 != figFoodKoma)
                {
                    //------------------------------------------------------------
                    // 指した駒と、取った駒の移動
                    //------------------------------------------------------------

                    //------------------------------
                    // 局面データの書き換え
                    //------------------------------
                    editNodeRef_KyokumenW.SetTemezumi(syuryoTemezumi);
                    editNodeRef_KyokumenW.AddObjects(
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
                    editNodeRef_KyokumenW.SetTemezumi(syuryoTemezumi);// これから作る局面の、手目済み。
                    editNodeRef_KyokumenW.AddObjects(
                        //
                        // 指した駒
                        //
                        new Finger[] { figMovedKoma }, new Busstop[] { afterStar }
                        );
                }


                exceptionArea = 1090;
                syuryoResult.FigFoodKoma = figFoodKoma; //取った駒更新
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って無視します。
                string message = "Util_IttesasuRoutine#Execute（B）： exceptionArea=" + exceptionArea + "\n" + ex.GetType().Name + "：" + ex.Message;
                errH.AppendLine(message);
                errH.Flush(LogTypes.Error);
                throw ex;
            }




            if (syuryoResult.FoodKomaSyurui != Komasyurui14.H00_Null___)
            {
                // 元のキーの、取った駒の種類だけを差替えます。
                editNodeRef_Move = Conv_Move.SetCaptured(
                    editNodeRef_Move,
                    syuryoResult.FoodKomaSyurui
                    );
            }


            //
            // ノード
            syuryoResult.SyuryoMove = editNodeRef_Move;
            syuryoResult.SyuryoKyokumenW = editNodeRef_KyokumenW;
            // この変数を返すのがポイント。棋譜とは別に、現局面。
        }

        /// <summary>
        /// 棋譜ツリーのカレントを変更します。
        /// </summary>
        public static void UpdateKifuTree(Tree kifu1, IttesasuResult ittesasuResult)
        {
            Node newNode = new NodeImpl(ittesasuResult.SyuryoMove, ittesasuResult.SyuryoKyokumenW);
            if (!kifu1.CurNode.HasTuginoitte(ittesasuResult.SyuryoMove))
            {
                //----------------------------------------
                // 次ノード追加（なければ）
                //----------------------------------------
                kifu1.GetSennititeCounter().CountUp_New(Conv_Sky.ToKyokumenHash(ittesasuResult.SyuryoKyokumenW), "After3_ChangeCurrent(次の一手なし)");
                kifu1.CurNode.PutTuginoitte_New(newNode);//次ノートを追加します。
            }
            else
            {
                //----------------------------------------
                // 次ノード上書き（あれば）
                //----------------------------------------
                kifu1.GetSennititeCounter().CountUp_New(Conv_Sky.ToKyokumenHash(ittesasuResult.SyuryoKyokumenW), "After3_ChangeCurrent（次の一手あり）");
                kifu1.CurNode.PutTuginoitte_Override(newNode);//次ノートを上書きします。
            }

            Node temp = kifu1.CurNode;
            kifu1.SetCurNode(newNode);//次ノードを、これからのカレントとします。
            newNode.SetParentNode(temp);
        }


        /// <summary>
        /// 動かす駒を移動先へ。
        /// </summary>
        /// <param name="figMovedKoma"></param>
        /// <param name="sasite">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="obsoluted_kifu_mutable"></param>
        /// <param name="isMakimodosi"></param>
        private static void Do24_UgokasuKoma_IdoSakiHe(
            out Finger figMovedKoma,
            Move move,
            Playerside kaisi_tebanside,
            Sky kaisi_Sky,
            KwLogger errH,
            //string hint,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            int exceptionArea = 0;

            try
            {
                exceptionArea = 99001000;
                figMovedKoma = Fingers.Error_1;

                //------------------------------------------------------------
                // 選択  ：  動かす駒
                //------------------------------------------------------------
                // 進むとき
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                //Debug.Assert(null != sasite, "Sasu24_UgokasuKoma_IdoSakiHe: 指し手がヌルでした。");
                if (Util_Sky_BoolQuery.IsDaAction(move))// 多分、ここで sasite がヌルになるエラーがある☆
                {
                    //----------
                    // 駒台から “打”
                    //----------
                    exceptionArea = 99002000;

                    SyElement srcMasu = Conv_Move.ToSrcMasu(move);
                    Komasyurui14 dstKs14 = Conv_Move.ToDstKomasyurui(move);

                    exceptionArea = 99002100;
                    // FIXME: 駒台の、どの駒を拾うか？
                    figMovedKoma = Util_Sky_FingerQuery.InOkibaSyuruiNow_IgnoreCase(
                        kaisi_Sky,
                        Conv_SyElement.ToOkiba(srcMasu),
                        dstKs14,
                        errH
                        );
                    Debug.Assert(figMovedKoma != Fingers.Error_1, "駒を動かせなかった？14");
                }
                else
                {
                    exceptionArea = 99003000;
                    //----------
                    // 将棋盤から
                    //----------

                    SyElement srcMasu = Conv_Move.ToSrcMasu(move);
                    Debug.Assert( !Masu_Honshogi.IsErrorBasho(srcMasu), "srcKoma.Masuエラー。15");
                    SyElement dstMasu = Conv_Move.ToDstMasu(move);
                    Playerside pside = Conv_Move.ToPlayerside(move);

                    exceptionArea = 99003100;
                    figMovedKoma = Util_Sky_FingerQuery.InMasuNow_FilteringBanjo(
                        kaisi_Sky,
                        pside,
                        srcMasu,// 将棋盤上と確定している☆（＾▽＾）
                        errH
                        );
                    Debug.Assert(figMovedKoma != Fingers.Error_1, "駒を動かせなかった？13");
                }
            }
            catch(Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って無視します。
                errH.DonimoNaranAkirameta(ex, "Util_IttesasuRoutine#Sasu24_UgokasuKoma_IdoSakiHe： exceptionArea=" + exceptionArea+"\n"
                    //+"hint=["+hint+"]"
                    );
                throw ex;
            }
        }



        /// <summary>
        /// [巻戻し]時の、駒台にもどる動きを吸収。
        /// </summary>
        /// <param name="syurui2"></param>
        /// <param name="sasite">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        /// <returns></returns>
        private static Busstop Do36_KomaOnDestinationMasu(
            Komasyurui14 syurui2,
            Move move,
            Sky src_Sky)
        {
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);

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
            Sky susunda_Sky_orNull_before,//駒を取られたとき、局面を変更します。
            out Finger out_figFoodKoma,
            out Busstop out_food_koma,
            out Playerside pside,
            out SyElement akiMasu,
            KwLogger errH
            )
        {
            //----------
            // 将棋盤上のその場所に駒はあるか
            //----------
            out_figFoodKoma = Util_Sky_FingersQuery.InMasuNow_Old(susunda_Sky_orNull_before, Conv_Busstop.ToMasu( dstKoma)).ToFirst();//盤上


            if (Fingers.Error_1 != out_figFoodKoma)
            {
                //>>>>> 指した先に駒があったなら

                //
                // 取られる駒
                //
                susunda_Sky_orNull_before.AssertFinger(out_figFoodKoma);
                out_food_koma = susunda_Sky_orNull_before.BusstopIndexOf(out_figFoodKoma);
#if DEBUG
                if (null != errH.KwDisplayer_OrNull.Dlgt_OnLog1Append_or_Null)
                {
                    errH.KwDisplayer_OrNull.Dlgt_OnLog1Append_or_Null("駒取った=" + Conv_Busstop.ToKomasyurui( out_food_koma) + Environment.NewLine);
                }
#endif
                //
                // 取られる駒は、駒置き場の空きマスに移動させます。
                //
                Okiba okiba;
                switch (Conv_Busstop.ToPlayerside( dstKoma))
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

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("エラー：　先後がおかしいです。");
                            sb.AppendLine("dst.Pside=" + Conv_Busstop.ToPlayerside( dstKoma));
                            throw new Exception(sb.ToString());
                        }
                }

                //
                // 駒台に駒を置く動き
                //
                {
                    // 駒台の空きスペース
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(okiba, susunda_Sky_orNull_before);


                    if (Masu_Honshogi.IsErrorBasho( akiMasu))
                    {
                        //>>>>> エラー：　駒台に空きスペースがありませんでした。

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("エラー：　駒台に空きスペースがありませんでした。");
                        sb.AppendLine("駒台=" + Okiba.Gote_Komadai);
                        throw new Exception(sb.ToString());
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
        public static SyElement GetKomadaiKomabukuroSpace(Okiba okiba, Sky src_Sky)
        {
            SyElement akiMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);

            // 先手駒台または後手駒台の、各マスの駒がある場所を調べます。
            bool[] exists = new bool[Conv_Masu10.KOMADAI_KOMABUKURO_SPACE_LENGTH];//駒台スペースは40マスです。


            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToOkiba(koma) == okiba)
                {
                    exists[
                        Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu( koma)) - Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(okiba))
                        ] = true;
                }
            });


            //駒台スペースは40マスです。
            for (int i = 0; i < Conv_Masu10.KOMADAI_KOMABUKURO_SPACE_LENGTH;i++ )
            {
                if (!exists[i])
                {
                    akiMasu = Masu_Honshogi.Masus_All[i + Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(okiba))];
                    goto gt_EndMethod;
                }
            }

        gt_EndMethod:

            //System.C onsole.WriteLine("ゲット駒台駒袋スペース＝" + akiMasu);

            return akiMasu;
        }


    }


}
