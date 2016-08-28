
namespace Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct
{

    /// <summary>
    /// テーブル形式の局面データ。
    /// </summary>
    public interface RO_Kyokumen1_ForFormat
    {

        /// <summary>
        /// 将棋盤上の駒。[suji][dan]。
        /// sujiは1～9。danは1～9。0は空欄。つまり 100要素ある。
        /// 「K」「+p」といった形式で書く。(SFEN形式)
        /// </summary>
        string[,] Ban { get; set; }

        /// <summary>
        /// 駒の種類は [0]から、空っぽ,空っぽ,▲飛,▲角,▲金,▲銀,▲桂,▲香,▲歩,空っぽ,△飛,△角,△金,△銀,△桂,△香,△歩 の順。
        /// </summary>
        int[] MotiSu { get; set; }

        /// <summary>
        /// 手目済み。初期局面を 0手目済み、初手を指した後の局面を 1手目済みとカウントします。
        /// </summary>
        int Temezumi { get; set; }
    }
}
