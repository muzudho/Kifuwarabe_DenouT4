using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P027_Settei_____.L500____Struct;
using Grayscale.P157_KyokumenPng.L___500_Struct;
using Grayscale.P158_LogKyokuPng.L500____UtilWriter;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P212_ConvPside__.L500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P226_Tree_______.L___500_Struct;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P256_SeizaFinger.L250____Struct;
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using Grayscale.P324_KifuTree___.L___250_Struct;
using Grayscale.P324_KifuTree___.L250____Struct;
using Grayscale.P325_PnlTaikyoku.L___250_Struct;
using Grayscale.P325_PnlTaikyoku.L250____Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;
using Grayscale.P341_Ittesasu___.L125____UtilB;
using Grayscale.P355_KifuParserA.L___500_Parser;
using Grayscale.P355_KifuParserA.L500____Parser;
using Grayscale.P542_Scoreing___.L___005_UsiLoop;
using Grayscale.P542_Scoreing___.L___240_Shogisasi;
using Grayscale.P560_UtilClient_.L500____Util;
using Grayscale.P575_KifuWarabe_.L125____AjimiEngine;
using Grayscale.P575_KifuWarabe_.L249____Noop;
using Grayscale.P440_KifuTreeLog.L500____Struct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P571_usiFrame1__.L___500_usiFrame___;


namespace Grayscale.P575_KifuWarabe_.L250____UsiLoop
{



    /// <summary>
    /// USIの２番目のループです。
    /// </summary>
    public class UsiLoop2
    {
        #region プロパティー

        /// <summary>
        /// 棋譜です。
        /// </summary>
        public KifuTree Kifu { get { return this.kifu; } }
        public void SetKifu(KifuTree kifu)
        {
            this.kifu = kifu;
        }
        private KifuTree kifu;


        /// <summary>
        /// 「go」の属性一覧です。
        /// </summary>
        public Dictionary<string, string> GoProperties { get; set; }


        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        public bool GoPonderNow { get; set; }


        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// </summary>
        public Dictionary<string, string> GameoverProperties { get; set; }

        #endregion

        private ShogiEngine Owner { get; set; }
        private Shogisasi Shogisasi { get; set; }
        private KwErrorHandler ErrH { get; set; }

        public UsiLoop2(ShogiEngine owner, Shogisasi shogisasi, KwErrorHandler errH, UsiFramework usiFramework)
        {
            this.Owner = owner;
            this.Shogisasi = shogisasi;
            this.ErrH = errH;

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
            {
                Playerside firstPside = Playerside.P1;
                // FIXME:平手とは限らないが、平手という前提で作っておく。
                this.SetKifu( new KifuTreeImpl(
                        new KifuNodeImpl(
                            Util_Sky258A.ROOT_SASITE,
                            new KyokumenWrapper( SkyConst.NewInstance(
                                    Util_SkyWriter.New_Hirate(firstPside),
                                    0 // 初期局面は 0手目済み
                                ))// きふわらべ起動時
                        )
                ));
                this.Kifu.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");// 平手 // FIXME:平手とは限らないが。

                Debug.Assert(!Conv_MasuHandle.OnKomabukuro(
                    Conv_SyElement.ToMasuNumber(((RO_Star)this.Kifu.CurNode.Value.KyokumenConst.StarlightIndexOf((Finger)0).Now).Masu)
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
                this.GoPonderNow = false;   // go ponderを将棋所に伝えたなら真
            }

            // gameoverの属性一覧
            {
                this.GameoverProperties = new Dictionary<string, string>();
                this.GameoverProperties["gameover"] = "";
            }

            usiFramework.OnCommandlineRead_AtBody2 = delegate (Object caller)
            {
                //ノンストップ版
                //string line = TimeoutReader.ReadLine(1000);//指定ミリ秒だけブロック

                //通常版
                string line = System.Console.In.ReadLine();

                if (null != line)
                {
                    UsiLoop2 usiLoop2 = (UsiLoop2)caller;

                    // 通信ログは必ず取ります。
                    usiLoop2.ErrH.Logger.WriteLine_C(line);

#if NOOPABLE
                if (this.owner.Option_enable_serverNoopable)
                {
                    noopTimer._03_AtResponsed(this.owner, line, errH);
                }
#endif
                }

                return line;
            };

            usiFramework.OnPosition_AtBody2 = delegate (string line, Object caller)
            {
                UsiLoop2 usiLoop2 = (UsiLoop2)caller;
                KwErrorHandler errH2 = Util_OwataMinister.ENGINE_DEFAULT;

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
                this.Log1("（＾△＾）positionきたｺﾚ！");
#endif
                    // 入力行を解析します。
                    KifuParserA_Result result = new KifuParserA_ResultImpl();
                    KifuParserA_Impl kifuParserA = new KifuParserA_Impl();
                    Model_Taikyoku model_Taikyoku = new Model_TaikyokuImpl(usiLoop2.Kifu);//FIXME:  この棋譜を委譲メソッドで修正するはず。 ShogiGui_Warabeは？
                    KifuParserA_Genjo genjo = new KifuParserA_GenjoImpl(line);
                    kifuParserA.Execute_All(
                        ref result,
                        model_Taikyoku,
                        genjo,
                        errH2
                        );
                    if (null != genjo.StartposImporter_OrNull)
                    {
                        // SFENの解析結果を渡すので、
                        // その解析結果をどう使うかは、委譲します。
                        Util_InClient.OnChangeSky_Im_Client(
                            model_Taikyoku,
                            genjo,
                            errH2
                            );
                    }


#if DEBUG
                this.Log2_Png_Tyokkin(line, (KifuNode)result.Out_newNode_OrNull, errH2);
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
                    Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta("Program「position」：" + ex.GetType().Name + "：" + ex.Message);
                }

                return PhaseResult_UsiLoop2.None;
            };

            usiFramework.OnGoponder_AtBody2 = delegate (string line, Object caller)
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
                    Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta("Program「go ponder」：" + ex.GetType().Name + "：" + ex.Message);
                }

                return PhaseResult_UsiLoop2.None;
            };

