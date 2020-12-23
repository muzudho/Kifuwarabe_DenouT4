using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.UseCases;
using Grayscale.Kifuwaragyoku.UseCases.Features;
using Nett;

namespace Grayscale.Kifuwaragyoku.Engine
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
            // 将棋エンジン　きふわらべ
            Playing playing = new Playing();

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
                    playing.Shogisasi = new ShogisasiImpl(playing);
                    Util_FvLoad.OpenFv(
                        playing.Shogisasi.FeatureVector,
                        Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Komawari")), LogTags.ProcessEngineDefault);
                }

                //------------------------------------------------------------------------------------------------------------------------
                // ファイル読込み
                //------------------------------------------------------------------------------------------------------------------------
                {
                    // データの読取「道」
                    if (Michi187Array.Load(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Michi187))))
                    {
                    }

                    // データの読取「配役」
                    string filepath_Haiyaku = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.Haiyaku185));
                    Util_Array_KomahaiyakuEx184.Load(filepath_Haiyaku, Encoding.UTF8);

                    // データの読取「強制転成表」　※駒配役を生成した後で。
                    string filepath_ForcePromotion = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.InputForcePromotion));
                    Array_ForcePromotion.Load(filepath_ForcePromotion, Encoding.UTF8);

#if DEBUG
                    {
                        string filepath_LogKyosei = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.OutputForcePromotion));
                        File.WriteAllText(filepath_LogKyosei, Array_ForcePromotion.LogHtml());
                    }
#endif

                    // データの読取「配役転換表」
                    string filepath_HaiyakuTenkan = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.InputSyuruiToHaiyaku));
                    Data_KomahaiyakuTransition.Load(filepath_HaiyakuTenkan, Encoding.UTF8);

