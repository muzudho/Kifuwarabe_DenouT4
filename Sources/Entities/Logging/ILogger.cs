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

        /// <summary>
        /// 「どうにもならん、あきらめた」
        /// 
        /// 例外が発生したが、対応できないのでログだけ出して例外をさらに上へ投げ返すとき。
        /// </summary>
        /// <param name="okottaBasho"></param>
        void Panic(string okottaBasho);
        void ShowDialog(string okottaBasho);


        /// <summary>
        /// 「どうにもならん、あきらめた」
        /// 
        /// 例外が発生したが、対応できないのでログだけ出して例外をさらに上へ投げ返すとき。
        /// </summary>
        /// <param name="okottaBasho"></param>
        void Panic(Exception ex, string okottaBasho);

    }
}
