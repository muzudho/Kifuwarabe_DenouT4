﻿// noop 可
//#define NOOPABLE

namespace Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe
{
#if DEBUG
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A150LogKyokuPng.B100KyokumenPng.C500Struct;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A150LogKyokuPng.B200LogKyokuPng.C500UtilWriter;
using Grayscale.A240_KifuTreeLog.B110KifuTreeLog.C500Struct;
// using Grayscale.Kifuwaragyoku.Entities.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Grayscale.A060Application.B210Tushin.C500Util;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;//FIXME:
using Grayscale.A500ShogiEngine.B200Scoreing.C005UsiLoop;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.UseCases;
using Nett;
#else
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Grayscale.A060Application.B210Tushin.C500Util;
    using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;
    using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;//FIXME:
    using Grayscale.A500ShogiEngine.B200Scoreing.C005UsiLoop;
    using Grayscale.Kifuwaragyoku.Entities.Logging;
    using Grayscale.Kifuwaragyoku.UseCases;
    using Nett;
#endif

    public class ProgramSupport : ShogiEngine
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        public ProgramSupport(IUsiFramework usiFramework)
        {
            // go ponderの属性一覧
            {
                this.GoPonder = false;   // go ponderを将棋所に伝えたなら真
            }

            // gameoverの属性一覧
            {
                this.GameoverProperties = new Dictionary<string, string>();
                this.GameoverProperties["gameover"] = "";
            }

            // 準備時
            usiFramework.OnCommandlineAtLoop1 = this.OnCommandlineAtLoop1;

            // 対局中
            usiFramework.OnStop = this.OnStop;
            usiFramework.OnGameover = this.OnGameover;
            usiFramework.OnLogDase = this.OnLogDase;
            // 対局終了時
            usiFramework.OnLoop2End = this.OnLoop2End;
        }


        /*
        public ISky PositionA { get {
                return this.Kifu_AtLoop2.CurNode1.GetNodeValue();
                //return this.m_positionA_;
            } }
        */

        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// Loop2で呼ばれます。
        /// </summary>
        public bool GoPonder { get; set; }


        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// Loop2で呼ばれます。
        /// </summary>
        public Dictionary<string, string> GameoverProperties { get; set; }

        /// <summary>
        /// Loop1のBody部で呼び出されます。
        /// </summary>
        /// <returns></returns>
        private string OnCommandlineAtLoop1()
        {
            // 将棋サーバーから何かメッセージが届いていないか、見てみます。
            string line = Util_Message.Download_Nonstop();

            if (null != line)
            {
                // 通信ログは必ず取ります。
                Logger.AppendLine(LogTags.ProcessEngineNetwork, line);
                Logger.Flush(LogTags.ProcessEngineNetwork, LogTypes.ToClient);
            }

            return line;
        }

        /// <summary>
        /// Loop2のBody部で呼び出されます。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private PhaseResultUsiLoop2 OnStop(string line)
        {
            try
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

                if (this.GoPonder)
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
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.Panic(LogTags.ProcessEngineDefault, "Program「stop」：" + ex.GetType().Name + " " + ex.Message);
                throw;//追加
            }

            return PhaseResultUsiLoop2.None;
        }

        /// <summary>
        /// Loop2のBody部で呼び出されます。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private PhaseResultUsiLoop2 OnGameover(string line)
        {
            try
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
                    this.GameoverProperties["gameover"] = (string)m.Groups[1].Value;
                }
                else
                {
                    this.GameoverProperties["gameover"] = "";
                }


                // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                return PhaseResultUsiLoop2.Break;
            }
            catch (Exception ex)
            {
                // エラー続行
                Logger.Panic(LogTags.ProcessEngineDefault, ex, "Program「gameover」：" + ex.GetType().Name + " " + ex.Message);
                return PhaseResultUsiLoop2.None;
            }
        }

        /// <summary>
        /// Loop2のBody部で呼び出されます。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private PhaseResultUsiLoop2 OnLogDase(string line)
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
        }


#if DEBUG
        private void Log1_AtLoop2(string message)
        {
            Util_Loggers.ProcessEngine_DEFAULT.AppendLine(message);
            Util_Loggers.ProcessEngine_DEFAULT.Flush(LogTypes.Plain);
        }
        private void Log2_Png_Tyokkin_AtLoop2(string line, Move move_forLog, ISky sky, ILogger logTag)
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
    }
}
