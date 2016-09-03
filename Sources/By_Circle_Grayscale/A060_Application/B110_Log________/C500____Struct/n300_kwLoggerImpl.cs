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
        /// コンストラクター。
        /// </summary>
        /// <param name="fileNameWoe">拡張子抜きのファイル名。(with out extension)</param>
        /// <param name="extension">ドット付き拡張子。(with dot)</param>
        /// <param name="enable">ログ出力の有無</param>
        /// <param name="print_TimeStamp">タイムスタンプ出力のON/OFF</param>
        public KwLoggerImpl(string fileNameWoe, string extension, bool enable, bool print_TimeStamp)
        {
            this.fileNameWoe = fileNameWoe;
            this.extension = extension;
            this.enable = enable;
            this.print_TimeStamp = print_TimeStamp;
        }


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
        /// テキストを、ログ・ファイルの末尾に追記します。改行付き。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine(
            string line,
            LogTypes logTypes
            )
        {
            if (!this.Enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
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
                    //メモを、ログ・ファイルの末尾に追記します。
                    case LogTypes.Memo:
                        sb.Append("Memo: ");
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

                sb.Append(line);
                sb.AppendLine();

                string message = sb.ToString();

                if (logTypes==LogTypes.Error)
                {
                    MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string filepath2 = Path.Combine(Application.StartupPath, this.FileName);
                System.IO.File.AppendAllText(filepath2, message);
            }
            catch (Exception ex)
            {
                Util_Loggers.ERROR.DonimoNaranAkirameta(ex, "ログ中☆");
                // ログ出力に失敗しても、続行します。

                ////>>>>> エラーが起こりました。
                //
                //// どうにもできないので  ログだけ取って　無視します。
                //string message = "Util_Log#WriteLine_Error：" + ex.Message;
                //System.IO.File.AppendAllText(Const_Filepath.m_EXE_TO_LOGGINGS + "_log_致命的ｴﾗｰ.txt", message);
            }

            gt_EndMethod:
            ;
        }

        /// <summary>
        /// 「どうにもならん、あきらめた」
        /// 
        /// 例外が発生したが、対応できないのでログだけ出します。
        /// デバッグ時は、ダイアログボックスを出します。
        /// </summary>
        /// <param name="okottaBasho"></param>
        public void DonimoNaranAkirameta(string okottaBasho)
        {
            //>>>>> エラーが起こりました。
            string message = "起こった場所：" + okottaBasho;
            Debug.Fail(message);

            // どうにもできないので  ログだけ取って、上に投げます。
            this.WriteLine(message, LogTypes.Error);
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
            this.WriteLine(message, LogTypes.Error);
            // ログ出力に失敗することがありますが、無視します。
        }

    }
}
