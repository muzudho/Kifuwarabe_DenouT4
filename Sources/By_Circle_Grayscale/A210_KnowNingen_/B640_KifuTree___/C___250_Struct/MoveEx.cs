using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct
{
    public interface MoveEx
    {
        Move Move { get; set; }



        /// <summary>
        /// スコア
        /// </summary>
        float Score { get; }
        void AddScore(float offset);
        void SetScore(float score);
    }
}
