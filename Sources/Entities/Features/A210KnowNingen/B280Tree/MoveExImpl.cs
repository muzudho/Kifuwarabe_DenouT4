﻿namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public class MoveExImpl : MoveEx
    {
        public MoveExImpl()
        {
            this.m_move_ = Move.Empty;
        }
        public MoveExImpl(Move move)
        {
            this.m_move_ = move;
        }
        public MoveExImpl(Move move, float score)
        {
            this.m_move_ = move;
            this.m_score_ = score;
        }



        public Move Move
        {
            get
            {
                return this.m_move_;
            }
        }
        public void SetMove(Move move)
        {
            this.m_move_ = move;
        }
        private Move m_move_;



        /// <summary>
        /// スコア
        /// </summary>
        public float Score { get { return this.m_score_; } }
        public void AddScore(float offset) { this.m_score_ += offset; }
        public void SetScore(float score) { this.m_score_ = score; }
        private float m_score_;
    }
}
