using Grayscale.A060Application.B210Tushin.C500Util;

namespace Grayscale.Kifuwaragyoku.UseCases
{
    public class Playing
    {
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        public static void Send(string line)
        {
            // 将棋サーバーに向かってメッセージを送り出します。
            Util_Message.Upload(line);

#if DEBUG
            // 送信記録をつけます。
            Util_Loggers.ProcessEngine_NETWORK.AppendLine(line);
            Util_Loggers.ProcessEngine_NETWORK.Flush(LogTypes.ToServer);
#endif
        }
    }
}
