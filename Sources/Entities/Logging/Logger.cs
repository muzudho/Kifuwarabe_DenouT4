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
        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }

        /// <summary>
        /// あとで消す☆（＾～＾）
        /// </summary>
        /// <param name="message"></param>
        [Conditional("DEBUG")]
        public static void Trace(string message)
        {
            Console.WriteLine(message);
        }

        static Logger()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var logDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.LogDirectory));

            var basename = Path.Combine(logDirectory, $"default_({System.Diagnostics.Process.GetCurrentProcess()})");
            AddLog(LogTags.Default, new LogRecord($"{basename}.log", 0, false, false, false, null));

            // ログを出せなかったときなど、致命的なエラーにも利用。
            AddLog(LogTags.ProcessNoneError, LogEntry(logDirectory, toml, SpecifiedFiles.N01ProcessNoneErrorLog, true, false, false, null));
            // 汎用ログ。千日手判定用。
            AddLog(LogTags.PeocessNoneSennitite, LogEntry(logDirectory, toml, SpecifiedFiles.N02PeocessNoneSennititeLog, true, false, false, null));
            AddLog(LogTags.ProcessServerDefault, LogEntry(logDirectory, toml, SpecifiedFiles.N03ProcessServerDefaultLog, true, false, false, null));
            // 擬似将棋サーバーのログ。ログ。送受信内容の記録専用です。
            AddLog(LogTags.ProcessServerNetworkAsync, LogEntry(logDirectory, toml, SpecifiedFiles.N04ProcessServerNetworkAsyncLog, true, true, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiDefault, LogEntry(logDirectory, toml, SpecifiedFiles.N05ProcessGuiDefaultLog, true, false, false, new ErrorControllerImpl()));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiKifuYomitori, LogEntry(logDirectory, toml, SpecifiedFiles.N06ProcessGuiKifuYomitoriLog, true, false, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiNetwork, LogEntry(logDirectory, toml, SpecifiedFiles.N07ProcessGuiNetworkLog, true, true, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiPaint, LogEntry(logDirectory, toml, SpecifiedFiles.N08ProcessGuiPaintLog, true, false, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiSennitite, LogEntry(logDirectory, toml, SpecifiedFiles.N09ProcessGuiSennititeLog, true, false, false, null));
            // AIMS GUIに対応する用のログ。
            AddLog(LogTags.ProcessAimsDefault, LogEntry(logDirectory, toml, SpecifiedFiles.N10ProcessAimsDefaultLog, true, false, false, null));
            // 将棋エンジンのログ。将棋エンジンきふわらべで汎用に使います。
            AddLog(LogTags.ProcessEngineDefault, LogEntry(logDirectory, toml, SpecifiedFiles.N11ProcessEngineDefaultLog, true, false, false, new ErrorControllerImpl()));
            // 将棋エンジンのログ。送受信内容の記録専用です。
            AddLog(LogTags.ProcessEngineNetwork, LogEntry(logDirectory, toml, SpecifiedFiles.N12ProcessEngineNetworkLog, true, true, false, null));
            // 将棋エンジンのログ。思考ルーチン専用です。
            AddLog(LogTags.ProcessEngineSennitite, LogEntry(logDirectory, toml, SpecifiedFiles.N13ProcessEngineSennititeLog, true, false, false, null));
            // その他のログ。汎用。テスト・プログラム用。
            AddLog(LogTags.ProcessTestProgramDefault, LogEntry(logDirectory, toml, SpecifiedFiles.N14ProcessTestProgramDefaultLog, true, false, false, null));
            // その他のログ。棋譜学習ソフト用。
            AddLog(LogTags.ProcessLearnerDefault, LogEntry(logDirectory, toml, SpecifiedFiles.N15ProcessLearnerDefaultLog, true, false, false, new ErrorControllerImpl()));
            // その他のログ。スピード計測ソフト用。
            AddLog(LogTags.ProcessSpeedTestKeisoku, LogEntry(logDirectory, toml, SpecifiedFiles.N16ProcessSpeedTestKeisokuLog, true, false, false, null));
            // その他のログ。ユニット・テスト用。
            AddLog(LogTags.ProcessUnitTestDefault, LogEntry(logDirectory, toml, SpecifiedFiles.N17ProcessUnitTestDefaultLog, true, false, true, new ErrorControllerImpl()));
        }

        static ILogRecord LogEntry(string logDirectory, TomlTable toml, string resourceKey, bool enabled, bool timeStampPrintable, bool enableConsole, IErrorController kwDisplayer_OrNull)
        {
            var basename = Path.Combine(logDirectory, toml.Get<TomlTable>("Logs").Get<string>(resourceKey));
            return new LogRecord(basename, 0, enabled, timeStampPrintable, enableConsole, kwDisplayer_OrNull);
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
            var logDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.LogDirectory));

            string[] paths = Directory.GetFiles(logDirectory);
            foreach (string path in paths)
            {
                string name = Path.GetFileName(path);
                if (name.StartsWith("_log_"))
                {
                    string fullpath = Path.Combine(logDirectory, name);
                    //MessageBox.Show("fullpath=[" + fullpath + "]", "ログ・ファイルの削除");
                    System.IO.File.Delete(fullpath);
                }
            }
        }

        /// <summary>
        /// テキストを、ログ・ファイルの末尾に追記します。
        /// </summary>
        /// <param name="logTypes"></param>
        public static void Flush(ILogTag logTag, LogTypes logTypes, string message)
        {
            var record = GetRecord(logTag);

            if (!record.Enabled)
            {
                // ログ出力オフ
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

                if (logTypes == LogTypes.Error)
                {
                    MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string filepath2 = Path.Combine(Application.StartupPath, record.Basename);
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
            MessageBox.Show(message);
            Logger.Flush(logTag, LogTypes.Plain, message);
            // ログ出力に失敗することがありますが、無視します。
        }
    }
}
