﻿using System;
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
using Grayscale.A500ShogiEngine.B260UtilClient.C500Util;
using Grayscale.A210KnowNingen.B740KifuParserA.C500Parser;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C125AjimiEngine;
using Grayscale.A210KnowNingen.B410SeizaFinger.C250Struct;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using System.Collections.Generic;

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
                    playing.Shogisasi = new ShogisasiImpl(playing, programSupport);
                    Util_FvLoad.OpenFv(
                        playing.Shogisasi.FeatureVector,
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

                    while (true)
                    {
                        string line = usiFramework.OnCommandlineAtLoop1();

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
                            Logger.AppendLine(LogTags.ProcessEngineDefault, line);
                            Logger.Flush(LogTags.ProcessEngineDefault, LogTypes.ToClient);

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
                this.Log1_AtLoop2("（＾△＾）positionきたｺﾚ！");
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
                this.Log2_Png_Tyokkin_AtLoop2(line,
                    result.Out_newNode_OrNull.Key,
                    this.Kifu_AtLoop2.PositionA,
                    logger);
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

                }//全体ループ
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
