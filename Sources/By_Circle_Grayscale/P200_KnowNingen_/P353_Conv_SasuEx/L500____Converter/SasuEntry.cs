using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P219_Move_______.L___500_Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P353_Conv_SasuEx.L500____Converter
{
    public class SasuEntry
    {
        public Move NewMove { get; set; }

        public SasuEntry(
            Move newMove
            )
        {
            this.NewMove = newMove;
        }
    }
}