            usiFramework.OnGo_AtBody2 = delegate (string line, Object caller)
            {
                UsiLoop2 usiLoop2 = (UsiLoop2)caller;

                KwErrorHandler errH2 = Util_OwataMinister.ENGINE_DEFAULT;
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
                        usiLoop2.GoProperties["btime"] = (string)m.Groups[1].Value;
                        usiLoop2.GoProperties["wtime"] = (string)m.Groups[2].Value;
                        usiLoop2.GoProperties["byoyomi"] = (string)m.Groups[3].Value;
                    }
                    else
                    {
                        usiLoop2.GoProperties["btime"] = "";
                        usiLoop2.GoProperties["wtime"] = "";
                        usiLoop2.GoProperties["byoyomi"] = "";
                    }



                    //----------------------------------------
                    // 棋譜ツリー、局面データは、position コマンドで先に与えられているものとします。
                    //----------------------------------------

                    // ┏━━━━プログラム━━━━┓

                    int latestTemezumi = usiLoop2.Kifu.CurNode.Value.KyokumenConst.Temezumi;//現・手目済

                    //#if DEBUG
                    // MessageBox.Show("["+latestTemezumi+"]手目済　["+this.owner.PlayerInfo.Playerside+"]の手番");
                    //#endif

                    SkyConst src_Sky = usiLoop2.Kifu.NodeAt(latestTemezumi).Value.KyokumenConst;//現局面

                    //errH2.Logger.WriteLine_AddMemo("将棋サーバー「" + latestTemezumi + "手目、きふわらべ　さんの手番ですよ！」　" + line);


