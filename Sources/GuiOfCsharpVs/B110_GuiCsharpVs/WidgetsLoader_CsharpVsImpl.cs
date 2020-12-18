using Grayscale.A630GuiCsharp.B110ShogiGui.C492Widgets;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;
using Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C491Event;

namespace Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C492Widget
{
    public class WidgetsLoader_CsharpVsImpl : WidgetsLoader_CsharpImpl
    {

        public WidgetsLoader_CsharpVsImpl(string fileName, MainGui_Csharp shogibanGui)
            : base(fileName, shogibanGui)
        {
        }

        public override void Step3_SetEvent(object obj_shogiGui)
        {
            MainGui_Csharp shogibanGui1 = (MainGui_Csharp)obj_shogiGui;

            //----------
            // 将棋エンジン起動ボタン_学習
            //----------
            {
                UserWidget widget = shogibanGui1.GetWidget("BtnShogiEngineKidoL");
                widget.Delegate_MouseHitEvent = Event_CsharpVsImpl.GetInstance().Delegate_BtnShogiEngineKidoL;
            }

            //----------
            // 将棋エンジン起動ボタン_CP起動
            //----------
            {
                UserWidget widget = shogibanGui1.GetWidget("BtnShogiEngineKidoF");
                widget.Delegate_MouseHitEvent = Event_CsharpVsImpl.GetInstance().Delegate_BtnComputerBoot;
            }

            //----------
            // 将棋エンジン起動ボタン_CP先
            //----------
            {
                UserWidget widget = shogibanGui1.GetWidget("BtnShogiEngineKidoT");
                widget.Delegate_MouseHitEvent = Event_CsharpVsImpl.GetInstance().Delegate_BtnComputerSente;
            }
        }

    }
}
