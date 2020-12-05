﻿using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___081_Canvas
{
    public interface Shape_PnlTaikyoku : Shape, Shape_Canvas
    {
        Shape_BtnKoma Btn_MovedKoma();
        Shape_BtnKoma Btn_TumandeiruKoma(object obj_shogiGui);//ShogiGui

        /// <summary>
        /// 40個の駒。
        /// </summary>
        Shape_BtnKoma[] Btn40Komas { get; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒置き。[0]先手、[1]後手、[2]駒袋。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_PnlKomadai[] KomadaiArr { get; set; }

        /// <summary>
        /// 「取った駒_巻戻し用」
        /// </summary>
        Busstop MousePos_FoodKoma { get; set; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// マウスで駒を動かしたときに使います。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        /// 棋譜[再生]中は使いません。
        /// 
        /// </summary>
        Busstop MouseBusstopOrNull2 { get; }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かし終わった駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Finger MovedKoma { get; }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成る、で移動先
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_BtnMasu NaruBtnMasu { get; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 要求：　成る／成らないダイアログボックス
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///     0: なし
        ///     1: 成るか成らないかボタンを表示して決定待ち中。
        /// 
        /// </summary>
        bool Requested_NaruDialogToShow { get; }
        void Request_NaruDialogToShow(bool show);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かしたい駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        bool SelectFirstTouch { get; set; }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 差し手符号。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        /// <param name="fugo"></param>
        void SetFugo(string fugo);

        void SetHMovedKoma(Finger value);

        void SetMouseBusstopOrNull2(Busstop mouseDd);

        void SetNaruMasu(Shape_BtnMasu naruMasu);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋盤
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_PnlShogiban Shogiban { get; set; }
    }

}