                    //----------------------------------------
                    // 王の状態を調べます。
                    //----------------------------------------
                    Result_KingState result_kingState;
                    {
                        result_kingState = Result_KingState.Empty;

                        RO_Star king1p = Util_Starlightable.AsKoma(src_Sky.StarlightIndexOf(Finger_Honshogi.SenteOh).Now);
                        RO_Star king2p = Util_Starlightable.AsKoma(src_Sky.StarlightIndexOf(Finger_Honshogi.GoteOh).Now);
                        //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("将棋サーバー「ではここで、王さまがどこにいるか確認してみましょう」");
                        //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("▲王の置き場＝" + Conv_SyElement.Masu_ToOkiba(koma1.Masu));
                        //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("△王の置き場＝" + Conv_SyElement.Masu_ToOkiba(koma2.Masu));

                        if (Conv_SyElement.ToOkiba(king1p.Masu) != Okiba.ShogiBan)
                        {
                            // 先手の王さまが将棋盤上にいないとき☆
                            result_kingState = Result_KingState.Lost_SenteOh;
                        }
                        else if (Conv_SyElement.ToOkiba(king2p.Masu) != Okiba.ShogiBan)
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
                                usiLoop2.Owner.Send("bestmove resign");//投了
                            }
                            break;
                        default:// どちらの王さまも、まだまだ健在だぜ☆！
                            {
                                List<KifuNode> bestKifuNodeList = new List<KifuNode>();

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
                                int multiPV_Count = 1;// 2;
                                {
                                    // 最善手、次善手、三次善手、四次善手、五次善手
                                    for (int iMultiPV = 0; iMultiPV < multiPV_Count; iMultiPV++)
                                    {
                                        bestKifuNodeList.Add(usiLoop2.Shogisasi.WA_Bestmove(
                                            isHonshogi,
                                            usiLoop2.Kifu,
                                            errH2)
                                            );
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
                                KifuNode bestKifuNode = null;
                                // 最善手、次善手、三次善手、四次善手、五次善手
                                float bestScore = float.MinValue;
                                for (int iMultiPV = 0; iMultiPV < bestKifuNodeList.Count; iMultiPV++)
                                {
                                    KifuNode node = bestKifuNodeList[iMultiPV];

                                    if (null != node && null != node.KyHyokaSheet_Mutable && bestScore <= node.Score)
                                    {
                                        bestScore = node.Score;
                                        bestKifuNode = node;
                                    }
                                }

                                exceptionArea = 2300;
                                Starbeamable bestSasite2;
                                if (null == bestKifuNode)
                                {
                                    // 投了
                                    bestSasite2 = Util_Sky258A.NULL_OBJECT_SASITE;
                                }
                                else
                                {
                                    bestSasite2 = bestKifuNode.Key;
                                }

                                exceptionArea = 2400;
                                if (Util_Sky_BoolQuery.isEnableSfen(bestSasite2))
                                {
                                    string sfenText = Conv_SasiteStr_Sfen.ToSasiteStr_Sfen(bestSasite2);

                                    // ログが重過ぎる☆！
                                    //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("(Warabe)指し手のチョイス： bestmove＝[" + sfenText + "]" +
                                    //    "　棋譜＝" + KirokuGakari.ToJsaKifuText(this.Kifu, errH2));

                                    //----------------------------------------
                                    // スコア 試し
                                    //----------------------------------------
                                    {
                                        //int hyojiScore = (int)(bestScore / 100.0d);//FIXME:適当に調整した。
                                        int hyojiScore = (int)bestScore;
                                        if (usiLoop2.Kifu.CurNode.Value.KyokumenConst.KaisiPside == Playerside.P2)
                                        {
                                            // 符号を逆転
                                            hyojiScore = -hyojiScore;
                                        }
                                        usiLoop2.Owner.Send("info time 1 depth 1 nodes 1 score cp " + hyojiScore.ToString() + " pv ");//FIXME:
                                                                                                                                      //+ " pv 3a3b L*4h 4c4d"
                                    }


                                    //----------------------------------------
                                    // 指し手を送ります。
                                    //----------------------------------------
                                    usiLoop2.Owner.Send("bestmove " + sfenText);
                                }
                                else // 指し手がないときは、SFENが書けない☆　投了だぜ☆
                                {
                                    // ログが重過ぎる☆！
                                    //OwataMinister.WARABE_ENGINE.Logger.WriteLine_AddMemo("(Warabe)指し手のチョイス： 指し手がないときは、SFENが書けない☆　投了だぜ☆ｗｗ（＞＿＜）" +
                                    //    "　棋譜＝" + KirokuGakari.ToJsaKifuText(this.Kifu, errH2));

                                    //----------------------------------------
                                    // 投了ｗ！
                                    //----------------------------------------
                                    usiLoop2.Owner.Send("bestmove resign");
                                }


                                exceptionArea = 2500;
                                //------------------------------------------------------------
                                // 以前の手カッター
                                //------------------------------------------------------------
                                Util_KifuTree282.IzennoHenkaCutter(usiLoop2.Kifu, errH2);
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
                                Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta(ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの１０です。");
                                throw ex;
                            }
                        case 2200:
                            {
                                Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta(ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの４０です。");
                                throw ex;
                            }
                        case 2300:
                            {
                                Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta(ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの５０です。");
                                throw ex;
                            }
                        case 2400:
                            {
                                Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta(ex, "マルチＰＶから、ベスト指し手をチョイスしようとしたときの９０です。");
                                throw ex;
                            }
                        default:
                            {
                                // エラーが起こりました。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                // どうにもできないので  ログだけ取って無視します。
                                Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta("Program「go」：" + ex.GetType().Name + " " + ex.Message + "：goを受け取ったときです。：");
                            }
                            break;
                    }
                }


                //System.C onsole.WriteLine();

                //throw new Exception("デバッグだぜ☆！　エラーはキャッチできたかな～☆？（＾▽＾）");
                return PhaseResult_UsiLoop2.None;
            };

            usiFramework.OnStop_AtBody2 = delegate (string line, Object caller)
            {
                UsiLoop2 usiLoop2 = (UsiLoop2)caller;

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

                    if (usiLoop2.GoPonderNow)
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
                        usiLoop2.Owner.Send("bestmove 9a9b");
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
                        usiLoop2.Owner.Send("bestmove 9a9b");
                    }

                }
                catch (Exception ex)
                {
                    // エラーが起こりました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    // どうにもできないので  ログだけ取って無視します。
                    Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta("Program「stop」：" + ex.GetType().Name + " " + ex.Message);
                }

                return PhaseResult_UsiLoop2.None;
            };

