﻿using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A630GuiCsharp.B110ShogiGui.C125Scene;

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C250Timed
{
    public abstract class TimedAbstract : Timed
    {

        abstract public void Step(ILogTag errH);

    }
}