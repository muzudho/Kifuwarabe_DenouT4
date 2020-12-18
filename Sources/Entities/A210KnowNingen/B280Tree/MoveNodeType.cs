namespace Grayscale.A210KnowNingen.B280Tree.C500Struct
{
    /// <summary>
    /// ツリー構造の移動の区分
    /// </summary>
    public enum MoveNodeType
    {
        /// <summary>
        /// 移動なし
        /// </summary>
        None,

        /// <summary>
        /// 進む
        /// </summary>
        Do,

        /// <summary>
        /// 戻る
        /// </summary>
        Undo,

        /// <summary>
        /// 初期化
        /// </summary>
        Clear
    }
}
