using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Grayscale.A060Application.B210Tushin.C500Util;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B410SeizaFinger.C250Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B640_KifuTree___.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A500ShogiEngine.B200Scoreing.C240Shogisasi;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C125AjimiEngine;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.UseCases
{
    public class Playing
    {
        /// <summary>
        /// 棋譜です。
        /// Loop2で使います。
        /// </summary>
        public Tree Kifu { get { return this.m_kifu_; } }
        private Tree m_kifu_;

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
        /// 「go ponder」の属性一覧です。
        /// Loop2で呼ばれます。
        /// </summary>
        public bool GoPonderFlag { get; set; }

        public Playing()
        {
            //-------------+----------------------------------------------------------------------------------------------------------
            // データ設計  |
            //-------------+----------------------------------------------------------------------------------------------------------
            // 将棋所から送られてくるデータを、一覧表に変えたものです。
            this.EngineOptions = new EngineOptionsImpl();
            this.EngineOptions.AddOption(EngineOptionNames.USI_PONDER, new EngineOptionBoolImpl());// ポンダーに対応している将棋サーバーなら真です。
            this.EngineOptions.AddOption(EngineOptionNames.NOOPABLE, new EngineOptionBoolImpl());// 独自実装のコマンドなので、ＯＦＦにしておきます。
            this.EngineOptions.AddOption(EngineOptionNames.THINKING_MILLI_SECOND, new EngineOptionNumberImpl(30000));//30秒//90000//60000//8000//4000

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

            // 棋譜
            ISky positionInit = UtilSkyCreator.New_Hirate();// きふわらべ起動時
            {
                // FIXME:平手とは限らないが、平手という前提で作っておく。
                this.m_earth_ = new EarthImpl();
                this.m_kifu_ = new TreeImpl(positionInit);
                this.Earth.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");// 平手 // FIXME:平手とは限らないが。

                this.m_kifu_.PositionA.AssertFinger((Finger)0);
                Debug.Assert(!Conv_Masu.OnKomabukuro(
                    Conv_Masu.ToMasuHandle(
                        Conv_Busstop.ToMasu(this.m_kifu_.PositionA.BusstopIndexOf((Finger)0))
                        )
                    ), "駒が駒袋にあった。");
            }

            // go ponderの属性一覧
            {
                this.GoPonderFlag = false;   // go ponderを将棋所に伝えたなら真
            }
        }

        /// <summary>
        /// Loop2で呼ばれます。
        /// </summary>
        /// <param name="kifu"></param>
        public void SetKifu(Tree kifu)
        {
            this.m_kifu_ = kifu;
            //this.m_positionA_ = kifu.GetSky();
        }
        //private ISky m_positionA_;

        /// <summary>
        /// Loop2で使います。
        /// </summary>
        public Earth Earth { get { return this.m_earth_; } }
        public void SetEarth(Earth earth1)
        {
            this.m_earth_ = earth1;
        }
        private Earth m_earth_;

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        public EngineOptions EngineOptions { get; set; }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        public static void Send(string line)
        {
            // 将棋サーバーに向かってメッセージを送り出します。
            Util_Message.Upload(line);

#if DEBUG
            // 送信記録をつけます。
            Util_Loggers.ProcessEngine_NETWORK.AppendLine(line);
            Util_Loggers.ProcessEngine_NETWORK.Flush(LogTypes.ToServer);
#endif
        }

        public void UsiOk(string engineName, string engineAuthor)
        {
            //------------------------------------------------------------
            // あなたは USI ですか？
            //------------------------------------------------------------
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


            //------------------------------------------------------------
            // エンジン設定ダイアログボックスを作ります
            //------------------------------------------------------------
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
            Playing.Send("option name 子 type check default true");
            Playing.Send("option name USI type spin default 2 min 1 max 13");
            Playing.Send("option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー");
            Playing.Send("option name 卯 type button default うさぎ");
            Playing.Send("option name 辰 type string default DRAGON");
            Playing.Send("option name 巳 type filename default スネーク.html");


            //------------------------------------------------------------
            // USI です！！
            //------------------------------------------------------------
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
            Playing.Send($"id name {engineName}");
            Playing.Send($"id author {engineAuthor}");
            Playing.Send("usiok");
        }

        public void SetOption(string name, string value)
        {

        }

        /// <summary>
        /// 既存項目の場合、型に合わせて上書き。なければ文字列型として項目を新規追加。
        /// 
        /// GUIから思考エンジンへ送られてくる方は１つ目が "setoption"、思考エンジンからGUIへ送る方は"option"。
        /// setoptionの場合、プロパティは name と value だけになり、型情報がない（全て文字列型）。
        /// 
        /// チェックボックス
        /// "option name 子 type check default true"
        /// 
        /// スピンボックス
        /// "option name USI type spin default 2 min 1 max 13"
        /// 
        /// コンボボックス
        /// "option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー"
        /// 
        /// ボタン
        /// "option name 卯 type button default うさぎ"
        /// 
        /// 文字列
        /// "option name 辰 type string default DRAGON"
        /// 
        /// ファイル名
        /// "option name 巳 type filename default スネーク.html"
        /// </summary>
        /// <param name="line">コマンドライン</param>
        public void AddOption_ByCommandline(string line)
        {
            string[] tokens = line.Split(' ');
            int index = 0;
            if ("setoption" == tokens[index] || "option" == tokens[index])
            {
                // プロパティ名
                string propertyName = "";

                // プロパティ値
                string valueName = "";
                string valueType = "";
                string valueDefault = "";
                List<string> valueVars = new List<string>();
                string valueMin = "";
                string valueMax = "";

                // 部品ごとに、ばらばらにするぜ☆（＾▽＾）
                index++;
                for (; index < tokens.Length; index++)
                {
                    string token = tokens[index];

                    if ("" == propertyName)
                    {
                        // プロパティ名
                        if (token == "name" || token == "type" || token == "default" || token == "var" || token == "min" || token == "max")
                        {
                            propertyName = token;
                        }
                    }
                    else
                    {
                        // プロパティ値
                        switch (token)
                        {
                            case "name": valueName = token; break;
                            case "type": valueType = token; break;
                            case "default": valueDefault = token; break;
                            case "var": valueVars.Add(token); break;
                            case "min": valueMin = token; break;
                            case "max": valueMax = token; break;
                            default: break;
                        }
                    }
                }

                // 部品を組み立てるぜ☆（＾▽＾）
                IEngineOption option = null;
                switch (valueType)
                {
                    case "check": option = new EngineOptionBoolImpl(valueDefault); break;
                    case "spin": option = new EngineOptionSpinImpl(valueDefault, valueMin, valueMax); break;
                    case "combo": option = new EngineOptionComboImpl(valueDefault, valueVars); break;
                    case "button": option = new EngineOptionButtonImpl(valueDefault); break;
                    case "filename": option = new EngineOptionFilenameImpl(valueDefault); break;

                    case "string": //thru
                    default: option = new EngineOptionStringImpl(valueDefault); break;
                }

                // 同じものがすでにないか調べるぜ☆（＾▽＾）
                if (this.EngineOptions.ContainsKey(valueName))
                {
                    // 既に同じものがある場合。
                    bool typeCheck;
                    switch (valueType)
                    {
                        case "check": typeCheck = this.EngineOptions.m_entries_[valueName] is EngineOptionBoolImpl; break;
                        case "spin": typeCheck = this.EngineOptions.m_entries_[valueName] is EngineOptionSpinImpl; break;
                        case "combo": typeCheck = this.EngineOptions.m_entries_[valueName] is EngineOptionComboImpl; break;
                        case "button": typeCheck = this.EngineOptions.m_entries_[valueName] is EngineOptionButtonImpl; break;
                        case "filename": typeCheck = this.EngineOptions.m_entries_[valueName] is EngineOptionFilenameImpl; break;

                        case "string": //thru
                        default: typeCheck = this.EngineOptions.m_entries_[valueName] is EngineOptionStringImpl; break;
                    }

                    if (!typeCheck)
                    {
                        throw new ApplicationException("オプションの型変換エラー。");
                    }

                    this.EngineOptions.m_entries_[valueName].Reset(
                        valueDefault,
                        valueVars,
                        valueMin,
                        valueMax
                        );
                }
                else
                {
                    // 新規の場合
                    this.EngineOptions.m_entries_.Add(valueName, option);
                }
            }
        }

        public void ReadyOk()
        {
            //------------------------------------------------------------
            // それでは定刻になりましたので……
            //------------------------------------------------------------
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
            Playing.Send("readyok");
        }

        public void UsiNewGame()
        {
            //------------------------------------------------------------
            // 対局時計が ポチッ とされました
            //------------------------------------------------------------
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
        }

        public void Quit()
        {
            //------------------------------------------------------------
            // おつかれさまでした
            //------------------------------------------------------------
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

            //------------------------------------------------------------
            // ﾉｼ
            //------------------------------------------------------------
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
#if DEBUG
            Util_Loggers.ProcessEngine_DEFAULT.AppendLine("(^-^)ﾉｼ");
            Util_Loggers.ProcessEngine_DEFAULT.Flush(LogTypes.Plain);
#endif
        }

        public void Position()
        {
            //------------------------------------------------------------
            // これが棋譜です
            //------------------------------------------------------------
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
        }

        public void GoPonder()
        {
            //------------------------------------------------------------
            // 将棋所が次に呼びかけるまで、考えていてください
            //------------------------------------------------------------
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

            //------------------------------------------------------------
            // じっとがまん
            //------------------------------------------------------------
            //
            // まだ指してはいけません。
            // 指したら反則です。相手はまだ指していないのだ☆ｗ
            //
        }

        public void Go(string btime, string wtime, string byoyomi, string binc, string winc)
        {
            //------------------------------------------------------------
            // あなたの手番です
            //------------------------------------------------------------
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


            //------------------------------------------------------------
            // 先手 3:00  後手 0:00  記録係「50秒ぉ～」
            //------------------------------------------------------------
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

            //----------------------------------------
            // 棋譜ツリー、局面データは、position コマンドで先に与えられているものとします。
            //----------------------------------------

            // ┏━━━━プログラム━━━━┓

            MoveEx curNode1 = this.Kifu.MoveEx_Current;
            ISky positionA = this.Kifu.PositionA;
            int latestTemezumi = positionA.Temezumi;//現・手目済// curNode1.GetNodeValue()

            //#if DEBUG
            // MessageBox.Show("["+latestTemezumi+"]手目済　["+this.owner.PlayerInfo.Playerside+"]の手番");
            //#endif


            bool test = true;
            if (test)
            {
                Logger.AppendLine(LogTags.ProcessEngineDefault, "サーバーから受信した局面☆（＾▽＾）");
                Logger.AppendLine(LogTags.ProcessEngineDefault, Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(
                    ConvMove.ToPlayerside(curNode1.Move),
                    positionA, LogTags.ProcessEngineDefault)));
                Logger.Flush(LogTags.ProcessEngineDefault, LogTypes.Plain);
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
                        Playing.Send("bestmove resign");//投了
                    }
                    break;
                default:// どちらの王さまも、まだまだ健在だぜ☆！
                    {
                        List<MoveEx> multiPvNodeExList = new List<MoveEx>();

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
                        string[] searchedPv = new string[Playing.SEARCHED_PV_LENGTH];
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

                                    this.Earth,
                                    this.Kifu,// ツリーを伸ばしているぜ☆（＾～＾）
                                    this.Kifu.PositionA.GetKaisiPside(),
                                    this.Kifu.PositionA,//.CurNode1.GetNodeValue(),

                                    LogTags.ProcessEngineDefault)
                                    );

                                this.Kifu.MoveEx_SetCurrent(
                                    TreeImpl.OnDoCurrentMove(
                                        this.Kifu.MoveEx_Current,
                                        this.Kifu,
                                        this.Kifu.PositionA,
                                        LogTags.ProcessEngineDefault));
                            }


