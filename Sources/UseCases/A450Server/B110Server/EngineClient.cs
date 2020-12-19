using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A450Server.B110Server.C125Receiver;
using Grayscale.A450Server.B110Server.C496EngineWrapper;

namespace Grayscale.A450Server.B110Server.C497EngineClient
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
        IReceiver Receiver { get; }


        void Start(string shogiEngineFilePath);

        /// <summary>
        /// 将棋エンジンのプロセスです。
        /// </summary>
        EngineProcessWrapper ShogiEngineProcessWrapper { get; set; }

        /// <summary>
        /// 手番が変わったときに、実行する処理をここに書いてください。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="logTag"></param>
        void OnChangedTurn(
            Earth earth,

            //MoveEx curNode,
            Tree kifu1,

            Playerside kaisiPside,
            ILogTag logTag);

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        void Send_Shutdown(ILogTag logTag);

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        void Send_Logdase(ILogTag logTag);

    }
}
