using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using System;
using System.Text;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C250____Learn
{
    /// <summary>
    /// 合法手リストのアイテム。
    /// </summary>
    public class GohosyuListItem
    {

        /// <summary>
        /// 合法手の連番。
        /// </summary>
        public int Count { get { return this.count; } }
        private int count;

        /// <summary>
        /// 指し手（SFEN符号に変換できるもの）
        /// </summary>
        public Move Move { get { return this.m_move_; } }
        private Move m_move_;

        /// <summary>
        /// JSAの指し手符号
        /// </summary>
        public string JsaSasiteStr { get { return this.jsaSasiteStr; } }
        private string jsaSasiteStr;

        public GohosyuListItem(int count, Move move, string jsaSasiteStr)
        {
            this.count = count;
            this.m_move_ = move;
            this.jsaSasiteStr = jsaSasiteStr;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(String.Format("{0,3}", this.Count));//合法手は 0～593手。
            sb.Append(")");

            // 日本将棋連盟式の指し手の符号表示は全角。「▲５三銀右不成」の７文字幅を最大と想定。
            sb.Append(this.JsaSasiteStr);
            switch (this.JsaSasiteStr.Length)
            {
                case 0: sb.Append("　　　　　　　"); break;
                case 1: sb.Append("　　　　　　"); break;
                case 2: sb.Append("　　　　　"); break;
                case 3: sb.Append("　　　　"); break;
                case 4: sb.Append("　　　"); break;
                case 5: sb.Append("　　"); break;
                case 6: sb.Append("　"); break;
                default: break;
            }
            sb.Append("　");
            sb.Append(String.Format("{0,-5}", this.Move));

            return sb.ToString();
        }

    }
}
