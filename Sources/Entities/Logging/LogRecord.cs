using Grayscale.Kifuwaragyoku.Entities.Configuration;

namespace Grayscale.Kifuwaragyoku.Entities.Logging
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public class LogRecord : ILogRecord
    {
        public LogRecord(IResFile logFile, bool enabled, bool timeStampPrintable, bool enableConsole)
        {
            this.LogFile = logFile;
            this.Enabled = enabled;
            this.TimeStampPrintable = timeStampPrintable;
            this.EnableConsole = enableConsole;
        }

        /// <summary>
        /// 出力先ファイル。
        /// </summary>
        public IResFile LogFile { get; private set; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// タイムスタンプの有無。
        /// </summary>
        public bool TimeStampPrintable { get; private set; } = false;

        /// <summary>
        /// コンソール出力の有無。
        /// </summary>
        public bool EnableConsole { get; set; }
    }
}
