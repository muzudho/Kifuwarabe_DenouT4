using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A450_Server_____.B110_Server_____.C___498_Server
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
