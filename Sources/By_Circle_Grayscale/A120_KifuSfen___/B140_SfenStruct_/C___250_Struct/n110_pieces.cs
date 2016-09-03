namespace Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct
{
    /// <summary>
    /// 駒の種類。先後を含む。持ち駒を指定するのに便利（配列のインデックス）。
    /// 成りは含んでいない。
    /// </summary>
    public enum Pieces
    {
        /// <summary>
        /// 意味はないが、とりあえず空けている☆
        /// 先頭を 0 から始めておくと、for文が見やすくなる。
        /// </summary>
        None,

        /// <summary>
        /// ▲王
        /// </summary>
        K = 1,

        /// <summary>
        /// ▲飛車
        /// </summary>
        R,

        /// <summary>
        /// ▲角
        /// </summary>
        B,

        /// <summary>
        /// ▲金
        /// </summary>
        G,

        /// <summary>
        /// ▲銀
        /// </summary>
        S,

        /// <summary>
        /// ▲桂
        /// </summary>
        N,

        /// <summary>
        /// ▲香
        /// </summary>
        L,

        /// <summary>
        /// ▲歩
        /// </summary>
        P,

        /// <summary>
        /// △王
        /// </summary>
        k,

        /// <summary>
        /// △飛車
        /// </summary>
        r,

        /// <summary>
        /// △角
        /// </summary>
        b,

        /// <summary>
        /// △金
        /// </summary>
        g,

        /// <summary>
        /// △銀
        /// </summary>
        s,

        /// <summary>
        /// △桂
        /// </summary>
        n,

        /// <summary>
        /// △香
        /// </summary>
        l,

        /// <summary>
        /// △歩
        /// </summary>
        p,

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
