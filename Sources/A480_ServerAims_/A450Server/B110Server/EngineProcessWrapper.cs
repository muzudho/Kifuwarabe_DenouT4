using System.Diagnostics;
using Grayscale.A060Application.B110Log.C500Struct;

namespace Grayscale.A450Server.B110Server.C496EngineWrapper
{
    public delegate void DELEGATE_ShogiServer_ToEngine(string line, ILogger errH);

    /// <summary>
    /// 至将棋エンジン 通信インターフェース。
    /// </summary>
    public interface EngineProcessWrapper
    {
        DELEGATE_ShogiServer_ToEngine Delegate_ShogiServer_ToEngine { get; }
        void SetDelegate_ShogiServer_ToEngine(DELEGATE_ShogiServer_ToEngine delegateMethod);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// これが、将棋エンジン（プロセス）です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Process ShogiEngine { get; }
        void SetShogiEngine(Process shogiEngine);


        /// <summary>
        /// 将棋エンジンに、"noop from server"を送信します。
        /// </summary>
        void Send_Noop_from_server(ILogger errH);


        /// <summary>
        /// 将棋エンジンに、"usinewgame"を送信します。
        /// </summary>
        void Send_Usinewgame(ILogger errH);


        /// <summary>
        /// 将棋エンジンに、"usi"を送信します。
        /// </summary>
        void Send_Usi(ILogger errH);



        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        void Send_Shutdown(ILogger errH);



        /// <summary>
        /// 将棋エンジンに、"setoption ～略～"を送信します。
        /// </summary>
        void Send_Setoption(string setoption, ILogger errH);



        /// <summary>
        /// 将棋エンジンに、"quit"を送信します。
        /// </summary>
        void Send_Quit(ILogger errH);



        /// <summary>
        /// 将棋エンジンに、"position ～略～"を送信します。
        /// </summary>
        void Send_Position(string position, ILogger errH);



        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        void Send_Logdase(ILogger errH);



        /// <summary>
        /// 将棋エンジンに、"isready"を送信します。
        /// </summary>
        void Send_Isready(ILogger errH);



        /// <summary>
        /// 将棋エンジンが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        bool IsLive_ShogiEngine();



        /// <summary>
        /// 将棋エンジンに、"go"を送信します。
        /// </summary>
        void Send_Go(ILogger errH);



        /// <summary>
        /// 将棋エンジンに、"gameover lose"を送信します。
        /// </summary>
        void Send_Gameover_lose(ILogger errH);

    }
}
