using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
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
        Tree KifuTree { get; }
        void SetKifuTree(Tree kifu1);

        Earth Earth { get; }
        //void SetEarth(Earth earth1);
    }
}
