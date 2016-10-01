using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___125_Receiver;
using Grayscale.A450_Server_____.B110_Server_____.C___496_EngineWrapper;

namespace Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient
{

    /// <summary>
    /// 将棋エンジン クライアント。
    /// TODO: MainGui に統合したい。
    /// </summary>
    public interface EngineClient
    {
        object Owner_Server { get; }//Server型
        void SetOwner_Server(object owner_Server);//Server型

        /// <summary>
        /// レシーバー
        /// </summary>
        Receiver Receiver { get; }


        void Start(string shogiEngineFilePath);

        /// <summary>
        /// 将棋エンジンのプロセスです。
        /// </summary>
        EngineProcessWrapper ShogiEngineProcessWrapper { get; set; }

        /// <summary>
        /// 手番が変わったときに、実行する処理をここに書いてください。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="errH"></param>
        void OnChangedTurn(
            Earth earth,

            //MoveEx curNode,
            Tree kifu1,

            Playerside kaisiPside,
            KwLogger errH);

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        void Send_Shutdown(KwLogger errH);

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        void Send_Logdase(KwLogger errH);

    }
}