#if DEBUG
                    {
                        string filepath_LogHaiyakuTenkan = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>(SpecifiedFiles.OutputSyuruiToHaiyaku);
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

                    while (true)
                    {
                        // 将棋サーバーから何かメッセージが届いていないか、見てみます。
                        string line = Util_Message.Download_Nonstop();

                        // 通信ログは必ず取ります。
                        Logger.Flush(LogTags.ProcessEngineNetwork, LogTypes.ToClient, line);

#if NOOPABLE
                noopTimer._04_AtResponsed(this.Owner, line);
#endif

                        if ("usi" == line)
                        {
                            var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                            var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");

                            playing.UsiOk($"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor);
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
                                // TODO パーサーを関数の中から こっちに出したいぜ☆（＾～＾）
                                playing.AddOption_ByCommandline(line);
                            }
                        }
                        else if ("isready" == line)
                        {
                            playing.ReadyOk();
                        }
                        else if ("usinewgame" == line)
                        {
                            playing.UsiNewGame();

                            // 無限ループ（１つ目）を抜けます。無限ループ（２つ目）に進みます。
                            break;
                        }
                        else if ("quit" == line)
                        {
                            playing.Quit();
                            // このプログラムを終了します。
                            return;
                        }
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
                    }

                    // ループ（２つ目）
                    playing.Shogisasi.OnTaikyokuKaisi();//対局開始時の処理。

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

                            // ノンストップ版
                            //string line = TimeoutReader.ReadLine(1000);//指定ミリ秒だけブロック。不具合がある☆（＾～＾）
                            // ブロッキングIO版
                            string line = System.Console.In.ReadLine();

                            // 通信ログは必ず取ります。
                            Logger.Flush(LogTags.ProcessEngineDefault, LogTypes.ToClient, line);

#if NOOPABLE
                if (this.owner.Option_enable_serverNoopable)
                {
                    noopTimer._03_AtResponsed(this.owner, line, logTag);
                }
#endif

                            if (line.StartsWith("position"))
                            {
                                ILogTag logTag = LogTags.ProcessEngineDefault;

                                // 手番になったときに、“まず”、将棋所から送られてくる文字が position です。
                                // このメッセージを読むと、駒の配置が分かります。
                                //
                                // “が”、まだ指してはいけません。
#if DEBUG
                                Util_Loggers.ProcessEngine_DEFAULT.AppendLine("（＾△＾）positionきたｺﾚ！");
                                Util_Loggers.ProcessEngine_DEFAULT.Flush(LogTypes.Plain);
#endif
                                // 入力行を解析します。
                                IKifuParserAResult result = new KifuParserA_ResultImpl();
                                KifuParserAImpl kifuParserA = new KifuParserAImpl();
                                IKifuParserAGenjo genjo = new KifuParserA_GenjoImpl(line);
                                kifuParserA.Execute_All_CurrentMutable(
                                    ref result,

                                    playing.Earth,
                                    playing.Kifu,

                                    genjo,
                                    logTag
                                    );
                                if (null != genjo.StartposImporter_OrNull)
                                {
                                    // SFENの解析結果を渡すので、
                                    // その解析結果をどう使うかは、委譲します。
                                    Util_InClient.OnChangeSky_Im_Client(

                                        playing.Earth,
                                        playing.Kifu,

                                        genjo,
                                        logTag
                                        );
                                }


#if DEBUG
                                Move move_forLog = result.Out_newNode_OrNull.Key;
                                ISky sky = this.Kifu_AtLoop2.PositionA;
                                ILogger logTag = logger;
                                {
                                    var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                                    var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                                    //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo(
                                    //    Util_Sky307.Json_1Sky(this.Kifu.CurNode.Value.ToKyokumenConst, "現局面になっているのかなんだぜ☆？　line=[" + line + "]　棋譜＝" + KirokuGakari.ToJsaKifuText(this.Kifu, OwataMinister.WARABE_ENGINE),
                                    //        "PgCS",
                                    //        this.Kifu.CurNode.Value.ToKyokumenConst.Temezumi
                                    //    )
                                    //);

                                    //
                                    // 局面画像ﾛｸﾞ
                                    //
                                    {
                                        // 出力先
                                        string fileName = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("ChokkinNoMovePngFilename"));

                                        SyElement srcMasu = ConvMove.ToSrcMasu(move_forLog);
                                        SyElement dstMasu = ConvMove.ToDstMasu(move_forLog);
                                        Komasyurui14 captured = ConvMove.ToCaptured(move_forLog);
                                        int srcMasuNum = Conv_Masu.ToMasuHandle(srcMasu);
                                        int dstMasuNum = Conv_Masu.ToMasuHandle(dstMasu);

                                        KyokumenPngArgs_FoodOrDropKoma foodKoma;
                                        if (Komasyurui14.H00_Null___ != captured)
                                        {
                                            switch (Util_Komasyurui14.NarazuCaseHandle(captured))
                                            {
                                                case Komasyurui14.H00_Null___: foodKoma = KyokumenPngArgs_FoodOrDropKoma.NONE; break;
                                                case Komasyurui14.H01_Fu_____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.FU__; break;
                                                case Komasyurui14.H02_Kyo____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.KYO_; break;
                                                case Komasyurui14.H03_Kei____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.KEI_; break;
                                                case Komasyurui14.H04_Gin____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.GIN_; break;
                                                case Komasyurui14.H05_Kin____: foodKoma = KyokumenPngArgs_FoodOrDropKoma.KIN_; break;
                                                case Komasyurui14.H07_Hisya__: foodKoma = KyokumenPngArgs_FoodOrDropKoma.HI__; break;
                                                case Komasyurui14.H08_Kaku___: foodKoma = KyokumenPngArgs_FoodOrDropKoma.KAKU; break;
                                                default: foodKoma = KyokumenPngArgs_FoodOrDropKoma.UNKNOWN; break;
                                            }
                                        }
                                        else
                                        {
                                            foodKoma = KyokumenPngArgs_FoodOrDropKoma.NONE;
                                        }

                                        // 直近の指し手。
                                        Util_KyokumenPng_Writer.Write1(
                                            ConvKifuNode.ToRO_Kyokumen1(sky, logTag),
                                            srcMasuNum,
                                            dstMasuNum,
                                            foodKoma,
                                            ConvMove.ToSfen(move_forLog),
                                            "",
                                            fileName,
                                            UtilKifuTreeLogWriter.REPORT_ENVIRONMENT,
                                            logTag
                                            );
                                    }
                                }
#endif

                                //------------------------------------------------------------
                                // じっとがまん
                                //------------------------------------------------------------
                                //
                                // 応答は無用です。
                                // 多分、将棋所もまだ準備ができていないのではないでしょうか（？）
                                //
                                playing.Position();

                                result_Usi_Loop2 = PhaseResultUsiLoop2.None;
                            }
                            else if (line.StartsWith("go ponder"))
                            {
                                playing.GoPonder();
                                result_Usi_Loop2 = PhaseResultUsiLoop2.None;
                            }
                            // 「go ponder」「go mate」「go infinite」とは区別します。
                            else if (line.StartsWith("go"))
                            {
                                Regex regex = new Regex(@"go btime (\d+) wtime (\d+) byoyomi (\d+)", RegexOptions.Singleline);
                                Match m = regex.Match(line);

                                if (m.Success)
                                {
                                    playing.Go((string)m.Groups[1].Value, (string)m.Groups[2].Value, (string)m.Groups[3].Value, "", "");
                                }
                                else
                                {
                                    // (2020-12-16 wed) フィッシャー・クロック・ルールに対応☆（＾～＾）
                                    regex = new Regex(@"go btime (\d+) wtime (\d+) binc (\d+) winc (\d+)", RegexOptions.Singleline);
                                    m = regex.Match(line);

                                    playing.Go((string)m.Groups[1].Value, (string)m.Groups[2].Value, "", (string)m.Groups[3].Value, (string)m.Groups[4].Value);
                                }

                                //throw new Exception("デバッグだぜ☆！　エラーはキャッチできたかな～☆？（＾▽＾）");
                                result_Usi_Loop2 = PhaseResultUsiLoop2.None;
                            }
                            else if (line.StartsWith("stop"))
                            {
                                playing.Stop();
                                result_Usi_Loop2 = PhaseResultUsiLoop2.None;
                            }
                            else if (line.StartsWith("gameover"))
                            {
                                //------------------------------------------------------------
                                // 対局が終わりました
                                //------------------------------------------------------------
                                //
                                // 図.
                                //
                                //      log.txt
                                //      ┌────────────────────────────────────────
                                //      ～
                                //      │2014/08/02 3:08:34> gameover lose
                                //      │
                                //

                                // 対局が終わったときに送られてくる文字が gameover です。

                                //------------------------------------------------------------
                                // 「あ、勝ちました」「あ、引き分けました」「あ、負けました」
                                //------------------------------------------------------------
                                //
                                // 上図のメッセージのままだと使いにくいので、
                                // あとで使いやすいように Key と Value の表に分けて持ち直します。
                                //
                                // 図.
                                //
                                //      gameoverDictionary
                                //      ┌──────┬──────┐
                                //      │Key         │Value       │
                                //      ┝━━━━━━┿━━━━━━┥
                                //      │gameover    │lose        │
                                //      └──────┴──────┘
                                //
                                Regex regex = new Regex(@"gameover (.)", RegexOptions.Singleline);
                                Match m = regex.Match(line);

                                if (m.Success)
                                {
                                    playing.GameOver((string)m.Groups[1].Value);
                                }

                                // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                                result_Usi_Loop2 = PhaseResultUsiLoop2.Break;
                            }
                            //独自拡張
                            else if ("logdase" == line)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append("ログ出せ機能は廃止だぜ～☆（＾▽＾）");
                                File.WriteAllText(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("LogDaseMeirei")), sb.ToString());

                                result_Usi_Loop2 = PhaseResultUsiLoop2.None;
                            }
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

                    //-------------------+----------------------------------------------------------------------------------------------------
                    // スナップショット  |
                    //-------------------+----------------------------------------------------------------------------------------------------
                    // 対局後のタイミングで、データの中身を確認しておきます。
                    // Key と Value の表の形をしています。（順不同）
                    //
                    // 図.
                    //      ※内容はサンプルです。実際と異なる場合があります。
                    //
                    //      setoption
                    //      ┌──────┬──────┐
                    //      │Key         │Value       │
                    //      ┝━━━━━━┿━━━━━━┥
                    //      │USI_Ponder  │true        │
                    //      ├──────┼──────┤
                    //      │USI_Hash    │256         │
                    //      └──────┴──────┘
                    //
                    //      goDictionary
                    //      ┌──────┬──────┐
                    //      │Key         │Value       │
                    //      ┝━━━━━━┿━━━━━━┥
                    //      │btime       │599000      │
                    //      ├──────┼──────┤
                    //      │wtime       │600000      │
                    //      ├──────┼──────┤
                    //      │byoyomi     │60000       │
                    //      └──────┴──────┘
                    //
                    //      goMateDictionary
                    //      ┌──────┬──────┐
                    //      │Key         │Value       │
                    //      ┝━━━━━━┿━━━━━━┥
                    //      │mate        │599000      │
                    //      └──────┴──────┘
                    //
                    //      gameoverDictionary
                    //      ┌──────┬──────┐
                    //      │Key         │Value       │
                    //      ┝━━━━━━┿━━━━━━┥
                    //      │gameover    │lose        │
                    //      └──────┴──────┘
                    //
