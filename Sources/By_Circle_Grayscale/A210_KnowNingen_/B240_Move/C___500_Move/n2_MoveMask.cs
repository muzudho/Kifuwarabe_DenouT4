namespace Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct
{
    public enum MoveMask
    {
        Zero = 0x00,

        /// <summary>
        /// 自筋
        /// </summary>
        SrcSuji = 0x0f << MoveShift.SrcSuji,

        /// <summary>
        /// 自段
        /// </summary>
        SrcDan = 0x0f << MoveShift.SrcDan,

        /// <summary>
        /// 至筋
        /// </summary>
        DstSuji = 0x0f << MoveShift.DstSuji,

        /// <summary>
        /// 至段
        /// </summary>
        DstDan = 0x0f << MoveShift.DstDan,

        /// <summary>
        /// 移動した駒の種類
        /// </summary>
        Komasyurui = 0x0f << MoveShift.Komasyurui,

        /// <summary>
        /// 取った駒の種類
        /// </summary>
        Captured = 0x0f << MoveShift.Captured,

        /// <summary>
        /// 成らない
        /// </summary>
        Promotion = 0x01 << MoveShift.Promotion,

        /// <summary>
        /// 打たない
        /// </summary>
        Drop = 0x01 << MoveShift.Drop,

        /// <summary>
        /// 手番
        /// </summary>
        Playerside = 0x01 << MoveShift.Playerside,

        /// <summary>
        /// エラーチェック
        /// </summary>
        ErrorCheck = 0x01 << MoveShift.ErrorCheck,

    }
}
