using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;//FIXME:
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using System.Diagnostics;

namespace Grayscale.A060_Application.B110_Log________.C500____Struct
{


    /// <summary>
    /// 継承できる列挙型として利用☆
    /// 
    /// きふわらべのロガー。
    /// </summary>
    public class KwLoggerImpl : KwLogger
    {

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="fileNameWoe">拡張子抜きのファイル名。(with out extension)</param>
        /// <param name="extension">ドット付き拡張子。(with dot)</param>
        /// <param name="enable">ログ出力の有無</param>
        /// <param name="print_TimeStamp">タイムスタンプ出力のON/OFF</param>
        public KwLoggerImpl(string fileNameWoe, string extension, bool enable, bool print_TimeStamp, bool enableConsole, KwDisplayer kwDisplayer_OrNull)
        {
            this.fileNameWoe = fileNameWoe;
            this.extension = extension;
            this.enable = enable;
            this.print_TimeStamp = print_TimeStamp;
            this.EnableConsole = enableConsole;
            this.m_buffer_ = new StringBuilder();
            this.KwDisplayer_OrNull = kwDisplayer_OrNull;
        }


        public KwDisplayer KwDisplayer_OrNull { get; set; }


        private StringBuilder m_buffer_;

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get { return this.FileNameWoe + this.Extension; } }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileNameWoe { get { return this.fileNameWoe; } }
        private string fileNameWoe;

        /// <summary>
        /// 拡張子
        /// </summary>
        public string Extension { get { return this.extension; } }
        private string extension;

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        public bool Enable { get { return this.enable; } }
        private bool enable;


        /// <summary>
        /// タイムスタンプ出力の有無。
        /// </summary>
        public bool Print_TimeStamp { get { return this.print_TimeStamp; } }
        private bool print_TimeStamp;

        /// <summary>
        /// コンソール出力の有無。
        /// </summary>
        public bool EnableConsole { get; set; }

        /// <summary>
        /// Equalsをオーバーライドしたので、このメソッドのオーバーライドも必要になります。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            KwLogger p = obj as KwLogger;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.FileNameWoe+this.Extension == p.FileNameWoe+p.Extension);
        }



        /// <summary>
        /// ログを蓄えます。改行なし。
        /// </summary>
        /// <param name="token"></param>
        public void Append(string token)
        {
            if (!this.Enable)
            {
                // ログ出力オフ
                return;
            }

            // ログ追記 TODO:非同期
            try
            {
                this.m_buffer_.AppendLine(token);
            }
            catch (Exception ex)
            {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "ログ中☆");
                // ログ出力に失敗しても、続行します。
            }
        }
        /// <summary>
        /// ログを蓄えます。改行付き。
        /// </summary>
        /// <param name="line"></param>
        public void AppendLine(string line)
        {
            if (!this.Enable)
            {
                // ログ出力オフ
                return;
            }

            // ログ追記 TODO:非同期
            try
            {
                this.m_buffer_.AppendLine(line);
            }
            catch (Exception ex)
            {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "ログ中☆");
                // ログ出力に失敗しても、続行します。
            }
        }

        /// <summary>
        /// テキストを、ログ・ファイルの末尾に追記します。
        /// </summary>
        /// <param name="logTypes"></param>
        public void Flush(LogTypes logTypes)
        {
            if (!this.Enable)
            {
                // ログ出力オフ
                return;
            }

            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (this.Print_TimeStamp)
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

                sb.Append(this.m_buffer_.ToString());
                string message = sb.ToString();
                this.m_buffer_.Clear();

                if (logTypes == LogTypes.Error)
                {
                    MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string filepath2 = Path.Combine(Application.StartupPath, this.FileName);
                System.IO.File.AppendAllText(filepath2, message);

                if (this.EnableConsole)
                {
                    System.Console.Write(message);
                }
            }
            catch (Exception ex)
            {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "ログ中☆");
                // ログ出力に失敗しても、続行します。
            }
        }

        /// <summary>
        /// 「どうにもならん、あきらめた」
        /// 
        /// 例外が発生したが、対応できないのでログだけ出します。
        /// デバッグ時は、ダイアログボックスを出します。
        /// </summary>
        /// <param name="message1"></param>
        public void DonimoNaranAkirameta(string message1)
        {
            //>>>>> エラーが起こりました。
            string message2 = "エラー：" + message1;
            Debug.Fail(message2);

            // どうにもできないので  ログだけ取って、上に投げます。
            this.AppendLine(message2);
            this.Flush(LogTypes.Error);
            // ログ出力に失敗することがありますが、無視します。
        }
        public void ShowDialog(string message)
        {
            this.AppendLine(message);
            MessageBox.Show(message);
            this.Flush(LogTypes.Plain);
            // ログ出力に失敗することがありますが、無視します。
        }

        /// <summary>
        /// 「どうにもならん、あきらめた」
        /// 
        /// 例外が発生したが、対応できないのでログだけ出します。
        /// デバッグ時は、ダイアログボックスを出します。
        /// </summary>
        /// <param name="okottaBasho"></param>
        public void DonimoNaranAkirameta(Exception ex, string okottaBasho)
        {
            //>>>>> エラーが起こりました。
            string message = ex.GetType().Name + " " + ex.Message + "：" + okottaBasho;
            Debug.Fail(message);

            // どうにもできないので  ログだけ取って、上に投げます。
            this.AppendLine(message);
            this.Flush(LogTypes.Error);
            // ログ出力に失敗することがありますが、無視します。
        }

    }
}
