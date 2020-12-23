using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface IKifuParserAResult
    {
        MoveEx Out_newNode_OrNull { get; }

        IPosition NewSky { get; }

        void SetNode(MoveEx node, IPosition sky);
    }
}
