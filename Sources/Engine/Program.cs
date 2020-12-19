using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe;
using Grayscale.Kifuwaragyoku.UseCases;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;
using Nett;
using System.IO;
using System.Text.RegularExpressions;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C100Shogisasi;
using Grayscale.A500ShogiEngine.B523UtilFv.C510UtilFvLoad;
using Grayscale.A210KnowNingen.B380Michi.C500Word;
using Grayscale.A210KnowNingen.B390KomahaiyaEx.C500Util;
using System.Text;
using Grayscale.A210KnowNingen.B490ForcePromot.C250Struct;
using Grayscale.A210KnowNingen.B300_KomahaiyaTr.C500Table;

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
                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                //-------------------+----------------------------------------------------------------------------------------------------
                // ログファイル削除  |
                //-------------------+----------------------------------------------------------------------------------------------------
                {
                    //
                    // 図.
                    //
                    //      フォルダー
                    //          ├─ Engine.KifuWarabe.exe
                    //          └─ log.txt               ←これを削除
                    //
                    Logger.RemoveAllLogFiles();
                }

                //------------------------------------------------------------------------------------------------------------------------
                // 思考エンジンの、記憶を読み取ります。
                //------------------------------------------------------------------------------------------------------------------------
                {
                    programSupport.Shogisasi = new ShogisasiImpl(playing, programSupport);
                    Util_FvLoad.OpenFv(
                        programSupport.Shogisasi.FeatureVector,
                        Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Komawari")), LogTags.ProcessEngineDefault);
                }

                //------------------------------------------------------------------------------------------------------------------------
                // ファイル読込み
                //------------------------------------------------------------------------------------------------------------------------
                {
                    // データの読取「道」
                    if (Michi187Array.Load(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Michi187"))))
                    {
                    }

                    // データの読取「配役」
                    string filepath_Haiyaku = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Haiyaku185"));
                    Util_Array_KomahaiyakuEx184.Load(filepath_Haiyaku, Encoding.UTF8);

                    // データの読取「強制転成表」　※駒配役を生成した後で。
                    string filepath_ForcePromotion = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("InputForcePromotion"));
                    Array_ForcePromotion.Load(filepath_ForcePromotion, Encoding.UTF8);

#if DEBUG
                    {
                        string filepath_LogKyosei = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("OutputForcePromotion"));
                        File.WriteAllText(filepath_LogKyosei, Array_ForcePromotion.LogHtml());
                    }
#endif

                    // データの読取「配役転換表」
                    string filepath_HaiyakuTenkan = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("InputSyuruiToHaiyaku"));
                    Data_KomahaiyakuTransition.Load(filepath_HaiyakuTenkan, Encoding.UTF8);

#if DEBUG
                    {
                        string filepath_LogHaiyakuTenkan = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("OutputSyuruiToHaiyaku");
                        File.WriteAllText(filepath_LogHaiyakuTenkan, Data_KomahaiyakuTransition.Format_LogHtml());
                    }
#endif
                }

                //-------------+----------------------------------------------------------------------------------------------------------
                // ログ書込み  |  ＜この将棋エンジン＞  製品名、バージョン番号
                //-------------+----------------------------------------------------------------------------------------------------------
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      │2014/08/02 1:04:59> v(^▽^)v ｲｪｰｲ☆ ... fugafuga 1.00.0
                //      │
                //      │
                //
                //
                // 製品名とバージョン番号は、次のファイルに書かれているものを使っています。
                // 場所：  [ソリューション エクスプローラー]-[ソリューション名]-[プロジェクト名]-[Properties]-[AssemblyInfo.cs] の中の、[AssemblyProduct]と[AssemblyVersion] を参照。
                //
                // バージョン番号を「1.00.0」形式（メジャー番号.マイナー番号.ビルド番号)で書くのは作者の趣味です。
                //
                {
                    string versionStr;

                    // バージョン番号
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    versionStr = String.Format("{0}.{1}.{2}", version.Major, version.Minor.ToString("00"), version.Build);

                    //seihinName += " " + versionStr;
#if DEBUG
                    Util_Loggers.ProcessEngine_DEFAULT.AppendLine("v(^▽^)v ｲｪｰｲ☆ ... " + this.SeihinName + " " + versionStr);
                    Util_Loggers.ProcessEngine_DEFAULT.Flush(LogTypes.Plain);
#endif
                }

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

                        if ("usi" == line)
                        {
                            var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                            var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");

                            playing.UsiOk($"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor);
                            result_Usi_Loop1 = PhaseResultUsiLoop1.None;
                        }
                        else if (line.StartsWith("setoption"))
                        {
                            //------------------------------------------------------------
                            // 設定してください
                            //------------------------------------------------------------
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 8:19:36> setoption name USI_Ponder value true
                            //      │2014/08/02 8:19:36> setoption name USI_Hash value 256
                            //      │
                            //
                            // ↑ゲーム開始時には、[対局]ダイアログボックスの[エンジン共通設定]の２つの内容が送られてきます。
                            //      ・[相手の手番中に先読み] チェックボックス
                            //      ・[ハッシュメモリ  ★　MB] スピン
                            //
                            // または
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 23:47:35> setoption name 卯
                            //      │2014/08/02 23:47:35> setoption name 卯
                            //      │2014/08/02 23:48:29> setoption name 子 value true
                            //      │2014/08/02 23:48:29> setoption name USI value 6
                            //      │2014/08/02 23:48:29> setoption name 寅 value 馬
                            //      │2014/08/02 23:48:29> setoption name 辰 value DRAGONabcde
                            //      │2014/08/02 23:48:29> setoption name 巳 value C:\Users\Takahashi\Documents\新しいビットマップ イメージ.bmp
                            //      │
                            //
                            //
                            // 将棋所から、[エンジン設定] ダイアログボックスの内容が送られてきます。
                            // このダイアログボックスは、将棋エンジンから将棋所に  ダイアログボックスを作るようにメッセージを送って作ったものです。
                            //

                            //------------------------------------------------------------
                            // 設定を一覧表に変えます
                            //------------------------------------------------------------
                            //
                            // 上図のメッセージのままだと使いにくいので、
                            // あとで使いやすいように Key と Value の表に分けて持ち直します。
                            //
                            // 図.
                            //
                            //      setoptionDictionary
                            //      ┌──────┬──────┐
                            //      │Key         │Value       │
                            //      ┝━━━━━━┿━━━━━━┥
                            //      │USI_Ponder  │true        │
                            //      ├──────┼──────┤
                            //      │USI_Hash    │256         │
                            //      └──────┴──────┘
                            //
                            Regex regex = new Regex(@"setoption name ([^ ]+)(?: value (.*))?", RegexOptions.Singleline);
                            Match m = regex.Match(line);

                            if (m.Success)
                            {
                                // 項目を設定します。未定義の項目の場合、文字列型として新規追加します。
                                playing.AddOption_ByCommandline(line);
                                /*
                                string name = (string)m.Groups[1].Value;
                                string value = "";

                                if (3 <= m.Groups.Count)
                                {
                                    // 「value ★」も省略されずにありました。
                                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    value = (string)m.Groups[2].Value;
                                }

                                // 項目を設定します。未定義の項目の場合、文字列型として新規追加します。
                                owner.EngineOptions.ParseValue_AutoAdd(name, value);
                                */
                            }

                            result_Usi_Loop1 = PhaseResultUsiLoop1.None;
                        }
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
