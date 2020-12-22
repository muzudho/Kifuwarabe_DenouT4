using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.A450Server.B110Server.C497EngineClient;

namespace Grayscale.A450Server.B110Server.C498Server
{
    /// <summary>
    /// 将棋サーバー。
    /// </summary>
    public interface Server
    {
        /// <summary>
        /// 将棋エンジン。
        /// </summary>
        EngineClient EngineClient { get; }

        /// <summary>
        /// 将棋エンジンからの入力文字列（入力欄に入ったもの）を、一旦　蓄えたもの。
        /// </summary>
        string InputString99 { get; }
        void AddInputString99(string inputString99);
        void SetInputString99(string inputString99);
        void ClearInputString99();


        Earth Earth { get; }
        Tree KifuTree { get; }
    }
}
