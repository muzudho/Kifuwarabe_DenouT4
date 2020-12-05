using Grayscale.A210_KnowNingen_.B240_Move_______.C500Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500Struct
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
