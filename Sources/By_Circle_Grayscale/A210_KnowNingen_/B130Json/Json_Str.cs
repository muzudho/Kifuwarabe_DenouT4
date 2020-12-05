using System.Text;
using Grayscale.A210KnowNingen.B130Json.C500Struct;

namespace Grayscale.A210KnowNingen.B130Json.C500Struct
{
    /// <summary>
    /// 文字列
    /// </summary>
    public class Json_Str : IJsonVal
    {

        public string Value { get; set; }

        public Json_Str(string value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\"");
            sb.Append(this.Value);
            sb.Append("\"");

            return sb.ToString();
        }
    }
}
