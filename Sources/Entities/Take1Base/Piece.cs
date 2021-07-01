namespace Grayscale.Kifuwaragyoku.Entities.Take1Base
{
    /// <summary>
    /// 駒の種類。先後を含む。持ち駒を指定するのに便利（配列のインデックス）。
    /// 成りは含んでいない。
    /// </summary>
    public enum Piece
    {
        /// <summary>
        /// 意味はないが、とりあえず空けている☆
        /// 先頭を 0 から始めておくと、for文が見やすくなる。
        /// </summary>
        None,

        /// <summary>
        /// ▲王
        /// </summary>
        K1 = 1,

        /// <summary>
        /// ▲飛車
        /// </summary>
        R1,

        /// <summary>
        /// ▲角
        /// </summary>
        B1,

        /// <summary>
        /// ▲金
        /// </summary>
        G1,

        /// <summary>
        /// ▲銀
        /// </summary>
        S1,

        /// <summary>
        /// ▲桂
        /// </summary>
        N1,

        /// <summary>
        /// ▲香
        /// </summary>
        L1,

        /// <summary>
        /// ▲歩
        /// </summary>
        P1,

        /// <summary>
        /// △王
        /// </summary>
        K2,

        /// <summary>
        /// △飛車
        /// </summary>
        R2,

        /// <summary>
        /// △角
        /// </summary>
        B2,

        /// <summary>
        /// △金
        /// </summary>
        G2,

        /// <summary>
        /// △銀
        /// </summary>
        S2,

        /// <summary>
        /// △桂
        /// </summary>
        N2,

        /// <summary>
        /// △香
        /// </summary>
        L2,

        /// <summary>
        /// △歩
        /// </summary>
        P2,

        /// <summary>
        /// 列挙型の配列要素部（データ部）のサイズ☆
        /// </summary>
        Num,

        /// <summary>
        /// 列挙型の配列要素部（データ部）のサイズ☆
        /// </summary>
        NumGote = Num,

        /// <summary>
        /// データ部の最初のインデックスのこと。
        /// </summary>
        StartSente = 1,

        /// <summary>
        /// 列挙型の配列要素部（データ部）のサイズ☆
        /// </summary>
        NumSente = 9,

        /// <summary>
        /// データ部の最初のインデックスのこと。
        /// </summary>
        StartGote = 9,
    }
}
