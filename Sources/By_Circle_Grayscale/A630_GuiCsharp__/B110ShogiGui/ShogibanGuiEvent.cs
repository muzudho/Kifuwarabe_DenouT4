using Grayscale.A060Application.B110Log.C500Struct;

/// <summary>
/// この名前空間は、デリゲートを定義しているだけ。
/// </summary>
namespace Grayscale.A630GuiCsharp.B110ShogiGui.C491Event
{
    /// <summary>
    /// クリックされたときの動きです。
    /// </summary>
    /// <param name="shape_PnlTaikyoku"></param>
    public delegate void DELEGATE_MouseHitEvent(
         object obj_shogiGui //ShogiGui
        , object userWidget // UerWidget
        , object shape_BtnKoma_Selected //Shape_BtnKoma
        , KwLogger errH2
    );
}
