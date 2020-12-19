using System;
using System.Collections.Generic;
using Grayscale.A060Application.B210Tushin.C500Util;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;

namespace Grayscale.Kifuwaragyoku.UseCases
{
    public class Playing
    {
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
        }

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

        }

    }
}
