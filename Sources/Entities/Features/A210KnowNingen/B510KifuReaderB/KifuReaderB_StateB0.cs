namespace Grayscale.Kifuwaragyoku.Entities.Features
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