            usiFramework.OnGameover_AtBody2 = delegate (string line, Object caller)
            {
                UsiLoop2 usiLoop2 = (UsiLoop2)caller;

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
                        usiLoop2.GameoverProperties["gameover"] = (string)m.Groups[1].Value;
                    }
                    else
                    {
                        usiLoop2.GameoverProperties["gameover"] = "";
                    }


                    // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                    return PhaseResult_UsiLoop2.Break;
                }
                catch (Exception ex)
                {
                    // エラー続行
                    Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta(ex, "Program「gameover」：" + ex.GetType().Name + " " + ex.Message);
                    return PhaseResult_UsiLoop2.None;
                }
            };

            usiFramework.OnLogdase_AtBody2 = delegate (string line, Object caller)
            {
                UsiLoop2 usiLoop2 = (UsiLoop2)caller;

                StringBuilder sb = new StringBuilder();
                sb.Append("ログだせ～（＾▽＾）");

                usiLoop2.Kifu.ForeachZenpuku(
                    usiLoop2.Kifu.GetRoot(), (int temezumi, KyokumenWrapper sky, Node<Starbeamable, KyokumenWrapper> node, ref bool toBreak) =>
                    {
                    //sb.AppendLine("(^-^)");

                    if (null != node)
                        {
                            if (null != node.Key)
                            {
                                string sfenText = Conv_SasiteStr_Sfen.ToSasiteStr_Sfen(node.Key);
                                sb.Append(sfenText);
                                sb.AppendLine();
                            }
                        }
                    });

                File.WriteAllText(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS, "_log_ログ出せ命令.txt"), sb.ToString());

                return PhaseResult_UsiLoop2.None;
            };
        }



        private void Log1(string message)
        {
            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo(message);
        }
        private void Log2_Png_Tyokkin(string line, KifuNode kifuNode, KwErrorHandler errH)
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

                int srcMasu_orMinusOne = -1;
                int dstMasu_orMinusOne = -1;
                if (null != kifuNode.Key)
                {
                    srcMasu_orMinusOne = Conv_SyElement.ToMasuNumber(((RO_Star)kifuNode.Key.LongTimeAgo).Masu);
                    dstMasu_orMinusOne = Conv_SyElement.ToMasuNumber(((RO_Star)kifuNode.Key.Now).Masu);
                }

                KyokumenPngArgs_FoodOrDropKoma foodKoma;
                if (null != kifuNode.Key.FoodKomaSyurui)
                {
                    switch (Util_Komasyurui14.NarazuCaseHandle((Komasyurui14)kifuNode.Key.FoodKomaSyurui))
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
                    Conv_KifuNode.ToRO_Kyokumen1(kifuNode, errH),
                    srcMasu_orMinusOne,
                    dstMasu_orMinusOne,
                    foodKoma,
                    Conv_SasiteStr_Sfen.ToSasiteStr_Sfen(kifuNode.Key),//Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(kifuNode, kifuNode.Value, errH),
                    "",
                    fileName,
                    Util_KifuTreeLogWriter.REPORT_ENVIRONMENT,
                    errH
                    );
            }
        }

    }
}
