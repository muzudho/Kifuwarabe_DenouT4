using System.Collections.Generic;
using System.Text;
using Grayscale.A210KnowNingen.B130Json.C500Struct;

namespace Grayscale.A210KnowNingen.B130Json.C500Struct
{
    public class Json_Obj : IJsonVal
    {

        public List<Json_Prop> props { get; set; }

        public Json_Obj()
        {
            this.props = new List<Json_Prop>();
        }

        public void Add(Json_Prop prop)
        {
            this.props.Add(prop);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            int count = 0;
            foreach (Json_Prop prop in this.props)
            {
                if (0 < count)
                {
                    sb.Append(",");
                }

                sb.Append(prop.ToString());
                count++;
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
