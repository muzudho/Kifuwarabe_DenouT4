﻿using System.Diagnostics;

#if DEBUG
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    /// <summary>
    /// C# GUI のコンピューター対戦用。受信機能。
    /// </summary>
    public class ReceiverForCsharpVsImpl : IReceiver
    {

        #region プロパティー

        /// <summary>
        /// 将棋エンジンを掴んでいるオブジェクトです。
        /// </summary>
        public object Owner_EngineClient { get { return this.owner_EngineClient; } }//EngineClient型
        public void SetOwner_EngineClient(object owner_EngineClient)//EngineClient型
        {
            this.owner_EngineClient = owner_EngineClient;
        }
        private object owner_EngineClient;//EngineClient型

        #endregion

        /// <summary>
        /// デフォルト・コンストラクター。
        /// </summary>
        /// <param name="ownerServer"></param>
        /// <param name="shogiEngineProcessWrapper"></param>
        public ReceiverForCsharpVsImpl()
        {
            // 生成後に、SetOwner_EngineClient( ) を使って設定してください。
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンから、データを非同期受信(*1)します。
        /// ************************************************************************************************************************
        /// 
        ///         *1…こっちの都合に合わせず、データが飛んできます。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnListenUpload_Async(object sender, DataReceivedEventArgs e)
        {
            string line = e.Data;

            if (null == line)
            {
                // 無視
            }
            else
            {
                //>>>>>>>>>> メッセージを受け取りました。
#if DEBUG
                logTag.AppendLine(line);
                logTag.Flush(LogTypes.ToServer);
#endif

                if ("noop" == line)
                {
                    //------------------------------------------------------------
                    // この部分は成功してないので、役に立っていないはず。
                    //------------------------------------------------------------

                    // noop を受け取ったぜ☆！

                    // すぐに返すと受け取れないので、数秒開けます。
                    System.Threading.Thread.Sleep(3000);

                    ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Noop_from_server();
                }
                else if (line.StartsWith("option"))
                {

                }
                else if ("usiok" == line)
                {
                    //------------------------------------------------------------
                    // 将棋サーバーへ： setoption
                    //------------------------------------------------------------

                    // 「私は将棋サーバーですが、USIプロトコルのponderコマンドには対応していませんので、送ってこないでください」
                    ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Setoption("setoption name USI_Ponder value false");

                    // 将棋エンジンへ：　「私は将棋サーバーです。noop コマンドを送ってくれば、すぐに ok コマンドを返します。1分間を空けてください」
                    ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Setoption("setoption name noopable value true");

                    //------------------------------------------------------------
                    // 「準備はいいですか？」
                    //------------------------------------------------------------
                    ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Isready();
                }
                else if ("readyok" == line)
                {

                    //------------------------------------------------------------
                    // 対局開始！
                    //------------------------------------------------------------
                    ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Usinewgame();

                }
                else if (line.StartsWith("info"))
                {
                }
                else if (line.StartsWith("bestmove resign"))
                {
                    // 将棋エンジンが、投了されました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    //------------------------------------------------------------
                    // あなたの負けです☆
                    //------------------------------------------------------------
                    ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Gameover_lose();

                    //------------------------------------------------------------
                    // 将棋エンジンを終了してください☆
                    //------------------------------------------------------------
                    ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Quit();
                }
                else if (line.StartsWith("bestmove"))
                {
                    // 将棋エンジンが、手を指されました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    ((Server)((EngineClient)this.Owner_EngineClient).Owner_Server).AddInputString99(
                        line.Substring("bestmove".Length + "".Length)
                        );

#if DEBUG
                    logTag.AppendLine("USI受信：bestmove input99=[" + ((Server)((EngineClient)this.Owner_EngineClient).Owner_Server).InputString99 + "]");
                    logTag.Flush(LogTypes.Plain);
#endif
                }
                else
                {
                }
            }
        }

    }
}
