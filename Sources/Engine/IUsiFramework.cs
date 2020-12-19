using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;

namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame
{
    public delegate void Func01();
    public delegate PhaseResultUsiLoop2 Func02(string line);
    public delegate PhaseResultUsiLoop1 Func04(string line);
    public delegate string Func05();

    public interface IUsiFramework
    {
        Func01 OnApplicationEnd { get; set; }
        Func04 OnIsready { get; set; }
        Func04 OnUsinewgame { get; set; }
        Func04 OnQuit { get; set; }
        Func05 OnCommandlineAtLoop1 { get; set; }

        Func01 OnLoop2Begin { get; set; }
        Func01 OnLoop2End { get; set; }

        Func05 OnCommandlineAtLoop2 { get; set; }
        Func02 OnPosition { get; set; }
        Func02 OnGoponder { get; set; }

        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// </summary>
        Func02 OnGo { get; set; }

        Func02 OnStop { get; set; }
        Func02 OnGameover { get; set; }
        Func02 OnLogDase { get; set; }
    }
}
