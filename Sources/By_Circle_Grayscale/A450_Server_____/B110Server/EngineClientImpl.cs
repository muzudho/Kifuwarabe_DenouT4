﻿using System;
using System.Diagnostics;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B210Tushin.C500Util;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA;
using Grayscale.A450Server.B110Server.C125Receiver;
using Grayscale.A450Server.B110Server.C496EngineWrapper;

namespace Grayscale.A450Server.B110Server.C497EngineClient
{


    /// <summary>
    /// ************************************************************************************************************************
    ///  プロセスラッパー
    /// ************************************************************************************************************************
    /// 
    ///     １つの将棋エンジンと通信します。１対１の関係になります。
    ///     このクラスを、将棋エンジンのコンソールだ、と想像して使います。
    /// 
    /// </summary>
    public class EngineClientImpl : EngineClient
    {


        #region プロパティ類

        /// <summary>
        /// オーナー・サーバー
        /// </summary>
        public object Owner_Server { get { return this.ownerServer; } }//Server型
        public void SetOwner_Server(object owner)//Server型
        {
            this.ownerServer = owner;
        }
        private object ownerServer;//Server型

        /// <summary>
        /// レシーバー
        /// </summary>
        public IReceiver Receiver { get { return this.receiver; } }
        private IReceiver receiver;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋エンジンと会話できるオブジェクトです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public EngineProcessWrapper ShogiEngineProcessWrapper { get; set; }

        #endregion

        public EngineClientImpl(IReceiver receiver)
        {
            this.receiver = receiver;
            this.receiver.SetOwner_EngineClient(this);

            this.ShogiEngineProcessWrapper = new EngineProcessWrapperImpl();

#if DEBUG
            this.ShogiEngineProcessWrapper.SetDelegate_ShogiServer_ToEngine( (string line, KwLogger errH) =>
            {
                //
                // USIコマンドを将棋エンジンに送ったタイミングで、なにかすることがあれば、
                // ここに書きます。
                //
                errH.AppendLine(line);
                errH.Flush(LogTypes.ToClient);
            });
#endif
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンを起動します。
        /// ************************************************************************************************************************
        /// </summary>
        public void Start(string shogiEngineFilePath)
        {
            try
            {
                if (this.ShogiEngineProcessWrapper.IsLive_ShogiEngine())
                {
                    Util_Message.Show("将棋エンジンサービスは終了していません。");
                    goto gt_EndMethod;
                }

                //------------------------------
                // ログファイルを削除します。
                //------------------------------
                ErrorControllerReference.RemoveAllLogFiles();


                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = shogiEngineFilePath; // 実行するファイル名
                //startInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
                startInfo.UseShellExecute = false; // シェル機能を使用しない
                startInfo.RedirectStandardInput = true;//標準入力をリダイレクト
                startInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト

                this.ShogiEngineProcessWrapper.SetShogiEngine(Process.Start(startInfo)); // アプリの実行開始

                //  OutputDataReceivedイベントハンドラを追加
                this.ShogiEngineProcessWrapper.ShogiEngine.OutputDataReceived += this.Receiver.OnListenUpload_Async;
                this.ShogiEngineProcessWrapper.ShogiEngine.Exited += this.OnExited;

                // 非同期受信スタート☆！
                this.ShogiEngineProcessWrapper.ShogiEngine.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                Util_Message.Show(ex.GetType().Name + "：" + ex.Message);
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// この将棋サーバーを終了したときにする挙動を、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExited(object sender, System.EventArgs e)
        {
            ILogger errH = ErrorControllerReference.ProcessEngineDefault;
            this.ShogiEngineProcessWrapper.Send_Shutdown(errH);
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 手番が替わったときの挙動を、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        public void OnChangedTurn(
            Earth earth1,

            //MoveEx endNode1,
            Tree kifu1,

            Playerside kaisiPside,
            ILogger errH)
        {
            if (!this.ShogiEngineProcessWrapper.IsLive_ShogiEngine())
            {
                goto gt_EndMethod;
            }

            // FIXME:
            switch (kaisiPside)
            {
                case Playerside.P2:
                    // 仮に、コンピューターが後手番とします。

                    //------------------------------------------------------------
                    // とりあえず、コンピューターが後手ということにしておきます。
                    //------------------------------------------------------------

                    // 例：「position startpos moves 7g7f」
                    this.ShogiEngineProcessWrapper.Send_Position(
                        UtilKirokuGakari.ToSfen_PositionCommand(
                            earth1,

                            kifu1//endNode1//エンドノード

                            ), errH);

                    this.ShogiEngineProcessWrapper.Send_Go(errH);

                    break;
                default:
                    break;
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public void Send_Shutdown(ILogger errH)
        {
            this.ShogiEngineProcessWrapper.Send_Shutdown(errH);
        }

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public void Send_Logdase(ILogger errH)
        {
            this.ShogiEngineProcessWrapper.Send_Logdase(errH);
        }

        ///// <summary>
        ///// 将棋エンジンを先手にするために、go を出します。
        ///// </summary>
        //public void Send_Go(KwLogger errH)
        //{
        //    this.ShogiEngineProcessWrapper.Send_Go(errH);
        //}

    }


}