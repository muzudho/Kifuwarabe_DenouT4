// noop 可
//#define NOOPABLE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B210Tushin.C500Util;
using Grayscale.A060Application.B310Settei.C500Struct;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;//FIXME:
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B300_KomahaiyaTr.C500Table;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B380Michi.C500Word;
using Grayscale.A210KnowNingen.B390KomahaiyaEx.C500Util;
using Grayscale.A210KnowNingen.B410SeizaFinger.C250Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B490ForcePromot.C250Struct;
using Grayscale.A210KnowNingen.B640_KifuTree___.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A210KnowNingen.B740KifuParserA.C500Parser;
using Grayscale.A500ShogiEngine.B200Scoreing.C005UsiLoop;
using Grayscale.A500ShogiEngine.B200Scoreing.C240Shogisasi;
using Grayscale.A500ShogiEngine.B260UtilClient.C500Util;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C100Shogisasi;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C125AjimiEngine;
using Grayscale.A500ShogiEngine.B523UtilFv.C510UtilFvLoad;
using Nett;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A150LogKyokuPng.B100KyokumenPng.C500Struct;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A150LogKyokuPng.B200LogKyokuPng.C500UtilWriter;
using Grayscale.A240_KifuTreeLog.B110KifuTreeLog.C500Struct;
// using Grayscale.A060Application.B110Log.C500Struct;
#endif

namespace Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe
{

    public class KifuWarabeImpl : ShogiEngine
    {

        #region コンストラクター
        /// <summary>
        /// コンストラクター
        /// </summary>
        public KifuWarabeImpl(IUsiFramework usiFramework)
        {
            this.Logger = ErrorControllerReference.ProcessEngineDefault;

            //-------------+----------------------------------------------------------------------------------------------------------
            // データ設計  |
            //-------------+----------------------------------------------------------------------------------------------------------
            // 将棋所から送られてくるデータを、一覧表に変えたものです。
            this.EngineOptions = new EngineOptionsImpl();
            this.EngineOptions.AddOption(EngineOptionNames.USI_PONDER, new EngineOptionBoolImpl());// ポンダーに対応している将棋サーバーなら真です。
            this.EngineOptions.AddOption(EngineOptionNames.NOOPABLE, new EngineOptionBoolImpl());// 独自実装のコマンドなので、ＯＦＦにしておきます。
            this.EngineOptions.AddOption(EngineOptionNames.THINKING_MILLI_SECOND, new EngineOptionNumberImpl(30000));//30秒//90000//60000//8000//4000






            #region ↓詳説  ＜n手目＞
            //
            // 図.
            //
            //      この将棋エンジンが後手とします。
            //
            //      ┌──┬─────────────┬──────┬──────┬────────────────────────────────────┐
            //      │順番│                          │計算        │temezumiCount │解説                                                                    │
            //      ┝━━┿━━━━━━━━━━━━━┿━━━━━━┿━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥
            //      │   1│初回                      │            │            │相手が先手、この将棋エンジンが後手とします。                            │
            //      │    │                          │            │0           │もし、この将棋エンジンが先手なら、初回は temezumiCount = -1 とします。    │
            //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
            //      │   2│position                  │+-0         │            │                                                                        │
            //      │    │    (相手が指しても、     │            │            │                                                                        │
            //      │    │     指していないときでも │            │            │                                                                        │
            //      │    │     送られてきます)      │            │0           │                                                                        │
            //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
            //      │   3│go                        │+2          │            │+2 します                                                               │
            //      │    │    (相手が指した)        │            │2           │    ※「go」は、「go ponder」「go mate」「go infinite」とは区別します。 │
            //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
            //      │   4│go ponder                 │+-0         │            │                                                                        │
            //      │    │    (相手はまだ指してない)│            │2           │                                                                        │
            //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
            //      │   5│自分が指した              │+-0         │            │相手が指してから +2 すると決めたので、                                  │
            //      │    │                          │            │2           │自分が指したときにはカウントを変えません。                              │
            //      └──┴─────────────┴──────┴──────┴────────────────────────────────────┘
            //
            #endregion

            // 棋譜
            ISky positionInit = UtilSkyCreator.New_Hirate();// きふわらべ起動時
            {
                // FIXME:平手とは限らないが、平手という前提で作っておく。
                this.m_earth_AtLoop2_ = new EarthImpl();
                this.m_kifu_AtLoop2_ = new TreeImpl(positionInit);
                this.Earth_AtLoop2.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");// 平手 // FIXME:平手とは限らないが。

                this.m_kifu_AtLoop2_.PositionA.AssertFinger((Finger)0);
                Debug.Assert(!Conv_Masu.OnKomabukuro(
                    Conv_Masu.ToMasuHandle(
                        Conv_Busstop.ToMasu(this.m_kifu_AtLoop2_.PositionA.BusstopIndexOf((Finger)0))
                        )
                    ), "駒が駒袋にあった。");
            }

            // goの属性一覧
            {
                this.GoProperties_AtLoop2 = new Dictionary<string, string>();
                this.GoProperties_AtLoop2["btime"] = "";
                this.GoProperties_AtLoop2["wtime"] = "";
                this.GoProperties_AtLoop2["byoyomi"] = "";
            }

            // go ponderの属性一覧
            {
                this.GoPonderNow_AtLoop2 = false;   // go ponderを将棋所に伝えたなら真
            }

            // gameoverの属性一覧
            {
                this.GameoverProperties_AtLoop2 = new Dictionary<string, string>();
                this.GameoverProperties_AtLoop2["gameover"] = "";
            }


            // アプリケーション開始時
            usiFramework.OnApplicationBegin = this.OnApplicationBegin;

            // 準備時
            usiFramework.OnUsiReceived_AtLoop1Body = this.OnUsiReceived_AtLoop1Body;
            usiFramework.OnSetoptionReceived_AtLoop1Body = this.OnSetoptionReceived_AtLoop1Body;
            usiFramework.OnIsreadyReceived_AtLoop1Body = this.OnIsreadyReceived_AtLoop1Body;
            usiFramework.OnUsinewgameReceived_AtLoop1Body = this.OnUsinewgameReceived_AtLoop1Body;
            usiFramework.OnQuitReceived_AtLoop1Body = this.OnQuitReceived_AtLoop1Body;
            usiFramework.OnCommandlineRead_AtLoop1Body = this.OnCommandlineRead_AtLoop1Body;

            // 対局開始時
            usiFramework.OnLoop2Begin = this.OnLoop2Begin;
            // 対局中
            usiFramework.OnCommandlineRead_AtLoop2Body = this.OnCommandlineRead_AtLoop2Body;

            usiFramework.OnPositionReceived_AtLoop2Body = this.OnPositionReceived_AtLoop2Body;
            usiFramework.OnGoponderReceived_AtLoop2Body = this.OnGoponderReceived_AtLoop2Body;
            usiFramework.OnGoReceived_AtLoop2Body = this.OnGoReceived_AtLoop2Body;
            usiFramework.OnStopReceived_AtLoop2Body = this.OnStopReceived_AtLoop2Body;
            usiFramework.OnGameoverReceived_AtLoop2Body = this.OnGameoverReceived_AtLoop2Body;
            usiFramework.OnLogdaseReceived_AtLoop2Body = this.OnLogdaseReceived_AtLoop2Body;
            // 対局終了時
            usiFramework.OnLoop2End = this.OnLoop2End;
            // アプリケーション終了時
            usiFramework.OnApplicationEnd = this.OnApplicationEnd;
        }
        #endregion



