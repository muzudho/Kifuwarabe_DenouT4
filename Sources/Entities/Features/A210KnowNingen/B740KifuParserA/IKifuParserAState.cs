using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface IKifuParserAState
    {

        string Execute(
            out MoveNodeType moveNodeType,
            ref IKifuParserAResult result,
            IPlaying playing,
            Move move1,
            IPosition positionA,

            out IKifuParserAState nextState,
            IKifuParserA owner,
            IKifuParserAGenjo genjo);

    }
}
