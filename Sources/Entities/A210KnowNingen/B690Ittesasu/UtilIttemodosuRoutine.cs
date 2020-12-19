using System;
using System.Diagnostics;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B310Shogiban.C250Struct;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA
{

    /// <summary>
    /// 一手戻すルーチン。
    /// </summary>
    public abstract class UtilIttemodosuRoutine
    {

        /// <summary>
        /// 一手巻き戻す
        /// </summary>
        /// <param name="ittemodosuResult"></param>
        /// <param name="kaisi_Temezumi"></param>
        /// <param name="moved"></param>
        /// <param name="kaisiKyokumenW"></param>
        /// <param name="logTag"></param>
        public static void UndoMove(
            out IIttemodosuResult ittemodosuResult,
            Move moved,
            Playerside psideA,
            ISky positionA,
            string hint,
            ILogTag logTag
            )
        {
            long exception_area = 1000140;
            try
            {
                bool log = false;
                if (log)
                {
                    Logger.AppendLine(logTag,"戻す前 " + hint);
                    Logger.Append(logTag, Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(psideA, positionA, logTag)));
                    Logger.Flush(logTag, LogTypes.Plain);
                }

                ittemodosuResult = new IttemodosuResultImpl(Fingers.Error_1, Fingers.Error_1, null, Komasyurui14.H00_Null___);
                Finger figMovedKoma;

                //
                // 動かす駒を移動先へ。
                //
                UtilIttemodosuRoutine.Undo25_UgokasuKoma(
                    out figMovedKoma,
                    moved,
                    positionA,
                    logTag
                    );
                ittemodosuResult.FigMovedKoma = figMovedKoma; //動かした駒更新

                exception_area = 20000;

                if (Fingers.Error_1 == figMovedKoma)
                {
                    Logger.Panic(logTag,
                        "戻せる駒が無かった☆ hint:" + hint + "\n" +
                        Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(psideA, positionA, logTag), positionA, moved)
                        );
                    goto gt_EndMethod;
                }

                //
                // 巻き戻しなら、非成りに戻します。
                //
                Komasyurui14 syurui2 = UtilIttemodosuRoutine.Do30_MakimodosiNara_HinariNiModosu(moved);

                exception_area = 30000;

                // 戻し先か。
                Busstop dst = UtilIttemodosuRoutine.Undo37_KomaOnModosisakiMasu(syurui2,
                        moved,
                        positionA);

                exception_area = 40000;

                // ISky 局面データは、この関数の途中で何回か変更されます。ローカル変数に退避しておくと、同期が取れなくなります。

                //------------------------------------------------------------
                // あれば、取られていた駒を取得
                //------------------------------------------------------------
                Finger figFoodKoma;//取られていた駒
                UtilIttemodosuRoutine.Do62_TorareteitaKoma_ifExists(
                    out figFoodKoma,//変更される場合あり。
                    moved,
                    positionA,//巻き戻しのとき
                    logTag
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
                    SyElement dstMasu = ConvMove.ToDstMasu(moved);
                    Playerside pside10 = ConvMove.ToPlayerside(moved);
                    Komasyurui14 captured = ConvMove.ToCaptured(moved);

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

                    Logger.AppendLine(logTag,"戻した後 " + hint);

                    exception_area = 700041;

                    ShogibanImpl shogiban = Conv_Sky.ToShogiban(psideA, positionA, logTag);

                    exception_area = 700051;

                    Logger.Append(logTag,
                        Conv_Shogiban.ToLog_Type2(
                            shogiban,
                            positionA, moved)
                    );

                    exception_area = 700051;

                    Logger.Flush(logTag, LogTypes.Plain);

                    exception_area = 700061;
                }
            }
            catch (Exception ex)
            {
                Logger.Panic(logTag, ex, "駒を戻しているとき☆ hint=" + hint + " exception_area=" + exception_area);
                throw;
            }
        }

        /// <summary>
        /// 動かす駒を移動先へ。
        /// </summary>
        /// <param name="figMovedKoma"></param>
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="obsoluted_kifu_mutable"></param>
        /// <param name="isMakimodosi"></param>
        private static void Undo25_UgokasuKoma(
            out Finger figMovedKoma,
            Move moved,
            ISky positionA,
            ILogTag logTag
            )
        {
            //------------------------------------------------------------
            // 選択  ：  動かす駒
            //------------------------------------------------------------
            // [巻戻し]のとき
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

            // 打った駒も、指した駒も、結局は将棋盤の上にあるはず。

            // 動かす駒
            figMovedKoma = UtilSkyFingerQuery.InMasuNow_FilteringBanjo(
                positionA,
                ConvMove.ToPlayerside(moved),
                ConvMove.ToDstMasu(moved),//[巻戻し]のときは、先位置が　駒の居場所。
                logTag
                );
            Debug.Assert(figMovedKoma != Fingers.Error_1, "駒を動かせなかった？ Dst=" + Conv_Masu.ToLog_FromBanjo(ConvMove.ToDstMasu(moved)));
        }

        /// <summary>
        /// [巻戻し]時の、駒台にもどる動きを吸収。
        /// </summary>
        /// <param name="syurui2"></param>
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        /// <returns></returns>
        private static Busstop Undo37_KomaOnModosisakiMasu(
            Komasyurui14 syurui2,
            Move move,
            ISky positionA
            )
        {
            Playerside pside = ConvMove.ToPlayerside(move);

            SyElement masu;
            if (ConvMove.ToDrop(move))
            {
                // 打なら

                // 駒台の空いている場所

                masu = UtilIttesasuRoutine.GetKomadaiKomabukuroSpace(Conv_Playerside.ToKomadai(pside), positionA);
                // 必ず空いている場所があるものとします。
            }
            else
            {
                // 打以外なら
                // 戻し先
                SyElement srcMasu = ConvMove.ToSrcMasu(move, positionA);

                if (
                    Okiba.Gote_Komadai == Conv_Masu.ToOkiba(srcMasu)
                    || Okiba.Sente_Komadai == Conv_Masu.ToOkiba(srcMasu)
                    )
                {
                    //（古い仕様） １手前が駒台なら

                    // 駒台の空いている場所
                    masu = UtilIttesasuRoutine.GetKomadaiKomabukuroSpace(Conv_Masu.ToOkiba(srcMasu), positionA);
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
        /// <param name="move"></param>
        /// <param name="kaisi_Sky"></param>
        /// <param name="out_figFoodKoma"></param>
        /// <param name="logTag"></param>
        private static void Do62_TorareteitaKoma_ifExists(
            out Finger out_figFoodKoma,
            Move move,
            ISky kaisi_Sky,//巻き戻しのとき
            ILogTag logTag
        )
        {
            Komasyurui14 captured = ConvMove.ToCaptured(move);
            if (Komasyurui14.H00_Null___ != captured)
            {
                //----------------------------------------
                // 取られていた駒があった場合
                //----------------------------------------
                Playerside pside = ConvMove.ToPlayerside(move);

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
                out_figFoodKoma = UtilSkyFingerQuery.InOkibaSyuruiNow_IgnoreCase(
                    kaisi_Sky, okiba, captured, logTag);
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
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
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

                Komasyurui14 dstKs = ConvMove.ToDstKomasyurui(move);


                if (UtilSkyBoolQuery.IsNattaMove(move))
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