        #region プロパティー
        public ILogger Logger { get; set; }

        /// <summary>
        /// 読み筋を格納する配列の容量。
        /// </summary>
        public const int SEARCHED_PV_LENGTH = 2048;

        /// <summary>
        /// 将棋エンジンの中の一大要素「思考エンジン」です。
        /// 指す１手の答えを出すのが仕事です。
        /// </summary>
        public Shogisasi Shogisasi { get; set; }

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        public EngineOptions EngineOptions { get; set; }


        /// <summary>
        /// 棋譜です。
        /// </summary>
        public Tree Kifu_AtLoop2 { get { return this.m_kifu_AtLoop2_; } }
        /*
        public Sky PositionA { get {
                return this.Kifu_AtLoop2.CurNode1.GetNodeValue();
                //return this.m_positionA_;
            } }
        */
        public void SetKifu_AtLoop2(Tree kifu)
        {
            this.m_kifu_AtLoop2_ = kifu;
            //this.m_positionA_ = kifu.GetSky();
        }
        //private Sky m_positionA_;
        private Tree m_kifu_AtLoop2_;

        public Earth Earth_AtLoop2 { get { return this.m_earth_AtLoop2_; } }
        public void SetEarth_AtLoop2(Earth earth1)
        {
            this.m_earth_AtLoop2_ = earth1;
        }
        private Earth m_earth_AtLoop2_;


        /// <summary>
        /// 「go」の属性一覧です。
        /// </summary>
        public Dictionary<string, string> GoProperties_AtLoop2 { get; set; }


        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        public bool GoPonderNow_AtLoop2 { get; set; }


        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// </summary>
        public Dictionary<string, string> GameoverProperties_AtLoop2 { get; set; }

        #endregion


        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        public void Send(string line)
        {
            // 将棋サーバーに向かってメッセージを送り出します。
            Util_Message.Upload(line);

#if DEBUG
            // 送信記録をつけます。
            Util_Loggers.ProcessEngine_NETWORK.AppendLine(line);
            Util_Loggers.ProcessEngine_NETWORK.Flush(LogTypes.ToServer);
#endif
        }
        #endregion










