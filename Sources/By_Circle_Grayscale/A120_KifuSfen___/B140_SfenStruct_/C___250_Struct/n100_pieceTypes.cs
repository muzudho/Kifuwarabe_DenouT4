namespace Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct
{
    /// <summary>
    /// 駒の種類。先後を含まない。
    /// 
    /// 持ち駒を指定するにはこれは不便☆（＾▽＾）Piecesを使う☆
    /// 駒袋に使っている。
    /// </summary>
    public enum PieceTypes
    {
        /// <summary>
        /// 意味はないが、とりあえず空けている☆
        /// 先頭を 0 から始めておくと、for文が見やすくなる。
        /// </summary>
        None,

        /// <summary>
        /// 王
        /// </summary>
        K = 1,

        /// <summary>
        /// 飛車
        /// </summary>
        R,

        /// <summary>
        /// 角
        /// </summary>
        B,

        /// <summary>
        /// 金
        /// </summary>
        G,

        /// <summary>
        /// 銀
        /// </summary>
        S,

        /// <summary>
        /// 桂
        /// </summary>
        N,

        /// <summary>
        /// 香
        /// </summary>
        L,

        /// <summary>
        /// 歩
        /// </summary>
        P,

        /// <summary>
        /// 列挙型の配列要素部（データ部）のサイズ☆
        /// </summary>
        Num,

        /// <summary>
        /// データ部の最初のインデックスのこと。
        /// </summary>
        Start = 1,
    }
}
