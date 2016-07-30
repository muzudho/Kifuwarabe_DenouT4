using Grayscale.P571_usiFrame1__.L___500_usiFrame___;
using Grayscale.P575_KifuWarabe_.L250____UsiLoop;
using System;

namespace Grayscale.P579_usiFrame2__.L500____usiFrame___
{
    public class UsiFrameworkImpl : UsiFramework
    {
        public void OnBegin_InAll(Func01 func01)
        {
            func01();
        }
        public void OnBody_InAll(Func01 func01)
        {
            func01();
        }
        public void OnEnd_InAll(Func01 func01)
        {
            func01();
        }

        public void OnBegin_InLoop1(Object caller)
        {
        }
        public PhaseResult_UsiLoop1 OnBody_InLoop1(Object caller)
        {
            //
            // サーバーに noop を送ってもよいかどうかは setoption コマンドがくるまで分からないので、
            // 作ってしまっておきます。
            // 1回も役に立たずに Loop2 に行くようなら、正常です。
#if NOOPABLE
            NoopTimerImpl noopTimer = new NoopTimerImpl();
            noopTimer._01_BeforeLoop();
#endif

            PhaseResult_UsiLoop1 result_UsiLoop1 = PhaseResult_UsiLoop1.None;

            while (true)
            {
                string line = this.OnCommandlineRead_AtBody1(caller);

                if (null == line)//次の行が無ければヌル。
                {
                    // メッセージは届いていませんでした。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
#if NOOPABLE
                    bool isTimeoutShutdown_temp;
                    noopTimer._03_AtEmptyMessage(this.Owner, out isTimeoutShutdown_temp);
                    if (isTimeoutShutdown_temp)
                    {
                        //MessageBox.Show("ループ１でタイムアウトだぜ☆！");
                        out_isTimeoutShutdown = isTimeoutShutdown_temp;
                        result_UsiLoop1 = PhaseResult_UsiLoop1.TimeoutShutdown;
                        goto end_loop1;
                    }
#endif

                    goto gt_NextTime1;
                }


#if NOOPABLE
                noopTimer._04_AtResponsed(this.Owner, line);
#endif




                if ("usi" == line) { result_UsiLoop1 = this.OnUsi_AtBody1(line, caller); }
                else if (line.StartsWith("setoption")) { result_UsiLoop1 = this.OnSetoption_AtBody1(line, caller); }
                else if ("isready" == line) { result_UsiLoop1 = this.OnIsready_AtBody1(line, caller); }
                else if ("usinewgame" == line) { result_UsiLoop1 = this.OnUsinewgame_AtBody1(line, caller); }
                else if ("quit" == line) { result_UsiLoop1 = this.OnQuit_AtBody1(line, caller); }
                else
                {
                    //------------------------------------------------------------
                    // ○△□×！？
                    //------------------------------------------------------------
                    #region ↓詳説
                    //
                    // ／(＾×＾)＼
                    //

                    // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                    // USIプロトコルの独習を進め、対応／未対応を選んでください。
                    //
                    // ログだけ取って、スルーします。
                    #endregion
                }

                switch (result_UsiLoop1)
                {
                    case PhaseResult_UsiLoop1.Break:
                        goto end_loop1;

                    case PhaseResult_UsiLoop1.Quit:
                        goto end_loop1;

                    default:
                        break;
                }

                gt_NextTime1:
                ;
            }

            end_loop1:
            return result_UsiLoop1;
        }
        public void OnEnd_InLoop1(Object caller)
        {
        }

        public void OnBegin_InLoop2(Func01 func01)
        {
            func01();
        }

        public void OnBody_InLoop2(Object caller)
        {
            while (true)
            {

                //PerformanceMetrics performanceMetrics = new PerformanceMetrics();//使ってない？

#if NOOPABLE
                                            // サーバーに noop を送ってもよい場合だけ有効にします。
                                            NoopTimerImpl noopTimer = null;
                                            if(this.owner.Option_enable_serverNoopable)
                                            {
                                                noopTimer = new NoopTimerImpl();
                                                noopTimer._01_BeforeLoop();
                                            }
#endif




                PhaseResult_UsiLoop2 result_UsiLoop2;
                {
                    result_UsiLoop2 = PhaseResult_UsiLoop2.None;

                    string line = this.OnCommandlineRead_AtBody2(caller);

                    if (null == line)//次の行が無ければヌル。
                    {
                        // メッセージは届いていませんでした。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

#if NOOPABLE
                                        if (this.owner.Option_enable_serverNoopable)
                                        {
                                            bool isTimeoutShutdown_temp;
                                            noopTimer._02_AtEmptyMessage(this.owner, out isTimeoutShutdown_temp,errH);
                                            if (isTimeoutShutdown_temp)
                                            {
                                                //MessageBox.Show("ループ２でタイムアウトだぜ☆！");
                                                result_UsiLoop2 = PhaseResult_UsiLoop2.TimeoutShutdown;
                                                goto end_loop2;
                                            }
                                        }
#endif

                        goto gt_NextLine_loop2;
                    }



                    if (line.StartsWith("position")) { result_UsiLoop2 = this.OnPosition_AtBody2(line, caller); }
                    else if (line.StartsWith("go ponder")) { result_UsiLoop2 = this.OnGoponder_AtBody2(line, caller); }
                    else if (line.StartsWith("go")) { result_UsiLoop2 = this.OnGo_AtBody2(line, caller); }// 「go ponder」「go mate」「go infinite」とは区別します。
                    else if (line.StartsWith("stop")) { result_UsiLoop2 = this.OnStop_AtBody2(line, caller); }
                    else if (line.StartsWith("gameover")) { result_UsiLoop2 = this.OnGameover_AtBody2(line, caller); }
                    else if ("logdase" == line) { result_UsiLoop2 = this.OnLogdase_AtBody2(line, caller); }//独自拡張
                    else
                    {
                        //------------------------------------------------------------
                        // ○△□×！？
                        //------------------------------------------------------------
                        #region ↓詳説
                        //
                        // ／(＾×＾)＼
                        //

                        // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                        // USIプロトコルの独習を進め、対応／未対応を選んでください。
                        //
                        // ログだけ取って、スルーします。
                        #endregion
                    }

                    gt_NextLine_loop2:
                    ;

                    //return result_UsiLoop2;
                }




                switch (result_UsiLoop2)
                {
                    case PhaseResult_UsiLoop2.Break:
                        goto end_loop2;

                    default:
                        break;
                }
            }

            end_loop2:
            ;
        }

        public void OnEnd_InLoop2(Func01 func01)
        {
            func01();
        }

        public Func03 OnCommandlineRead_AtBody2 { get; set; }
        public Func02 OnPosition_AtBody2 { get; set; }
        public Func02 OnGoponder_AtBody2 { get; set; }

        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// </summary>
        public Func02 OnGo_AtBody2 { get; set; }

        public Func02 OnStop_AtBody2 { get; set; }
        public Func02 OnGameover_AtBody2 { get; set; }

        /// <summary>
        /// 独自コマンド「ログ出せ」
        /// </summary>
        public Func02 OnLogdase_AtBody2 { get; set; }

        public Func04 OnUsi_AtBody1 { get; set; }
        public Func04 OnSetoption_AtBody1 { get; set; }
        public Func04 OnIsready_AtBody1 { get; set; }
        public Func04 OnUsinewgame_AtBody1 { get; set; }
        public Func04 OnQuit_AtBody1 { get; set; }
        public Func03 OnCommandlineRead_AtBody1 { get; set; }
    }
}
