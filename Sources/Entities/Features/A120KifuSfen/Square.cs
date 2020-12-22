
namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    /// <summary>
    /// 升番地。
    /// </summary>
    public abstract class Square
    {
        /// <summary>
        /// 本将棋の将棋盤の筋、段を、升番地へ変換。
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        public static int From(int file, int rank)
        {
            int sq = -1;

            if (1 <= file && file <= 9 && 1 <= rank && rank <= 9)
            {
                sq = (file - 1) * 9 + (rank - 1);
            }

            if (sq < 0 || 80 < sq)
            {
                sq = -1;//範囲外が指定されることもあります。
            }

            return sq;
        }
    }
}
