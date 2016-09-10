using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;

using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;

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
        /// <param name="errH"></param>
        public static void UndoMove(
            out IttemodosuResult ittemodosuResult,
            int kaisi_Temezumi,
            Move moved,
            Sky kaisi_Sky,
            KwLogger errH
            )
        {
            ittemodosuResult = new IttemodosuResultImpl(Fingers.Error_1, Fingers.Error_1, null, Komasyurui14.H00_Null___);

            //
            // 動かす駒を移動先へ。
            //
            Finger figMovedKoma;
            Util_IttemodosuRoutine.Do25_UgokasuKoma(
                out figMovedKoma,
                moved,
                kaisi_Sky,
                errH
                );
            ittemodosuResult.FigMovedKoma = figMovedKoma; //動かした駒更新


            if (Fingers.Error_1 == ittemodosuResult.FigMovedKoma)
            {
                errH.DonimoNaranAkirameta("戻せる駒が無かった☆");
                return;
            }


            //
            // 巻き戻しなら、非成りに戻します。
            //
            Komasyurui14 syurui2 = Util_IttemodosuRoutine.Do30_MakimodosiNara_HinariNiModosu(moved);


            Busstop dst;
            {
                dst = Util_IttemodosuRoutine.Do37_KomaOnDestinationMasu(syurui2,
                    moved,
                    kaisi_Sky);
            }



            // Sky 局面データは、この関数の途中で何回か変更されます。ローカル変数に退避しておくと、同期が取れなくなります。

            //------------------------------------------------------------
            // あれば、取られていた駒を取得
            //------------------------------------------------------------
            Finger figFoodKoma;//取られていた駒
            Util_IttemodosuRoutine.Do62_TorareteitaKoma_ifExists(
                out figFoodKoma,//変更される場合あり。
                moved,
                kaisi_Sky,//巻き戻しのとき
                errH
                );
            ittemodosuResult.FigFoodKoma = figFoodKoma; //取られていた駒更新

            kaisi_Sky.SetTemezumi(kaisi_Temezumi - 1);

            //------------------------------------------------------------
            // 指されていた駒の移動
            //------------------------------------------------------------
            kaisi_Sky.AddObjects(
                //
                // 指されていた駒
                //
                new Finger[] { figMovedKoma }, new Busstop[] { dst });

            if (Fingers.Error_1 != figFoodKoma)
            {
                //------------------------------------------------------------
                // 取られていた駒を戻す
                //------------------------------------------------------------

                //------------------------------
                // 指し手の、取った駒部分を差替えます。
                //------------------------------
                SyElement dstMasu = Conv_Move.ToDstMasu(moved);
                Playerside pside = Conv_Move.ToPlayerside(moved);
                Komasyurui14 captured = Conv_Move.ToCaptured(moved);

                kaisi_Sky.AddObjects(
                    //
                    // 指されていた駒と、取られていた駒
                    //
                    new Finger[] { figFoodKoma },
                    new Busstop[] { Conv_Busstop.ToBusstop(
                        Conv_Playerside.Reverse(pside),//先後を逆にして盤上に置きます。
                        dstMasu,// マス
                        captured
                    )
                    }
                    );
            }
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            // この時点で、必ず現局面データに差替えあり
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■


            // ノード
            ittemodosuResult.SyuryoSky = kaisi_Sky;// この変数を返すのがポイント。棋譜とは別に、現局面。
        }

        public static void UpdateKifuTree(
            KifuTree kifu_mutable
            )
        {
            Node<Move, Sky> removedLeaf = kifu_mutable.PopCurrentNode();
        }


        /// <summary>
        /// 動かす駒を移動先へ。
        /// </summary>
        /// <param name="figMovedKoma"></param>
        /// <param name="sasite">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="obsoluted_kifu_mutable"></param>
        /// <param name="isMakimodosi"></param>
        private static void Do25_UgokasuKoma(
            out Finger figMovedKoma,
            Move move,
            Sky kaisi_Sky,
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
                kaisi_Sky,
                Conv_Move.ToPlayerside(move),
                Conv_Move.ToDstMasu(move),//[巻戻し]のときは、先位置が　駒の居場所。
                errH
                );
            Debug.Assert(figMovedKoma != Fingers.Error_1, "駒を動かせなかった？ Dst="+ Conv_MasuNum.ToLog_FromBanjoMasu(Conv_Move.ToDstMasu(move)));
        }

        /// <summary>
        /// [巻戻し]時の、駒台にもどる動きを吸収。
        /// </summary>
        /// <param name="syurui2"></param>
        /// <param name="sasite">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        /// <returns></returns>
        private static Busstop Do37_KomaOnDestinationMasu(
            Komasyurui14 syurui2,
            Move move,
            Sky src_Sky
            )
        {
            SyElement srcMasu = Conv_Move.ToSrcMasu(move);
            Playerside pside = Conv_Move.ToPlayerside(move);

            SyElement masu;

            if (
                Okiba.Gote_Komadai == Conv_SyElement.ToOkiba(srcMasu)
                || Okiba.Sente_Komadai == Conv_SyElement.ToOkiba(srcMasu)
                )
            {
                //>>>>> １手前が駒台なら

                // 駒台の空いている場所
                masu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Conv_SyElement.ToOkiba(srcMasu), src_Sky);
                // 必ず空いている場所があるものとします。
            }
            else
            {
                //>>>>> １手前が将棋盤上なら

                // その位置
                masu = srcMasu;//戻し先
            }



            return Conv_Busstop.ToBusstop(pside,
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
