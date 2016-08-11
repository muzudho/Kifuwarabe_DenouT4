using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___492_Widgets
{
    /// <summary>
    /// ウィジェット読込みクラス。
    /// </summary>
    public interface WidgetsLoader
    {
        string FileName { get; set; }
        MainGui_Csharp ShogibanGui { get; set; }

        void Step1_ReadFile();
        void Step2_Compile_AllWidget(object obj_shogiGui);
        void Step3_SetEvent(object obj_shogiGui);

    }
}
