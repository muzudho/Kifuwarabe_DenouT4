using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;//FIXME:
using System;
using System.IO;
using System.Windows.Forms;

namespace Grayscale.A060_Application.B110_Log________.C500____Struct
{


    /// <summary>
    /// ロガー、エラー・ハンドラーを集中管理します。
    /// </summary>
    public class Util_Loggers
    {
        public static readonly KwLogger ProcessNone_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_default_false_(" + System.Diagnostics.Process.GetCurrentProcess() + ")"), ".txt", false, false, false, null);

        /// <summary>
        /// ログを出せなかったときなど、致命的なエラーにも利用。
        /// </summary>
        public static readonly KwLogger ProcessNone_ERROR = new KwLoggerImpl( Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_エラー"), ".txt", true, false, false, null);




        #region 汎用ログ
        /// <summary>
        /// 千日手判定用。
        /// </summary>
        public static readonly KwLogger PeocessNone_SENNITITE = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_Default_千日手判定"), ".txt", true, false, false, null);
        #endregion



        #region 擬似将棋サーバーのログ
        public static readonly KwLogger ProcessServer_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｻｰﾊﾞｰ_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false, false, null);
        /// <summary>
        /// ログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly KwLogger ProcessServer_NETWORK_ASYNC = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｻｰﾊﾞｰ_非同期通信"), ".txt", true, true, false, null);
        #endregion


        #region C# GUIのログ
        public static readonly KwLogger ProcessGui_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGUI_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false, false, new KwDisplayerImpl());
        public static readonly KwLogger ProcessGui_KIFU_YOMITORI = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGUI_棋譜読取"), ".txt", true, false, false, null);
        public static readonly KwLogger ProcessGui_NETWORK = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGUI_通信"), ".txt", true, true, false, null);
        public static readonly KwLogger ProcessGui_PAINT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGUI_ﾍﾟｲﾝﾄ"), ".txt", true, false, false, null);
        public static readonly KwLogger ProcessGui_SENNITITE = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGui_千日手判定"), ".txt", true, false, false, null);
        #endregion

        #region AIMS GUIに対応する用のログ
        public static readonly KwLogger ProcessAims_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_AIMS対応用_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false, false, null);
        #endregion


        #region 将棋エンジンのログ
        /// <summary>
        /// ログ。将棋エンジンきふわらべで汎用に使います。
        /// </summary>
        public static readonly KwLogger ProcessEngine_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｴﾝｼﾞﾝ_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false, false, new KwDisplayerImpl());

        /// <summary>
        /// ログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly KwLogger ProcessEngine_NETWORK = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｴﾝｼﾞﾝ_通信"), ".txt", true, true, false, null);
        /// <summary>
        /// ログ。思考ルーチン専用です。
        /// </summary>
        public static readonly KwLogger ProcessEngine_SENNITITE = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｴﾝｼﾞﾝ_千日手判定"), ".txt", true, false, false, null);
        #endregion


        #region その他のログ

        /// <summary>
        /// 汎用。テスト・プログラム用。
        /// </summary>
        public static readonly KwLogger ProcessTestProgram_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_テスト・プログラム用（汎用）"), ".txt", true, false, false, null);

        /// <summary>
        /// 棋譜学習ソフト用。
        /// </summary>
        public static readonly KwLogger ProcessLearner_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_棋譜学習ソフト用"), ".txt", true, false, false, new KwDisplayerImpl());

        /// <summary>
        /// スピード計測ソフト用。
        /// </summary>
        public static readonly KwLogger ProcessSpeedTest_KEISOKU = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_スピード計測ソフト用"), ".txt", true, false, false, null);

        /// <summary>
        /// ユニット・テスト用。
        /// </summary>
        public static readonly KwLogger ProcessUnitTest_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_ユニットテスト用"), ".txt", true, false, true, new KwDisplayerImpl());
        #endregion



        /// <summary>
        /// ************************************************************************************************************************
        /// ログファイルを削除します。(連番がなければ)
        /// ************************************************************************************************************************
        /// 
        /// FIXME: アプリ起動後、ログが少し取られ始めたあとに削除が開始されることがあります。
        /// FIXME: 将棋エンジン起動時に、またログが削除されることがあります。
        /// </summary>
        public static void Remove_AllLogFiles()
        {
            try
            {
                string[] paths = Directory.GetFiles(Path.Combine(Application.StartupPath, Const_Filepath.m_EXE_TO_LOGGINGS));
                foreach(string path in paths)
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
            catch (Exception ex) {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "ﾛｸﾞﾌｧｲﾙ削除中☆");
                throw ex;
            }
        }


    }
}
