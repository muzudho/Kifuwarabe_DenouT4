using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;

using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;
using Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C___125_Receiver;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C___060_Phase;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C___070_ServerBase;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A480_ServerAims_.B110_AimsServer_.C125____Receiver
{
    public class Receiver_ForAimsImpl : Receiver_ForCsharpVsImpl, Receiver_ForAims
    {

        /// <summary>
        /// オーナー・サーバー。
        /// </summary>
        public AimsServerBase Owner_AimsServer { get { return this.owner_AimsServer; } }
        public void SetOwner_AimsServer(AimsServerBase owner_AimsServer)
        {
            this.owner_AimsServer = owner_AimsServer;
        }
        private AimsServerBase owner_AimsServer;
        
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
        public override void OnListenUpload_Async(object sender, DataReceivedEventArgs e)
        {
            KwLogger errH = Util_Loggers.ProcessAims_DEFAULT;

            string line = e.Data;

            if (null == line)
            {
                // 無視
            }
            else
            {
                switch(this.Owner_AimsServer.Phase_AimsServer)
                {
                    case Phase_AimsServer._01_Server_Booted: break;//thru
                    case Phase_AimsServer._02_WaitAimsUsiok: break;//thru
                    case Phase_AimsServer._03_WaitEngineLive: break;//thru
                    case Phase_AimsServer._04_WaitEngineUsiok:
                        {
                            if (line.StartsWith("option "))
                            {
                                //ok MessageBox.Show("将棋サーバー「Firstフェーズで、将棋エンジンから、オプション[" + line + "]が返ってきたぜ☆！」");
                            }
                            else if (line.StartsWith("id "))
                            {
                                //ok MessageBox.Show("将棋サーバー「Firstフェーズで、将棋エンジンから、アイディー[" + line + "]が返ってきたぜ☆！」");
                            }
                            else if ("usiok" == line)
                            {
                                //------------------------------------------------------------
                                // AIMS GUIへ：　「isready」を転送します。
                                //------------------------------------------------------------
                                //MessageBox.Show("将棋サーバー「将棋エンジンから、usiokが返ってきたぜ☆！\n" +
                                //    "サーバーは WaitAimsIsready フェーズに移るぜ☆\n"+
                                //    "AIMS GUIには isready を送るぜ☆！」");
                                this.Owner_AimsServer.SetPhase_AimsServer(Phase_AimsServer._05_WaitAimsReadyok);
                                Console.Out.WriteLine("isready");
                            }
                            else
                            {
                                MessageBox.Show("将棋サーバー「WaitEngineUsiokフェーズで、将棋エンジンから、[" + line + "]が返ってきたぜ☆！」");
                            }
                        }
                        break;
                    case Phase_AimsServer._05_WaitEngineReadyok:
                        {
                            //------------------------------------------------------------
                            // 将棋エンジンからの「readyok」コマンドを待っています。
                            //------------------------------------------------------------
                            if ("readyok" == line)
                            {
                                //MessageBox.Show("将棋サーバー「将棋エンジンから、readyokが返ってきたぜ☆！\n" +
                                //    "AIMS GUI が先手と仮定し、\n" +
                                //    "サーバーは WaitAimsBestmove フェーズに移るぜ☆\n" +
                                //    "AIMS GUI、将棋エンジンの双方に usinewgame コマンドを送るぜ☆！」");

                                //------------------------------------------------------------
                                // 対局開始！
                                //------------------------------------------------------------
                                Console.Out.WriteLine("usinewgame");
                                ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Usinewgame(errH);

                                // FIXME:平手とは限らないが、平手という前提で、毎回一から作りなおします。
                                Sky positionInit = Util_SkyCreator.New_Hirate();
                                this.Owner_AimsServer.SetKifuTree(new TreeImpl(positionInit));
                                this.Owner_AimsServer.Earth.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");


                                ////
                                //// どちらが先手かにもよるが。
                                //// AIMS GUI が先手と仮定します。
                                ////
                                MessageBox.Show("将棋サーバー「AIMS GUI が先手と仮定し、\n" +
                                    "サーバーは WaitAimsBestmove フェーズに移るぜ☆\n" +
                                    "将棋エンジンには position    コマンド、\n"+
                                    "AIMS GUI  には position,go コマンドを送るぜ☆！」");
                                this.Owner_AimsServer.SetPhase_AimsServer(Phase_AimsServer._101_WaitAimsBestmove);

                                // 将棋エンジンに対して
                                // 例：「position startpos moves 7g7f」
                                ((EngineClient)this.Owner_EngineClient).ShogiEngineProcessWrapper.Send_Position(
                                    Util_KirokuGakari.ToSfen_PositionCommand(
                                        this.Owner_AimsServer.Earth,
                                        this.Owner_AimsServer.KifuTree//.CurrentNode//エンドノード
                                        ), errH
                                );

                                // AIMS GUIに対して
                                // 例：「position startpos moves 7g7f」
                                Console.Out.WriteLine( Util_KirokuGakari.ToSfen_PositionCommand(
                                    this.Owner_AimsServer.Earth,
                                    this.Owner_AimsServer.KifuTree//.CurrentNode//エンドノード
                                    ));

                                Console.Out.WriteLine("go");
                            }
                        }
                        break;
                    default:
                        {
                            MessageBox.Show("将棋サーバー「該当なしフェーズで、将棋エンジンから、[" + line + "]が返ってきたぜ☆！」");
                        }
                        break;
                }
                    /*
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

                    ((Server)((EngineClient)this.Owner_EngineClient).Owner_Server).SetInputString99(
                        ((Server)((EngineClient)this.Owner_EngineClient).Owner_Server).InputString99 + line.Substring("bestmove".Length + "".Length)
                        );

                    OwataMinister.BY_GUI.Logger.WriteLine_AddMemo("USI受信：bestmove input99=[" + ((Server)((EngineClient)this.Owner_EngineClient).Owner_Server).InputString99 + "]");
                }
                     */
            }
        }


    }
}
