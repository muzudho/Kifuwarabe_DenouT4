namespace Grayscale.Kifuwaragyoku.Entities.Logging
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public interface ILogRecord
    {
        /// <summary>
        /// ファイル名。
        /// </summary>
        string Basename { get; set; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// タイムスタンプの有無。
        /// </summary>
        bool TimeStampPrintable { get; }

        /// <summary>
        /// コンソール出力の有無。
        /// </summary>
        bool EnableConsole { get; set; }

        IErrorController KwDisplayerOrNull { get; set; }
    }
}
