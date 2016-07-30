using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P571_usiFrame1__.L___500_usiFrame___;
using Grayscale.P575_KifuWarabe_.L250____UsiLoop;
using Grayscale.P575_KifuWarabe_.L500____KifuWarabe;
using Grayscale.P579_usiFrame2__.L500____usiFrame___;
using System;

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
            // USIフレームワーク
            UsiFramework usiFramework = new UsiFrameworkImpl();


            // 将棋エンジン　きふわらべ
            KifuWarabeImpl kifuWarabe = new KifuWarabeImpl();

            KwErrorHandler errH = Util_OwataMinister.ENGINE_DEFAULT;


            usiFramework.OnBegin_InAll(()=> {
                kifuWarabe.AtBegin(errH);
            });
            usiFramework.OnBody_InAll(() => {

                try
                {
                    #region ↑詳説
                    // 
                    // 図.
                    // 
                    //     プログラムの開始：  ここの先頭行から始まります。
                    //     プログラムの実行：  この中で、ずっと無限ループし続けています。
                    //     プログラムの終了：  この中の最終行を終えたとき、
                    //                         または途中で Environment.Exit(0); が呼ばれたときに終わります。
                    //                         また、コンソールウィンドウの[×]ボタンを押して強制終了されたときも  ぶつ切り  で突然終わります。
                    #endregion

                    //************************************************************************************************************************
                    // ループ（全体）
                    //************************************************************************************************************************
                    #region ↓詳説
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
                    #endregion

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
                        UsiLoop1 usiLoop1 = new UsiLoop1(kifuWarabe, usiFramework);
                        usiFramework.OnBegin_InLoop1(usiLoop1);
                        PhaseResult_UsiLoop1 result_UsiLoop1 = usiFramework.OnBody_InLoop1(usiLoop1);
                        usiFramework.OnEnd_InLoop1(usiLoop1);

                        if (result_UsiLoop1 == PhaseResult_UsiLoop1.TimeoutShutdown)
                        {
                            //MessageBox.Show("ループ１で矯正終了するんだぜ☆！");
                            return;//全体ループを抜けます。
                        }
                        else if (result_UsiLoop1 == PhaseResult_UsiLoop1.Quit)
                        {
                            return;//全体ループを抜けます。
                        }

                        //************************************************************************************************************************
                        // ループ（２つ目）
                        //************************************************************************************************************************
                        UsiLoop2 usiLoop2 = new UsiLoop2(kifuWarabe, kifuWarabe.shogisasi, errH, usiFramework);
                        usiFramework.OnBegin_InLoop2(() =>
                        {
                            kifuWarabe.shogisasi.OnTaikyokuKaisi();//対局開始時の処理。
                        });

                        usiFramework.OnBody_InLoop2(usiLoop2);

                        usiFramework.OnEnd_InLoop2(() =>
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
                            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("KifuParserA_Impl.LOGGING_BY_ENGINE, ┏━確認━━━━setoptionDictionary ━┓");
                            foreach (KeyValuePair<string, string> pair in this.owner.SetoptionDictionary)
                            {
                                Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                            }
                            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
                            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("┏━確認━━━━goDictionary━━━━━┓");
                            foreach (KeyValuePair<string, string> pair in this.GoProperties)
                            {
                                Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                            }

                            //Dictionary<string, string> goMateProperties = new Dictionary<string, string>();
                            //goMateProperties["mate"] = "";
                            //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
                            //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┏━確認━━━━goMateDictionary━━━┓");
                            //foreach (KeyValuePair<string, string> pair in this.goMateProperties)
                            //{
                            //    LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                            //}

                            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
                            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("┏━確認━━━━gameoverDictionary━━┓");
                            foreach (KeyValuePair<string, string> pair in this.GameoverProperties)
                            {
                                Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                            }
                            Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
#endif
                        });
                        if (result_UsiLoop1 == PhaseResult_UsiLoop1.TimeoutShutdown)
                        {
                            //MessageBox.Show("ループ２で矯正終了するんだぜ☆！");
                            return;//全体ループを抜けます。
                        }
                    }//全体ループ
                }
                catch (Exception ex)
                {
                    // エラーが起こりました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    // どうにもできないので  ログだけ取って無視します。
                    Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta("Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
                }

            });

            usiFramework.OnEnd_InAll(() => {
                kifuWarabe.AtEnd();
            });


        }
    }
}
