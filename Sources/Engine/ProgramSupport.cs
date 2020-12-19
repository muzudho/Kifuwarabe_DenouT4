﻿// noop 可
//#define NOOPABLE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B210Tushin.C500Util;
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
using Grayscale.Kifuwaragyoku.UseCases;

#if DEBUG
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A150LogKyokuPng.B100KyokumenPng.C500Struct;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A150LogKyokuPng.B200LogKyokuPng.C500UtilWriter;
using Grayscale.A240_KifuTreeLog.B110KifuTreeLog.C500Struct;
// using Grayscale.Kifuwaragyoku.Entities.Logging;
#endif

namespace Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe
{
    public class ProgramSupport : ShogiEngine
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        public ProgramSupport(IUsiFramework usiFramework)
        {
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

            // goの属性一覧
            {
                this.GoProperties = new Dictionary<string, string>();
                this.GoProperties["btime"] = "";
                this.GoProperties["wtime"] = "";
                this.GoProperties["byoyomi"] = "";
            }

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

            // 対局開始時
            usiFramework.OnLoop2Begin = this.OnLoop2Begin;
            // 対局中
            usiFramework.OnCommandlineAtLoop2 = this.OnCommandlineAtLoop2;

            usiFramework.OnPosition = this.OnPositionAtLoop2;
            usiFramework.OnGoponder = this.OnGoponderAtLoop2;
            usiFramework.OnGo = this.OnGo;
            usiFramework.OnStop = this.OnStop;
            usiFramework.OnGameover = this.OnGameover;
            usiFramework.OnLogDase = this.OnLogDase;
            // 対局終了時
            usiFramework.OnLoop2End = this.OnLoop2End;
        }

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
        /// 棋譜です。
        /// Loop2で使います。
        /// </summary>
        public Tree Kifu { get { return this.m_kifu_; } }
        /*
        public ISky PositionA { get {
                return this.Kifu_AtLoop2.CurNode1.GetNodeValue();
                //return this.m_positionA_;
            } }
        */

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
        private Tree m_kifu_;

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
        /// 「go」の属性一覧です。
        /// </summary>
        public Dictionary<string, string> GoProperties { get; set; }


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
                Logger.AppendLine(LogTags.ProcessEngineNetwork,line);
                Logger.Flush(LogTags.ProcessEngineNetwork,LogTypes.ToClient);
            }

            return line;
        }


        private void OnLoop2Begin()
        {
            this.Shogisasi.OnTaikyokuKaisi();//対局開始時の処理。
        }

        /// <summary>
        /// Loop2のBody部で呼び出されます。
        /// </summary>
        /// <returns></returns>
        private string OnCommandlineAtLoop2()
        {
            //ノンストップ版
            //string line = TimeoutReader.ReadLine(1000);//指定ミリ秒だけブロック

            //通常版
            string line = System.Console.In.ReadLine();

            if (null != line)
            {
                // 通信ログは必ず取ります。
                Logger.AppendLine(LogTags.ProcessEngineDefault, line);
                Logger.Flush(LogTags.ProcessEngineDefault, LogTypes.ToClient);

#if NOOPABLE
                if (this.owner.Option_enable_serverNoopable)
                {
                    noopTimer._03_AtResponsed(this.owner, line, logTag);
                }
#endif
            }

            return line;
        }

        /// <summary>
        /// Loop2のBody部で呼び出されます。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private PhaseResultUsiLoop2 OnPositionAtLoop2(string line)
        {
            ILogTag logTag = LogTags.ProcessEngineDefault;

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

                    this.Earth,
                    this.Kifu,

                    genjo,
                    logTag
                    );
                if (null != genjo.StartposImporter_OrNull)
                {
                    // SFENの解析結果を渡すので、
                    // その解析結果をどう使うかは、委譲します。
                    Util_InClient.OnChangeSky_Im_Client(

                        this.Earth,
                        this.Kifu,

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
                Logger.Panic(LogTags.ProcessEngineDefault, "Program「position」：" + ex.GetType().Name + "：" + ex.Message);
                throw;
            }

            return PhaseResultUsiLoop2.None;
        }

        /// <summary>
        /// Loop2のBody部で呼び出されます。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private PhaseResultUsiLoop2 OnGoponderAtLoop2(string line)
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
                Logger.Panic(LogTags.ProcessEngineDefault, "Program「go ponder」：" + ex.GetType().Name + "：" + ex.Message);
                throw;
            }

            return PhaseResultUsiLoop2.None;
        }

        /// <summary>
        /// Loop2のBody部で呼び出されます。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private PhaseResultUsiLoop2 OnGo(string line)
        {
            int exceptionArea = 0;

            try
            {

                exceptionArea = 1000;
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
                Regex regex = new Regex(@"go btime (\d+) wtime (\d+) byoyomi (\d+)", RegexOptions.Singleline);
                Match m = regex.Match(line);

                if (m.Success)
                {
                    this.GoProperties["btime"] = (string)m.Groups[1].Value;
                    this.GoProperties["wtime"] = (string)m.Groups[2].Value;
                    this.GoProperties["byoyomi"] = (string)m.Groups[3].Value;
                }
                else
                {
                    this.GoProperties["btime"] = "";
                    this.GoProperties["wtime"] = "";
                    this.GoProperties["byoyomi"] = "";
                }



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
                    Logger.AppendLine(LogTags.ProcessEngineDefault,"サーバーから受信した局面☆（＾▽＾）");
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
                            string[] searchedPv = new string[ProgramSupport.SEARCHED_PV_LENGTH];
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

                                    this.Kifu.MoveEx_SetCurrent(TreeImpl.OnDoCurrentMove(this.Kifu.MoveEx_Current, this.Kifu, this.Kifu.PositionA, LogTags.ProcessEngineDefault));
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
                            Logger.Panic(LogTags.ProcessEngineDefault, ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの１０です。");
                            throw;
                        }
                    case 2200:
                        {
                            Logger.Panic(LogTags.ProcessEngineDefault, ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの４０です。");
                            throw;
                        }
                    case 2300:
                        {
                            Logger.Panic(LogTags.ProcessEngineDefault, ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの５０です。");
                            throw;
                        }
                    case 2400:
                        {
                            Logger.Panic(LogTags.ProcessEngineDefault, ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの９０です。");
                            throw;
                        }
                    default:
                        {
                            // エラーが起こりました。
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            // どうにもできないので  ログだけ取って無視します。
                            Logger.Panic(LogTags.ProcessEngineDefault, "Program「go」：" + ex.GetType().Name + " " + ex.Message + "：goを受け取ったときです。：");
                            throw;//追加
                        }
                        //break;
                }
            }


            //System.C onsole.WriteLine();

            //throw new Exception("デバッグだぜ☆！　エラーはキャッチできたかな～☆？（＾▽＾）");
            return PhaseResultUsiLoop2.None;
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