#if DEBUG
                                //// 内容をログ出力
                                //// 最善手、次善手、三次善手、四次善手、五次善手
                                //StringBuilder sb = new StringBuilder();
                                //for (int iMultiPV = 0; iMultiPV < 5; iMultiPV++)
                                //{
                                //    string sfenText = Util_Sky.ToSfenMoveText(bestMoveList[iMultiPV]);
                                //    sb.AppendLine("[" + iMultiPV + "]" + sfenText);
                                //}
                                //System.Windows.Forms.MessageBox.Show(sb.ToString());
#endif
                        }

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
                                if (this.Kifu.PositionA.GetKaisiPside() == Playerside.P2)
                                {
                                    // 符号を逆転
                                    hyojiScore = -hyojiScore;
                                }

                                // infostring
                                StringBuilder sb = new StringBuilder();
                                sb.Append($"info time {this.Shogisasi.TimeManager.Stopwatch.ElapsedMilliseconds} depth {searchedMaxDepth} nodes {searchedNodes} score cp {hyojiScore.ToString()} pv ");
                                //+ " pv 3a3b L*4h 4c4d"
                                foreach (string sfen1 in searchedPv)
                                {
                                    // (2020-12-13 sun)余計な空白を付けていたので削ったが、もう少し すっきり書きたいぜ☆（＾～＾）
                                    if (sfen1 != null)
                                    {
                                        var sfen2 = sfen1.Trim();
                                        if ("" != sfen2)
                                        {
                                            sb.Append($"{sfen2} ");
                                        }
                                    }
                                }
                                Playing.Send(sb.ToString().TrimEnd());//FIXME:
                            }

                            // 指し手を送ります。
                            Playing.Send($"bestmove {sfenText}");
                        }
                        else // 指し手がないときは、SFENが書けない☆　投了だぜ☆
                        {
                            // ログが重過ぎる☆！
                            //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("(Warabe)指し手のチョイス： 指し手がないときは、SFENが書けない☆　投了だぜ☆ｗｗ（＞＿＜）" +
                            //    "　棋譜＝" + KirokuGakari.ToJsaKifuText(this.Kifu, errH2));

                            //----------------------------------------
                            // 投了ｗ！
                            //----------------------------------------
                            Playing.Send("bestmove resign");
                        }

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

            //System.C onsole.WriteLine();
        }

        public void Stop()
        {
            //------------------------------------------------------------
            // あなたの手番です  （すぐ指してください！）
            //------------------------------------------------------------
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

            if (this.GoPonderFlag)
            {
                //------------------------------------------------------------
                // 将棋エンジン「（予想手が間違っていたって？）  △９二香 を指そうと思っていたんだが」
                //------------------------------------------------------------
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
                // この指し手は、無視されます。（無視されますが、送る必要があります）
                Playing.Send("bestmove 9a9b");
            }
            else
            {
                //------------------------------------------------------------
                // じゃあ、△９二香で
                //------------------------------------------------------------
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
                Playing.Send("bestmove 9a9b");
            }
        }

    }
}
