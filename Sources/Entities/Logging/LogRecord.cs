namespace Grayscale.Kifuwaragyoku.Entities.Logging
{
    /// <summary>
    /// ログの書き込み先情報。
    /// </summary>
    public class LogRecord : ILogRecord
    {
        /// <summary>
        /// ファイル名。
        /// </summary>
        public string Basename { get; set; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        public bool Enabled { get { return this.enabled; } }
        private bool enabled;

        /// <summary>
        /// タイムスタンプの有無。
        /// </summary>
        public bool TimeStampPrintable { get; private set; } = false;

        /// <summary>
        /// コンソール出力の有無。
        /// </summary>
        public bool EnableConsole { get; set; }

        public IErrorController KwDisplayerOrNull { get; set; }

        public LogRecord(string basename, int n, bool enabled, bool timeStampPrintable, bool enableConsole, IErrorController kwDisplayer_OrNull)
        {
            this.Basename = basename;
            this.enabled = enabled;
            this.TimeStampPrintable = timeStampPrintable;
            this.EnableConsole = enableConsole;
            this.KwDisplayerOrNull = kwDisplayer_OrNull;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            ILogRecord p = obj as ILogRecord;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return ($"{this.Basename}" == $"{p.Basename}");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