        private PhaseResultUsiLoop1 OnUsiReceived_AtLoop1Body(string line)
        {
            //------------------------------------------------------------
            // あなたは USI ですか？
            //------------------------------------------------------------
            #region ↓詳説
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 1:31:35> usi
            //      │
            //
            //
            // 将棋所で [対局(G)]-[エンジン管理...]-[追加...] でファイルを選んだときに、
            // 送られてくる文字が usi です。
            #endregion


            //------------------------------------------------------------
            // エンジン設定ダイアログボックスを作ります
            //------------------------------------------------------------
            #region ↓詳説
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 23:40:15< option name 子 type check default true
            //      │2014/08/02 23:40:15< option name USI type spin default 2 min 1 max 13
            //      │2014/08/02 23:40:15< option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー
            //      │2014/08/02 23:40:15< option name 卯 type button default うさぎ
            //      │2014/08/02 23:40:15< option name 辰 type string default DRAGON
            //      │2014/08/02 23:40:15< option name 巳 type filename default スネーク.html
            //      │
            //
            //
            // 将棋所で [エンジン設定] ボタンを押したときに出てくるダイアログボックスに、
            //      ・チェックボックス
            //      ・スピン
            //      ・コンボボックス
            //      ・ボタン
            //      ・テキストボックス
            //      ・ファイル選択テキストボックス
            // を置くことができます。
            //
            #endregion
            this.Send("option name 子 type check default true");
            this.Send("option name USI type spin default 2 min 1 max 13");
            this.Send("option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー");
            this.Send("option name 卯 type button default うさぎ");
            this.Send("option name 辰 type string default DRAGON");
            this.Send("option name 巳 type filename default スネーク.html");


            //------------------------------------------------------------
            // USI です！！
            //------------------------------------------------------------
            #region ↓詳説
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 2:03:33< id name fugafuga 1.00.0
            //      │2014/08/02 2:03:33< id author hogehoge
            //      │2014/08/02 2:03:33< usiok
            //      │
            //
            // プログラム名と、作者名を送り返す必要があります。
            // オプションも送り返せば、受け取ってくれます。
            // usi を受け取ってから、5秒以内に usiok を送り返して完了です。
            #endregion
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
            var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");
            this.Send($"id name {engineName}");
            this.Send($"id author {engineAuthor}");
            this.Send("usiok");

            return PhaseResultUsiLoop1.None;
        }


