using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe;
using Grayscale.Kifuwaragyoku.UseCases;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;

namespace Grayscale.P580_Form_______
{
    /// <summary>
    /// プログラムのエントリー・ポイントです。
    /// </summary>
    class Program
    {
        /// <summary>
        /// Ｃ＃のプログラムは、
        /// この Main 関数から始まり、 Main 関数を抜けて終わります。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Playing playing = new Playing();

            // USIフレームワーク
            IUsiFramework usiFramework = new UsiFrameworkImpl();

            // 将棋エンジン　きふわらべ
            ProgramSupport programSupport = new ProgramSupport(usiFramework);

            try
            {
                usiFramework.OnApplicationBegin();

                // 
                // 図.
                // 
                //     プログラムの開始：  ここの先頭行から始まります。
                //     プログラムの実行：  この中で、ずっと無限ループし続けています。
                //     プログラムの終了：  この中の最終行を終えたとき、
                //                         または途中で Environment.Exit(0); が呼ばれたときに終わります。
                //                         また、コンソールウィンドウの[×]ボタンを押して強制終了されたときも  ぶつ切り  で突然終わります。

                //************************************************************************************************************************
                // ループ（全体）
                //************************************************************************************************************************
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
                        string line = usiFramework.OnCommandlineAtLoop1();

                        // (2020-12-13 sun) ノン・ブロッキングなら このコードが意味あったんだが☆（＾～＾）
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

                            // 将棋サーバーに向かってメッセージを送り出します。
                            // Console.WriteLine("ループ１でメッセージは無かったぜ☆（＾～＾）"); // TODO (2020-12-13 sun) 消す。
                            continue;
                        }


#if NOOPABLE
                noopTimer._04_AtResponsed(this.Owner, line);
#endif




                        if ("usi" == line) { result_Usi_Loop1 = usiFramework.OnUsi(line); }
                        else if (line.StartsWith("setoption")) { result_Usi_Loop1 = usiFramework.OnSetoption(line); }
                        else if ("isready" == line) { result_Usi_Loop1 = usiFramework.OnIsready(line); }
                        else if ("usinewgame" == line) { result_Usi_Loop1 = usiFramework.OnUsinewgame(line); }
                        else if ("quit" == line) { result_Usi_Loop1 = usiFramework.OnQuit(line); }
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
                    }

                end_loop1:


                    usiFramework.OnLoop1End();

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
                    usiFramework.OnLoop2Begin();

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

                            string line = usiFramework.OnCommandlineAtLoop2();

                            // (2020-12-13 sun) ノン・ブロッキングなら このコードが意味あったんだが☆（＾～＾）
                            if (null == line)//次の行が無ければヌル。
                            {
                                // メッセージは届いていませんでした。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

#if NOOPABLE
                        if (this.owner.Option_enable_serverNoopable)
                        {
                            bool isTimeoutShutdown_temp;
                            noopTimer._02_AtEmptyMessage(this.owner, out isTimeoutShutdown_temp,logTag);
                            if (isTimeoutShutdown_temp)
                            {
                                //MessageBox.Show("ループ２でタイムアウトだぜ☆！");
                                result_Usi_Loop2 = PhaseResult_Usi_Loop2.TimeoutShutdown;
                                goto end_loop2;
                            }
                        }
#endif

                                // Console.WriteLine("ループ２でメッセージは無かったぜ☆（＾～＾）"); // TODO (2020-12-13 sun) 消す。
                                continue;
                            }



                            if (line.StartsWith("position")) { result_Usi_Loop2 = usiFramework.OnPosition(line); }
                            else if (line.StartsWith("go ponder")) { result_Usi_Loop2 = usiFramework.OnGoponder(line); }
                            else if (line.StartsWith("go")) { result_Usi_Loop2 = usiFramework.OnGo(line); }// 「go ponder」「go mate」「go infinite」とは区別します。
                            else if (line.StartsWith("stop")) { result_Usi_Loop2 = usiFramework.OnStop(line); }
                            else if (line.StartsWith("gameover")) { result_Usi_Loop2 = usiFramework.OnGameover(line); }
                            else if ("logdase" == line) { result_Usi_Loop2 = usiFramework.OnLogDase(line); }//独自拡張
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

                    usiFramework.OnLoop2End();

                    if (result_Usi_Loop1 == PhaseResultUsiLoop1.TimeoutShutdown)
                    {
                        //MessageBox.Show("ループ２で矯正終了するんだぜ☆！");
                        return;//全体ループを抜けます。
                    }
                }//全体ループ


                usiFramework.OnApplicationEnd();
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.Panic(LogTags.ProcessEngineDefault, "Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
                //throw;//追加
            }
        }
    }
}
