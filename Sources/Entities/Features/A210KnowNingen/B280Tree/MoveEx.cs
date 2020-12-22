namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface MoveEx
    {
        Move Move { get; }
        void SetMove(Move move);



        /// <summary>
        /// スコア
        /// </summary>
        float Score { get; }
        void AddScore(float offset);
        void SetScore(float score);
    }
}
