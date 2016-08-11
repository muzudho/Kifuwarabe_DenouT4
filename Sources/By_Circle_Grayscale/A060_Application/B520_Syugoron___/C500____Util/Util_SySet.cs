using Grayscale.A060_Application.B510_Conv_Sy____.C500____Converter;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using System.Text;

namespace Grayscale.A060_Application.B520_Syugoron___.C500____Util
{
    public abstract class Util_SySet
    {

        public static string Dump_Elements(SySet<SyElement> sySet)
        {
            StringBuilder sb = new StringBuilder();

            foreach(SyElement syElement in sySet.Elements)
            {
                sb.Append(Conv_Sy.Query_Word( syElement.Bitfield));
                sb.Append(",");
            }

            return sb.ToString();
        }

    }
}
