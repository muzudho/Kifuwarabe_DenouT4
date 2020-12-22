using System;

namespace Grayscale.Kifuwaragyoku.Entities.Logging
{
    /// <summary>
    /// ロガー。
    /// </summary>
    public interface ILogger
    {
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

        void ShowDialog(string okottaBasho);
    }
}
