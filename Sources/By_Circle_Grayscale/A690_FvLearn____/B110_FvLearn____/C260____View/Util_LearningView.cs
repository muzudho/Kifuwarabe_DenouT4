using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C125____UtilB;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B800_ConvCsa____.C500____Converter;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C500____KifuWarabe;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___250_Learn;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C250____Learn;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
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
            KwErrorHandler errH
            )
        {
            //
            // まず、リストを空っぽにします。
            //
            uc_Main.LstSasite.Items.Clear();

            KifuTree kifu1 = new KifuTreeImpl(
                new KifuNodeImpl(
                    Conv_Move.GetErrorMove(),
                    new KyokumenWrapper(Util_SkyWriter.New_Hirate())//日本の符号読取時
                )
            );
            //kifu1.AssertPside(kifu1.CurNode, "ShowSasiteList",errH);

            List<CsaKifuSasite> sasiteList = learningData.CsaKifu.SasiteList;
            foreach (CsaKifuSasite csaSasite in sasiteList)
            {
                // 開始局面
                Sky kaisi_Sky = kifu1.CurNode.Value.Kyokumen;

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
                        figSrcKoma = Util_Sky_FingerQuery.InBanjoMasuNow(kaisi_Sky, pside, srcMasu, errH);
                    }
                    kaisi_Sky.AssertFinger(figSrcKoma);
                    Busstop srcKoma = kaisi_Sky.BusstopIndexOf(figSrcKoma);

                    // 先位置
                    SyElement dstMasu = Util_CsaSasite.ToDstMasu(csaSasite);
                    Finger figFoodKoma = Util_Sky_FingerQuery.InMasuNow_FilteringBanjo(kaisi_Sky, pside, dstMasu, errH);
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

                {
                    //----------------------------------------
                    // 一手指したい。
                    //----------------------------------------
                    //
                    //↓↓一手指し
                    Sky susunda_Sky_orNull;
                    IttesasuResult ittesasuResult;
                    Util_IttesasuRoutine.Before1(
                        kifu1.CurNode.Value,
                        new IttesasuArgImpl(                            
                            ((KifuNode)kifu1.CurNode).Value.Kyokumen.KaisiPside,
                            nextMove,
                            kifu1.CurNode.Value.Kyokumen.Temezumi + 1//1手進める
                        ),
                        out susunda_Sky_orNull,
                        out ittesasuResult,
                        //kifu1,//診断用
                        errH,
                        "Utli_LearningViews#ShowSasiteList"
                    );
                    Debug.Assert(ittesasuResult.Get_SyuryoNode_OrNull != null, "ittesasuResult.Get_SyuryoNode_OrNull がヌル☆？！");
                    Util_IttesasuRoutine.Before2(
                        ref ittesasuResult,
                        susunda_Sky_orNull,
                        errH
                    );
                    //
                    //次ノートを追加します。次ノードを、これからのカレントとします。
                    //
                    //kifu1.AssertChildPside(kifu1.CurNode.Value.ToKyokumenConst.KaisiPside, ittesasuResult.Get_SyuryoNode_OrNull.Value.ToKyokumenConst.KaisiPside);
                    Util_IttesasuRoutine.After3_ChangeCurrent(
                        kifu1,
                        ittesasuResult.Get_SyuryoNode_OrNull.Key,
                        ittesasuResult.Get_SyuryoNode_OrNull,
                        errH
                        );
                    // これで、棋譜ツリーに、構造変更があったはず。
                    //↑↑一手指し
                }


                Move move;
                if (kifu1.CurNode.IsRoot())
                {
                    move = Move.Empty;
                }
                else
                {
                    // FIXME: 未テスト。
                    move = Conv_Move.ToMove_ByCsa(csaSasite, kifu1.CurNode.GetParentNode().Value.Kyokumen);
                }
                HonpuSasiteListItemImpl listItem = new HonpuSasiteListItemImpl(csaSasite, move);
                uc_Main.LstSasite.Items.Add(listItem);
            }

            //----------------------------------------
            // ソート
            //----------------------------------------
            //List<SasiteListItemImpl> list = new List<SasiteListItemImpl>();
            //list.Sort((SasiteListItemImpl a, SasiteListItemImpl b) =>
            //{
            //    return a - b;
            //});
        }



        /// <summary>
        /// ノード情報の表示
        /// </summary>
        /// <param name="uc_Main"></param>
        public static void Aa_ShowNode2(LearningData learningData, Uc_Main uc_Main, KwErrorHandler errH)
        {
            // 手目済み
            uc_Main.TxtTemezumi.Text = learningData.Kifu.CurNode.Value.Kyokumen.Temezumi.ToString();

            // 総ノード数
            uc_Main.TxtAllNodesCount.Text = learningData.Kifu.CountAllNodes().ToString();

            // 合法手の数
            uc_Main.TxtGohosyuTe.Text = ((KifuNode)learningData.Kifu.CurNode).Count_ChildNodes.ToString();
        }

        /// <summary>
        /// 合法手リストの表示
        /// </summary>
        /// <param name="uc_Main"></param>
        public static void Aa_ShowGohosyu2(LearningData learningData, Uc_Main uc_Main, KwErrorHandler errH)
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
                ((KifuNode)learningData.Kifu.CurNode).Foreach_ChildNodes((Move key, Node<Move, KyokumenWrapper> node, ref bool toBreak) =>
                {
#if DEBUG || LEARN
                    KyHyokaMeisai_Koumoku komawariMeisai;
                    KyHyokaMeisai_Koumoku ppMeisai;
#endif

                    learningData.DoScoreing_ForLearning(
                        (KifuNode)node
#if DEBUG || LEARN
,
                        out komawariMeisai,
                        out ppMeisai
#endif
);

                    GohosyuListItem item = new GohosyuListItem(
                        itemNumber,
                        key,
                        Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(node, errH)
#if DEBUG || LEARN
,
                        komawariMeisai,
                        ppMeisai
#endif
);
                    list.Add(item);

                    itemNumber++;
                });

                //----------------------------------------
                // ソート
                //----------------------------------------
                //
                // 先手は正の数、後手は負の数で、絶対値の高いもの順。
                list.Sort((GohosyuListItem a, GohosyuListItem b) =>
                {
                    int result;

                    int aScore =
#if DEBUG || LEARN
                        (int)(
                        a.KomawariMeisai.UtiwakeValue +
                        a.PpMeisai.UtiwakeValue);
#else
 0;
#endif

                    int bScore =
#if DEBUG || LEARN
 (int)(
                        b.KomawariMeisai.UtiwakeValue +
                        b.PpMeisai.UtiwakeValue);
#else
 0;
#endif

                    switch (learningData.Kifu.CurNode.Value.Kyokumen.KaisiPside)
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
            LearningData learningData, Uc_Main uc_Main, KwErrorHandler errH)
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
            if (null != errH.Dlgt_OnLog1Append_or_Null)
            {
                errH.Dlgt_OnLog1Append_or_Null("sfen=" + Conv_Move.ToSfen(move) + Environment.NewLine);
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
                if (learningData.Kifu.CurNode.HasChildNode(move))
                {
                    Node<Move, KyokumenWrapper> nextNode = learningData.Kifu.CurNode.GetChildNode(move);
                    nextMove = nextNode.Key;//次の棋譜ノードのキーが、指し手（きふわらべ式）になっています。

                }
                else
                {
                    nextMove = Conv_Move.GetErrorMove();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("指し手[" + Conv_Move.ToSfen(move) + "]はありませんでした。\n" + learningData.DumpToAllGohosyu(learningData.Kifu.CurNode.Value.Kyokumen));

                    //Debug.Fail(sb.ToString());
                    errH.DonimoNaranAkirameta("Util_LearningView#Ittesasu_ByBtnClick：" + sb.ToString());
                    MessageBox.Show(sb.ToString(), "エラー");
                }
            }

            //----------------------------------------
            // 一手指したい。
            //----------------------------------------
            //↓↓一手指し
            Sky susunda_Sky_orNull;
            IttesasuResult ittesasuResult;
            Util_IttesasuRoutine.Before1(
                learningData.Kifu.CurNode.Value,
                new IttesasuArgImpl(                    
                    ((KifuNode)learningData.Kifu.CurNode).Value.Kyokumen.KaisiPside,
                    nextMove,// FIXME: エラールートだと、これがヌル
                    learningData.Kifu.CurNode.Value.Kyokumen.Temezumi + 1//1手進める
                ),
                out susunda_Sky_orNull,
                out ittesasuResult,
                //this.Kifu,//診断用
                errH,
                "Util_LearningView#Ittesasu_ByBtnClick"
            );
            Debug.Assert(ittesasuResult.Get_SyuryoNode_OrNull != null, "ittesasuResult.Get_SyuryoNode_OrNull がヌル☆？！");
            Util_IttesasuRoutine.Before2(
                ref ittesasuResult,
                susunda_Sky_orNull,
                errH
            );
            //
            //次ノートを追加します。次ノードを、これからのカレントとします。
            //
            //this.Kifu.AssertChildPside(this.Kifu.CurNode.Value.ToKyokumenConst.KaisiPside, ittesasuResult.Get_SyuryoNode_OrNull.Value.ToKyokumenConst.KaisiPside);
            Util_IttesasuRoutine.After3_ChangeCurrent(
                learningData.Kifu,
                ittesasuResult.Get_SyuryoNode_OrNull.Key,
                ittesasuResult.Get_SyuryoNode_OrNull,
                errH
                );
            // これで、棋譜ツリーに、構造変更があったはず。
            //↑↑一手指し

            //----------------------------------------
            // カレント・ノードより古い、以前読んだ手を削除したい。
            //----------------------------------------
            System.Console.WriteLine("カレント・ノード＝" + Conv_Move.ToSfen( learningData.Kifu.CurNode.Key));
            int result_removedCount = Util_KifuTree282.IzennoHenkaCutter(learningData.Kifu, errH);
            System.Console.WriteLine("削除した要素数＝" + result_removedCount);

            ////----------------------------------------
            //// 合法手一覧を作成したい。
            ////----------------------------------------
            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[KifuWarabeImpl.SEARCHED_PV_LENGTH];
            learningData.Aa_Yomi(
                ref searchedMaxDepth,
                ref searchedNodes,
                searchedPv,
                errH);
            // ノード情報の表示
            Util_LearningView.Aa_ShowNode2(uc_Main.LearningData, uc_Main, errH);

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
