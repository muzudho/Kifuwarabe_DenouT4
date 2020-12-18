using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
{
    public interface IKifuParserAState
    {

        string Execute(
            out MoveNodeType moveNodeType,
            ref IKifuParserAResult result,

            Earth earth1,
            Move move1,
            ISky positionA,

            out IKifuParserAState nextState,
            IKifuParserA owner,
            IKifuParserAGenjo genjo,
            ILogger errH
            );

    }
}
