namespace Grayscale.Kifuwaragyoku.Entities.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using Nett;

    public static class Logger
    {
        /// <summary>
        /// あとで消す☆（＾～＾）
        /// </summary>
        /// <param name="message"></param>
        [Conditional("DEBUG")]
        public static void Trace(string message)
        {
            Console.WriteLine(message);
        }

        private static StringBuilder m_buffer_;

        static ILogRecord LogEntry(string profilePath, TomlTable toml, string resourceKey, bool enabled, bool timeStampPrintable, bool enableConsole, IErrorController kwDisplayer_OrNull)
        {
            return new LogRecord(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(resourceKey)), enabled, timeStampPrintable, enableConsole, kwDisplayer_OrNull);
        }

        static Logger()
        {
            Logger.m_buffer_ = new StringBuilder();

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            AddLog(LogTags.Default, new LogRecord(Path.Combine(profilePath, $"default_({System.Diagnostics.Process.GetCurrentProcess()})"), false, false, false, null));

            // ログを出せなかったときなど、致命的なエラーにも利用。
            AddLog(LogTags.ProcessNoneError, LogEntry(profilePath, toml, "N01ProcessNoneErrorLog", true, false, false, null));
            // 汎用ログ。千日手判定用。
            AddLog(LogTags.PeocessNoneSennitite, LogEntry(profilePath, toml, "N02PeocessNoneSennititeLog", true, false, false, null));
            AddLog(LogTags.ProcessServerDefault, LogEntry(profilePath, toml, "N03ProcessServerDefaultLog", true, false, false, null));
            // 擬似将棋サーバーのログ。ログ。送受信内容の記録専用です。
            AddLog(LogTags.ProcessServerNetworkAsync, LogEntry(profilePath, toml, "N04ProcessServerNetworkAsyncLog", true, true, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiDefault, LogEntry(profilePath, toml, "N05ProcessGuiDefaultLog", true, false, false, new ErrorControllerImpl()));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiKifuYomitori, LogEntry(profilePath, toml, "N06ProcessGuiKifuYomitoriLog", true, false, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiNetwork, LogEntry(profilePath, toml, "N07ProcessGuiNetworkLog", true, true, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiPaint, LogEntry(profilePath, toml, "N08ProcessGuiPaintLog", true, false, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiSennitite, LogEntry(profilePath, toml, "N09ProcessGuiSennititeLog", true, false, false, null));
            // AIMS GUIに対応する用のログ。
            AddLog(LogTags.ProcessAimsDefault, LogEntry(profilePath, toml, "N10ProcessAimsDefaultLog", true, false, false, null));
            // 将棋エンジンのログ。将棋エンジンきふわらべで汎用に使います。
            AddLog(LogTags.ProcessEngineDefault, LogEntry(profilePath, toml, "N11ProcessEngineDefaultLog", true, false, false, new ErrorControllerImpl()));
            // 将棋エンジンのログ。送受信内容の記録専用です。
            AddLog(LogTags.ProcessEngineNetwork, LogEntry(profilePath, toml, "N12ProcessEngineNetworkLog", true, true, false, null));
            // 将棋エンジンのログ。思考ルーチン専用です。
            AddLog(LogTags.ProcessEngineSennitite, LogEntry(profilePath, toml, "N13ProcessEngineSennititeLog", true, false, false, null));
            // その他のログ。汎用。テスト・プログラム用。
            AddLog(LogTags.ProcessTestProgramDefault, LogEntry(profilePath, toml, "N14ProcessTestProgramDefaultLog", true, false, false, null));
            // その他のログ。棋譜学習ソフト用。
            AddLog(LogTags.ProcessLearnerDefault, LogEntry(profilePath, toml, "N15ProcessLearnerDefaultLog", true, false, false, new ErrorControllerImpl()));
            // その他のログ。スピード計測ソフト用。
            AddLog(LogTags.ProcessSpeedTestKeisoku, LogEntry(profilePath, toml, "N16ProcessSpeedTestKeisokuLog", true, false, false, null));
            // その他のログ。ユニット・テスト用。
            AddLog(LogTags.ProcessUnitTestDefault, LogEntry(profilePath, toml, "N17ProcessUnitTestDefaultLog", true, false, true, new ErrorControllerImpl()));
        }

        /// <summary>
        /// アドレスの登録。ログ・ファイルのリムーブに使用。
        /// </summary>
        public static Dictionary<ILogTag, ILogRecord> LogMap
        {
            get
            {
                if (Logger.logMap == null)
                {
                    Logger.logMap = new Dictionary<ILogTag, ILogRecord>();
                }
                return Logger.logMap;
            }
        }
        private static Dictionary<ILogTag, ILogRecord> logMap;

        public static void AddLog(ILogTag key, ILogRecord value)
        {
            Logger.LogMap.Add(key, value);
        }

        public static ILogRecord GetRecord(ILogTag logTag)
        {
            try
            {
                return LogMap[logTag];
            }
            catch (Exception ex)
            {
                Logger.Trace($"エラー: GetRecord(). [{logTag.Name}] {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// ログファイルを削除します。(連番がなければ)
        /// 
        /// FIXME: アプリ起動後、ログが少し取られ始めたあとに削除が開始されることがあります。
        /// FIXME: 将棋エンジン起動時に、またログが削除されることがあります。
        /// </summary>
        public static void RemoveAllLogFiles()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var logsDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("LogsDirectory"));

            string[] paths = Directory.GetFiles(logsDirectory);
            foreach (string path in paths)
            {
                string name = Path.GetFileName(path);
                if (name.StartsWith("_log_"))
                {
                    string fullpath = Path.Combine(logsDirectory, name);
                    //MessageBox.Show("fullpath=[" + fullpath + "]", "ログ・ファイルの削除");
                    System.IO.File.Delete(fullpath);
                }
            }
        }

        public static StringBuilder FlushBuf()
        {
            var buf = Logger.m_buffer_;
            Logger.m_buffer_.Clear();
            return buf;
        }

        /// <summary>
        /// テキストを、ログ・ファイルの末尾に追記します。
        /// </summary>
        /// <param name="logTypes"></param>
        public static void Flush(ILogTag logTag, LogTypes logTypes, StringBuilder buf2)
        {
            var record = GetRecord(logTag);

            if (!record.Enabled)
            {
                // ログ出力オフ
                Logger.m_buffer_ = buf2;
                return;
            }

            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (record.TimeStampPrintable)
                {
                    sb.Append(DateTime.Now.ToString());
                    sb.Append(" ");
                }

                switch (logTypes)
                {
                    case LogTypes.Plain:
                        break;
                    case LogTypes.Error://エラーを、ログ・ファイルに記録します。
                        sb.Append("Error:");
                        break;
                    case LogTypes.ToServer:
                        sb.Append("<     ");
                        break;
                    case LogTypes.ToClient:
                        sb.Append(">     ");
                        break;
                }

                var message = buf2.ToString();

                if (logTypes == LogTypes.Error)
                {
                    MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string filepath2 = Path.Combine(Application.StartupPath, record.FileName);
                System.IO.File.AppendAllText(filepath2, message);

                if (record.EnableConsole)
                {
                    System.Console.Write(message);
                }
            }
            catch (Exception)
            {
                // 循環参照になるので、ログを取れません。
                // ログ出力に失敗しても、続行します。
            }
        }

        public static void ShowDialog(ILogTag logTag, string message)
        {
            var buf = Logger.FlushBuf();
            buf.AppendLine(message);
            MessageBox.Show(buf.ToString());
            Logger.Flush(logTag, LogTypes.Plain, buf);
            // ログ出力に失敗することがありますが、無視します。
        }
    }
}
