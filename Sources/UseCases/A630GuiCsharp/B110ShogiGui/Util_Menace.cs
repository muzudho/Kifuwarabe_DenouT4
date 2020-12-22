using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C249Function
{
    public abstract class Util_Menace
    {
        /// <summary>
        /// v(^▽^)v超能力『メナス』だぜ☆ 未来の脅威を予測し、可視化するぜ☆ｗｗｗ
        /// </summary>
        public static void Menace(MainGui_Csharp mainGui, ILogTag logger)
        {
            if (0 < mainGui.SkyWrapper_Gui.GuiSky.Temezumi)
            {
                // 処理の順序が悪く、初回はうまく判定できない。
                ISky positionA = mainGui.SkyWrapper_Gui.GuiSky;
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
                        psideA != Conv_Busstop.ToPlayerside(koma)
                        )
                    {
                        // 駒の利き
                        SySet<SyElement> kikiZukei = UtilSkySyugoQuery.KomaKidou_Potential(figKoma, positionA);

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
