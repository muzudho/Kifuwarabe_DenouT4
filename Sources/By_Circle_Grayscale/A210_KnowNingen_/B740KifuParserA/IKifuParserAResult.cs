using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
{
    public interface IKifuParserAResult
    {
        MoveEx Out_newNode_OrNull { get; }

        ISky NewSky { get; }

        void SetNode(MoveEx node, ISky sky);
    }
}
