using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C125____UtilB;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B800_ConvCsa____.C500____Converter;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C500____KifuWarabe;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___250_Learn;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C250____Learn;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using System.Diagnostics;
#endif

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C260____View
{
    public abstract class Util_LearningView
    {

        /// <summary>
        /// 指し手一覧を、リストボックスに表示します。
        /// </summary>
        /// <param name="uc_Main"></param>
        public static void ShowSasiteList(
            LearningData learningData,
            Uc_Main uc_Main,
            KwLogger logger)
        {
            //
            // まず、リストを空っぽにします。
            //
            uc_Main.LstSasite.Items.Clear();

            Earth earth1 = new EarthImpl();
            Sky positionA = Util_SkyCreator.New_Hirate();//日本の符号読取時
            Tree kifu1 = new TreeImpl(positionA);
            //kifu1.AssertPside(kifu1.CurNode, "ShowSasiteList",errH);

            List<CsaKifuSasite> sasiteList = learningData.CsaKifu.SasiteList;
            foreach (CsaKifuSasite csaSasite in sasiteList)
            {
                // 開始局面
                Sky kaisi_Sky = positionA;

                //
                // csaSasite を データ指し手 に変換するには？
                //
                Move nextMove;
                {
                    Playerside pside = Util_CsaSasite.ToPside(csaSasite);

                    // 元位置
                    SyElement srcMasu = Util_CsaSasite.ToSrcMasu(csaSasite);
                    Finger figSrcKoma;
                    if (Masu_Honshogi.IsErrorBasho(srcMasu))// 駒台の "00" かも。
                    {
                        //駒台の駒。
                        Komasyurui14 utuKomasyurui = Util_Komasyurui14.NarazuCaseHandle(Util_CsaSasite.ToKomasyurui(csaSasite));// 打つ駒の種類。

                        Okiba komadai;
                        switch (pside)
                        {
                            case Playerside.P1: komadai = Okiba.Sente_Komadai; break;
                            case Playerside.P2: komadai = Okiba.Gote_Komadai; break;
                            default: komadai = Okiba.Empty; break;
                        }

                        figSrcKoma = Util_Sky_FingersQuery.InOkibaPsideKomasyuruiNow(kaisi_Sky, komadai, pside, utuKomasyurui).ToFirst();
                    }
                    else
                    {
                        // 盤上の駒。
                        figSrcKoma = Util_Sky_FingerQuery.InBanjoMasuNow(kaisi_Sky, pside, srcMasu, logger);
                    }
                    kaisi_Sky.AssertFinger(figSrcKoma);
                    Busstop srcKoma = kaisi_Sky.BusstopIndexOf(figSrcKoma);

                    // 先位置
                    SyElement dstMasu = Util_CsaSasite.ToDstMasu(csaSasite);
                    Finger figFoodKoma = Util_Sky_FingerQuery.InMasuNow_FilteringBanjo(kaisi_Sky, pside, dstMasu, logger);
                    Komasyurui14 foodKomasyurui;
                    if (figFoodKoma == Fingers.Error_1)
                    {
                        // 駒のない枡
                        foodKomasyurui = Komasyurui14.H00_Null___;//取った駒無し。
                    }
                    else
                    {
                        // 駒のある枡
                        kaisi_Sky.AssertFinger(figFoodKoma);
                        foodKomasyurui = Conv_Busstop.ToKomasyurui(kaisi_Sky.BusstopIndexOf(figFoodKoma));//取った駒有り。
                    }
                    Busstop busstop = Conv_Busstop.ToBusstop(
                        pside,
                        dstMasu,
                        Util_CsaSasite.ToKomasyurui(csaSasite)
                    );

                    nextMove = Conv_Move.ToMove(
                        Conv_Busstop.ToMasu( srcKoma),// 移動元
                        Conv_Busstop.ToMasu(busstop),// 移動先
                        Conv_Busstop.ToKomasyurui( srcKoma),
                        Conv_Busstop.ToKomasyurui(busstop),//これで成りかどうか判定
                        foodKomasyurui,////取った駒
                        Conv_Busstop.ToPlayerside( srcKoma),
                        false
                    );
                }

                MoveEx curNodeB;
                {
                    //----------------------------------------
                    // 一手指したい。
                    //----------------------------------------
                    //
                    //↓↓一手指し
                    IttesasuResult ittesasuResult;
                    Util_IttesasuRoutine.DoMove_Normal(
                        out ittesasuResult,
                        ref nextMove,
                        positionA,
                        logger
                    );
                    Util_IttesasuRoutine.BeforeUpdateKifuTree(
                        earth1,
                        kifu1,
                        nextMove,
                        ittesasuResult.SyuryoKyokumenW,
                        logger
                        );
                    curNodeB = kifu1.MoveEx_Current;
                    // これで、棋譜ツリーに、構造変更があったはず。
                    //↑↑一手指し
                }


                Move move;
                /*
                if (curNodeB.IsRoot(kifu1,logger))
                {
                    move = Move.Empty;
                }
                else
                {
                */
                    // FIXME: 未テスト。
                    move = Conv_Move.ToMove_ByCsa(csaSasite, kifu1.PositionA);
                //}
                HonpuSasiteListItemImpl listItem = new HonpuSasiteListItemImpl(csaSasite, move);
                uc_Main.LstSasite.Items.Add(listItem);
            }
        }



        /// <summary>
        /// ノード情報の表示
        /// </summary>
        /// <param name="uc_Main"></param>
        public static void Aa_ShowNode2(
            LearningData learningData,
            Sky positionA,
            Uc_Main uc_Main, KwLogger errH)
        {
            // 手目済み
            uc_Main.TxtTemezumi.Text = positionA.Temezumi.ToString();

            // 総ノード数
            uc_Main.TxtAllNodesCount.Text = "--";// FIXME: 一旦削除

            // 合法手の数
            uc_Main.TxtGohosyuTe.Text = "不明";
        }

        /// <summary>
        /// 合法手リストの表示
        /// </summary>
        /// <param name="uc_Main"></param>
        public static void Aa_ShowGohosyu2(LearningData learningData, Uc_Main uc_Main, KwLogger logger)
        {
            //----------------------------------------
            // フォルダー作成
            //----------------------------------------
            //this.Kifu.CreateAllFolders(Const_Filepath.LOGS + "temp", 4);

            {

                //----------------------------------------
                // 合法手のリストを作成
                //----------------------------------------
                List<GohosyuListItem> list = new List<GohosyuListItem>();
                //uc_Main.LstGohosyu.Items.Clear();
                int itemNumber = 0;
                Sky positionA = learningData.PositionA;
                List<Move> pvList = learningData.KifuA.Pv_ToList();
                {
                    Move moveB = learningData.ToCurChildItem();
                    pvList.Add(moveB);

                    Util_IttesasuSuperRoutine.DoMove_Super1(
                        Conv_Move.ToPlayerside(moveB),
                        ref positionA,//指定局面
                        ref moveB,
                        "D100",
                        logger
                    );



                    learningData.DoScoreing_ForLearning(
                        Conv_Move.ToPlayerside(moveB),
                        positionA
                        );

                    GohosyuListItem item = new GohosyuListItem(
                        itemNumber,
                        moveB,
                        Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(
                            moveB,
                            pvList,
                            positionA, logger)
                            );
                    list.Add(item);

                    itemNumber++;

                    IttemodosuResult ittemodosuResult;
                    Util_IttemodosuRoutine.UndoMove(
                        out ittemodosuResult,
                        moveB,
                        Conv_Move.ToPlayerside(moveB),
                        positionA,
                        "D900",
                        logger
                        );
                    positionA = ittemodosuResult.SyuryoSky;


                    pvList.RemoveAt(pvList.Count - 1);
                }

                //----------------------------------------
                // ソート
                //----------------------------------------
                //
                // 先手は正の数、後手は負の数で、絶対値の高いもの順。
                list.Sort((GohosyuListItem a, GohosyuListItem b) =>
                {
                    int result;

                    int aScore = 0;

                    int bScore = 0;

                    switch (learningData.PositionA.GetKaisiPside())
                    {
                        case Playerside.P1: result = bScore - aScore; break;
                        case Playerside.P2: result = aScore - bScore; break;
                        default: result = 0; break;
                    }
                    return result;
                });


                
                uc_Main.LstGohosyu.Items.Clear();
                uc_Main.LstGohosyu.Items.AddRange(list.ToArray());
                //foreach (GohosyuListItem item in list)
                //{
                //    uc_Main.LstGohosyu.Items.Add(item);
                //}
            }
        }



        /// <summary>
        /// [一手指す]ボタン。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Ittesasu_ByBtnClick(
            ref bool isRequestShowGohosyu,
            ref bool isRequestChangeKyokumenPng,
            LearningData learningData, Uc_Main uc_Main, KwLogger logger)
        {
#if DEBUG
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
#endif

            //
            // リストの先頭の項目を取得したい。
            //
            if (uc_Main.LstSasite.Items.Count < 1)
            {
                goto gt_EndMethod;
            }



            // リストボックスの先頭から指し手をSFEN形式で１つ取得。
            HonpuSasiteListItemImpl item = (HonpuSasiteListItemImpl)uc_Main.LstSasite.Items[0];
            Move move = item.Move;
            if (null != logger.KwDisplayer_OrNull.Dlgt_OnLog1Append_or_Null)
            {
                logger.KwDisplayer_OrNull.Dlgt_OnLog1Append_or_Null("sfen=" + Conv_Move.ToSfen(move) + Environment.NewLine);
            }

            //
            // 現局面の合法手は、既に読んであるとします。（棋譜ツリーのNextNodesが既に設定されていること）
            //


            //
            // 合法手の一覧は既に作成されているものとします。
            // 次の手に進みます。
            //
            Move nextMove;
            {
                nextMove = Move.Empty;// Conv_Move.GetErrorMove();
                StringBuilder sb = new StringBuilder();
                sb.Append("指し手[" + Conv_Move.ToSfen(move) + "]はありませんでした。\n" + learningData.DumpToAllGohosyu(learningData.PositionA));

                //Debug.Fail(sb.ToString());
                logger.DonimoNaranAkirameta("Util_LearningView#Ittesasu_ByBtnClick：" + sb.ToString());
                MessageBox.Show(sb.ToString(), "エラー");
            }

            //----------------------------------------
            // 一手指したい。
            //----------------------------------------
            //↓↓一手指し
            IttesasuResult ittesasuResult;
            Util_IttesasuRoutine.DoMove_Normal(
                out ittesasuResult,
                ref nextMove,
                learningData.PositionA,
                logger
            );
            Util_IttesasuRoutine.BeforeUpdateKifuTree(
                learningData.Earth,
                learningData.KifuA,
                nextMove,
                ittesasuResult.SyuryoKyokumenW,
                logger
                );
            // これで、棋譜ツリーに、構造変更があったはず。
            //↑↑一手指し

            /*
            //----------------------------------------
            // カレント・ノードより古い、以前読んだ手を削除したい。
            //----------------------------------------
            System.Console.WriteLine("カレント・ノード＝" + Conv_Move.ToSfen( learningData.GetMove()));
            int result_removedCount = Util_KifuTree282.IzennoHenkaCutter(
                learningData.KifuA, errH);
            System.Console.WriteLine("削除した要素数＝" + result_removedCount);
            */

            ////----------------------------------------
            //// 合法手一覧を作成したい。
            ////----------------------------------------
            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[KifuWarabeImpl.SEARCHED_PV_LENGTH];
            learningData.Aa_Yomi(
                ref searchedMaxDepth,
                ref searchedNodes,
                learningData.KifuA,
                learningData.PositionA,
                searchedPv,
                logger);
            // ノード情報の表示
            Util_LearningView.Aa_ShowNode2(
                uc_Main.LearningData,
                uc_Main.LearningData.PositionA,
                uc_Main, logger);

            // 合法手表示の更新を要求します。 
            isRequestShowGohosyu = true;
            // 局面PNG画像を更新を要求。
            isRequestChangeKyokumenPng = true;            

            //
            // リストの頭１個を除外します。
            //
            uc_Main.LstSasite.Items.RemoveAt(0);

#if DEBUG
            sw1.Stop();
            Console.WriteLine("一手指すボタン合計 = {0}", sw1.Elapsed);
            Console.WriteLine("────────────────────────────────────────");
#endif

        gt_EndMethod:
            ;
            //----------------------------------------
            // ボタン表示の回復
            //----------------------------------------
            if (0 < uc_Main.LstSasite.Items.Count)
            {
                uc_Main.BtnUpdateKyokumenHyoka.Enabled = true;//[局面評価更新]ボタン連打防止解除。
            }
        }

    }
}
