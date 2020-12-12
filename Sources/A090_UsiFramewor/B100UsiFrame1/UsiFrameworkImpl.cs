using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;

namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___
{
    public class UsiFrameworkImpl : IUsiFramework
    {
        public UsiFrameworkImpl()
        {
            this.OnApplicationBegin = this.m_nullFunc01;
            this.OnApplicationEnd = this.m_nullFunc01;
            this.OnCommandlineAtLoop1 = this.m_nullFunc05;
            this.OnCommandlineAtLoop2 = this.m_nullFunc05;
            this.OnGameover = this.m_nullFunc02;
            this.OnGoponder = this.m_nullFunc02;
            this.OnGo = this.m_nullFunc02;
            this.OnIsready = this.m_nullFunc04;
            this.OnLogDase = this.m_nullFunc02;
            this.OnLoop1Begin = this.m_nullFunc01;
            this.OnLoop1End = this.m_nullFunc01;
            this.OnLoop2Begin = this.m_nullFunc01;
            this.OnLoop2End = this.m_nullFunc01;
            this.OnPosition = this.m_nullFunc02;
            this.OnQuit = this.m_nullFunc04;
            this.OnSetoption = this.m_nullFunc04;
            this.OnStop = this.m_nullFunc02;
            this.OnUsinewgame = this.m_nullFunc04;
            this.OnUsi = this.m_nullFunc04;
        }

        public Func01 m_nullFunc01 = delegate ()
        {

        };
        public Func02 m_nullFunc02 = delegate (string line)
        {
            return PhaseResultUsiLoop2.None;
        };
        public Func04 m_nullFunc04 = delegate (string line)
        {
            return PhaseResultUsiLoop1.None;
        };
        public Func05 m_nullFunc05 = delegate ()
        {
            return "";
        };


        /// <summary>
        /// 実行します。
        /// </summary>
        /// <param name="yourShogiEngine"></param>
        public void Execute()
        {
            this.OnApplicationBegin();
            this.executeInAllBody();
            this.OnApplicationEnd();
        }

        public Func01 OnApplicationBegin { get; set; }
        private void executeInAllBody()
        {
            #region ↑詳説
            // 
            // 図.
            // 
            //     プログラムの開始：  ここの先頭行から始まります。
            //     プログラムの実行：  この中で、ずっと無限ループし続けています。
            //     プログラムの終了：  この中の最終行を終えたとき、
            //                         または途中で Environment.Exit(0); が呼ばれたときに終わります。
            //                         また、コンソールウィンドウの[×]ボタンを押して強制終了されたときも  ぶつ切り  で突然終わります。
            #endregion

            //************************************************************************************************************************
            // ループ（全体）
            //************************************************************************************************************************
            #region ↓詳説
            //
            // 図.
            //
            //      無限ループ（全体）
            //          │
            //          ├─無限ループ（１）
            //          │                      将棋エンジンであることが認知されるまで、目で訴え続けます(^▽^)
            //          │                      認知されると、無限ループ（２）に進みます。
            //          │
            //          └─無限ループ（２）
            //                                  対局中、ずっとです。
            //                                  対局が終わると、無限ループ（１）に戻ります。
            //
            // 無限ループの中に、２つの無限ループが入っています。
            //
            #endregion

            while (true)//全体ループ
            {
#if DEBUG_STOPPABLE
        MessageBox.Show("きふわらべのMainの無限ループでブレイク☆！", "デバッグ");
        System.Diagnostics.Debugger.Break();
#endif
                // 将棋サーバーからのメッセージの受信や、
                // 思考は、ここで行っています。

                //************************************************************************************************************************
                // ループ（１つ目）
                //************************************************************************************************************************
                this.OnLoop1Begin();
                PhaseResultUsiLoop1 result_Usi_Loop1 = this.executeLoop1Body();
                this.OnLoop1End();

                if (result_Usi_Loop1 == PhaseResultUsiLoop1.TimeoutShutdown)
                {
                    //MessageBox.Show("ループ１で矯正終了するんだぜ☆！");
                    return;//全体ループを抜けます。
                }
                else if (result_Usi_Loop1 == PhaseResultUsiLoop1.Quit)
                {
                    return;//全体ループを抜けます。
                }

                //************************************************************************************************************************
                // ループ（２つ目）
                //************************************************************************************************************************
                this.OnLoop2Begin();
                this.executeLoop2Body();
                this.OnLoop2End();

                if (result_Usi_Loop1 == PhaseResultUsiLoop1.TimeoutShutdown)
                {
                    //MessageBox.Show("ループ２で矯正終了するんだぜ☆！");
                    return;//全体ループを抜けます。
                }
            }//全体ループ

        }
        public Func01 OnApplicationEnd { get; set; }

        public Func01 OnLoop1Begin { get; set; }
        public PhaseResultUsiLoop1 executeLoop1Body()
        {
            //
            // サーバーに noop を送ってもよいかどうかは setoption コマンドがくるまで分からないので、
            // 作ってしまっておきます。
            // 1回も役に立たずに Loop2 に行くようなら、正常です。
#if NOOPABLE
            NoopTimerImpl noopTimer = new NoopTimerImpl();
            noopTimer._01_BeforeLoop();
#endif

            PhaseResultUsiLoop1 result_Usi_Loop1 = PhaseResultUsiLoop1.None;

            while (true)
            {
                string line = this.OnCommandlineAtLoop1();

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
                        result_Usi_Loop1 = PhaseResult_Usi_Loop1.TimeoutShutdown;
                        goto end_loop1;
                    }
#endif

                    goto gt_NextTime1;
                }


#if NOOPABLE
                noopTimer._04_AtResponsed(this.Owner, line);
#endif




                if ("usi" == line) { result_Usi_Loop1 = this.OnUsi(line); }
                else if (line.StartsWith("setoption")) { result_Usi_Loop1 = this.OnSetoption(line); }
                else if ("isready" == line) { result_Usi_Loop1 = this.OnIsready(line); }
                else if ("usinewgame" == line) { result_Usi_Loop1 = this.OnUsinewgame(line); }
                else if ("quit" == line) { result_Usi_Loop1 = this.OnQuit(line); }
                else
                {
                    //------------------------------------------------------------
                    // ○△□×！？
                    //------------------------------------------------------------
                    //
                    // ／(＾×＾)＼
                    //

                    // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                    // USIプロトコルの独習を進め、対応／未対応を選んでください。
                    //
                    // ログだけ取って、スルーします。
                }

                switch (result_Usi_Loop1)
                {
                    case PhaseResultUsiLoop1.Break:
                        goto end_loop1;

                    case PhaseResultUsiLoop1.Quit:
                        goto end_loop1;

                    default:
                        break;
                }

            gt_NextTime1:
                ;
            }

        end_loop1:
            return result_Usi_Loop1;
        }
        public Func01 OnLoop1End { get; set; }

        public Func01 OnLoop2Begin { get; set; }

        public void executeLoop2Body()
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




                PhaseResultUsiLoop2 result_Usi_Loop2;
                {
                    result_Usi_Loop2 = PhaseResultUsiLoop2.None;

                    string line = this.OnCommandlineAtLoop2();

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
                                result_Usi_Loop2 = PhaseResult_Usi_Loop2.TimeoutShutdown;
                                goto end_loop2;
                            }
                        }