        private PhaseResultUsiLoop1 OnSetoptionReceived_AtLoop1Body(string line)
        {
            //------------------------------------------------------------
            // 設定してください
            //------------------------------------------------------------
            #region ↓詳説
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
            #endregion

            //------------------------------------------------------------
            // 設定を一覧表に変えます
            //------------------------------------------------------------
            #region ↓詳説
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
            #endregion
            Regex regex = new Regex(@"setoption name ([^ ]+)(?: value (.*))?", RegexOptions.Singleline);
            Match m = regex.Match(line);

            if (m.Success)
            {
                // 項目を設定します。未定義の項目の場合、文字列型として新規追加します。
                this.EngineOptions.AddOption_ByCommandline(line);
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

            return PhaseResultUsiLoop1.None;
        }


        private PhaseResultUsiLoop1 OnIsreadyReceived_AtLoop1Body(string line)
        {
            //------------------------------------------------------------
            // それでは定刻になりましたので……
            //------------------------------------------------------------
            #region ↓詳説
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 1:31:35> isready
            //      │
            //
            //
            // 対局開始前に、将棋所から送られてくる文字が isready です。
            #endregion


            //------------------------------------------------------------
            // 将棋エンジン「おっおっ、設定を終わらせておかなければ（汗、汗…）」
            //------------------------------------------------------------
#if DEBUG
            // ログ出力
            Util_Loggers.ProcessEngine_DEFAULT.AppendLine(this.EngineOptions.ToString());
            Util_Loggers.ProcessEngine_DEFAULT.Flush(LogTypes.Plain);
#endif

            //------------------------------------------------------------
            // よろしくお願いします(^▽^)！
            //------------------------------------------------------------
            #region ↓詳説
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 2:03:33< readyok
            //      │
            //
            //
            // いつでも対局する準備が整っていましたら、 readyok を送り返します。
            #endregion
            this.Send("readyok");

            return PhaseResultUsiLoop1.None;
        }

        private PhaseResultUsiLoop1 OnUsinewgameReceived_AtLoop1Body(string line)
        {
            //------------------------------------------------------------
            // 対局時計が ポチッ とされました
            //------------------------------------------------------------
            #region ↓詳説
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 2:03:33> usinewgame
            //      │
            //
            //
            // 対局が始まったときに送られてくる文字が usinewgame です。
            #endregion


            // 無限ループ（１つ目）を抜けます。無限ループ（２つ目）に進みます。
            return PhaseResultUsiLoop1.Break;
        }

        private PhaseResultUsiLoop1 OnQuitReceived_AtLoop1Body(string line)
        {
            //------------------------------------------------------------
            // おつかれさまでした
            //------------------------------------------------------------
            #region ↓詳説
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 1:31:38> quit
            //      │
            //
            //
            // 将棋エンジンを止めるときに送られてくる文字が quit です。
            #endregion


            //------------------------------------------------------------
            // ﾉｼ
            //------------------------------------------------------------
            #region ↓詳説
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 3:08:34> (^-^)ﾉｼ
            //      │
            //
            //
            #endregion
#if DEBUG
            Util_Loggers.ProcessEngine_DEFAULT.AppendLine("(^-^)ﾉｼ");
            Util_Loggers.ProcessEngine_DEFAULT.Flush(LogTypes.Plain);
#endif


            // このプログラムを終了します。
            return PhaseResultUsiLoop1.Quit;
        }


        private string OnCommandlineRead_AtLoop1Body()
        {
            // 将棋サーバーから何かメッセージが届いていないか、見てみます。
            string line = Util_Message.Download_Nonstop();

            if (null != line)
            {
                // 通信ログは必ず取ります。
                ErrorControllerReference.ProcessEngineNetwork.AppendLine(line);
                ErrorControllerReference.ProcessEngineNetwork.Flush(LogTypes.ToClient);
            }

            return line;
        }


        private void OnLoop2Begin()
        {
            this.Shogisasi.OnTaikyokuKaisi();//対局開始時の処理。
        }


        private string OnCommandlineRead_AtLoop2Body()
        {
            //ノンストップ版
            //string line = TimeoutReader.ReadLine(1000);//指定ミリ秒だけブロック

            //通常版
            string line = System.Console.In.ReadLine();

            if (null != line)
            {
                // 通信ログは必ず取ります。
                this.Logger.AppendLine(line);
                this.Logger.Flush(LogTypes.ToClient);

#if NOOPABLE
                if (this.owner.Option_enable_serverNoopable)
                {
                    noopTimer._03_AtResponsed(this.owner, line, errH);
                }
#endif
            }

            return line;
        }

        private PhaseResultUsiLoop2 OnPositionReceived_AtLoop2Body(string line)
        {
            ILogger logger = ErrorControllerReference.ProcessEngineDefault;

            try
            {
                //------------------------------------------------------------
                // これが棋譜です
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> position startpos moves 2g2f
                //      │
                //
                // ↑↓この将棋エンジンは後手で、平手初期局面から、先手が初手  ▲２六歩  を指されたことが分かります。
                //
                //        ９  ８  ７  ６  ５  ４  ３  ２  １                 ９  ８  ７  ６  ５  ４  ３  ２  １
                //      ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐             ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //      │香│桂│銀│金│玉│金│銀│桂│香│一           │ｌ│ｎ│ｓ│ｇ│ｋ│ｇ│ｓ│ｎ│ｌ│ａ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │飛│  │  │  │  │  │角│  │二           │  │ｒ│  │  │  │  │  │ｂ│  │ｂ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │歩│歩│歩│歩│歩│歩│歩│歩│歩│三           │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │  │  │四           │  │  │  │  │  │  │  │  │  │ｄ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │  │  │五           │  │  │  │  │  │  │  │  │  │ｅ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │歩│  │六           │  │  │  │  │  │  │  │Ｐ│  │ｆ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │歩│歩│歩│歩│歩│歩│歩│  │歩│七           │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│  │Ｐ│ｇ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │角│  │  │  │  │  │飛│  │八           │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │香│桂│銀│金│玉│金│銀│桂│香│九           │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ
                //      └─┴─┴─┴─┴─┴─┴─┴─┴─┘             └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                //
                // または
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> position sfen lnsgkgsnl/9/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL w - 1 moves 5a6b 7g7f 3a3b
                //      │
                //
                // ↑↓将棋所のサンプルによると、“２枚落ち初期局面から△６二玉、▲７六歩、△３二銀と進んだ局面”とのことです。
                //
                //                                           ＜初期局面＞    ９  ８  ７  ６  ５  ４  ３  ２  １
                //                                                         ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //                                                         │ｌ│ｎ│ｓ│ｇ│ｋ│ｇ│ｓ│ｎ│ｌ│ａ  ←lnsgkgsnl
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │  │  │  │  │  │  │  │  │ｂ  ←9
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ  ←ppppppppp
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │  │  │  │  │  │  │  │  │ｄ  ←9
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │  │  │  │  │  │  │  │  │ｅ  ←9
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │  │  │  │  │  │  │  │  │ｆ  ←9
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│ｇ  ←PPPPPPPPP
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ  ←1B5R1
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ  ←LNSGKGSNL
                //                                                         └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                //
                //        ９  ８  ７  ６  ５  ４  ３  ２  １   ＜３手目＞    ９  ８  ７  ６  ５  ４  ３  ２  １
                //      ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐             ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //      │香│桂│銀│金│  │金│  │桂│香│一           │ｌ│ｎ│ｓ│ｇ│  │ｇ│  │ｎ│ｌ│ａ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │玉│  │  │銀│  │  │二           │  │  │  │ｋ│  │  │ｓ│  │  │ｂ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │歩│歩│歩│歩│歩│歩│歩│歩│歩│三           │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │  │  │四           │  │  │  │  │  │  │  │  │  │ｄ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │  │  │五           │  │  │  │  │  │  │  │  │  │ｅ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │歩│  │  │  │  │  │  │六           │  │  │Ｐ│  │  │  │  │  │  │ｆ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │歩│歩│  │歩│歩│歩│歩│歩│歩│七           │Ｐ│Ｐ│  │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│ｇ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │角│  │  │  │  │  │飛│  │八           │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │香│桂│銀│金│玉│金│銀│桂│香│九           │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ
                //      └─┴─┴─┴─┴─┴─┴─┴─┴─┘             └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                //

                // 手番になったときに、“まず”、将棋所から送られてくる文字が position です。
                // このメッセージを読むと、駒の配置が分かります。
                //
                // “が”、まだ指してはいけません。
                #endregion
#if DEBUG
                this.Log1_AtLoop2("（＾△＾）positionきたｺﾚ！");
#endif
                // 入力行を解析します。
                IKifuParserAResult result = new KifuParserA_ResultImpl();
                KifuParserAImpl kifuParserA = new KifuParserAImpl();
                IKifuParserAGenjo genjo = new KifuParserA_GenjoImpl(line);
                kifuParserA.Execute_All_CurrentMutable(
                    ref result,

                    this.Earth_AtLoop2,
                    this.Kifu_AtLoop2,

                    genjo,
                    logger
                    );
                if (null != genjo.StartposImporter_OrNull)
                {
                    // SFENの解析結果を渡すので、
                    // その解析結果をどう使うかは、委譲します。
                    Util_InClient.OnChangeSky_Im_Client(

                        this.Earth_AtLoop2,
                        this.Kifu_AtLoop2,

                        genjo,
                        logger
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
                #region ↓詳説
                //
                // 応答は無用です。
                // 多分、将棋所もまだ準備ができていないのではないでしょうか（？）
                //
                #endregion
            }
            catch (Exception ex)
            {
                // エラー：どうにもできないので  ログだけ取って無視します。
                ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta("Program「position」：" + ex.GetType().Name + "：" + ex.Message);
                throw;
            }

            return PhaseResultUsiLoop2.None;
        }

        private PhaseResultUsiLoop2 OnGoponderReceived_AtLoop2Body(string line)
        {
            try
            {

                //------------------------------------------------------------
                // 将棋所が次に呼びかけるまで、考えていてください
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> go ponder
                //      │
                //

                // 先読み用です。
                // 今回のプログラムでは対応しません。
                //
                // 将棋エンジンが  将棋所に向かって  「bestmove ★ ponder ★」といったメッセージを送ったとき、
                // 将棋所は「go ponder」というメッセージを返してくると思います。
                //
                // 恐らく  このメッセージを受け取っても、将棋エンジンは気にせず  考え続けていればいいのではないでしょうか。
                #endregion


                //------------------------------------------------------------
                // じっとがまん
                //------------------------------------------------------------
                #region ↓詳説
                //
                // まだ指してはいけません。
                // 指したら反則です。相手はまだ指していないのだ☆ｗ
                //
                #endregion
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta("Program「go ponder」：" + ex.GetType().Name + "：" + ex.Message);
                throw;
            }

            return PhaseResultUsiLoop2.None;
        }

        private PhaseResultUsiLoop2 OnGoReceived_AtLoop2Body(string line)
        {
            int exceptionArea = 0;

            try
            {

                exceptionArea = 1000;
                //------------------------------------------------------------
                // あなたの手番です
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:36:19> go btime 599000 wtime 600000 byoyomi 60000
                //      │
                //
                // もう指していいときに、将棋所から送られてくる文字が go です。
                //
                #endregion


                //------------------------------------------------------------
                // 先手 3:00  後手 0:00  記録係「50秒ぉ～」
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 上図のメッセージのままだと使いにくいので、
                // あとで使いやすいように Key と Value の表に分けて持ち直します。
                //
                // 図.
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
                //      単位はミリ秒ですので、599000 は 59.9秒 です。
                //
                #endregion
                Regex regex = new Regex(@"go btime (\d+) wtime (\d+) byoyomi (\d+)", RegexOptions.Singleline);
                Match m = regex.Match(line);

                if (m.Success)
                {
                    this.GoProperties_AtLoop2["btime"] = (string)m.Groups[1].Value;
                    this.GoProperties_AtLoop2["wtime"] = (string)m.Groups[2].Value;
                    this.GoProperties_AtLoop2["byoyomi"] = (string)m.Groups[3].Value;
                }
                else
                {
                    this.GoProperties_AtLoop2["btime"] = "";
                    this.GoProperties_AtLoop2["wtime"] = "";
                    this.GoProperties_AtLoop2["byoyomi"] = "";
                }



                //----------------------------------------
                // 棋譜ツリー、局面データは、position コマンドで先に与えられているものとします。
                //----------------------------------------

                // ┏━━━━プログラム━━━━┓

                MoveEx curNode1 = this.Kifu_AtLoop2.MoveEx_Current;
                ISky positionA = this.Kifu_AtLoop2.PositionA;
                int latestTemezumi = positionA.Temezumi;//現・手目済// curNode1.GetNodeValue()

                //#if DEBUG
                // MessageBox.Show("["+latestTemezumi+"]手目済　["+this.owner.PlayerInfo.Playerside+"]の手番");
                //#endif


                bool test = true;
                if (test)
                {
                    this.Logger.AppendLine("サーバーから受信した局面☆（＾▽＾）");
                    this.Logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(
                        ConvMove.ToPlayerside(curNode1.Move),
                        positionA, Logger)));
                    this.Logger.Flush(LogTypes.Plain);
                }

                //errH2.Logger.WriteLine_AddMemo("将棋サーバー「" + latestTemezumi + "手目、きふわらべ　さんの手番ですよ！」　" + line);


                //----------------------------------------
                // 王の状態を調べます。
                //----------------------------------------
                Result_KingState result_kingState;
                {
                    result_kingState = Result_KingState.Empty;

                    positionA.AssertFinger(Finger_Honshogi.SenteOh);
                    Busstop king1p = positionA.BusstopIndexOf(Finger_Honshogi.SenteOh);

                    positionA.AssertFinger(Finger_Honshogi.GoteOh);
                    Busstop king2p = positionA.BusstopIndexOf(Finger_Honshogi.GoteOh);
                    //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("将棋サーバー「ではここで、王さまがどこにいるか確認してみましょう」");
                    //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("▲王の置き場＝" + Conv_Masu.Masu_ToOkiba(koma1.Masu));
                    //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("△王の置き場＝" + Conv_Masu.Masu_ToOkiba(koma2.Masu));

                    if (Conv_Busstop.ToOkiba(king1p) != Okiba.ShogiBan)
                    {
                        // 先手の王さまが将棋盤上にいないとき☆
                        result_kingState = Result_KingState.Lost_SenteOh;
                    }
                    else if (Conv_Busstop.ToOkiba(king2p) != Okiba.ShogiBan)
                    {
                        // または、後手の王さまが将棋盤上にいないとき☆
                        result_kingState = Result_KingState.Lost_GoteOh;
                    }
                    else
                    {
                        result_kingState = Result_KingState.Empty;
                    }
                }

                exceptionArea = 2000;
                //------------------------------------------------------------
                // わたしの手番のとき、王様が　将棋盤上からいなくなっていれば、投了します。
                //------------------------------------------------------------
                //
                //      将棋ＧＵＩ『きふならべ』用☆　将棋盤上に王さまがいないときに、本将棋で　go　コマンドが送られてくることは無いのでは☆？
                //
                switch (result_kingState)
                {
                    case Result_KingState.Lost_SenteOh:// 先手の王さまが将棋盤上にいないとき☆
                    case Result_KingState.Lost_GoteOh:// または、後手の王さまが将棋盤上にいないとき☆
                        {
                            //------------------------------------------------------------
                            // 投了
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 2:36:21< bestmove resign
                            //      │
                            //

                            // この将棋エンジンは、後手とします。
                            // ２０手目、投了  を決め打ちで返します。
                            #endregion
                            this.Send("bestmove resign");//投了
                        }
                        break;
                    default:// どちらの王さまも、まだまだ健在だぜ☆！
                        {
                            List<MoveEx> multiPvNodeExList = new List<MoveEx>();

                            exceptionArea = 2100;
                            //------------------------------------------------------------
                            // 指し手のチョイス
                            //------------------------------------------------------------
                            bool isHonshogi = true;



                            //------------------------------------------------------------
                            // MultiPV のテスト中☆
                            //------------------------------------------------------------
                            //
                            // 指し手を決めます。
                            // TODO: その指し手の評価値がいくらだったのか調べたい。
                            //
                            // FIXME: ログがＭｕｌｔｉＰＶ別になっていないので、混ざって、同じ手を２度指しているみたいに見えてしまう☆
                            //
                            int searchedMaxDepth = 0;
                            ulong searchedNodes = 0;
                            string[] searchedPv = new string[KifuWarabeImpl.SEARCHED_PV_LENGTH];
                            int multiPV_Count = 1;// 2;
                            {
                                // 最善手、次善手、三次善手、四次善手、五次善手
                                for (int iMultiPV = 0; iMultiPV < multiPV_Count; iMultiPV++)
                                {
                                    // null を返すことがある？
                                    multiPvNodeExList.Add(this.Shogisasi.WA_Bestmove(
                                        ref searchedMaxDepth,
                                        ref searchedNodes,
                                        searchedPv,
                                        isHonshogi,

                                        this.Earth_AtLoop2,
                                        this.Kifu_AtLoop2,// ツリーを伸ばしているぜ☆（＾～＾）
                                        this.Kifu_AtLoop2.PositionA.GetKaisiPside(),
                                        this.Kifu_AtLoop2.PositionA,//.CurNode1.GetNodeValue(),

                                        this.Logger)
                                        );

                                    this.Kifu_AtLoop2.MoveEx_SetCurrent(TreeImpl.OnDoCurrentMove(this.Kifu_AtLoop2.MoveEx_Current, this.Kifu_AtLoop2, this.Kifu_AtLoop2.PositionA, this.Logger));
                                }


#if DEBUG
                                //// 内容をログ出力
                                //// 最善手、次善手、三次善手、四次善手、五次善手
                                //StringBuilder sb = new StringBuilder();
                                //for (int iMultiPV = 0; iMultiPV < 5; iMultiPV++)
                                //{
                                //    string sfenText = Util_Sky.ToSfenSasiteText(bestSasiteList[iMultiPV]);
                                //    sb.AppendLine("[" + iMultiPV + "]" + sfenText);
                                //}
                                //System.Windows.Forms.MessageBox.Show(sb.ToString());
#endif
                            }

                            exceptionArea = 2200;
                            Move bestmove = Move.Empty;
                            // 最善手、次善手、三次善手、四次善手、五次善手
                            float bestScore = float.MinValue;
                            for (int iMultiPV = 0; iMultiPV < multiPvNodeExList.Count; iMultiPV++)
                            {
                                MoveEx nodeEx = multiPvNodeExList[iMultiPV];

                                if (
                                    null != nodeEx // 投了か？
                                    && bestScore <= nodeEx.Score)
                                {
                                    bestScore = nodeEx.Score;
                                    bestmove = nodeEx.Move;
                                }
                            }

                            exceptionArea = 2300;

                            exceptionArea = 2400;

                            if (
                                // 投了ではなく
                                Move.Empty != bestmove
                                //&&
                                // src,dstが指定されていれば。
                                //Util_Sky_BoolQuery.isEnableSfen(bestKifuNode.Key)
                                )
                            {
                                // Ｍｏｖｅを使っていきたい。
                                string sfenText = ConvMove.ToSfen(bestmove);

                                // ログが重過ぎる☆！
                                //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("(Warabe)指し手のチョイス： bestmove＝[" + sfenText + "]" +
                                //    "　棋譜＝" + KirokuGakari.ToJsaKifuText(this.Kifu, errH2));

                                //----------------------------------------
                                // スコア 試し
                                //----------------------------------------
                                {
                                    int hyojiScore = (int)bestScore;
                                    if (this.Kifu_AtLoop2.PositionA.GetKaisiPside() == Playerside.P2)
                                    {
                                        // 符号を逆転
                                        hyojiScore = -hyojiScore;
                                    }

                                    // infostring
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("info time ");
                                    sb.Append(this.Shogisasi.TimeManager.Stopwatch.ElapsedMilliseconds);
                                    sb.Append(" depth ");
                                    sb.Append(searchedMaxDepth);
                                    sb.Append(" nodes ");
                                    sb.Append(searchedNodes);
                                    sb.Append(" score cp ");
                                    sb.Append(hyojiScore.ToString());
                                    sb.Append(" pv ");//+ " pv 3a3b L*4h 4c4d"
                                    foreach (string sfen in searchedPv)
                                    {
                                        if ("" != sfen)
                                        {
                                            sb.Append(sfen);
                                            sb.Append(" ");
                                        }
                                    }
                                    this.Send(sb.ToString());//FIXME:                                                                                           
                                }


                                //----------------------------------------
                                // 指し手を送ります。
                                //----------------------------------------
                                this.Send("bestmove " + sfenText);
                            }
                            else // 指し手がないときは、SFENが書けない☆　投了だぜ☆
                            {
                                // ログが重過ぎる☆！
                                //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("(Warabe)指し手のチョイス： 指し手がないときは、SFENが書けない☆　投了だぜ☆ｗｗ（＞＿＜）" +
                                //    "　棋譜＝" + KirokuGakari.ToJsaKifuText(this.Kifu, errH2));

                                //----------------------------------------
                                // 投了ｗ！
                                //----------------------------------------
                                this.Send("bestmove resign");
                            }


                            exceptionArea = 2500;
                            /*
                            //------------------------------------------------------------
                            // 以前の手カッター
                            //------------------------------------------------------------
                            Util_KifuTree282.IzennoHenkaCutter(
                                this.Kifu_AtLoop2, this.Logger);
                            */
                        }
                        break;
                }
                // ┗━━━━プログラム━━━━┛


            }
            catch (Exception ex)
            {
                switch (exceptionArea)
                {
                    case 2100:
                        {
                            ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta(ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの１０です。");
                            throw;
                        }
                    case 2200:
                        {
                            ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta(ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの４０です。");
                            throw;
                        }
                    case 2300:
                        {
                            ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta(ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの５０です。");
                            throw;
                        }
                    case 2400:
                        {
                            ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta(ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの９０です。");
                            throw;
                        }
                    default:
                        {
                            // エラーが起こりました。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            // どうにもできないので  ログだけ取って無視します。
                            ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta("Program「go」：" + ex.GetType().Name + " " + ex.Message + "：goを受け取ったときです。：");
                            throw;//追加
                        }
                        //break;
                }
            }


            //System.C onsole.WriteLine();

            //throw new Exception("デバッグだぜ☆！　エラーはキャッチできたかな～☆？（＾▽＾）");
            return PhaseResultUsiLoop2.None;
        }


        private PhaseResultUsiLoop2 OnStopReceived_AtLoop2Body(string line)
        {
            try
            {

                //------------------------------------------------------------
                // あなたの手番です  （すぐ指してください！）
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> stop
                //      │
                //

                // 何らかの理由で  すぐ指してほしいときに、将棋所から送られてくる文字が stop です。
                //
                // 理由は２つ考えることができます。
                //  （１）１手前に、将棋エンジンが  将棋所に向かって「予想手」付きで指し手を伝えたのだが、
                //        相手の応手が「予想手」とは違ったので、予想手にもとづく思考を  今すぐ変えて欲しいとき。
                //
                //  （２）「急いで指すボタン」が押されたときなどに送られてくるようです？
                //
                // stop するのは思考です。  stop を受け取ったら  すぐに最善手を指してください。
                #endregion

                if (this.GoPonderNow_AtLoop2)
                {
                    //------------------------------------------------------------
                    // 将棋エンジン「（予想手が間違っていたって？）  △９二香 を指そうと思っていたんだが」
                    //------------------------------------------------------------
                    #region ↓詳説
                    //
                    // 図.
                    //
                    //      log.txt
                    //      ┌────────────────────────────────────────
                    //      ～
                    //      │2014/08/02 2:36:21< bestmove 9a9b
                    //      │
                    //
                    //
                    //      １手前の指し手で、将棋エンジンが「bestmove ★ ponder ★」という形で  予想手付きで将棋所にメッセージを送っていたとき、
                    //      その予想手が外れていたならば、将棋所は「stop」を返してきます。
                    //      このとき  思考を打ち切って最善手の指し手をすぐに返信するわけですが、将棋所はこの返信を無視します☆ｗ
                    //      （この指し手は、外れていた予想手について考えていた“最善手”ですからゴミのように捨てられます）
                    //      その後、将棋所から「position」「go」が再送されてくるのだと思います。
                    //
                    //          将棋エンジン「bestmove ★ ponder ★」
                    //              ↓
                    //          将棋所      「stop」
                    //              ↓
                    //          将棋エンジン「うその指し手返信」（無視されます）←今ここ
                    //              ↓
                    //          将棋所      「position」「go」
                    //              ↓
                    //          将棋エンジン「本当の指し手」
                    //
                    //      という流れと思います。
                    #endregion
                    // この指し手は、無視されます。（無視されますが、送る必要があります）
                    this.Send("bestmove 9a9b");
                }
                else
                {
                    //------------------------------------------------------------
                    // じゃあ、△９二香で
                    //------------------------------------------------------------
                    #region ↓詳説
                    //
                    // 図.
                    //
                    //      log.txt
                    //      ┌────────────────────────────────────────
                    //      ～
                    //      │2014/08/02 2:36:21< bestmove 9a9b
                    //      │
                    //
                    //
                    // 特に何もなく、すぐ指せというのですから、今考えている最善手をすぐに指します。
                    #endregion
                    this.Send("bestmove 9a9b");
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta("Program「stop」：" + ex.GetType().Name + " " + ex.Message);
                throw;//追加
            }

            return PhaseResultUsiLoop2.None;
        }


        private PhaseResultUsiLoop2 OnGameoverReceived_AtLoop2Body(string line)
        {
            try
            {
                //------------------------------------------------------------
                // 対局が終わりました
                //------------------------------------------------------------
                #region ↓詳説
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
                #endregion

                //------------------------------------------------------------
                // 「あ、勝ちました」「あ、引き分けました」「あ、負けました」
                //------------------------------------------------------------
                #region ↓詳説
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
                #endregion
                Regex regex = new Regex(@"gameover (.)", RegexOptions.Singleline);
                Match m = regex.Match(line);

                if (m.Success)
                {
                    this.GameoverProperties_AtLoop2["gameover"] = (string)m.Groups[1].Value;
                }
                else
                {
                    this.GameoverProperties_AtLoop2["gameover"] = "";
                }


                // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                return PhaseResultUsiLoop2.Break;
            }
            catch (Exception ex)
            {
                // エラー続行
                ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta(ex, "Program「gameover」：" + ex.GetType().Name + " " + ex.Message);
                return PhaseResultUsiLoop2.None;
            }
        }

        private PhaseResultUsiLoop2 OnLogdaseReceived_AtLoop2Body(string line)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            StringBuilder sb = new StringBuilder();
            sb.Append("ログ出せ機能は廃止だぜ～☆（＾▽＾）");
            File.WriteAllText(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("LogDaseMeirei")), sb.ToString());

            return PhaseResultUsiLoop2.None;
        }

        private void OnLoop2End()
        {
            //-------------------+----------------------------------------------------------------------------------------------------
            // スナップショット  |
            //-------------------+----------------------------------------------------------------------------------------------------
            #region ↓詳説
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
            #endregion
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
        }


#if DEBUG
        private void Log1_AtLoop2(string message)
        {
            Util_Loggers.ProcessEngine_DEFAULT.AppendLine(message);
            Util_Loggers.ProcessEngine_DEFAULT.Flush(LogTypes.Plain);
        }
        private void Log2_Png_Tyokkin_AtLoop2(string line, Move move_forLog, Sky sky, KwLogger errH)
        {
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
                string fileName = "_log_直近の指し手.png";

                SyElement srcMasu = Conv_Move.ToSrcMasu(move_forLog);
                SyElement dstMasu = Conv_Move.ToDstMasu(move_forLog);
                Komasyurui14 captured = Conv_Move.ToCaptured(move_forLog);
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
                    Conv_KifuNode.ToRO_Kyokumen1(sky, errH),
                    srcMasuNum,
                    dstMasuNum,
                    foodKoma,
                    Conv_Move.ToSfen(move_forLog),
                    "",
                    fileName,
                    Util_KifuTreeLogWriter.REPORT_ENVIRONMENT,
                    errH
                    );
            }
        }
#endif


        #region 処理の流れ
        public void OnApplicationBegin()
        {
            int exception_area = 0;
            try
            {
                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                exception_area = 500;
                //-------------------+----------------------------------------------------------------------------------------------------
                // ログファイル削除  |
                //-------------------+----------------------------------------------------------------------------------------------------
                {
                    #region ↓詳説
                    //
                    // 図.
                    //
                    //      フォルダー
                    //          ├─ Engine.KifuWarabe.exe
                    //          └─ log.txt               ←これを削除
                    //
                    #endregion
                    ErrorControllerReference.RemoveAllLogFiles();
                }


                exception_area = 1000;
                //------------------------------------------------------------------------------------------------------------------------
                // 思考エンジンの、記憶を読み取ります。
                //------------------------------------------------------------------------------------------------------------------------
                {
                    this.Shogisasi = new ShogisasiImpl(this);
                    Util_FvLoad.OpenFv(this.Shogisasi.FeatureVector, Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Fv00Komawari")), this.Logger);
                }


                exception_area = 2000;
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

                exception_area = 4000;
                //-------------+----------------------------------------------------------------------------------------------------------
                // ログ書込み  |  ＜この将棋エンジン＞  製品名、バージョン番号
                //-------------+----------------------------------------------------------------------------------------------------------
                #region ↓詳説
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
                #endregion
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

            }
            catch (Exception ex)
            {
                switch (exception_area)
                {
                    case 1000:
                        ErrorControllerReference.ProcessEngineDefault.DonimoNaranAkirameta("フィーチャーベクターCSVを読み込んでいるとき。" + ex.GetType().Name + "：" + ex.Message);
                        throw;
                        //break;
                }
                throw;
            }
        }


        public void OnApplicationEnd()
        {
        }
        #endregion

    }
}
