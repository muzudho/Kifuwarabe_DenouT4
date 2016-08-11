﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;//FIXME:
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

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
        /// ************************************************************************************************************************
        /// メモを、ログ・ファイルの末尾に追記します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_AddMemo(
            string line
            )
        {
            bool enable = this.Enable;
            string filepath2 = Path.Combine( Application.StartupPath, this.FileName);

            if (!enable)
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
                    sb.Append(" : ");
                }
                else
                {
                    sb.Append("Memo:   ");
                }

                sb.Append(line);
                sb.AppendLine();

                System.IO.File.AppendAllText(filepath2, sb.ToString());
            }
            catch (Exception ex) {
                Util_OwataMinister.ERROR.DonimoNaranAkirameta(ex, "ログ中☆");
                // ログ出力に失敗しても、続行します。
            }

        gt_EndMethod:
            ;
        }





        /// <summary>
        /// ************************************************************************************************************************
        /// エラーを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_Error(
            string line
            )
        {
            bool enable = this.Enable;
            bool printTimestamp = this.Print_TimeStamp;
            string filepath2 = Path.Combine(Application.StartupPath, this.FileName);

            if (!enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Error:   ");

                // タイムスタンプ
                if (printTimestamp)
                {
                    sb.Append(DateTime.Now.ToString());
                    sb.Append(" : ");
                }

                sb.Append(line);
                sb.AppendLine();

                string message = sb.ToString();
                MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                System.IO.File.AppendAllText(filepath2, message);
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = "Util_Log#WriteLine_Error：" + ex.Message;
                System.IO.File.AppendAllText(Const_Filepath.m_EXE_TO_LOGGINGS + "_log_致命的ｴﾗｰ.txt", message);
            }

        gt_EndMethod:
            ;
        }









        /// <summary>
        /// ************************************************************************************************************************
        /// メモを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_OverMemo(
            string line
            )
        {
            bool enable = this.Enable;
            string filepath2 = Path.Combine(Application.StartupPath, this.FileName);

            if (!enable)
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
                    sb.Append(" : ");
                }
                else
                {
                    sb.Append("Memo:   ");
                }

                sb.Append(line);
                sb.AppendLine();

                System.IO.File.WriteAllText(filepath2, sb.ToString());
            }
            catch (Exception ex) { Util_OwataMinister.ERROR.DonimoNaranAkirameta(ex, "ログ取り中☆"); throw ex; }

        gt_EndMethod:
            ;
        }








        /// <summary>
        /// ************************************************************************************************************************
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_S(
            string line
            //,
            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            bool enable = this.Enable;
            bool print_TimeStamp = this.Print_TimeStamp;
            string filepath2 = Path.Combine(Application.StartupPath, this.FileName);

            if (!enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString());
                sb.Append("<   ");
                sb.Append(line);
                //sb.Append("：");
                //sb.Append(memberName);
                //sb.Append("：");
                //sb.Append(sourceFilePath);
                //sb.Append("：");
                //sb.Append(sourceLineNumber);
                sb.AppendLine();

                System.IO.File.AppendAllText(filepath2, sb.ToString());
            }
            catch (Exception ex) { Util_OwataMinister.ERROR.DonimoNaranAkirameta(ex, "ログ取り中☆"); }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_C(
            string line
            //,
            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            bool enable = this.Enable;
            bool print_TimeStamp = this.Print_TimeStamp;
            string filepath2 = Path.Combine(Application.StartupPath, this.FileName);

            if (!enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString());
                sb.Append("  > ");
                sb.Append(line);
                //sb.Append("：");
                //sb.Append(memberName);
                //sb.Append("：");
                //sb.Append(sourceFilePath);
                //sb.Append("：");
                //sb.Append(sourceLineNumber);
                sb.AppendLine();

                System.IO.File.AppendAllText(filepath2, sb.ToString());
            }
            catch (Exception ex) { Util_OwataMinister.ERROR.DonimoNaranAkirameta(ex, "ログ取り中☆"); throw ex; }

        gt_EndMethod:
            ;
        }

    }


}
