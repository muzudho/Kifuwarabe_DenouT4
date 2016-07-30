using System;
using Grayscale.P575_KifuWarabe_.L250____UsiLoop;

namespace Grayscale.P571_usiFrame1__.L___500_usiFrame___
{
    public delegate void Func01();
    public delegate PhaseResult_UsiLoop2 Func02(string line, Object caller);
    public delegate string Func03(Object caller);
    public delegate PhaseResult_UsiLoop1 Func04(string line, Object caller);

    public interface UsiFramework
    {
        void OnBegin_InAll(Func01 fun01);
        void OnBody_InAll(Func01 fun01);
        void OnEnd_InAll(Func01 fun01);

        void OnBegin_InLoop1(Object caller);
        PhaseResult_UsiLoop1 OnBody_InLoop1(Object caller);
        void OnEnd_InLoop1(Object caller);

        void OnBegin_InLoop2(Func01 fun01);
        void OnBody_InLoop2(Object caller);
        void OnEnd_InLoop2(Func01 fun01);

        Func03 OnCommandlineRead_AtBody2 { get; set; }
        Func02 OnPosition_AtBody2 { get; set; }
        Func02 OnGoponder_AtBody2 { get; set; }

        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// </summary>
        Func02 OnGo_AtBody2 { get; set; }

        Func02 OnStop_AtBody2 { get; set; }
        Func02 OnGameover_AtBody2 { get; set; }
        Func02 OnLogdase_AtBody2 { get; set; }

        Func04 OnUsi_AtBody1 { get; set; }
        Func04 OnSetoption_AtBody1 { get; set; }
        Func04 OnIsready_AtBody1 { get; set; }
        Func04 OnUsinewgame_AtBody1 { get; set; }
        Func04 OnQuit_AtBody1 { get; set; }
        Func03 OnCommandlineRead_AtBody1 { get; set; }
    }
}
