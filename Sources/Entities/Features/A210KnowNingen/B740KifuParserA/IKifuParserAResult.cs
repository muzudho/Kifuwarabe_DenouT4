using Grayscale.Kifuwaragyoku.Entities.Features;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface IKifuParserAResult
    {
        MoveEx Out_newNode_OrNull { get; }

        ISky NewSky { get; }

        void SetNode(MoveEx node, ISky sky);
    }
}
