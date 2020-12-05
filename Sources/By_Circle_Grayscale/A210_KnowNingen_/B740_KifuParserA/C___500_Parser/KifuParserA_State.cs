using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C500Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser
{
    public interface KifuParserA_State
    {

        string Execute(
            out MoveNodeType moveNodeType,
            ref KifuParserA_Result result,

            Earth earth1,
            Move move1,
            Sky positionA,

            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KwLogger errH
            );

    }
}
