using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
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
