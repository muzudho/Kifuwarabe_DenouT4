using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C500____Util;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C249____Function
{
    public abstract class Util_Menace
    {
        /// <summary>
        /// v(^▽^)v超能力『メナス』だぜ☆ 未来の脅威を予測し、可視化するぜ☆ｗｗｗ
        /// </summary>
        public static void Menace( MainGui_Csharp mainGui, KwLogger logger)
        {
            if (0 < mainGui.SkyWrapper_Gui.GuiSky.Temezumi)
            {
                // 処理の順序が悪く、初回はうまく判定できない。
                Sky positionA = mainGui.SkyWrapper_Gui.GuiSky;
                Playerside psideA = mainGui.SkyWrapper_Gui.GuiSky.GetKaisiPside();


                //----------
                // 将棋盤上の駒
                //----------
                mainGui.RepaintRequest.SetFlag_RefreshRequest();

                // [クリアー]
                mainGui.Shape_PnlTaikyoku.Shogiban.ClearHMasu_KikiKomaList();

                // 全駒
                foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
                {
                    positionA.AssertFinger(figKoma);
                    Busstop koma = positionA.BusstopIndexOf(figKoma);


                    if (
                        Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma)
                        &&
                        psideA != Conv_Busstop.ToPlayerside( koma)
                        )
                    {
                        // 駒の利き
                        SySet<SyElement> kikiZukei = Util_Sky_SyugoQuery.KomaKidou_Potential(figKoma, positionA);

                        IEnumerable<SyElement> kikiMasuList = kikiZukei.Elements;
                        foreach (SyElement masu in kikiMasuList)
                        {
                            // その枡に利いている駒のハンドルを追加
                            if (!Masu_Honshogi.IsErrorBasho(masu))
                            {
                                mainGui.Shape_PnlTaikyoku.Shogiban.HMasu_KikiKomaList[Conv_Masu.ToMasuHandle(masu)].Add((int)figKoma);
                            }
                        }
                    }
                }
            }
        }

    }
}
