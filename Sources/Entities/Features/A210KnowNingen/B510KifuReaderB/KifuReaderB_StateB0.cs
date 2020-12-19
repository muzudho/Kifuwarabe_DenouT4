using Grayscale.A210KnowNingen.B510KifuReaderB.C500Reader;

namespace Grayscale.A210KnowNingen.B510KifuReaderB.C500Reader
{
    public class KifuReaderB_StateB0 : IKifuReaderBState
    {

        public void Execute(string inputLine, out string nextCommand, out string rest)
        {
            nextCommand = "";
            rest = inputLine;
        }

    }
}
