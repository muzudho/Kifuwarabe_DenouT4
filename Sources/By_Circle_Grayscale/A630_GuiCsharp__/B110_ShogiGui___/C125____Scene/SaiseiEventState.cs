using Grayscale.A060_Application.B110_Log________.C___500_Struct;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C125____Scene
{
    /// <summary>
    /// [再生]イベントの状態です。
    /// </summary>
    public class SaiseiEventState
    {

        public SaiseiEventStateName Name2 { get { return this.name2; } }
        private SaiseiEventStateName name2;

        public KwLogger Flg_logTag { get { return this.flg_logTag; } }
        private KwLogger flg_logTag;


        public SaiseiEventState()
        {
            this.name2 = SaiseiEventStateName.Ignore;
        }

        public SaiseiEventState(SaiseiEventStateName name2, KwLogger flg_logTag)
        {
            this.name2 = name2;
            this.flg_logTag = flg_logTag;
        }

    }
}
