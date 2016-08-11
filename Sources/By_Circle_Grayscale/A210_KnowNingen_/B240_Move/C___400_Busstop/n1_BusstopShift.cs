namespace Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct
{
    /// <summary>
    /// 止まっている駒の位置。
    /// 
    /// ビット演算ができるのは 32bit int型。
    /// 
    /// 4         3         2         1Byte
    /// 0000 0000 0000 0000 0000 0000 0000 0000
    ///                                    |A |
    ///                               |B |
    ///                          |C | 
    ///                        D
    ///                       E
    ///                      F
    /// G
    /// 
    /// A: 筋(0～15)
    /// B: 段(0～15)
    /// C: 駒の種類(0～15)
    /// D: 盤上/駒台(0～1)
    /// E: 手番(0～1)
    /// F: エラーチェック(0～15)
    /// G: 符号
    /// </summary>
    public enum BusstopShift
    {
        Zero = 0,

        /// <summary>
        /// 筋
        /// </summary>
        Suji = 4*0,

        /// <summary>
        /// 段
        /// </summary>
        Dan = 4 * 1,

        /// <summary>
        /// 駒の種類
        /// </summary>
        Komasyurui = 4 * 2,

        /// <summary>
        /// 駒台
        /// </summary>
        Komadai = 4 * 3,

        /// <summary>
        /// 手番
        /// </summary>
        Playerside = 4 * 3 + 1,

        /// <summary>
        /// エラーチェック
        /// </summary>
        ErrorCheck = 4 * 3 + 2,
    }
}
