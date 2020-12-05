using System.Text;
using Grayscale.A060Application.B510ConvSy.C500Converter;
using Grayscale.A060Application.B520Syugoron.C250Struct;

namespace Grayscale.A060Application.B520Syugoron.C500Util
{
    public abstract class Util_SySet
    {

        public static string Dump_Elements(SySet<SyElement> sySet)
        {
            StringBuilder sb = new StringBuilder();

            foreach (SyElement syElement in sySet.Elements)
            {
                sb.Append(Conv_Sy.Query_Word(syElement.Bitfield));
                sb.Append(",");
            }

            return sb.ToString();
        }

    }
}
