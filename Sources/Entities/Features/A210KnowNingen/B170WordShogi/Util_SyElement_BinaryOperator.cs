namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    /// <summary>
    /// 二項演算子
    /// </summary>
    public abstract class Util_SyElement_BinaryOperator
    {
        public static DLGT_SyElement_BynaryOperate Dlgt_Equals_MasuNumber
        {
            get
            {
                return Util_SyElement_BinaryOperator.dlgt_Equals_MasuNumber;
            }
        }
        private static DLGT_SyElement_BynaryOperate dlgt_Equals_MasuNumber = delegate (SyElement operand1, SyElement operand2)
        {
            bool result;

            // 両方が null なら、等価とします。
            if (operand1 == null && operand2 == null)
            {
                result = true;
                goto gt_EndMethod;
            }

            // 片方だけが null なら、等価ではないものとします。
            if (operand1 == null || operand2 == null)
            {
                result = false;
                goto gt_EndMethod;
            }

            // 両方が Basho型であると想定しています。
            if ((operand1 is INewBasho) && (operand2 is INewBasho))
            {
                result = ((INewBasho)operand1).MasuNumber == ((INewBasho)operand2).MasuNumber;
            }
            else
            {
                // operand1 の equalsメソッドに任せます。
                result = operand1.Equals(operand2);
            }

        gt_EndMethod:
            return result;
        };
    }
}
