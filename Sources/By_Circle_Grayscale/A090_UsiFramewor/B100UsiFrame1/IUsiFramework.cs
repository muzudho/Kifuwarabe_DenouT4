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

        Func04 OnUsiReceived_AtLoop1Body { get; set; }
        Func04 OnSetoptionReceived_AtLoop1Body { get; set; }
        Func04 OnIsreadyReceived_AtLoop1Body { get; set; }
        Func04 OnUsinewgameReceived_AtLoop1Body { get; set; }
        Func04 OnQuitReceived_AtLoop1Body { get; set; }
        Func05 OnCommandlineRead_AtLoop1Body { get; set; }
        #endregion

        #region 対局中フェーズ
        Func01 OnLoop2Begin { get; set; }
        Func01 OnLoop2End { get; set; }

        Func05 OnCommandlineRead_AtLoop2Body { get; set; }
        Func02 OnPositionReceived_AtLoop2Body { get; set; }
        Func02 OnGoponderReceived_AtLoop2Body { get; set; }

        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// </summary>
        Func02 OnGoReceived_AtLoop2Body { get; set; }

        Func02 OnStopReceived_AtLoop2Body { get; set; }
        Func02 OnGameoverReceived_AtLoop2Body { get; set; }
        Func02 OnLogdaseReceived_AtLoop2Body { get; set; }
        #endregion
    }
}
