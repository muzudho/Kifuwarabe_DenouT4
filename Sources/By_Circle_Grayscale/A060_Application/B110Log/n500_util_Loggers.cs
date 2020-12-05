using System;
using System.IO;
using System.Windows.Forms;
using Grayscale.A060Application.B310Settei.C500Struct;//FIXME:

namespace Grayscale.A060Application.B110Log.C500Struct
{
    /// <summary>
    /// ロガー、エラー・ハンドラーを集中管理します。
    /// </summary>
    public static class ErrorControllerReference
    {
        public static readonly ILogger ProcessNoneDefault = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_default_false_(" + System.Diagnostics.Process.GetCurrentProcess() + ")"), ".txt", false, false, false, null);

        /// <summary>
        /// ログを出せなかったときなど、致命的なエラーにも利用。
        /// </summary>
        public static readonly ILogger ProcessNoneError = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_エラー"), ".txt", true, false, false, null);

        /// <summary>
        /// 汎用ログ。千日手判定用。
        /// </summary>
        public static readonly ILogger PeocessNoneSennitite = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_Default_千日手判定"), ".txt", true, false, false, null);

        public static readonly ILogger ProcessServerDefault = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_ｻｰﾊﾞｰ_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false, false, null);
        /// <summary>
        /// 擬似将棋サーバーのログ。ログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly ILogger ProcessServerNetworkAsync = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_ｻｰﾊﾞｰ_非同期通信"), ".txt", true, true, false, null);

        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiDefault = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_CsharpGUI_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false, false, new ErrorControllerImpl());
        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiKifuYomitori = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_CsharpGUI_棋譜読取"), ".txt", true, false, false, null);
        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiNetwork = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_CsharpGUI_通信"), ".txt", true, true, false, null);
        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiPaint = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_CsharpGUI_ﾍﾟｲﾝﾄ"), ".txt", true, false, false, null);
        /// <summary>
        /// C# GUIのログ。
        /// </summary>
        public static readonly ILogger ProcessGuiSennitite = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_CsharpGui_千日手判定"), ".txt", true, false, false, null);

        /// <summary>
        /// AIMS GUIに対応する用のログ。
        /// </summary>
        public static readonly ILogger ProcessAimsDefault = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_AIMS対応用_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false, false, null);

        /// <summary>
        /// 将棋エンジンのログ。将棋エンジンきふわらべで汎用に使います。
        /// </summary>
        public static readonly ILogger ProcessEngineDefault = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_ｴﾝｼﾞﾝ_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false, false, new ErrorControllerImpl());

        /// <summary>
        /// 将棋エンジンのログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly ILogger ProcessEngineNetwork = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_ｴﾝｼﾞﾝ_通信"), ".txt", true, true, false, null);
        /// <summary>
        /// 将棋エンジンのログ。思考ルーチン専用です。
        /// </summary>
        public static readonly ILogger ProcessEngineSennitite = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_ｴﾝｼﾞﾝ_千日手判定"), ".txt", true, false, false, null);


        /// <summary>
        /// その他のログ。汎用。テスト・プログラム用。
        /// </summary>
        public static readonly ILogger ProcessTestProgramDefault = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_テスト・プログラム用（汎用）"), ".txt", true, false, false, null);

        /// <summary>
        /// その他のログ。棋譜学習ソフト用。
        /// </summary>
        public static readonly ILogger ProcessLearnerDefault = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_棋譜学習ソフト用"), ".txt", true, false, false, new ErrorControllerImpl());

        /// <summary>
        /// その他のログ。スピード計測ソフト用。
        /// </summary>
        public static readonly ILogger ProcessSpeedTestKeisoku = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_スピード計測ソフト用"), ".txt", true, false, false, null);

        /// <summary>
        /// その他のログ。ユニット・テスト用。
        /// </summary>
        public static readonly ILogger ProcessUnitTestDefault = new LoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_ユニットテスト用"), ".txt", true, false, true, new ErrorControllerImpl());



        /// <summary>
        /// ログファイルを削除します。(連番がなければ)
        /// 
        /// FIXME: アプリ起動後、ログが少し取られ始めたあとに削除が開始されることがあります。
        /// FIXME: 将棋エンジン起動時に、またログが削除されることがあります。
        /// </summary>
        public static void RemoveAllLogFiles()
        {
            try
            {
                string[] paths = Directory.GetFiles(Path.Combine(Application.StartupPath, Const_Filepath.m_EXE_TO_LOGGINGS));
                foreach (string path in paths)
                {
                    string name = Path.GetFileName(path);
                    if (name.StartsWith("_log_"))
                    {
                        string fullpath = Path.Combine(Application.StartupPath, Const_Filepath.m_EXE_TO_LOGGINGS, name);
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