#endif

                        goto gt_NextLine_loop2;
                    }



                    if (line.StartsWith("position")) { result_Usi_Loop2 = this.OnPosition(line); }
                    else if (line.StartsWith("go ponder")) { result_Usi_Loop2 = this.OnGoponder(line); }
                    else if (line.StartsWith("go")) { result_Usi_Loop2 = this.OnGo(line); }// 「go ponder」「go mate」「go infinite」とは区別します。
                    else if (line.StartsWith("stop")) { result_Usi_Loop2 = this.OnStop(line); }
                    else if (line.StartsWith("gameover")) { result_Usi_Loop2 = this.OnGameover(line); }
                    else if ("logdase" == line) { result_Usi_Loop2 = this.OnLogDase(line); }//独自拡張
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
                }




                switch (result_Usi_Loop2)
                {
                    case PhaseResultUsiLoop2.Break:
                        goto end_loop2;

                    default:
                        break;
                }
            }

        end_loop2:
            ;
        }

        /// <summary>
        /// Loop2のEnd部で呼ばれます。
        /// </summary>
        public Func01 OnLoop2End { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func05 OnCommandlineAtLoop2 { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnPosition { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnGoponder { get; set; }

        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnGo { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnStop { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnGameover { get; set; }

        /// <summary>
        /// 独自コマンド「ログ出せ」
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnLogDase { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnUsi { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnSetoption { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnIsready { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnUsinewgame { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnQuit { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func05 OnCommandlineAtLoop1 { get; set; }
    }
}
