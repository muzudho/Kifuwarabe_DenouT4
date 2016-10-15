using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C250____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C260____Operator;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A450_Server_____.B110_Server_____.C250____Util;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___125_Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___492_Widgets;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___499_Repaint;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C080____Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C125____Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C249____Function;
using System.Collections.Generic;
using System.Drawing;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
using System.Windows.Forms;
#endif

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C250____Timed
{


    /// <summary>
    /// マウス操作の一連の流れです。（主に１手指す動き）
    /// </summary>
    public class TimedB_MouseCapture : Timed_Abstract
    {

        private MainGui_Csharp mainGui;

        /// <summary>
        /// マウス操作の状態です。
        /// </summary>
        public Queue<MouseEventState> MouseEventQueue { get; set; }


        public static void Check_MouseoverKomaKiki(object obj_shogiGui, Finger finger, KwLogger logger)
        {
            MainGui_Csharp shogiGui = (MainGui_Csharp)obj_shogiGui;

            shogiGui.SkyWrapper_Gui.GuiSky.AssertFinger(finger);
            Busstop busstop = shogiGui.SkyWrapper_Gui.GuiSky.BusstopIndexOf(finger);

            shogiGui.Shape_PnlTaikyoku.Shogiban.KikiBan = new SySet_Default<SyElement>("利き盤");// .Clear();

            // 駒の利き
            SySet<SyElement> kikiZukei = Util_Sky_SyugoQuery.KomaKidou_Potential(finger, shogiGui.SkyWrapper_Gui.GuiSky);
            //kikiZukei.DebugWrite("駒の利きLv1");

            // 味方の駒
            Sky positionA = shogiGui.Link_Server.KifuTree.PositionA;
            Playerside psideA = positionA.GetKaisiPside();

            //shogiGui.Model_PnlTaikyoku.Kifu.AssertPside(shogiGui.Model_PnlTaikyoku.Kifu.CurNode, "Check_MouseoverKomaKiki",errH);
            SySet<SyElement> mikataZukei = Util_Sky_SyugoQuery.Masus_Now(
                positionA, psideA
                );
            //mikataZukei.DebugWrite("味方の駒");

            // 駒の利き上に駒がないか。
            SySet<SyElement> ban2 = kikiZukei.Minus_Closed(mikataZukei, Util_SyElement_BinaryOperator.Dlgt_Equals_MasuNumber);
            //kikiZukei.DebugWrite("駒の利きLv2");

            shogiGui.Shape_PnlTaikyoku.Shogiban.KikiBan = ban2;

        }



        public TimedB_MouseCapture(MainGui_Csharp shogibanGui)
        {
            this.mainGui = shogibanGui;
            this.MouseEventQueue = new Queue<MouseEventState>();
        }


        public override void Step(KwLogger logger)
        {
            // 入っているマウス操作イベントのうち、マウスムーブは　１つに　集約　します。
            bool bMouseMove_SceneB_1TumamitaiKoma = false;

            // 入っているマウス操作イベントは、全部捨てていきます。
            MouseEventState[] queue = this.MouseEventQueue.ToArray();
            this.MouseEventQueue.Clear();
            foreach (MouseEventState eventState in queue)
            {
                switch (this.mainGui.Scene)
                {
                    case SceneName.SceneB_1TumamitaiKoma:
                        {
                            #region つまみたい駒


                            switch (eventState.Name2)
                            {
                                case MouseEventStateName.Arive:
                                    {
                                        #region アライブ
                                        //------------------------------
                                        // メナス
                                        //------------------------------
                                        Util_Menace.Menace(this.mainGui, eventState.Flg_logTag);
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseMove:
                                    {
                                        #region マウスムーブ
                                        if (bMouseMove_SceneB_1TumamitaiKoma)
                                        {
                                            continue;
                                        }
                                        bMouseMove_SceneB_1TumamitaiKoma = true;

                                        Sky src_Sky = mainGui.SkyWrapper_Gui.GuiSky;

                                        Point mouse = eventState.MouseLocation;

                                        //----------
                                        // 将棋盤：升目
                                        //----------
                                        foreach (UserWidget widget in mainGui.Widgets.Values)
                                        {
                                            if (
                                                eventState.WindowName == widget.Window &&
                                                "Masu" == widget.Type && Okiba.ShogiBan == widget.Okiba)
                                            {
                                                Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                                                cell.LightByMouse(mouse.X, mouse.Y);
                                                if (cell.Light)
                                                {
                                                    mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                                }
                                                break;
                                            }
                                        }

                                        //----------
                                        // 駒置き、駒袋：升目
                                        //----------
                                        foreach (UserWidget widget in mainGui.Widgets.Values)
                                        {
                                            if (
                                                eventState.WindowName == widget.Window &&
                                                "Masu" == widget.Type && widget.Okiba.HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai | Okiba.KomaBukuro))
                                            {
                                                Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                                                cell.LightByMouse(mouse.X, mouse.Y);
                                                if (cell.Light)
                                                {
                                                    mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                                }
                                            }
                                        }

                                        //----------
                                        // 駒
                                        //----------
                                        foreach (Shape_BtnKomaImpl btnKoma in mainGui.Shape_PnlTaikyoku.Btn40Komas)
                                        {
                                            btnKoma.LightByMouse(mouse.X, mouse.Y);
                                            if (btnKoma.Light)
                                            {
                                                mainGui.RepaintRequest.SetFlag_RefreshRequest();

                                                src_Sky.AssertFinger(btnKoma.Koma);
                                                Busstop koma = src_Sky.BusstopIndexOf(btnKoma.Koma);

                                                if (Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma))
                                                {
                                                    // マウスオーバーした駒の利き
                                                    TimedB_MouseCapture.Check_MouseoverKomaKiki(mainGui, btnKoma.Koma, eventState.Flg_logTag);


                                                    break;
                                                }
                                            }
                                        }


                                        //----------
                                        // ウィジェット
                                        //----------
                                        foreach (UserWidget widget in mainGui.Widgets.Values)
                                        {
                                            if (
                                                eventState.WindowName == widget.Window &&
                                                widget.IsLight_OnFlowB_1TumamitaiKoma)
                                            {
                                                widget.LightByMouse(mouse.X, mouse.Y);
                                                if (widget.Light) { mainGui.RepaintRequest.SetFlag_RefreshRequest(); }
                                            }
                                        }

                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseLeftButtonDown:
                                    {
                                        #region マウス左ボタンダウン
                                        SceneName nextPhaseB = SceneName.Ignore;
                                        Sky src_Sky = mainGui.SkyWrapper_Gui.GuiSky;

                                        //----------
                                        // 駒
                                        //----------
                                        foreach (Shape_BtnKomaImpl btnKoma in mainGui.Shape_PnlTaikyoku.Btn40Komas)
                                        {
                                            if (btnKoma.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                            {
                                                //>>>>>>>>>> 駒にヒットしました。

                                                if (null != mainGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(mainGui))
                                                {
                                                    //>>>>>>>>>> 既に選択されている駒があります。→★成ろうとしたときの、取られる相手の駒かも。
                                                    goto gt_Next1;
                                                }

                                                // 既に選択されている駒には無効
                                                if (mainGui.FigTumandeiruKoma == (int)btnKoma.Koma)
                                                {
                                                    goto gt_Next1;
                                                }



                                                if (btnKoma.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y)) //>>>>> 駒をつまみました。
                                                {
                                                    // 駒をつまみます。
                                                    mainGui.SetFigTumandeiruKoma((int)btnKoma.Koma);
                                                    mainGui.Shape_PnlTaikyoku.SelectFirstTouch = true;

                                                    nextPhaseB = SceneName.SceneB_2OkuKoma;

                                                    src_Sky.AssertFinger(btnKoma.Koma);
                                                    mainGui.Shape_PnlTaikyoku.SetMouseBusstopOrNull2(
                                                        src_Sky.BusstopIndexOf(btnKoma.Koma)//TODO:改造
                                                        );

                                                    mainGui.Shape_PnlTaikyoku.SetHMovedKoma(Fingers.Error_1);
                                                    mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                                }
                                            }

                                        gt_Next1:
                                            ;
                                        }


                                        //----------
                                        // 既に選択されている駒
                                        //----------
                                        Shape_BtnKoma btnKoma_Selected = mainGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(mainGui);



                                        //----------
                                        // 各種ボタン
                                        //----------
                                        {
                                            foreach (UserWidget widget in mainGui.Widgets.Values)
                                            {
                                                if (
                                                    eventState.WindowName == widget.Window &&
                                                    widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    if (null != widget.Delegate_MouseHitEvent)
                                                    {
                                                        widget.Delegate_MouseHitEvent(
                                                            mainGui
                                                           , widget
                                                           , btnKoma_Selected
                                                           , eventState.Flg_logTag
                                                           );
                                                    }
                                                }
                                            }
                                        }


                                        mainGui.SetScene(nextPhaseB);

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        mainGui.Response("MouseOperation", eventState.Flg_logTag);
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseLeftButtonUp:
                                    {
                                        #region マウス左ボタンアップ
                                        Sky src_GuiSky = mainGui.SkyWrapper_Gui.GuiSky;

                                        //----------
                                        // 将棋盤：升目
                                        //----------
                                        foreach (UserWidget widget in mainGui.Widgets.Values)
                                        {
                                            if (
                                                eventState.WindowName == widget.Window &&
                                                "Masu" == widget.Type && Okiba.ShogiBan == widget.Okiba)
                                            {
                                                Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                                                if (cell.DeselectByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                                }
                                            }
                                        }

                                        //----------
                                        // 駒置き、駒袋：升目
                                        //----------
                                        foreach (UserWidget widget in mainGui.Widgets.Values)
                                        {
                                            if (
                                                eventState.WindowName == widget.Window &&
                                                "Masu" == widget.Type && widget.Okiba.HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai | Okiba.KomaBukuro))
                                            {
                                                Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                                                if (cell.DeselectByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                                }
                                            }
                                        }

                                        //----------
                                        // 駒
                                        //----------
                                        foreach (Shape_BtnKomaImpl btnKoma in mainGui.Shape_PnlTaikyoku.Btn40Komas)
                                        {
                                            if (btnKoma.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                            {
                                                //>>>>> つまんでいる駒から、指を放しました
                                                mainGui.SetFigTumandeiruKoma(-1);


                                                src_GuiSky.AssertFinger(btnKoma.Koma);
                                                Busstop koma1 = src_GuiSky.BusstopIndexOf(btnKoma.Koma);


                                                if (Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma1))
                                                {
                                                    //----------
                                                    // 移動済表示
                                                    //----------
                                                    mainGui.Shape_PnlTaikyoku.SetHMovedKoma(btnKoma.Koma);

                                                    //------------------------------
                                                    // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                                                    //------------------------------
                                                    // 棋譜

                                                    Busstop dstStarlight = mainGui.Shape_PnlTaikyoku.MouseBusstopOrNull2;
                                                    System.Diagnostics.Debug.Assert(Busstop.Empty != dstStarlight, "mouseStarlightがヌル");

                                                    src_GuiSky.AssertFinger(btnKoma.Koma);
                                                    Busstop srcStarlight = src_GuiSky.BusstopIndexOf(btnKoma.Koma);
                                                    System.Diagnostics.Debug.Assert(Busstop.Empty != srcStarlight, "komaStarlightがヌル");

                                                    Move move = Conv_Move.ToMove(
                                                        Conv_Busstop.ToMasu(dstStarlight),
                                                        Conv_Busstop.ToMasu(srcStarlight),
                                                        Conv_Busstop.ToKomasyurui(dstStarlight),
                                                        Conv_Busstop.ToKomasyurui(srcStarlight),//これで成りかどうか判定
                                                        mainGui.Shape_PnlTaikyoku.MousePos_FoodKoma != Busstop.Empty ? Conv_Busstop.ToKomasyurui( mainGui.Shape_PnlTaikyoku.MousePos_FoodKoma) : Komasyurui14.H00_Null___,
                                                        Conv_Busstop.ToPlayerside(dstStarlight),
                                                        false
                                                    );// 選択している駒の元の場所と、移動先

                                                    //
                                                    // TODO: 一手[巻戻し]のときは追加したくない
                                                    //
                                                    Sky sky_newChild = new SkyImpl(src_GuiSky);
                                                    sky_newChild.SetKaisiPside(Playerside.P2);//FIXME:人間が先手でハードコーディング中
                                                    sky_newChild.SetTemezumi(mainGui.SkyWrapper_Gui.GuiSky.Temezumi + 1);//1手進ませる。
                                                    MoveEx newNode = new MoveExImpl(move);
                                                    //MessageBox.Show(
                                                    //    "追加前\n"+
                                                    //    "newNode=KaisiPside=" + newNode.Value.ToKyokumenConst.KaisiPside,
                                                    //    "デバッグ");

                                                    //マウスの左ボタンを放したときです。
                                                    //----------------------------------------
                                                    // 次ノード追加
                                                    //----------------------------------------
                                                    mainGui.Link_Server.Earth.GetSennititeCounter().CountUp_New(Conv_Sky.ToKyokumenHash(sky_newChild), "TimedB.Step(1)");

                                                    mainGui.Link_Server.KifuTree.MoveEx_SetCurrent(TreeImpl.OnDoCurrentMove(newNode, mainGui.Link_Server.KifuTree, sky_newChild,logger));

                                                    string jsaFugoStr;
                                                    Util_Functions_Server.AfterSetCurNode_Srv(
                                                        mainGui.SkyWrapper_Gui,
                                                        mainGui.Link_Server.KifuTree.MoveEx_Current,
                                                        mainGui.Link_Server.KifuTree.MoveEx_Current.Move,
                                                        sky_newChild,
                                                        out jsaFugoStr,
                                                        mainGui.Link_Server.KifuTree,
                                                        logger);
                                                    mainGui.RepaintRequest.SetFlag_RefreshRequest();


                                                    //------------------------------
                                                    // 符号表示
                                                    //------------------------------
                                                    // つまみたい駒の上でマウスの左ボタンを放したとき。
                                                    mainGui.Shape_PnlTaikyoku.SetFugo(jsaFugoStr);



                                                    //------------------------------
                                                    // チェンジターン
                                                    //------------------------------
                                                    if (!mainGui.Shape_PnlTaikyoku.Requested_NaruDialogToShow)
                                                    {
                                                        mainGui.ChangedTurn(
                                                            mainGui.Link_Server.KifuTree,
                                                            mainGui.Link_Server.KifuTree.PositionA.GetKaisiPside(),
                                                            eventState.Flg_logTag);//マウス左ボタンを放したのでチェンジターンします。
                                                    }

                                                    mainGui.RepaintRequest.SyuturyokuRequest = RepaintRequestGedanTxt.Kifu;
                                                    mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                                }
                                            }
                                        }




                                        //------------------------------------------------------------
                                        // 選択解除か否か
                                        //------------------------------------------------------------
                                        {
                                            foreach (UserWidget widget in mainGui.Widgets.Values)
                                            {
                                                if (
                                                    eventState.WindowName == widget.Window &&
                                                    widget.DeselectByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y, mainGui))
                                                {
                                                    mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                                }
                                            }
                                        }

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        mainGui.Response("MouseOperation", eventState.Flg_logTag);

                                        #endregion
                                    }
                                    break;

                            }
                            #endregion
                        }
                        break;

                    case SceneName.SceneB_2OkuKoma:
                        {
                            #region 置く駒

                            switch (eventState.Name2)
                            {
                                case MouseEventStateName.MouseLeftButtonUp:
                                    {
                                        #region マウス左ボタンアップ
                                        Sky src_GuiSky = mainGui.SkyWrapper_Gui.GuiSky;


                                        //----------
                                        // 駒
                                        //----------
                                        foreach (Shape_BtnKomaImpl btnKoma in mainGui.Shape_PnlTaikyoku.Btn40Komas)
                                        {
                                            if (btnKoma.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                            {
                                                //>>>>> マウスが重なっていました

                                                if (mainGui.Shape_PnlTaikyoku.SelectFirstTouch)
                                                {
                                                    // クリックのマウスアップ
                                                    mainGui.Shape_PnlTaikyoku.SelectFirstTouch = false;
                                                }
                                                else
                                                {
                                                    src_GuiSky.AssertFinger(btnKoma.Koma);
                                                    Busstop koma = src_GuiSky.BusstopIndexOf(btnKoma.Koma);


                                                    if (Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma))
                                                    {
                                                        //>>>>> 将棋盤の上に置いてあった駒から、指を放しました
                                                        //System.C onsole.WriteLine("つまんでいる駒を放します。(4)");
                                                        mainGui.SetFigTumandeiruKoma(-1);


                                                        //----------
                                                        // 移動済表示
                                                        //----------
                                                        mainGui.Shape_PnlTaikyoku.SetHMovedKoma(btnKoma.Koma);

                                                        //------------------------------
                                                        // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                                                        //------------------------------

                                                        src_GuiSky.AssertFinger(btnKoma.Koma);
                                                        
                                                        Move move = Conv_Move.ToMove(
                                                            Conv_Busstop.ToMasu(mainGui.Shape_PnlTaikyoku.MouseBusstopOrNull2),
                                                            Conv_Busstop.ToMasu(src_GuiSky.BusstopIndexOf(btnKoma.Koma)),
                                                            Conv_Busstop.ToKomasyurui(mainGui.Shape_PnlTaikyoku.MouseBusstopOrNull2),
                                                            Conv_Busstop.ToKomasyurui(src_GuiSky.BusstopIndexOf(btnKoma.Koma)),//これで成りかどうか判定
                                                            mainGui.Shape_PnlTaikyoku.MousePos_FoodKoma != Busstop.Empty ? Conv_Busstop.ToKomasyurui( mainGui.Shape_PnlTaikyoku.MousePos_FoodKoma) : Komasyurui14.H00_Null___,
                                                            Conv_Busstop.ToPlayerside(mainGui.Shape_PnlTaikyoku.MouseBusstopOrNull2),
                                                            false
                                                            );// 選択している駒の元の場所と、移動先

                                                        // 駒を置いたので、次のノードを準備しておく☆？
                                                        Sky sky_newChild = new SkyImpl(src_GuiSky);
                                                        MoveEx newNode =
                                                            new MoveExImpl(move);
                                                        sky_newChild.SetTemezumi( mainGui.SkyWrapper_Gui.GuiSky.Temezumi + 1);//1手進ませる。


                                                        //マウスの左ボタンを放したときです。
                                                        //----------------------------------------
                                                        // 次ノード追加
                                                        //----------------------------------------
                                                        mainGui.Link_Server.Earth.GetSennititeCounter().CountUp_New(Conv_Sky.ToKyokumenHash(sky_newChild), "TimedB.Step(2)");

                                                        mainGui.Link_Server.KifuTree.MoveEx_SetCurrent(TreeImpl.OnDoCurrentMove(newNode, mainGui.Link_Server.KifuTree, sky_newChild,logger));

                                                        string jsaFugoStr;

                                                        Util_Functions_Server.AfterSetCurNode_Srv(
                                                            mainGui.SkyWrapper_Gui,
                                                            mainGui.Link_Server.KifuTree.MoveEx_Current,
                                                            mainGui.Link_Server.KifuTree.MoveEx_Current.Move,
                                                            sky_newChild,
                                                            out jsaFugoStr,
                                                            mainGui.Link_Server.KifuTree,
                                                            logger);
                                                        mainGui.RepaintRequest.SetFlag_RefreshRequest();


                                                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                                                        // ここでツリーの内容は変わっている。
                                                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

                                                        //------------------------------
                                                        // 符号表示
                                                        //------------------------------
                                                        // 置いた駒からマウスの左ボタンを放したとき
                                                        mainGui.Shape_PnlTaikyoku.SetFugo(jsaFugoStr);



                                                        //------------------------------
                                                        // チェンジターン
                                                        //------------------------------
                                                        if (!mainGui.Shape_PnlTaikyoku.Requested_NaruDialogToShow)
                                                        {
                                                            //System.C onsole.WriteLine("マウス左ボタンを放したのでチェンジターンします。");
                                                            mainGui.ChangedTurn(
                                                                mainGui.Link_Server.KifuTree,
                                                                mainGui.Link_Server.KifuTree.PositionA.GetKaisiPside(),
                                                                eventState.Flg_logTag);//マウス左ボタンを放したのでチェンジターンします。
                                                        }

                                                        mainGui.RepaintRequest.SyuturyokuRequest = RepaintRequestGedanTxt.Kifu;
                                                        mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                                    }



                                                }
                                            }
                                        }

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        mainGui.Response("MouseOperation", eventState.Flg_logTag);
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseLeftButtonDown:
                                    {
                                        #region マウス左ボタンダウン
                                        SceneName nextPhaseB = SceneName.Ignore;

                                        //System.C onsole.WriteLine("B2マウスダウン");

                                        //----------
                                        // つまんでいる駒
                                        //----------
                                        Shape_BtnKoma btnTumandeiruKoma = mainGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(mainGui);
                                        if (null == btnTumandeiruKoma)
                                        {
                                            //System.C onsole.WriteLine("つまんでいる駒なし");
                                            goto gt_nextBlock;
                                        }

                                        //>>>>> 選択されている駒があるとき

                                        mainGui.SkyWrapper_Gui.GuiSky.AssertFinger(btnTumandeiruKoma.Finger);
                                        Busstop tumandeiruLight = mainGui.SkyWrapper_Gui.GuiSky.BusstopIndexOf(btnTumandeiruKoma.Finger);


                                        //----------
                                        // 指したい先
                                        //----------
                                        Shape_BtnMasuImpl btnSasitaiMasu = null;

                                        //----------
                                        // 将棋盤：升目   ＜移動先など＞
                                        //----------
                                        foreach (UserWidget widget in mainGui.Widgets.Values)
                                        {
                                            if (
                                                eventState.WindowName == widget.Window &&
                                                "Masu" == widget.Type && Okiba.ShogiBan == widget.Okiba)
                                            {
                                                if (widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))//>>>>> 指したいマスはここです。
                                                {
                                                    btnSasitaiMasu = (Shape_BtnMasuImpl)widget.Object;
                                                    break;
                                                }
                                            }
                                        }


                                        //----------
                                        // 駒置き、駒袋：升目
                                        //----------
                                        foreach (UserWidget widget in mainGui.Widgets.Values)
                                        {
                                            if (
                                                eventState.WindowName == widget.Window &&
                                                "Masu" == widget.Type && widget.Okiba.HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai | Okiba.KomaBukuro))
                                            {
                                                Shape_BtnMasuImpl btnSasitaiMasu2 = (Shape_BtnMasuImpl)widget.Object;
                                                if (btnSasitaiMasu2.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))//>>>>> 升目をクリックしたとき
                                                {
                                                    bool match = false;

                                                    mainGui.SkyWrapper_Gui.GuiSky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
                                                    {
                                                        if (Conv_Busstop.ToMasu( koma) == btnSasitaiMasu2.Zahyo)
                                                        {
                                                            //>>>>> そこに駒が置いてあった。
#if DEBUG
                                    MessageBox.Show("駒が置いてあった","デバッグ中");
#endif
                                                            match = true;
                                                            toBreak = true;
                                                        }
                                                    });

                                                    if (!match)
                                                    {
                                                        btnSasitaiMasu = btnSasitaiMasu2;
                                                        goto gt_EndKomaoki;
                                                    }
                                                }
                                            }
                                        }
                                    gt_EndKomaoki:
                                        ;

                                        if (null == btnSasitaiMasu)
                                        {
                                            // 指したいマスなし
                                            goto gt_nextBlock;
                                        }



                                        //指したいマスが選択されたとき

                                        // TODO:合法手かどうか判定したい。

                                        if (Okiba.ShogiBan == Conv_Masu.ToOkiba(btnSasitaiMasu.Zahyo))//>>>>> 将棋盤：升目   ＜移動先など＞
                                        {

                                            //------------------------------
                                            // 成る／成らない
                                            //------------------------------
                                            //
                                            //      盤上の、不成の駒で、　／　相手陣に入るものか、相手陣から出てくる駒　※先手・後手区別なし
                                            //
                                            Busstop koma = tumandeiruLight;

                                            if (
                                                    Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma) && Util_Sky_BoolQuery.IsNareruKoma(Conv_Busstop.ToKomasyurui(koma))
                                                    &&
                                                    (
                                                        Conv_Masu.InBanjoAitejin(
                                                            btnSasitaiMasu.Zahyo,
                                                            mainGui.Link_Server.KifuTree.PositionA.GetKaisiPside()
                                                            )
                                                        ||
                                                        Util_Sky_BoolQuery.InBanjoAitejin(Conv_Busstop.ToMasu( koma), Conv_Busstop.ToPlayerside( koma))
                                                    )
                                                )
                                            {
                                                // 成るか／成らないか ダイアログボックスを表示します。
                                                mainGui.Shape_PnlTaikyoku.Request_NaruDialogToShow(true);
                                            }


                                            if (mainGui.Shape_PnlTaikyoku.Requested_NaruDialogToShow)
                                            {
                                                // 成る／成らないボタン表示
                                                mainGui.GetWidget("BtnNaru").Visible = true;
                                                mainGui.GetWidget("BtnNaranai").Visible = true;
                                                mainGui.Shape_PnlTaikyoku.SetNaruMasu(btnSasitaiMasu);
                                                nextPhaseB = SceneName.SceneB_3ErabuNaruNaranai;
                                            }
                                            else
                                            {
                                                mainGui.GetWidget("BtnNaru").Visible = false;
                                                mainGui.GetWidget("BtnNaranai").Visible = false;

                                                // 駒を動かします。
                                                {
                                                    // GuiからServerへ渡す情報
                                                    Komasyurui14 syurui;
                                                    Busstop dst;
                                                    
                                                    Util_Function_Csharp.Komamove1a_49Gui(out syurui, out dst, btnTumandeiruKoma, btnSasitaiMasu, mainGui);

                                                    // ServerからGuiへ渡す情報
                                                    bool torareruKomaAri;
                                                    Busstop koma_Food_after;
                                                    Util_Functions_Server.Komamove1a_50Srv(out torareruKomaAri, out koma_Food_after, dst, btnTumandeiruKoma.Koma, dst, mainGui.SkyWrapper_Gui, eventState.Flg_logTag);

                                                    Util_Function_Csharp.Komamove1a_51Gui(torareruKomaAri, koma_Food_after, mainGui);
                                                }

                                                nextPhaseB = SceneName.SceneB_1TumamitaiKoma;
                                            }

                                            mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                        }
                                        else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                                            Conv_Masu.ToOkiba(btnSasitaiMasu.Zahyo)))//>>>>> 駒置き：升目
                                        {
                                            //System.C onsole.WriteLine("駒台上");

                                            mainGui.SkyWrapper_Gui.GuiSky.AssertFinger(btnTumandeiruKoma.Koma);
                                            Busstop koma = mainGui.SkyWrapper_Gui.GuiSky.BusstopIndexOf(btnTumandeiruKoma.Koma);

                                            mainGui.SkyWrapper_Gui.GuiSky.SetTemezumi(mainGui.SkyWrapper_Gui.GuiSky.Temezumi + 1);//1手進める。
                                            mainGui.SkyWrapper_Gui.GuiSky.AddObjects(
                                                new Finger[] { btnTumandeiruKoma.Koma },
                                                new Busstop[] {
                                                    Conv_Busstop.ToBusstop(
                                                        Conv_Okiba.ToPside(Conv_Masu.ToOkiba(btnSasitaiMasu.Zahyo)),// 先手の駒置きに駒を置けば、先手の向きに揃えます。
                                                        btnSasitaiMasu.Zahyo,
                                                        Util_Komasyurui14.NarazuCaseHandle(Conv_Busstop.ToKomasyurui( koma))
                                                    )
                                                });

                                            nextPhaseB = SceneName.SceneB_1TumamitaiKoma;

                                            mainGui.RepaintRequest.SetFlag_RecalculateRequested();// 駒の再描画要求
                                            mainGui.RepaintRequest.SetFlag_RefreshRequest();
                                        }


                                    gt_nextBlock:

                                        //----------
                                        // 既に選択されている駒
                                        //----------
                                        Shape_BtnKoma btnKoma_Selected = mainGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(mainGui);



                                        //----------
                                        // 初期配置ボタン
                                        //----------

                                        {
                                            foreach (UserWidget widget in mainGui.Widgets.Values)
                                            {
                                                if (
                                                    eventState.WindowName == widget.Window &&
                                                    widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    if (null != widget.Delegate_MouseHitEvent)
                                                    {
                                                        widget.Delegate_MouseHitEvent(
                                                            mainGui
                                                           , widget
                                                           , btnKoma_Selected
                                                           , eventState.Flg_logTag
                                                           );
                                                    }
                                                }
                                            }
                                        }


                                        mainGui.SetScene(nextPhaseB);

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        mainGui.Response("MouseOperation", eventState.Flg_logTag);
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseRightButtonDown:
                                    {
                                        #region マウス右ボタンダウン
                                        // 各駒の、移動済フラグを解除
                                        //System.C onsole.WriteLine("つまんでいる駒を放します。(5)");
                                        mainGui.SetFigTumandeiruKoma(-1);
                                        mainGui.Shape_PnlTaikyoku.SelectFirstTouch = false;

                                        //------------------------------
                                        // 状態を戻します。
                                        //------------------------------
                                        mainGui.SetScene(SceneName.SceneB_1TumamitaiKoma);

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        mainGui.Response("MouseOperation", eventState.Flg_logTag);
                                        #endregion
                                    }
                                    break;
                            }
                            #endregion
                        }
                        break;

                    case SceneName.SceneB_3ErabuNaruNaranai:
                        {
                            #region 成る成らない

                            switch (eventState.Name2)
                            {
                                case MouseEventStateName.MouseLeftButtonDown:
                                    {
                                        #region マウス左ボタンダウン
                                        SceneName nextPhaseB = SceneName.Ignore;
                                        //GuiSky この関数の途中で変更される。ローカル変数に入れているものは古くなる。

                                        //----------
                                        // 既に選択されている駒
                                        //----------
                                        Shape_BtnKoma btnKoma_Selected = mainGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(mainGui);

                                        string[] buttonNames = new string[]{
                                            "BtnNaru",// [成る]ボタン
                                            "BtnNaranai"// [成らない]ボタン
                                        };
                                        foreach (string buttonName in buttonNames)
                                        {
                                            UserWidget widget = mainGui.GetWidget(buttonName);

                                            if (
                                                eventState.WindowName == widget.Window &&
                                                widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                            {
                                                if (null != widget.Delegate_MouseHitEvent)
                                                {
                                                    widget.Delegate_MouseHitEvent(
                                                        mainGui
                                                       , widget
                                                       , btnKoma_Selected
                                                       , eventState.Flg_logTag
                                                       );
                                                }
                                            }
                                        }


                                        //

                                        //----------
                                        // 初期配置ボタン
                                        //----------

                                        {
                                            foreach (UserWidget widget in mainGui.Widgets.Values)
                                            {
                                                if (
                                                    eventState.WindowName == widget.Window &&
                                                    widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    if (null != widget.Delegate_MouseHitEvent)
                                                    {
                                                        widget.Delegate_MouseHitEvent(
                                                            mainGui
                                                           , widget
                                                           , btnKoma_Selected
                                                           , eventState.Flg_logTag
                                                           );
                                                    }
                                                }
                                            }
                                        }


                                        mainGui.SetScene(nextPhaseB);

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        mainGui.Response("MouseOperation", eventState.Flg_logTag);
                                        #endregion
                                    }
                                    break;
                            }
                            #endregion

                        }
                        break;
                }
            }
            





        //gt_EndMethod1:
        //    ;
        }
    }


}
