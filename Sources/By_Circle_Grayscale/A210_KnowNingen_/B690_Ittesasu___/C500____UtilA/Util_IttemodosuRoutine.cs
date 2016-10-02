using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using System.Diagnostics;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using System;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA
{

    /// <summary>
    /// 一手戻すルーチン。
    /// </summary>
    public abstract class Util_IttemodosuRoutine
    {

        /// <summary>
        /// 一手巻き戻す
        /// </summary>
        /// <param name="ittemodosuResult"></param>
        /// <param name="kaisi_Temezumi"></param>
        /// <param name="moved"></param>
        /// <param name="kaisiKyokumenW"></param>
        /// <param name="logger"></param>
        public static void UndoMove(
            out IttemodosuResult ittemodosuResult,
            Move moved,
            Playerside psideA,
            Sky positionA,
            string hint,
            KwLogger logger
            )
        {
            long exception_area = 1000140;
            try
            {
                bool log = false;
                if (log)
                {
                    logger.AppendLine("戻す前 " + hint);
                    logger.Append(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(psideA, positionA, logger)));
                    logger.Flush(LogTypes.Plain);
                }

                ittemodosuResult = new IttemodosuResultImpl(Fingers.Error_1, Fingers.Error_1, null, Komasyurui14.H00_Null___);
                Finger figMovedKoma;

                //
                // 動かす駒を移動先へ。
                //
                Util_IttemodosuRoutine.Undo25_UgokasuKoma(
                    out figMovedKoma,
                    moved,
                    positionA,
                    logger
                    );
                ittemodosuResult.FigMovedKoma = figMovedKoma; //動かした駒更新

                exception_area = 20000;

                if (Fingers.Error_1 == figMovedKoma)
                {
                    logger.DonimoNaranAkirameta(
                        "戻せる駒が無かった☆ hint:" + hint + "\n" +
                        Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(psideA, positionA,logger), positionA, moved)
                        );
                    goto gt_EndMethod;
                }

                //
                // 巻き戻しなら、非成りに戻します。
                //
                Komasyurui14 syurui2 = Util_IttemodosuRoutine.Do30_MakimodosiNara_HinariNiModosu(moved);

                exception_area = 30000;

                // 戻し先か。
                Busstop dst = Util_IttemodosuRoutine.Undo37_KomaOnModosisakiMasu(syurui2,
                        moved,
                        positionA);

                exception_area = 40000;

                // Sky 局面データは、この関数の途中で何回か変更されます。ローカル変数に退避しておくと、同期が取れなくなります。

                //------------------------------------------------------------
                // あれば、取られていた駒を取得
                //------------------------------------------------------------
                Finger figFoodKoma;//取られていた駒
                Util_IttemodosuRoutine.Do62_TorareteitaKoma_ifExists(
                    out figFoodKoma,//変更される場合あり。
                    moved,
                    positionA,//巻き戻しのとき
                    logger
                    );
                ittemodosuResult.FigFoodKoma = figFoodKoma; //取られていた駒更新

                // １手戻す前に、先後を逆転させて、手目済みカウントを減らします。
                positionA.DecreasePsideTemezumi();

                exception_area = 50000;

                //------------------------------------------------------------
                // 指されていた駒の移動
                //------------------------------------------------------------
                positionA.AddObjects(
                    //
                    // 動かした駒と、戻し先
                    //
                    new Finger[] { figMovedKoma }, new Busstop[] { dst });

                exception_area = 60000;

                if (Fingers.Error_1 != figFoodKoma)
                {
                    //------------------------------------------------------------
                    // 取られていた駒を戻す
                    //------------------------------------------------------------

                    //------------------------------
                    // 指し手の、取った駒部分を差替えます。
                    //------------------------------
                    SyElement dstMasu = Conv_Move.ToDstMasu(moved);
                    Playerside pside10 = Conv_Move.ToPlayerside(moved);
                    Komasyurui14 captured = Conv_Move.ToCaptured(moved);

                    positionA.AddObjects(
                        //
                        // 指されていた駒と、取られていた駒
                        //
                        new Finger[] { figFoodKoma },
                        new Busstop[] { Conv_Busstop.ToBusstop(
                            Conv_Playerside.Reverse(pside10),//先後を逆にして盤上に置きます。
                            dstMasu,// マス
                            captured
                        )
                        }
                        );
                }
                // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                // この時点で、必ず現局面データに差替えあり
                // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

                exception_area = 700011;

                // ノード
                ittemodosuResult.SyuryoSky = positionA;// この変数を返すのがポイント。棋譜とは別に、現局面。

                exception_area = 700021;

                gt_EndMethod:
                if (log)
                {

                    exception_area = 700031;

                    logger.AppendLine("戻した後 " + hint);

                    exception_area = 700041;

                    ShogibanImpl shogiban = Conv_Sky.ToShogiban(psideA, positionA,logger);

                    exception_area = 700051;

                    logger.Append(
                        Conv_Shogiban.ToLog_Type2(
                            shogiban,
                            positionA, moved)
                    );

                    exception_area = 700051;

                    logger.Flush(LogTypes.Plain);

                    exception_area = 700061;
                }
            }
            catch (Exception ex)
            {
                logger.DonimoNaranAkirameta(ex, "駒を戻しているとき☆ hint=" + hint + " exception_area="+ exception_area);
                throw ex;
            }
        }

        /// <summary>
        /// 動かす駒を移動先へ。
        /// </summary>
        /// <param name="figMovedKoma"></param>
        /// <param name="sasite">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="obsoluted_kifu_mutable"></param>
        /// <param name="isMakimodosi"></param>
        private static void Undo25_UgokasuKoma(
            out Finger figMovedKoma,
            Move moved,
            Sky positionA,
            KwLogger errH
            )
        {
            //------------------------------------------------------------
            // 選択  ：  動かす駒
            //------------------------------------------------------------
            // [巻戻し]のとき
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

            // 打った駒も、指した駒も、結局は将棋盤の上にあるはず。

            // 動かす駒
            figMovedKoma = Util_Sky_FingerQuery.InMasuNow_FilteringBanjo(
                positionA,
                Conv_Move.ToPlayerside(moved),
                Conv_Move.ToDstMasu(moved),//[巻戻し]のときは、先位置が　駒の居場所。
                errH
                );
            Debug.Assert(figMovedKoma != Fingers.Error_1, "駒を動かせなかった？ Dst="+ Conv_Masu.ToLog_FromBanjo(Conv_Move.ToDstMasu(moved)));
        }

        /// <summary>
        /// [巻戻し]時の、駒台にもどる動きを吸収。
        /// </summary>
        /// <param name="syurui2"></param>
        /// <param name="sasite">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        /// <returns></returns>
        private static Busstop Undo37_KomaOnModosisakiMasu(
            Komasyurui14 syurui2,
            Move moved,
            Sky positionA
            )
        {
            Playerside pside = Conv_Move.ToPlayerside(moved);

            SyElement masu;
            if (Conv_Move.ToDrop(moved))
            {
                // 打なら

                // 駒台の空いている場所
                
                masu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Conv_Playerside.ToKomadai(pside), positionA);
                // 必ず空いている場所があるものとします。
            }
            else
            {
                // 打以外なら
                // 戻し先
                SyElement srcMasu = Conv_Move.ToSrcMasu(moved, positionA);

                if (
                    Okiba.Gote_Komadai == Conv_Masu.ToOkiba(srcMasu)
                    || Okiba.Sente_Komadai == Conv_Masu.ToOkiba(srcMasu)
                    )
                {
                    //（古い仕様） １手前が駒台なら

                    // 駒台の空いている場所
                    masu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Conv_Masu.ToOkiba(srcMasu), positionA);
                    // 必ず空いている場所があるものとします。
                }
                else
                {
                    //>>>>> １手前が将棋盤上なら

                    // その位置
                    masu = srcMasu;//戻し先
                }
            }




            return Conv_Busstop.ToBusstop(
                pside,
                masu,//戻し先
                syurui2);
        }

        /// <summary>
        /// あれば、取られていた駒を取得
        /// </summary>
        /// <param name="sasite"></param>
        /// <param name="kaisi_Sky"></param>
        /// <param name="out_figFoodKoma"></param>
        /// <param name="errH"></param>
        private static void Do62_TorareteitaKoma_ifExists(
            out Finger out_figFoodKoma,
            Move move,
            Sky kaisi_Sky,//巻き戻しのとき
            KwLogger errH
        )
        {
            Komasyurui14 captured = Conv_Move.ToCaptured(move);
            if (Komasyurui14.H00_Null___ != captured)
            {
                //----------------------------------------
                // 取られていた駒があった場合
                //----------------------------------------
                Playerside pside = Conv_Move.ToPlayerside(move);

                // 駒台から、駒を検索します。
                Okiba okiba;
                if (Playerside.P2 == pside)
                {
                    okiba = Okiba.Gote_Komadai;
                }
                else
                {
                    okiba = Okiba.Sente_Komadai;
                }


                // 取った駒は、種類が同じなら、駒台のどの駒でも同じです。
                out_figFoodKoma = Util_Sky_FingerQuery.InOkibaSyuruiNow_IgnoreCase(
                    kaisi_Sky, okiba, captured, errH);
            }
            else
            {
                //----------------------------------------
                // 駒は取られていなかった場合
                //----------------------------------------
                out_figFoodKoma = Fingers.Error_1;
            }
        }

        /// <summary>
        /// 巻き戻しなら、非成りに戻します。
        /// </summary>
        /// <param name="sasite">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="isBack"></param>
        /// <returns></returns>
        private static Komasyurui14 Do30_MakimodosiNara_HinariNiModosu(
            Move move)
        {
            //------------------------------------------------------------
            // 確定  ：  移動先升
            //------------------------------------------------------------
            Komasyurui14 syurui2;
            {
                //----------
                // 成るかどうか
                //----------

                Komasyurui14 dstKs = Conv_Move.ToDstKomasyurui(move);


                if (Util_Sky_BoolQuery.IsNatta_Sasite(move))
                {
                    // 正順で成ったのなら、巻戻しでは「非成」に戻します。
                    syurui2 = Util_Komasyurui14.NarazuCaseHandle(dstKs);
                }
                else
                {
                    syurui2 = dstKs;
                }
            }

            return syurui2;
        }

    }


}
