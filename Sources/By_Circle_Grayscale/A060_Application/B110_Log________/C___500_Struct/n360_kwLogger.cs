using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using System;

namespace Grayscale.A060_Application.B110_Log________.C___500_Struct
{
    /// <summary>
    /// ロガー、エラーハンドリング
    /// 
    /// きふわらべのロガー。
    /// </summary>
    public interface KwLogger
    {


        KwDisplayer KwDisplayer_OrNull { get; set; }


        /// <summary>
        /// ファイル名
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// 拡張子抜きファイル名（without extension）
        /// </summary>
        string FileNameWoe { get; }
        /// <summary>
        /// 拡張子
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        bool Enable { get; }

        /// <summary>
        /// タイムスタンプを出力するか。
        /// </summary>
        bool Print_TimeStamp { get; }


        /// <summary>
        /// ログを蓄えます。改行なし。
        /// </summary>
        /// <param name="token"></param>
        void Append(string token);
        /// <summary>
        /// ログを蓄えます。改行付き。
        /// </summary>
        /// <param name="line"></param>
        void AppendLine(string line);

        /// <summary>
        /// テキストを、ログ・ファイルの末尾に追記します。
        /// </summary>
        /// <param name="logTypes"></param>
        void Flush(LogTypes logTypes);

        /// <summary>
        /// 「どうにもならん、あきらめた」
        /// 
        /// 例外が発生したが、対応できないのでログだけ出して例外をさらに上へ投げ返すとき。
        /// </summary>
        /// <param name="okottaBasho"></param>
        void DonimoNaranAkirameta(string okottaBasho);
        void ShowDialog(string okottaBasho);


        /// <summary>
        /// 「どうにもならん、あきらめた」
        /// 
        /// 例外が発生したが、対応できないのでログだけ出して例外をさらに上へ投げ返すとき。
        /// </summary>
        /// <param name="okottaBasho"></param>
        void DonimoNaranAkirameta(Exception ex, string okottaBasho);

    }
}
