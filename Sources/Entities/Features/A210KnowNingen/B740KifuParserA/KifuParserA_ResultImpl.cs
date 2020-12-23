using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public class KifuParserA_ResultImpl : IKifuParserAResult
    {
        public MoveEx Out_newNode_OrNull { get { return this.m_out_newNode_OrNull_; } }
        private MoveEx m_out_newNode_OrNull_;

        public IPosition NewSky { get { return this.m_newSky_; } }
        private IPosition m_newSky_;

        public void SetNode(MoveEx node, IPosition sky)
        {
            this.m_out_newNode_OrNull_ = node;
            this.m_newSky_ = sky;
        }
    }
}
