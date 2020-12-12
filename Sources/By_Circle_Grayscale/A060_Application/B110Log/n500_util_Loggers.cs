using System;
using System.IO;
using System.Windows.Forms;
using Grayscale.A060Application.B310Settei.C500Struct;//FIXME:
using Nett;

namespace Grayscale.A060Application.B110Log.C500Struct
{
    /// <summary>
    /// ロガー、エラー・ハンドラーを集中管理します。
    /// </summary>
    public static class ErrorControllerReference
    {
        public static readonly ILogger ProcessNoneDefault;

        /// <summary>
        /// ログを出せなかったときなど、致命的なエラーにも利用。
        /// </summary>
        public static readonly ILogger ProcessNoneError;

        /// <summary>
        /// 汎用ログ。千日手判定用。
        /// </summary>
        public static readonly ILogger PeocessNoneSennitite;

        public static readonly ILogger ProcessServerDefault;

        /// <summary>
        /// 擬似将棋サーバーのログ。ログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly ILogger ProcessServerNetworkAsync;

        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiDefault;

        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiKifuYomitori;

        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiNetwork;

        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiPaint;

        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiSennitite;

        /// <summary>
        /// AIMS GUIに対応する用のログ。
        /// </summary>
        public static readonly ILogger ProcessAimsDefault;

        /// <summary>
        /// 将棋エンジンのログ。将棋エンジンきふわらべで汎用に使います。
        /// </summary>
        public static readonly ILogger ProcessEngineDefault;

        /// <summary>
        /// 将棋エンジンのログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly ILogger ProcessEngineNetwork;

        /// <summary>
        /// 将棋エンジンのログ。思考ルーチン専用です。
        /// </summary>
        public static readonly ILogger ProcessEngineSennitite;

        /// <summary>
        /// その他のログ。汎用。テスト・プログラム用。
        /// </summary>
        public static readonly ILogger ProcessTestProgramDefault;

        /// <summary>
        /// その他のログ。棋譜学習ソフト用。
        /// </summary>
        public static readonly ILogger ProcessLearnerDefault;

        /// <summary>
        /// その他のログ。スピード計測ソフト用。
        /// </summary>
        public static readonly ILogger ProcessSpeedTestKeisoku;

        /// <summary>
        /// その他のログ。ユニット・テスト用。
        /// </summary>
        public static readonly ILogger ProcessUnitTestDefault;

        static ErrorControllerReference()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            ErrorControllerReference.ProcessNoneDefault = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("DataDirectory"), $"default_({System.Diagnostics.Process.GetCurrentProcess()})"), ".txt", false, false, false, null);
            ErrorControllerReference.ProcessNoneError = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N01ProcessNoneErrorLog")), ".txt", true, false, false, null);
            ErrorControllerReference.PeocessNoneSennitite = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N02PeocessNoneSennititeLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessServerDefault = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N03ProcessServerDefaultLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessServerNetworkAsync = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N04ProcessServerNetworkAsyncLog")), ".txt", true, true, false, null);
            ErrorControllerReference.ProcessGuiDefault = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N05ProcessGuiDefaultLog")), ".txt", true, false, false, new ErrorControllerImpl());
            ErrorControllerReference.ProcessGuiKifuYomitori = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N06ProcessGuiKifuYomitoriLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessGuiNetwork = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N07ProcessGuiNetworkLog")), ".txt", true, true, false, null);
            ErrorControllerReference.ProcessGuiPaint = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N08ProcessGuiPaintLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessGuiSennitite = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N09ProcessGuiSennititeLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessAimsDefault = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N10ProcessAimsDefaultLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessEngineDefault = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N11ProcessEngineDefaultLog")), ".txt", true, false, false, new ErrorControllerImpl());
            ErrorControllerReference.ProcessEngineNetwork = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N12ProcessEngineNetworkLog")), ".txt", true, true, false, null);
            ErrorControllerReference.ProcessEngineSennitite = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N13ProcessEngineSennititeLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessTestProgramDefault = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N14ProcessTestProgramDefaultLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessLearnerDefault = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N15ProcessLearnerDefaultLog")), ".txt", true, false, false, new ErrorControllerImpl());
            ErrorControllerReference.ProcessSpeedTestKeisoku = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N16ProcessSpeedTestKeisokuLog")), ".txt", true, false, false, null);
            ErrorControllerReference.ProcessUnitTestDefault = new LoggerImpl(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("N17ProcessUnitTestDefaultLog")), ".txt", true, false, true, new ErrorControllerImpl());
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

            try
            {
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
            catch (Exception ex)
            {
                ErrorControllerReference.ProcessNoneError.DonimoNaranAkirameta(ex, "ﾛｸﾞﾌｧｲﾙ削除中☆");
                throw;
            }
        }
    }
}
