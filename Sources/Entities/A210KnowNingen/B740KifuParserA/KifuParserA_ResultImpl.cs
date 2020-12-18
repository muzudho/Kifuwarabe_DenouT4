using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B740KifuParserA.C500Parser;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
{
    public class KifuParserA_ResultImpl : IKifuParserAResult
    {
        public MoveEx Out_newNode_OrNull { get { return this.m_out_newNode_OrNull_; } }
        private MoveEx m_out_newNode_OrNull_;

        public ISky NewSky { get { return this.m_newSky_; } }
        private ISky m_newSky_;

        public void SetNode(MoveEx node, ISky sky)
        {
            this.m_out_newNode_OrNull_ = node;
            this.m_newSky_ = sky;
        }
    }
}
