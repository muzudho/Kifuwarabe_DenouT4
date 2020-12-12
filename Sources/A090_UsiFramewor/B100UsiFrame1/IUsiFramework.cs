using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;

namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame
{
    public delegate void Func01();
    public delegate PhaseResultUsiLoop2 Func02(string line);
    public delegate PhaseResultUsiLoop1 Func04(string line);
    public delegate string Func05();

    public interface IUsiFramework
    {
        #region 実行
        /// <summary>
        /// 実行します。
        /// </summary>
        void Execute();
        #endregion

        #region アプリケーション・フェーズ
        Func01 OnApplicationBegin { get; set; }
        Func01 OnApplicationEnd { get; set; }

        #endregion

        #region 準備フェーズ
        Func01 OnLoop1Begin { get; set; }
        Func01 OnLoop1End { get; set; }

        Func04 OnUsi { get; set; }
        Func04 OnSetoption { get; set; }
        Func04 OnIsready { get; set; }
        Func04 OnUsinewgame { get; set; }
        Func04 OnQuit { get; set; }
        Func05 OnCommandlineAtLoop1 { get; set; }
        #endregion

        #region 対局中フェーズ
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
        #endregion
    }
}
