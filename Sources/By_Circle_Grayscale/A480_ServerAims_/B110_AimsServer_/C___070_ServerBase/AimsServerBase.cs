using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C___060_Phase;

namespace Grayscale.A480_ServerAims_.B110_AimsServer_.C___070_ServerBase
{
    public interface AimsServerBase
    {
        /// <summary>
        /// フェーズ。
        /// </summary>
        Phase_AimsServer Phase_AimsServer { get; }
        void SetPhase_AimsServer(Phase_AimsServer phase_AimsServer);

        /// <summary>
        /// 対局モデル。棋譜ツリーなど。
        /// </summary>
        Model_Taikyoku Model_Taikyoku { get; }
    }
}
