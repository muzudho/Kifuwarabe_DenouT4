using System.Text;
using Grayscale.A180KifuCsa.B120KifuCsa.C250Struct;
using Grayscale.A210KnowNingen.B240Move.C500Struct;

namespace Grayscale.A690FvLearn.B110FvLearn.C250Learn
{
    /// <summary>
    /// 本譜指し手リストの項目。
    /// </summary>
    public class HonpuMoveListItemImpl
    {
        /// <summary>
        /// CSA棋譜 の指し手
        /// </summary>
        private CsaKifuMove MoveAsCsa { get; set; }

        /// <summary>
        /// 指し手（SFEN符号に変換できるもの）
        /// </summary>
        public Move Move { get; set; }

        public HonpuMoveListItemImpl(CsaKifuMove moveAsCsa, Move move)
        {
            this.MoveAsCsa = moveAsCsa;
            this.Move = move;// 
        }

        /// <summary>
        /// リストボックスで表示する文字列です。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.MoveAsCsa.OptionTemezumi);
            sb.Append("手目 ");
            sb.Append(this.MoveAsCsa.DestinationMasu);
            sb.Append(" ");
            sb.Append(this.MoveAsCsa.Second);
            sb.Append(" ");
            sb.Append(this.MoveAsCsa.Sengo);
            sb.Append(" ");
            sb.Append(this.MoveAsCsa.SourceMasu);
            sb.Append(" ");
            sb.Append(this.MoveAsCsa.Syurui);
            sb.Append(" ");
            sb.Append(this.Move);

            return sb.ToString();
        }

    }
}
