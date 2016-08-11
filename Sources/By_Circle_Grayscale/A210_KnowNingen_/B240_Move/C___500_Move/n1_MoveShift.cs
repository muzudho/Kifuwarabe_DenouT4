namespace Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct
{
    /// <summary>
    /// 指し手。
    /// 一意にする必要がある。（スコアなどを覚えさせてはいけない）
    /// 
    /// ビット演算ができるのは 32bit int型。
    /// 
    /// 4         3         2         1Byte
    /// 0000 0000 0000 0000 0000 0000 0000 0000
    ///                                    |A |
    ///                               |B |
    ///                          |C |
    ///                     |D |
    ///                |E | 
    ///           |F |
    ///         G
    ///        H
    ///       I
    ///      J
    /// K
    /// 
    /// A: 自筋(0～15)
    /// B: 自段(0～15)
    /// C: 至筋(0～15)
    /// D: 至段(0～15)
    /// E: 移動した駒の種類(0～15)
    /// F: 取った駒の種類(0～15)
    /// G: 成らない/成る(0～1)
    /// H: 打たない/打つ(0～1)
    /// I: 手番(0～1)
    /// J: エラーチェック(0～15)
    /// K: 符号
    /// 
    /// 以前は、次のような構造をしていた。これをビットフィールドにまとめた。
    /// （１）自（筋、段、移動した駒の種類、手番）
    /// （２）至（筋、段、成る、打つ）
    /// （３）取った駒
    /// （４）エラーチェック
    /// </summary>
    public enum MoveShift
    {
        Zero = 0,

        /// <summary>
        /// 自筋
        /// </summary>
        SrcSuji = 4*0,

        /// <summary>
        /// 自段
        /// </summary>
        SrcDan = 4 * 1,

        /// <summary>
        /// 至筋
        /// </summary>
        DstSuji = 4 * 2,

        /// <summary>
        /// 至段
        /// </summary>
        DstDan = 4 * 3,

        /// <summary>
        /// 移動した駒の種類
        /// </summary>
        Komasyurui = 4 * 4,

        /// <summary>
        /// 取った駒の種類
        /// </summary>
        Captured = 4 * 5,

        /// <summary>
        /// 成らない
        /// </summary>
        Promotion = 4 * 6,

        /// <summary>
        /// 打たない
        /// </summary>
        Drop = 4 * 6 + 1,

        /// <summary>
        /// 手番
        /// </summary>
        Playerside = 4 * 6 + 2,

        /// <summary>
        /// エラーチェック
        /// </summary>
        ErrorCheck = 4 * 6 + 3,
    }
}
