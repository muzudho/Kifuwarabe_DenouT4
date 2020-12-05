using Grayscale.A210_KnowNingen_.B270_Sky________.C500Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500Parser
{
    public class KifuParserA_ResultImpl : KifuParserA_Result
    {
        public MoveEx Out_newNode_OrNull { get { return this.m_out_newNode_OrNull_; } }
        private MoveEx m_out_newNode_OrNull_;

        public Sky NewSky { get { return this.m_newSky_; } }
        private Sky m_newSky_;

        public void SetNode(MoveEx node, Sky sky)
        {
            this.m_out_newNode_OrNull_ = node;
            this.m_newSky_ = sky;
        }
    }
}
