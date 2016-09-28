using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct
{
    public class MoveExImpl : MoveEx
    {
        public MoveExImpl()
        {
            this.Move = Conv_Move.GetErrorMove();
        }
        public MoveExImpl(Move move)
        {
            this.Move = move;
        }
        public MoveExImpl(Move move,float score)
        {
            this.Move = move;
            this.m_score_ = score;
        }



        public Move Move { get; set; }



        /// <summary>
        /// スコア
        /// </summary>
        public float Score { get { return this.m_score_; } }
        public void AddScore(float offset) { this.m_score_ += offset; }
        public void SetScore(float score) { this.m_score_ = score; }
        private float m_score_;
    }
}
