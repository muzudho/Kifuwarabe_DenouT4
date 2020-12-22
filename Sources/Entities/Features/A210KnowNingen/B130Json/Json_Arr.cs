﻿using System.Collections.Generic;
using System.Text;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    /// <summary>
    /// 配列
    /// </summary>
    public class Json_Arr : IJsonVal
    {

        public List<IJsonVal> elements { get; set; }

        public bool NewLineEnable { get; set; }

        public Json_Arr()
        {
            this.elements = new List<IJsonVal>();
        }

        public void Add(IJsonVal element)
        {
            this.elements.Add(element);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            if (this.NewLineEnable)
            {
                sb.AppendLine();
            }

            int count = 0;
            foreach (IJsonVal element in this.elements)
            {
                if (this.NewLineEnable)
                {
                    sb.Append("    ");
                }

                if (0 < count)
                {
                    sb.Append(",");
                }

                sb.Append(element.ToString());
                if (this.NewLineEnable)
                {
                    sb.AppendLine();
                }

                count++;
            }

            sb.Append("]");
            if (this.NewLineEnable)
            {
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
