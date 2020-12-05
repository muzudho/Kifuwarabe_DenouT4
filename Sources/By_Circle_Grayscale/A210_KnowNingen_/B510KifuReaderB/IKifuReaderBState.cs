
namespace Grayscale.A210KnowNingen.B510KifuReaderB.C500Reader
{
    public interface IKifuReaderBState
    {

        void Execute(string inputLine, out string nextCommand, out string rest);

    }
}
