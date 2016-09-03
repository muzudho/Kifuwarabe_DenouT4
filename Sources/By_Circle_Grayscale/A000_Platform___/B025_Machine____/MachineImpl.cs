//
// 将棋きふわらべ
//
//using Grayscale.Kifuwarabe_Igo_Unity_Think.n___070_core____; //Folderpath,Logger
using System.Diagnostics;
using System.IO;
using System.Text;  // TODO: Unityで使うなら、このusing文は消そうぜ☆（＾▽＾）
using System.Windows.Forms; // TODO: Unityで使うなら、このusing文は消そうぜ☆（＾▽＾）

namespace Grayscale.A000_Platform___.B025_Machine____
{
    /// <summary>
    /// デバッグ・モードでのみ実行する関数のかたちです。引数の無い隠し実験。
    /// </summary>
    public delegate void KakushiJikken();

    /// <summary>
    /// Unity で使えない関数をここへ投げた☆（＾▽＾）
    /// ここを空っぽ機能にすれば Unity でも動くんじゃないか☆（＾▽＾）
    /// </summary>
    public class MachineImpl
    {

        private static MachineImpl m_infoMachine_;
        /// <summary>
        /// これは、中身を空にしてはいけないぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static MachineImpl GetInstance()
        {
            // このメソッドの中身は残しておくこと。
            if (null == MachineImpl.m_infoMachine_)
            {
                MachineImpl.m_infoMachine_ = new MachineImpl();
            }
            return MachineImpl.m_infoMachine_;
        }
        private MachineImpl()
        {
            // このメソッドの中身は、残すところと、消せるところがあるぜ☆

            //this.m_log_ = "";
            this.m_log_ = new StringBuilder();// TODO: Unityで使うなら、文字列型にしてしまおうぜ☆（＾▽＾）

            {// TODO: Unityで使うなら、この個所は消そうぜ☆（＾▽＾）
                // ログ・ファイルがあれば削除します。
                if (File.Exists(this.m_logPath_))
                {
                    File.Delete(this.m_logPath_);
                }
            }
        }


        private string m_logPath_ = "_log_message.txt";// TODO: Unityで使うなら、このプロパティ―は消そうぜ☆（＾▽＾）

        /*
        /// <summary>
        /// 指定のフォルダーへのパスを返す。
        /// 無理なら ヌルを返しておくこと。
        /// </summary>
        /// <returns></returns>
        public string GetFolderpath_OrNull(Folderpath folderpath)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消して、ヌルを返そうぜ☆（＾▽＾）
            //return null;

            switch (folderpath)
            {
                case Folderpath.Config:
                    return System.IO.Path.Combine(Application.StartupPath, "config");
                case Folderpath.TerritoryCalculationTest:
                    return System.IO.Path.Combine(Application.StartupPath, "config/TerritoryCalculationTest");
                default:
                    // エラー
                    string message = "フォルダー指定がおかしい(A)=" + folderpath.ToString();
                    Debug.Fail(message);
                    throw new System.Exception(message);
            }
        }
        */
        /*
        /// <summary>
        /// 指定のファイルへのパスを返す。
        /// 無理なら ヌルを返しておくこと。
        /// </summary>
        /// <param name="folderpath"></param>
        /// <param name="fileNameWithoutExtension">例えば「ji1」を想定だが、「ji1.txt」でもOKとする。</param>
        /// <param name="dotExtension">例えば「.txt」や、空文字列。</param>
        /// <returns></returns>
        public string GetFilepath_OrNull(Folderpath folderpath, string fileNameWithoutExtension, string dotExtension)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消して、ヌルを返そうぜ☆（＾▽＾）
            //return null;

            switch (folderpath)
            {
                case Folderpath.Config:
                    return System.IO.Path.Combine(Application.StartupPath, "config", fileNameWithoutExtension +
                        dotExtension//".txt"
                        );
                case Folderpath.TerritoryCalculationTest:
                    return System.IO.Path.Combine(Application.StartupPath, "config/TerritoryCalculationTest", fileNameWithoutExtension +
                        dotExtension//""
                        );
                default:
                    // エラー
                    string message = "フォルダー指定がおかしい(A)=" + folderpath.ToString();
                    Debug.Fail(message);
                    throw new System.Exception(message);
            }
        }
        */

