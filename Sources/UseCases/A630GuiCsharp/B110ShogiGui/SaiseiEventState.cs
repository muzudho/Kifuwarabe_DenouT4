﻿using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C125Scene
{
    /// <summary>
    /// [再生]イベントの状態です。
    /// </summary>
    public class SaiseiEventState
    {

        public SaiseiEventStateName Name2 { get { return this.name2; } }
        private SaiseiEventStateName name2;

        public ILogTag Flg_logTag { get { return this.flg_logTag; } }
        private ILogTag flg_logTag;


        public SaiseiEventState()
        {
            this.name2 = SaiseiEventStateName.Ignore;
        }

        public SaiseiEventState(SaiseiEventStateName name2, ILogTag flg_logTag)
        {
            this.name2 = name2;
            this.flg_logTag = flg_logTag;
        }

    }
}
