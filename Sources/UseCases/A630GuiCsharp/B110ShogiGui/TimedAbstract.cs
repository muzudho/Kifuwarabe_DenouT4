using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A630GuiCsharp.B110ShogiGui.C125Scene;

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C250Timed
{
    public abstract class TimedAbstract : Timed
    {

        abstract public void Step(ILogger errH);

    }
}
