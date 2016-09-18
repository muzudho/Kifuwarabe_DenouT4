namespace Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct
{
    public enum BusstopMask
    {
        Zero = 0x00,

        /// <summary>
        /// 筋
        /// </summary>
        Suji = 0x0f << BusstopShift.Suji,

        /// <summary>
        /// 自段
        /// </summary>
        Dan = 0x0f << BusstopShift.Dan,

        /// <summary>
        /// 駒の種類
        /// </summary>
        Komasyurui = 0x0f << BusstopShift.Komasyurui,

        /// <summary>
        /// 盤上 or 駒台
        /// </summary>
        Komadai = 0x01 << BusstopShift.Komadai,

        /// <summary>
        /// 手番
        /// </summary>
        Playerside = 0x01 << BusstopShift.Playerside,

        /// <summary>
        /// エラーチェック
        /// </summary>
        ErrorCheck = 0x01 << BusstopShift.ErrorCheck,

    }
}
