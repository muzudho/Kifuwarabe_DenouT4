﻿namespace Grayscale.Kifuwaragyoku.Entities.Logging
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
        /// <summary>
        /// このクラスを使う前にセットしてください。
        /// </summary>
        public static void Init(IEngineConf engineConf)
        {
            EngineConf = engineConf;

            TraceRecord = LogEntry(SpecifiedFiles.Trace, true, true, false);
            DebugRecord = LogEntry(SpecifiedFiles.Debug, true, true, false);
            InfoRecord = LogEntry(SpecifiedFiles.Info, true, true, false);
            NoticeRecord = LogEntry(SpecifiedFiles.Notice, true, true, false);
            WarnRecord = LogEntry(SpecifiedFiles.Warn, true, true, false);
            ErrorRecord = LogEntry(SpecifiedFiles.Error, true, true, false);
            FatalRecord = LogEntry(SpecifiedFiles.Fatal, true, true, false);

            /*
            var logFile = LogFile.AsLog(logDirectory, $"default_({System.Diagnostics.Process.GetCurrentProcess()}).log");
            AddLog(LogTags.Default, new LogRecord(logFile, false, false, false, null));

            // ログを出せなかったときなど、致命的なエラーにも利用。
            AddLog(LogTags.ProcessNoneError, LogEntry(SpecifiedFiles.N01ProcessNoneErrorLog, true, false, false, null));
            // 汎用ログ。千日手判定用。
            AddLog(LogTags.PeocessNoneSennitite, LogEntry(SpecifiedFiles.N02PeocessNoneSennititeLog, true, false, false, null));
            AddLog(LogTags.ProcessServerDefault, LogEntry(SpecifiedFiles.N03ProcessServerDefaultLog, true, false, false, null));
            // 擬似将棋サーバーのログ。ログ。送受信内容の記録専用です。
            AddLog(LogTags.ProcessServerNetworkAsync, LogEntry(SpecifiedFiles.N04ProcessServerNetworkAsyncLog, true, true, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiDefault, LogEntry(SpecifiedFiles.N05ProcessGuiDefaultLog, true, false, false, new ErrorControllerImpl()));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiKifuYomitori, LogEntry(SpecifiedFiles.N06ProcessGuiKifuYomitoriLog, true, false, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiNetwork, LogEntry(SpecifiedFiles.N07ProcessGuiNetworkLog, true, true, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiPaint, LogEntry(SpecifiedFiles.N08ProcessGuiPaintLog, true, false, false, null));
            // C# GUIのログ。
            AddLog(LogTags.ProcessGuiSennitite, LogEntry(SpecifiedFiles.N09ProcessGuiSennititeLog, true, false, false, null));
            // AIMS GUIに対応する用のログ。
            AddLog(LogTags.ProcessAimsDefault, LogEntry(SpecifiedFiles.N10ProcessAimsDefaultLog, true, false, false, null));
            // 将棋エンジンのログ。将棋エンジンきふわらべで汎用に使います。
            AddLog(LogTags.ProcessEngineDefault, LogEntry(SpecifiedFiles.N11ProcessEngineDefaultLog, true, false, false, new ErrorControllerImpl()));
            // 将棋エンジンのログ。送受信内容の記録専用です。
            AddLog(LogTags.ProcessEngineNetwork, LogEntry(SpecifiedFiles.N12ProcessEngineNetworkLog, true, true, false, null));
            // 将棋エンジンのログ。思考ルーチン専用です。
            AddLog(LogTags.ProcessEngineSennitite, LogEntry(SpecifiedFiles.N13ProcessEngineSennititeLog, true, false, false, null));
            // その他のログ。汎用。テスト・プログラム用。
            AddLog(LogTags.ProcessTestProgramDefault, LogEntry(SpecifiedFiles.N14ProcessTestProgramDefaultLog, true, false, false, null));
            // その他のログ。棋譜学習ソフト用。
            AddLog(LogTags.ProcessLearnerDefault, LogEntry(SpecifiedFiles.N15ProcessLearnerDefaultLog, true, false, false, new ErrorControllerImpl()));
            // その他のログ。スピード計測ソフト用。
            AddLog(LogTags.ProcessSpeedTestKeisoku, LogEntry(SpecifiedFiles.N16ProcessSpeedTestKeisokuLog, true, false, false, null));
            // その他のログ。ユニット・テスト用。
            AddLog(LogTags.ProcessUnitTestDefault, LogEntry(SpecifiedFiles.N17ProcessUnitTestDefaultLog, true, false, true, new ErrorControllerImpl()));
            */
        }

        static ILogRecord LogEntry(string key, bool enabled, bool timeStampPrintable, bool enableConsole)
        {
            var logFile = ResFile.AsLog(EngineConf.LogDirectory, EngineConf.GetLogBasename(key));
            return new LogRecord(logFile, enabled, timeStampPrintable, enableConsole);
        }

        static IEngineConf EngineConf { get; set; }
        public static ILogRecord TraceRecord { get; private set; }
        public static ILogRecord DebugRecord { get; private set; }
        public static ILogRecord InfoRecord { get; private set; }
        public static ILogRecord NoticeRecord { get; private set; }
        public static ILogRecord WarnRecord { get; private set; }
        public static ILogRecord ErrorRecord { get; private set; }
        public static ILogRecord FatalRecord { get; private set; }

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
        /// ログ・ディレクトリー直下の ログファイルを削除します。
        /// 
        /// Example:
        /// [GUID]name.log
        /// name.log.png
        /// ...
        ///
        /// * 将棋エンジン起動後、ログが少し取られ始めたあとに削除を開始するようなところで実行しないでください。
        /// * TODO usinewgame のタイミングでログを削除したい。
        /// </summary>
        public static void RemoveAllLogFiles()
        {
            try
            {
                string[] paths = Directory.GetFiles(EngineConf.LogDirectory);
                foreach (string path in paths)
                {
                    string name = Path.GetFileName(path);
                    if (name.StartsWith("_log_") || name.EndsWith(".log") || name.Contains(".log."))
                    {
                        System.IO.File.Delete(path);
                    }
                }
            }
            catch (Exception)
            {
                // ログ・ファイルの削除に失敗しても無視します。
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
