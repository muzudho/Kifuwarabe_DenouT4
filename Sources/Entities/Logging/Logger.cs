namespace Grayscale.Kifuwaragyoku.Entities.Logging
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using Grayscale.Kifuwaragyoku.Entities.Configuration;
    using Nett;

    public static class Logger
    {
        static Logger()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var logDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.LogDirectory));

            TraceRecord = LogEntry(logDirectory, toml, SpecifiedFiles.Trace, true, true, false, null);
            DebugRecord = LogEntry(logDirectory, toml, SpecifiedFiles.Debug, true, true, false, null);
            InfoRecord = LogEntry(logDirectory, toml, SpecifiedFiles.Info, true, true, false, null);
            NoticeRecord = LogEntry(logDirectory, toml, SpecifiedFiles.Notice, true, true, false, null);
            WarnRecord = LogEntry(logDirectory, toml, SpecifiedFiles.Warn, true, true, false, null);
            ErrorRecord = LogEntry(logDirectory, toml, SpecifiedFiles.Error, true, true, false, null);
            FatalRecord = LogEntry(logDirectory, toml, SpecifiedFiles.Fatal, true, true, false, null);

            /*
            var logFile = LogFile.AsLog(logDirectory, $"default_({System.Diagnostics.Process.GetCurrentProcess()}).log");
            AddLog(LogTags.Default, new LogRecord(logFile, false, false, false, null));

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
            */
        }

        static ILogRecord LogEntry(string logDirectory, TomlTable toml, string resourceKey, bool enabled, bool timeStampPrintable, bool enableConsole, IErrorController kwDisplayer_OrNull)
        {
            var logFile = ResFile.AsLog(logDirectory, toml.Get<TomlTable>("Logs").Get<string>(resourceKey));
            return new LogRecord(logFile, enabled, timeStampPrintable, enableConsole, kwDisplayer_OrNull);
        }

        static readonly ILogRecord TraceRecord;
        static readonly ILogRecord DebugRecord;
        static readonly ILogRecord InfoRecord;
        static readonly ILogRecord NoticeRecord;
        static readonly ILogRecord WarnRecord;
        static readonly ILogRecord ErrorRecord;
        static readonly ILogRecord FatalRecord;

        /// <summary>
        /// テキストをそのまま、ファイルへ出力するためのものです。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteFile(IResFile logFile, string contents)
        {
            File.WriteAllText(logFile.Name, contents);
            // MessageBox.Show($a"ファイルを出力しました。
            //{path}");
        }

        /// <summary>
        /// トレース・レベル。
        /// </summary>
        /// <param name="line"></param>
        [Conditional("DEBUG")]
        public static void Trace(string line, IResFile targetOrNull = null)
        {
            Logger.XLine(TraceRecord, "Trace", line, targetOrNull);
        }

        /// <summary>
        /// デバッグ・レベル。
        /// </summary>
        /// <param name="line"></param>
        [Conditional("DEBUG")]
        public static void Debug(string line, IResFile targetOrNull = null)
        {
            Logger.XLine(DebugRecord, "Debug", line, targetOrNull);
        }

        /// <summary>
        /// インフォ・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Info(string line, IResFile targetOrNull = null)
        {
            Logger.XLine(InfoRecord, "Info", line, targetOrNull);
        }

        /// <summary>
        /// ノティス・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Notice(string line, IResFile targetOrNull = null)
        {
            Logger.XLine(NoticeRecord, "Notice", line, targetOrNull);
        }

        /// <summary>
        /// ワーン・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Warn(string line, IResFile targetOrNull = null)
        {
            Logger.XLine(WarnRecord, "Warn", line, targetOrNull);
        }

        /// <summary>
        /// エラー・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Error(string line, IResFile targetOrNull = null)
        {
            Logger.XLine(ErrorRecord, "Error", line, targetOrNull);
        }

        /// <summary>
        /// ファータル・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Fatal(string line, IResFile targetOrNull = null)
        {
            Logger.XLine(FatalRecord, "Fatal", line, targetOrNull);
        }

        /// <summary>
        /// ログ・ファイルに記録します。失敗しても無視します。
        /// </summary>
        /// <param name="line"></param>
        static void XLine(ILogRecord record, string level, string line, IResFile targetOrNull)
        {
            // ログ出力オフ
            if (!record.Enabled)
            {
                return;
            }

            // ログ追記
            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (record.TimeStampPrintable)
                {
                    sb.Append($"[{DateTime.Now.ToString()}] ");
                }

                sb.Append($"{level} {line}");
                sb.AppendLine();

                string message = sb.ToString();

                if (targetOrNull != null)
                {
                    System.IO.File.AppendAllText(targetOrNull.Name, message);
                }
                else
                {
                    System.IO.File.AppendAllText(record.LogFile.Name, message);
                }
            }
            catch (Exception)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので 無視します。
            }
        }

        /// <summary>
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// Client の C。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineC(
            string line
            //,
            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            // ログ追記
            // ：{memberName}：{sourceFilePath}：{sourceLineNumber}
            File.AppendAllText(NoticeRecord.LogFile.Name, $@"{DateTime.Now.ToString()}  > {line}
");
        }

        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// Server の S。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineS(
            string line
            //,
            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            // ログ追記
            // ：{memberName}：{sourceFilePath}：{sourceLineNumber}
            File.AppendAllText(NoticeRecord.LogFile.Name, $@"{DateTime.Now.ToString()}<   {line}
");
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

        public static void ShowDialog(string message)
        {
            MessageBox.Show(message);
            Logger.Trace(message);
            // ログ出力に失敗することがありますが、無視します。
        }
    }
}
