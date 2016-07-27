using Grayscale.P571_KifuWarabe_.L___490_Option__;
using System.Collections.Generic;

namespace Grayscale.P542_Scoreing___.L___005_UsiLoop
{
    public interface ShogiEngine
    {
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        void Send(string line);


        /// <summary>
        /// きふわらべの作者名です。
        /// </summary>
        string AuthorName { get; }

        /// <summary>
        /// 製品名です。
        /// </summary>
        string SeihinName { get; }

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        EngineOptions EngineOptions { get; set; }
    }
}