#if DEBUG
                    Util_Loggers.ProcessEngine_DEFAULT.AppendLine("KifuParserA_Impl.LOGGING_BY_ENGINE, 確認 setoptionDictionary");
                    Util_Loggers.ProcessEngine_DEFAULT.AppendLine(this.EngineOptions.ToString());

                    Util_Loggers.ProcessEngine_DEFAULT.AppendLine("┏━確認━━━━goDictionary━━━━━┓");
                    foreach (KeyValuePair<string, string> pair in this.GoProperties_AtLoop2)
                    {
                        Util_Loggers.ProcessEngine_DEFAULT.AppendLine(pair.Key + "=" + pair.Value);
                    }

                    //Dictionary<string, string> goMateProperties = new Dictionary<string, string>();
                    //goMateProperties["mate"] = "";
                    //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
                    //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┏━確認━━━━goMateDictionary━━━┓");
                    //foreach (KeyValuePair<string, string> pair in this.goMateProperties)
                    //{
                    //    LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                    //}

                    Util_Loggers.ProcessEngine_DEFAULT.AppendLine("┗━━━━━━━━━━━━━━━━━━┛");
                    Util_Loggers.ProcessEngine_DEFAULT.AppendLine("┏━確認━━━━gameoverDictionary━━┓");
                    foreach (KeyValuePair<string, string> pair in this.GameoverProperties_AtLoop2)
                    {
                        Util_Loggers.ProcessEngine_DEFAULT.AppendLine(pair.Key + "=" + pair.Value);
                    }
                    Util_Loggers.ProcessEngine_DEFAULT.AppendLine("┗━━━━━━━━━━━━━━━━━━┛");
                    Util_Loggers.ProcessEngine_DEFAULT.Flush(LogTypes.Plain);
#endif

                }//全体ループ
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.Trace($"(^ー^)「大外枠でキャッチ」：{ex}");
                Console.Out.WriteLine("bestmove resign");
                //throw;//追加
            }
        }
    }
}
