using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P005_Tushin_____.L500____Util;
using Grayscale.P542_Scoreing___.L___005_UsiLoop;
using System.Text.RegularExpressions;
using System;
using Grayscale.P571_usiFrame1__.L___500_usiFrame___;

namespace Grayscale.P575_KifuWarabe_.L250____UsiLoop
{

    /// <summary>
    /// USIの１番目のループです。
    /// </summary>
    public class UsiLoop1
    {
        public ShogiEngine Owner { get; set; }

        public UsiLoop1(ShogiEngine owner, UsiFramework usiFramework)
        {
            this.Owner = owner;

            usiFramework.OnUsi_AtBody1 = delegate (string line, Object caller)
            {
                UsiLoop1 usiLoop1 = (UsiLoop1)caller;

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
                usiLoop1.Owner.Send("option name 子 type check default true");
                usiLoop1.Owner.Send("option name USI type spin default 2 min 1 max 13");
                usiLoop1.Owner.Send("option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー");
                usiLoop1.Owner.Send("option name 卯 type button default うさぎ");
                usiLoop1.Owner.Send("option name 辰 type string default DRAGON");
                usiLoop1.Owner.Send("option name 巳 type filename default スネーク.html");


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
                usiLoop1.Owner.Send("id name " + usiLoop1.Owner.SeihinName);
                usiLoop1.Owner.Send("id author " + usiLoop1.Owner.AuthorName);
                usiLoop1.Owner.Send("usiok");

                return PhaseResult_UsiLoop1.None;
            };

            usiFramework.OnSetoption_AtBody1 = delegate (string line, Object caller)
            {
                UsiLoop1 usiLoop1 = (UsiLoop1)caller;

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
                    usiLoop1.Owner.EngineOptions.AddOption_ByCommandline(line);
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

                return PhaseResult_UsiLoop1.None;
            };

            usiFramework.OnIsready_AtBody1 = delegate (string line, Object caller)
            {
                UsiLoop1 usiLoop1 = (UsiLoop1)caller;

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
            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("┏━━━━━設定━━━━━┓");
            foreach (KeyValuePair<string, string> pair in this.Owner.SetoptionDictionary)
            {
                // ここで将棋エンジンの設定を済ませておいてください。
                Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
            }
            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("┗━━━━━━━━━━━━┛");
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
                usiLoop1.Owner.Send("readyok");

                return PhaseResult_UsiLoop1.None;
            };

            usiFramework.OnUsinewgame_AtBody1 = delegate (string line, Object caller)
            {
                UsiLoop1 usiLoop1 = (UsiLoop1)caller;

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
                return PhaseResult_UsiLoop1.Break;
            };

            usiFramework.OnQuit_AtBody1 = delegate (string line, Object caller)
            {
                UsiLoop1 usiLoop1 = (UsiLoop1)caller;

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
            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("(^-^)ﾉｼ");
#endif


                // このプログラムを終了します。
                return PhaseResult_UsiLoop1.Quit;
            };

            usiFramework.OnCommandlineRead_AtBody1 = delegate (Object caller)
            {
                // 将棋サーバーから何かメッセージが届いていないか、見てみます。
                string line = Util_Message.Download_Nonstop();

                if (null != line)
                {
                    // 通信ログは必ず取ります。
                    Util_OwataMinister.ENGINE_NETWORK.Logger.WriteLine_C(line);
                }

                return line;
            };
        }
    }
}
