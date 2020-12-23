using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface IKifuParserAState
    {

        string Execute(
            out MoveNodeType moveNodeType,
            ref IKifuParserAResult result,
            IPlaying playing,
            Move move1,
            ISky positionA,

            out IKifuParserAState nextState,
            IKifuParserA owner,
            IKifuParserAGenjo genjo);

    }
}