        /*
        /// <summary>
        /// 指定のフォルダーに含まれるファイルへのパスを返す。
        /// 無理なら ヌルを返しておくこと。
        /// </summary>
        /// <returns></returns>
        public string[] GetFilepathInFolder_OrNull(Folderpath folderpath)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消して、ヌルを返そうぜ☆（＾▽＾）
            //return null;

            switch (folderpath)
            {
                case Folderpath.Config:
                    return System.IO.Directory.GetFiles(System.IO.Path.Combine(Application.StartupPath, "config"));
                case Folderpath.TerritoryCalculationTest:
                    return System.IO.Directory.GetFiles(System.IO.Path.Combine(Application.StartupPath, "config/TerritoryCalculationTest"));
                default:
                    // エラー
                    string message = "フォルダー指定がおかしい(A)=" + folderpath.ToString();
                    Debug.Fail(message);
                    throw new System.Exception(message);
            }
        }
        */

        /*
        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        /// <param name="logger"></param>
        public void Flush10(Logger logger)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消そうぜ☆（＾▽＾）

            string message = logger.ToString();

            if (0 < message.Length)
            {
                System.Console.Write(message);

                if (!File.Exists(this.m_logPath_))
                {
                    // ログファイルが無ければ作成します。
                    FileStream fs = File.Create(this.m_logPath_);
                    fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                }

                File.AppendAllText(this.m_logPath_, message);
            }

            logger.Clear();
        }
        */

        /*
        /// <summary>
        /// デバッグ・ウィンドウのタイトル。
        /// </summary>
        /// <param name="title"></param>
        public void SetInformationWindowTitle30(string title, Logger logger)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消そうぜ☆（＾▽＾）

            System.Console.Title = title;

            logger.AppendLine(title);
            this.Flush10(logger);
        }
        */

        /// <summary>
        /// デバッグ・ウィンドウからのキー入力
        /// </summary>
        /// <returns></returns>
        public char ReadKey()
        {
            // TODO: Unityで使うなら、このメソッドの中身は消して、ダミー値を返そうぜ☆（＾▽＾）
            //return ' ';

            System.ConsoleKeyInfo keyInfo = System.Console.ReadKey();
            return keyInfo.KeyChar;
        }

        /// <summary>
        /// デバッグ・ウィンドウからの行入力
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            // TODO: Unityで使うなら、このメソッドの中身は消して、ダミー値を返そうぜ☆（＾▽＾）
            //return "";

            return System.Console.ReadLine();
        }

        /*
        /// <summary>
        /// デバッグ・モード用診断。
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="logger">出力テキストのバッファー。</param>
        /// <param name="message"></param>
        [Conditional("DEBUG")]
        public void Assert(bool condition, Logger logger, string message)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消そうぜ☆（＾▽＾）

            if (!condition)
            {
                logger.Append(message);
                string message2 = logger.ToString();
                MachineImpl.GetInstance().Flush10(logger);
                Debug.Assert(condition, message2);
            }
        }
        */

        /// <summary>
        /// デバッグ・モードでのみ実行するプログラムを書くものです。
        /// </summary>
        /// <param name="logger"></param>
        [Conditional("DEBUG")]
        public void KakushiJikken(KakushiJikken kakushiJikken)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消そうぜ☆（＾▽＾）

            kakushiJikken();
        }

        /*
        /// <summary>
        /// デバッグ・モード用診断。
        /// </summary>
        /// <param name="logger"></param>
        [Conditional("DEBUG")]
        public void Fail(Logger logger)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消そうぜ☆（＾▽＾）

            string message = logger.ToString();
            MachineImpl.GetInstance().Flush10(logger);
            Debug.Fail(message);
            throw new System.Exception(message);
        }
        */



        //private string m_log_;
        private StringBuilder m_log_;// TODO: Unityで使うなら、文字列型にしてしまおうぜ☆（＾▽＾）
        /// <summary>
        /// ログ文字列。
        /// </summary>
        public string Log_ToString()
        {
            //return this.m_log_;
            return this.m_log_.ToString();// TODO: Unityで使うなら、文字列型にしてしまおうぜ☆（＾▽＾）
        }
        public void Log_Clear()
        {
            //this.m_log_ = "";
            this.m_log_.Clear();// TODO: Unityで使うなら、文字列型にしてしまおうぜ☆（＾▽＾）
        }
        public int Log_GetLogLength()
        {
            return this.m_log_.Length;
        }
        public void Log_AppendLine()
        {
            //this.m_log_ += "\n";
            this.m_log_.AppendLine();// TODO: Unityで使うなら、文字列型にしてしまおうぜ☆（＾▽＾）
        }
        public void Log_AppendLine(string message)
        {
            //this.m_log_ += message+"\n";
            this.m_log_.AppendLine(message);// TODO: Unityで使うなら、文字列型にしてしまおうぜ☆（＾▽＾）
        }
        public void Log_Append(string message)
        {
            //this.m_log_ += message;
            this.m_log_.Append(message);// TODO: Unityで使うなら、文字列型にしてしまおうぜ☆（＾▽＾）
        }
    }
}
