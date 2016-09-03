namespace Grayscale.A060_Application.B110_Log________.C___500_Struct
{
    public enum LogTypes
    {
        None,

        /// <summary>
        /// メモを、ログ・ファイルの末尾に追記します。
        /// </summary>
        Memo,

        /// <summary>
        /// エラーを記入します。
        /// </summary>
        Error,

        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        ToServer,

        /// <summary>
        /// クライアントへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        ToClient

    }
}
