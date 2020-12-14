using Grayscale.A210KnowNingen.B240Move.C500Struct;

namespace Grayscale.A210KnowNingen.B280Tree.C500Struct
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
