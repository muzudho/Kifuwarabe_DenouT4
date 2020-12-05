using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A480ServerAims.B110AimsServer.C060Phase;

namespace Grayscale.A480ServerAims.B110AimsServer.C070ServerBase
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
        Tree KifuTree { get; }
        void SetKifuTree(Tree kifu1);

        Earth Earth { get; }
        //void SetEarth(Earth earth1);
    }
}
