
namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface IKifuReaderBState
    {

        void Execute(string inputLine, out string nextCommand, out string rest);

    }
}
