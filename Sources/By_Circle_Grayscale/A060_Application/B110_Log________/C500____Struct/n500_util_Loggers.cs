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
        public static readonly KwLogger DEFAULT_BY_PROCESS = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_default_false_(" + System.Diagnostics.Process.GetCurrentProcess() + ")"), ".txt", false, false);

        /// <summary>
        /// ログを出せなかったときなど、致命的なエラーにも利用。
        /// </summary>
        public static readonly KwLogger ERROR = new KwLoggerImpl( Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_エラー"), ".txt", true, false);




        #region 汎用ログ
        /// <summary>
        /// 千日手判定用。
        /// </summary>
        public static readonly KwLogger DEFAULT_SENNITITE = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_Default_千日手判定"), ".txt", true, false);
        #endregion



        #region 擬似将棋サーバーのログ
        public static readonly KwLogger SERVER_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｻｰﾊﾞｰ_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false);
        //public static readonly KwLogger SERVER_KIFU_YOMITORI = new KwLoggerImpl(new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｻｰﾊﾞｰ_棋譜読取"), ".txt", true, false));
        /// <summary>
        /// ログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly KwLogger SERVER_NETWORK_ASYNC = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｻｰﾊﾞｰ_非同期通信"), ".txt", true, true);
        #endregion


        #region C# GUIのログ
        public static readonly KwLogger CsharpGui_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGUI_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false);
        public static readonly KwLogger CsharpGui_KIFU_YOMITORI = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGUI_棋譜読取"), ".txt", true, false);
        public static readonly KwLogger CsharpGui_NETWORK = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGUI_通信"), ".txt", true, true);
        public static readonly KwLogger CsharpGui_PAINT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGUI_ﾍﾟｲﾝﾄ"), ".txt", true, false);
        public static readonly KwLogger CsharpGui_SENNITITE = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_CsharpGui_千日手判定"), ".txt", true, false);
        #endregion

        #region AIMS GUIに対応する用のログ
        public static readonly KwLogger AIMS_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_AIMS対応用_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false);
        #endregion


        #region 将棋エンジンのログ
        /// <summary>
        /// ログ。将棋エンジンきふわらべで汎用に使います。
        /// </summary>
        public static readonly KwLogger ENGINE_DEFAULT = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｴﾝｼﾞﾝ_ﾃﾞﾌｫﾙﾄ"), ".txt", true, false);

        /// <summary>
        /// ログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly KwLogger ENGINE_NETWORK = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｴﾝｼﾞﾝ_通信"), ".txt", true, true);
        /// <summary>
        /// ログ。思考ルーチン専用です。
        /// </summary>
        public static readonly KwLogger ENGINE_MOUSOU_RIREKI = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｴﾝｼﾞﾝ_妄想履歴"), ".txt", true, false);
        public static readonly KwLogger ENGINE_SENNITITE = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_ｴﾝｼﾞﾝ_千日手判定"), ".txt", true, false);
        #endregion


        #region その他のログ

        /// <summary>
        /// 汎用。テスト・プログラム用。
        /// </summary>
        public static readonly KwLogger TEST_PROGRAM = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_テスト・プログラム用（汎用）"), ".txt", true, false);

        /// <summary>
        /// 棋譜学習ソフト用。
        /// </summary>
        public static readonly KwLogger LEARNER = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_棋譜学習ソフト用"), ".txt", true, false);
        public static readonly KwDisplayer LEARNER_D = new KwDisplayerImpl();

        /// <summary>
        /// スピード計測ソフト用。
        /// </summary>
        public static readonly KwLogger SPEED_KEISOKU = new KwLoggerImpl(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log_スピード計測ソフト用"), ".txt", true, false);
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
                //string filepath2 = Path.Combine(Application.StartupPath, this.DefaultFile.FileName);
                //System.IO.File.Delete(filepath2);

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
            catch (Exception ex) { Util_Loggers.ERROR.DonimoNaranAkirameta(ex, "ﾛｸﾞﾌｧｲﾙ削除中☆"); throw ex; }
        }


    }
}
