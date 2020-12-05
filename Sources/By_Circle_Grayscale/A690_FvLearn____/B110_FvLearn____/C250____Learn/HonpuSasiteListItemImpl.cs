using System.Text;
using Grayscale.A180KifuCsa.B120KifuCsa.C250Struct;
using Grayscale.A210KnowNingen.B240Move.C500Struct;

namespace Grayscale.A690FvLearn.B110FvLearn.C250Learn
{
    /// <summary>
    /// 本譜指し手リストの項目。
    /// </summary>
    public class HonpuSasiteListItemImpl
    {
        /// <summary>
        /// CSA棋譜 の指し手
        /// </summary>
        private CsaKifuSasite CsaSasite { get; set; }

        /// <summary>
        /// 指し手（SFEN符号に変換できるもの）
        /// </summary>
        public Move Move { get; set; }

        public HonpuSasiteListItemImpl(CsaKifuSasite sasite, Move move)
        {
            this.CsaSasite = sasite;
            this.Move = move;// 
        }

        /// <summary>
        /// リストボックスで表示する文字列です。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.CsaSasite.OptionTemezumi);
            sb.Append("手目 ");
            sb.Append(this.CsaSasite.DestinationMasu);
            sb.Append(" ");
            sb.Append(this.CsaSasite.Second);
            sb.Append(" ");
            sb.Append(this.CsaSasite.Sengo);
            sb.Append(" ");
            sb.Append(this.CsaSasite.SourceMasu);
            sb.Append(" ");
            sb.Append(this.CsaSasite.Syurui);
            sb.Append(" ");
            sb.Append(this.Move);

            return sb.ToString();
        }

    }
}
