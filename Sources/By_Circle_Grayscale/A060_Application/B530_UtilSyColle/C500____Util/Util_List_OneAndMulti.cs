using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B510_Conv_Sy____.C500____Converter;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using System.Text;


namespace Grayscale.A060_Application.B530_UtilSyColle.C500____Util
{
    public abstract class Util_List_OneAndMulti<T1, T2>
    {

        /// <summary>
        /// 汎用。
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string Dump(List_OneAndMulti<T1,T2> collection)
        {
            int count = 0;

            StringBuilder sb = new StringBuilder();
            {
                foreach (Couple<T1, T2> item in collection.Items)
                {
                    if (item.B is SySet<SyElement>)
                    {
                        foreach (SyElement syElement in ((SySet<SyElement>)item.B).Elements)
                        {
                            sb.AppendLine("(" + count + ") a=[" + item.A.ToString() + "] b=[" + Conv_Sy.Query_Word(syElement.Bitfield) + "]");
                            count++;
                        }
                    }
                    else
                    {
                        sb.AppendLine("(" + count + ") a=[" + item.A.ToString() + "] b=[" + item.B.ToString() + "]");
                        count++;
                    }
                }
            }

            return sb.ToString();
        }
    }
}
