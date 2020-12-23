using System.Collections.Generic;
using System.Windows.Forms;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{

    /// <summary>
    /// [再生]ボタンを押したときの処理。
    /// </summary>
    public class TimedC_SaiseiCapture : TimedAbstract
    {


        private MainGui_Csharp mainGui;

        /// <summary>
        /// [再生]の状態です。
        /// </summary>
        public Queue<SaiseiEventState> SaiseiEventQueue { get; set; }


        private string restText;


        public TimedC_SaiseiCapture(MainGui_Csharp shogiGui)
        {
            this.mainGui = shogiGui;
            this.SaiseiEventQueue = new Queue<SaiseiEventState>();
        }

        public override void Step()
        {

            // 入っているマウス操作イベントは、全部捨てていきます。
            SaiseiEventState[] queue = this.SaiseiEventQueue.ToArray();
            this.SaiseiEventQueue.Clear();
            foreach (SaiseiEventState eventState in queue)
            {
                switch (eventState.Name2)
                {
                    case SaiseiEventStateName.Start:
                        {
                            #region スタート
                            //MessageBox.Show("再生を実行します2。");

                            mainGui.RepaintRequest = new RepaintRequestImpl();

                            this.restText = Util_Function_Csharp.ReadLine_FromTextbox();
                            this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Step));
                            #endregion
                        }
                        break;

                    case SaiseiEventStateName.Step:
                        {
                            #region ステップ

                            bool toBreak = false;
                            if ("" == restText)
                            {
                                toBreak = true;
                            }
                            else
                            {
                                // [コマ送り]に成功している間、コマ送りし続けます。
                                Util_Functions_Server.ReadLine_TuginoItteSusumu_Srv_CurrentMutable(
                                    ref restText,

                                    this.mainGui.Link_Server.Earth,
                                    this.mainGui.Link_Server.KifuTree,

                                    this.mainGui.SkyWrapper_Gui,
                                    out toBreak,
                                    "再生ボタン");

                                //TimedC.Saisei_Step(restText, shogiGui, eventState.Flg_logTag);// 再描画（ループが１回も実行されなかったとき用）
                                // 他のアプリが固まらないようにします。
                                Application.DoEvents();

                                // 早すぎると描画されないので、ウェイトを入れます。
                                System.Threading.Thread.Sleep(90);//45


                                //------------------------------
                                // 再描画
                                //------------------------------
                                Util_Function_Csharp.Komaokuri_Gui(
                                    restText,
                                    mainGui.Link_Server.KifuTree.MoveEx_Current,
                                    mainGui.Link_Server.KifuTree.PositionA,//.CurNode2ok.GetNodeValue()
                                    mainGui,
                                    mainGui.Link_Server.KifuTree);//追加

                                //------------------------------
                                // メナス
                                //------------------------------
                                Util_Menace.Menace(mainGui);

                                mainGui.Response("Saisei");// 再描画
                            }


                            if (toBreak)
                            {
                                // 終了
                                this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Finished));
                            }
                            else
                            {
                                // 続行
                                this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Step));
                            }
                            #endregion
                        }
                        break;
                }
            }
        }



    }
}
