﻿// アルファ法のデバッグ出力をする場合。
//#define DEBUG_ALPHA_METHOD

using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C500____Util;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C___500_struct__;
using Grayscale.A500_ShogiEngine.B220_Tansaku____.C___500_Tansaku;
using Grayscale.A500_ShogiEngine.B220_Tansaku____.C500____Struct;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C250____Log;
using Grayscale.A210_KnowNingen_.B810_LogGraphiEx.C500____Util;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
#else
#endif

namespace Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct
{

    /// <summary>
    /// 探索ルーチン
    /// </summary>
    public class Tansaku_FukasaYusen_Routine
    {

        public static Tansaku_Genjo CreateGenjo(
            int temezumi,
            bool isHonshogi,
            Mode_Tansaku mode_Tansaku,
            KwLogger errH
            )
        {
            // TODO:ここではログを出力せずに、ツリーの先端で出力したい。
            KaisetuBoards logF_moveKiki = new KaisetuBoards();

            // TODO:「読む」と、ツリー構造が作成されます。
            //int[] yomuLimitter = new int[]{
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    100, // 読みの2手目の横幅
            //    100, // 読みの3手目の横幅
            //    //2, // 読みの4手目の横幅
            //    //1 // 読みの5手目の横幅
            //};

            //// ↓これなら１手１秒で指せる☆
            //int[] yomuLimitter = new int[]{
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    150, // 読みの2手目の横幅
            //    150, // 読みの3手目の横幅
            //    //2 // 読みの4手目の横幅
            //    //1 // 読みの5手目の横幅
            //};

            //int[] yomuLimitter = new int[]{
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    600, // 読みの2手目の横幅
            //    600, // 読みの3手目の横幅
            //};

            //ok
            //int[] yomuLimitter = new int[]{
            //    0,   // 現局面は無視します。
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    600, // 読みの2手目の横幅
            //};

            int[] yomuLimitter;
#if DEBUG
            if (mode_Tansaku == Mode_Tansaku.Learning)
            {
                // 学習モードでは、スピード優先で、2手の読みとします。

                // ２手の読み。
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                };
            }
            else
            {
                /*
                // ２手の読み。
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                };
                // */

                //*
                // ３手の読み。
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                    600, // 読みの3手目の横幅
                };
                // */
            }
#else
            if (mode_Tansaku == Mode_Tansaku.Learning)
            {
                //System.Windows.Forms.MessageBox.Show("学習モード");
                // 学習モードでは、スピード優先で、2手の読みとします。

                //* // ２手の読み。
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                };
                // */

                /* // ３手の読み。
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                    600, // 読みの3手目の横幅
                };
                // */

            }
            else
            {
                //System.Windows.Forms.MessageBox.Show("本番モード");
                //* ２手の読み。
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                };
                // */

                /* ３手の読み。
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                    600, // 読みの3手目の横幅
                };
                // */

                // 読みを深くすると、玉の手しか読めなかった、ということがある。

                /* ４手の読み。
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                    600, // 読みの3手目の横幅
                    600, // 読みの4手目の横幅
                };
                // */
            }
#endif


            Tansaku_Args args = new Tansaku_ArgsImpl(isHonshogi, yomuLimitter, logF_moveKiki);
            Tansaku_Genjo genjo = new Tansaku_GenjoImpl(
                temezumi,
                args
                );

            return genjo;
        }

        /// <summary>
        /// 読む。
        /// 
        /// 棋譜ツリーを作成します。
        /// </summary>
        /// <param name="kifu">この棋譜ツリーの現局面に、次局面をぶら下げて行きます。</param>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static MoveEx WAA_Yomu_Start(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,

            int temezumi,
            Node kifuNode,

            bool isHonshogi,
            Mode_Tansaku mode_Tansaku,
            MoveEx alphabeta_otherBranchDecidedValue,
            EvaluationArgs args,
            KwLogger errH
            )
        {
            int exceptionArea = 0;

            try
            {
                exceptionArea = 10;
                Tansaku_Genjo genjo = Tansaku_FukasaYusen_Routine.CreateGenjo(
                    temezumi,
                    isHonshogi, mode_Tansaku, errH);

                MoveEx moveEx = kifuNode.MoveEx;
                Sky position = kifuNode.GetValue();

                int wideCount2 = 0;

                // 
                // （１）合法手に一対一対応した子ノードを作成し、ハブ・ノードにぶら下げます。
                //
                List<Move> movelist;
                int yomiDeep;
                MoveEx a_bestmoveChildren;
                Util_MovePicker.CreateMovelist_BeforeLoop(
                    genjo,

                    moveEx.Move,
                    position,

                    out movelist,
                    ref searchedMaxDepth,
                    out yomiDeep,
                    errH
                    );
                a_bestmoveChildren = new MoveExImpl(Move.Empty);
                a_bestmoveChildren.SetScore(Util_Scoreing.GetWorstScore(position.KaisiPside));// プレイヤー1ならmax値、プレイヤー2ならmin値。

                if (Tansaku_FukasaYusen_Routine.CanNotNextLoop(yomiDeep, wideCount2, movelist.Count, genjo, args.Shogisasi.TimeManager))
                {
                    // 1手も読まないのなら。
                    // FIXME: エラー？
                    //----------------------------------------
                    // もう深くよまないなら
                    //----------------------------------------

                    // 局面に評価を付けます。
                    Tansaku_FukasaYusen_Routine.Do_Leaf(
                        genjo,

                        moveEx,
                        position,

                        args,
                        errH
                        );

                    a_bestmoveChildren = Util_Scoreing.GetHighScore(moveEx, a_bestmoveChildren, position.KaisiPside);
                }
                else
                {
                    // ここが再帰のスタート地点☆（＾▽＾）
                    a_bestmoveChildren = Tansaku_FukasaYusen_Routine.WAAA_Yomu_Loop(
                        ref searchedMaxDepth,
                        ref searchedNodes,
                        searchedPv,
                        genjo,
                        alphabeta_otherBranchDecidedValue,

                        moveEx.Move,
                        position,
                        kifuNode,//改造前

                        movelist.Count,
                        args,
                        errH
                        );
                }


#if DEBUG
                exceptionArea = 20;
                if (0 < genjo.Args.LogF_moveKiki.boards.Count)//ﾛｸﾞが残っているなら
                {
                    ////
                    //// ログの書き出し
                    ////
                    //Util_GraphicalLog.WriteHtml5(
                    //    true,//enableLog,
                    //    "SasiteRoutine#Yomi_NextNodes(00)新ログ",
                    //    "[" + Util_GraphicalLog.BoardFileLog_ToJsonStr(genjo.Args.LogF_moveKiki) + "]"
                    //);

                    // 書き出した分はクリアーします。
                    genjo.Args.LogF_moveKiki.boards.Clear();
                }
#endif
            }
            catch (Exception ex)
            {
                switch (exceptionArea)
                {
                    case 10:
                        {
                            //>>>>> エラーが起こりました。
                            string message = ex.GetType().Name + " " + ex.Message + "：棋譜ツリーの読みの中盤５０です。：";
                            Debug.Fail(message);

                            // どうにもできないので  ログだけ取って、上に投げます。
                            errH.AppendLine(message);
                            errH.Flush(LogTypes.Error);
                            throw ex;
                        }
#if DEBUG
                    case 20:
                        {
                            errH.DonimoNaranAkirameta(ex, "棋譜ツリーの読みの後半９０です。"); throw ex;
                        }
#endif
                    default: throw ex;
                }
            }

            // ヌルになることがある？
            return Tansaku_FukasaYusen_Routine.ChoiceBest(isHonshogi, kifuNode, errH);
        }
        private static MoveEx ChoiceBest(
            bool isHonshogi,
            Node rootNode,
            KwLogger errH
        )
        {
            // 同着もいる☆
            List<MoveEx> bestmoveExs = new List<MoveEx>();

            // 評価値の高いノードだけを残します。（同点が残る）
            try
            {
                int exception_area = 0;

                try
                {
                    //
                    // ノードが２つもないようなら、スキップします。
                    //
                    if (rootNode.Children1.Count < 2)
                    {
                        goto gt_EndSort;
                    }


                    exception_area = 1000;

                    // ソートしたいので、リスト構造に移し変えます。
                    List<MoveEx> rankedMoveExs = new List<MoveEx>();
                    {
                        try
                        {
                            rootNode.Children1.Foreach_ChildNodes((Move key, Node node, ref bool toBreak) =>
                            {
                                rankedMoveExs.Add(node.MoveEx);
                            });

                            exception_area = 1000;

                            // ソートします。
                            rankedMoveExs.Sort((a, b) =>
                            {
                                float bScore;
                                float aScore;

                                // 比較できないものは 0 にしておく必要があります。
                                if (!(a is MoveEx) || !(b is MoveEx))
                                {
                                    return 0;
                                }

                                bScore = ((MoveEx)b).Score;
                                aScore = ((MoveEx)a).Score;

                                return (int)aScore.CompareTo(bScore);//点数が大きいほうが前に行きます。
                            });
                        }
                        catch (Exception ex)
                        {
                            errH.DonimoNaranAkirameta(ex, "ベストムーブ／ハイスコア抽出中 exception_area=[" + exception_area + "]"); throw ex;
                        }
                    }

                    exception_area = 1500;

                    // 先手は先頭、後手は最後尾の要素が、一番高いスコア（同着あり）
                    float goodestScore;
                    if (rootNode.GetValue().KaisiPside == Playerside.P2)
                    {
                        // 1番高いスコアを調べます。
                        goodestScore = rankedMoveExs[0].Score;
                        for (int iNode = 0; iNode < rankedMoveExs.Count; iNode++)
                        {
                            if (goodestScore == rankedMoveExs[iNode].Score)
                            {
                                bestmoveExs.Add(rankedMoveExs[iNode]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 2Pは、マイナスの方が良い。
                        goodestScore = rankedMoveExs[rankedMoveExs.Count - 1].Score;
                        for (int iNode = rankedMoveExs.Count - 1; -1 < iNode; iNode--)
                        {
                            if (goodestScore == rankedMoveExs[iNode].Score)
                            {
                                bestmoveExs.Add(rankedMoveExs[iNode]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errH.DonimoNaranAkirameta(ex, "ベストムーブ／ハイスコア抽出中 exception_area=[" + exception_area + "]"); throw ex;
                }

                gt_EndSort:
                ;
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "ベストムーブ後半２０：ハイスコア抽出"); throw ex; }


            // 評価値の同点があれば、同点決勝をして　1手に決めます。
            MoveEx bestmoveEx = null;// 投了のとき
            try
            {
                {
                    // 次のノードをシャッフル済みリストにします。
                    LarabeShuffle<MoveEx>.Shuffle_FisherYates(ref bestmoveExs);

                    // シャッフルした最初のノードを選びます。
                    if (0 < bestmoveExs.Count)
                    {
                        bestmoveEx = bestmoveExs[0];
                    }
                }
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "ベストムーブ後半３０：同点決勝"); throw ex; }

            return bestmoveEx;
        }

        /// <summary>
        /// もう次の手は読まない、というときは真☆
        /// </summary>
        /// <param name="yomiDeep"></param>
        /// <param name="wideCount2"></param>
        /// <param name="movelist_count"></param>
        /// <param name="genjo"></param>
        /// <returns></returns>
        public static bool CanNotNextLoop(
            int yomiDeep,
            int wideCount2,
            int movelist_count,
            Tansaku_Genjo genjo,
            TimeManager timeManager
            )
        {
            return
                timeManager.IsTimeOver()//思考の時間切れ
                ||
                (genjo.Args.YomuLimitter.Length <= yomiDeep + 1)//読みの深さ制限を超えているとき。
                || //または、
                (1 < yomiDeep && genjo.Args.YomuLimitter[yomiDeep] < wideCount2)//読みの１手目より後で、指定の横幅を超えているとき。
                || //または、
                (movelist_count < 1)//合法手がないとき
                ;
        }

        /// <summary>
        /// もう深く読まない場合の処理。
        /// </summary>
        private static void Do_Leaf(
            Tansaku_Genjo genjo,

            MoveEx moveEx,
            Sky position,

            EvaluationArgs args,
            KwLogger errH
            )
        {
            // 局面に評価値を付けます。
            Util_Scoreing.DoScoreing_Kyokumen(

                moveEx,//mutable: スコアを覚えるのに使っている。
                position,

                args,
                errH
                );

#if DEBUG_ALPHA_METHOD
                    errH.AppendLine_AddMemo("1. 手(" + node_yomi.Value.ToKyokumenConst.Temezumi + ")読(" + yomiDeep + ") 兄弟最善=[" + a_siblingDecidedValue + "] 子ベスト=[" + a_childrenBest + "]");
#endif

#if DEBUG
                bool enableLog = false;
                //
                // ログの書き出し
                //
                Util_GraphicalLog.WriteHtml5(
                    enableLog,
                    "指し手生成ログA",
                    "[" + Conv_KaisetuBoards.ToJsonStr(genjo.Args.LogF_moveKiki) + "]"
                );
            // 書き出した分はクリアーします。
            genjo.Args.LogF_moveKiki.boards.Clear();
#endif

            //#if DEBUG
            //                    //
            //                    // 盤１個分のログの準備
            //                    //
            //                    Util_LogBuilder510.Build_LogBoard(
            //                        nodePath,
            //                        niniNode,
            //                        kifu_forAssert,
            //                        reportEnvironment,
            //                        logF_kiki,
            //                        errH
            //                    );
            //#endif
        }

        /// <summary>
        /// 棋譜ツリーに、ノードを追加していきます。再帰します。
        /// </summary>
        /// <param name="genjo"></param>
        /// <param name="alphabeta_otherBranchDecidedValue"></param>
        /// <param name="args"></param>
        /// <param name="errH"></param>
        /// <returns>子の中で最善の点</returns>
        private static MoveEx WAAA_Yomu_Loop(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            Tansaku_Genjo genjo,
            MoveEx a_parentsiblingDecidedValue,

            Move move1,//改造後
            Sky position1,//改造後
            Node node_yomi_KAIZOMAE,//改造前

            int movelist_count,
            EvaluationArgs args,
            KwLogger errH
            )
        {
            int exceptionArea = 0;
            MoveEx a_childrenBest;

            try
            {
                exceptionArea = 1000;
                //
                // まず前提として、
                // 現手番の「被王手の局面」だけがピックアップされます。
                // これはつまり、次の局面がないときは、その枝は投了ということです。
                //


                // 
                // （１）合法手に一対一対応した子ノードを作成し、ハブ・ノードにぶら下げます。
                //
                List<Move> movelist2;
                int yomiDeep2;
                Util_MovePicker.CreateMovelist_BeforeLoop(
                    genjo,

                    move1,
                    position1,

                    out movelist2,
                    ref searchedMaxDepth,
                    out yomiDeep2,
                    errH
                    );
                a_childrenBest = new MoveExImpl(Move.Empty);
                a_childrenBest.SetScore(Util_Scoreing.GetWorstScore(position1.KaisiPside));// プレイヤー1ならmax値、プレイヤー2ならmin値。

                exceptionArea = 2000;

                int wideCount1 = 0;
                foreach (Move childMove2 in movelist2)//次に読む手
                {
                    MoveEx childMoveEx2 = new MoveExImpl(childMove2);

                    if (Tansaku_FukasaYusen_Routine.CanNotNextLoop(
                        yomiDeep2,
                        wideCount1,
                        movelist2.Count,
                        genjo,
                        args.Shogisasi.TimeManager
                    ))
                    {

                        exceptionArea = 3000;

                        //----------------------------------------
                        // もう深くよまないなら
                        //----------------------------------------
                        Tansaku_FukasaYusen_Routine.Do_Leaf(
                            genjo,

                            node_yomi_KAIZOMAE.MoveEx,//改造後: スコアを覚えるのに使っている。
                            position1,//改造前

                            args,
                            errH
                            );
                        a_childrenBest = Util_Scoreing.GetHighScore(node_yomi_KAIZOMAE.MoveEx, a_childrenBest, position1.KaisiPside);

                        wideCount1++;
                        break;
                    }
                    else
                    {
                        Sky childPosition2;
                        Node childNode2_KAIZOMAE;

                        try
                        {
                            exceptionArea = 4010;

                            //----------------------------------------
                            // 《９》まだ深く読むなら
                            //----------------------------------------
                            // 《８》カウンターを次局面へ

                            // 探索ノードのカウントを加算☆（＾～＾）少ないほど枝刈りの質が高いぜ☆
                            searchedNodes++;

                            // このノードは、途中節か葉か未確定。

                            //
                            // （２）指し手を、ノードに変換し、現在の局面に継ぎ足します。
                            //
                            exceptionArea = 4020;

                            // 指す前の駒を、盤上のマス目で指定
                            Finger figSasumaenoKoma = Util_Sky_FingersQuery.InMasuNow_Old(position1, Conv_Move.ToSrcMasu(childMove2)).ToFirst();

                            childPosition2 = Util_IttesasuSuperRoutine.DoMove_Super(
                                    position1,//指定局面
                                    figSasumaenoKoma,//指す駒
                                    Conv_Move.ToDstMasu(childMove2),//移動先升
                                    Conv_Move.ToPromotion(childMove2),//成るか。
                                    errH
                            );
                            childNode2_KAIZOMAE = new NodeImpl(
                                childMove2,
                                null//new Sky(childPosition)// TODO: これを止めたいんだぜ☆（＾～＾）？
                                );


                            exceptionArea = 44011;
                            //*
                            if (node_yomi_KAIZOMAE.Children1.Count<1)
                            {
                                // １件もなければ、そのまま追加☆（＾～＾）
                                node_yomi_KAIZOMAE.Children1.AddItem(
                                    childMove2,
                                    childNode2_KAIZOMAE,
                                    node_yomi_KAIZOMAE
                                    );
                            }
                            else
                            {
                                /*
                                // ２件以上あるなら、
                                // 適当に先頭の１個を取り出して、比較してハイスコアの方だけにするぜ☆（＾～＾）
                                a_childrenBest = Util_Scoreing.GetHighScore(
                                    node_yomi_KAIZOMAE.Children1.GetFirst().MoveEx,
                                    childMoveEx2,
                                    position1.KaisiPside
                                    );
                                    */
                                //node_yomi_KAIZOMAE.Children1.ClearAll();
                                /*
                                node_yomi_KAIZOMAE.Children1.AddItem(
                                    a_childrenBest.Move,
                                    new NodeImpl(a_childrenBest.Move,null),
                                    node_yomi_KAIZOMAE
                                    );
                                */
                                node_yomi_KAIZOMAE.Children1.AddItem(
                                    childMove2,
                                    childNode2_KAIZOMAE,
                                    node_yomi_KAIZOMAE
                                    );
                            }
                            //*/
                            /*
                            // TODO: ここで記憶☆（＾～＾）これを止めたいんだぜ☆（＾～＾）？
                            node_yomi_KAIZOMAE.Children1.AddItem(
                                childMove2, childNode2_KAIZOMAE, node_yomi_KAIZOMAE);
                            //*/


                            exceptionArea = 45099;
                        }
                        catch (Exception ex)
                        {
                            StringBuilder sb = new StringBuilder();

                            int i = 0;
                            foreach(Move entry2 in movelist2)
                            {
                                sb.Append(Conv_Move.ToSfen(entry2));
                                sb.Append(",");
                                if (0 == i % 15)
                                {
                                    sb.AppendLine();
                                }
                                i++;
                            }

                            errH.DonimoNaranAkirameta(ex, "棋譜ツリーで例外です(A)。exceptionArea=" + exceptionArea
                                + " entry.Key=" + Conv_Move.ToSfen(childMove2)
                                //+ " node_yomi.CountAllNodes=" + node_yomi_KAIZOMAE.CountAllNodes()
                                + " 指し手候補="+sb.ToString()); throw ex;

                        }

                        exceptionArea = 5000;

                        // これを呼び出す回数を減らすのが、アルファ法。
                        // 枝か、葉か、確定させにいきます。
                        MoveEx a_myScore = Tansaku_FukasaYusen_Routine.WAAA_Yomu_Loop(
                            ref searchedMaxDepth,
                            ref searchedNodes,
                            searchedPv,
                            genjo,
                            a_childrenBest,

                            childMove2,//改造後
                            childPosition2,//改造後
                            childNode2_KAIZOMAE,//改造前

                            movelist2.Count,
                            args,
                            errH);

                        exceptionArea = 7000;

                        //----------------------------------------
                        // 子要素の検索が終わった時点で、読み筋を格納☆
                        //----------------------------------------
                        searchedPv[yomiDeep2] = Conv_Move.ToSfen(childMove2); //FIXME:
                        searchedPv[yomiDeep2 + 1] = "";//後ろの１手を消しておいて 終端子扱いにする。


                        exceptionArea = 8000;

                        //----------------------------------------
                        // 子要素の検索が終わった時点
                        //----------------------------------------
                        bool alpha_cut;
                        Util_Scoreing.Update_BestScore_And_Check_AlphaCut(
                            yomiDeep2,// yomiDeep0,

                            position1.KaisiPside,

                            a_parentsiblingDecidedValue,
                            a_myScore.Score,
                            ref a_childrenBest,
                            out alpha_cut
                            );


                        exceptionArea = 9000;

                        wideCount1++;

#if DEBUG_ALPHA_METHOD
                errH.AppendLine_AddMemo("3. 手(" + node_yomi.Value.ToKyokumenConst.Temezumi + ")読(" + yomiDeep + ") 兄弟最善=[" + a_siblingDecidedValue + "] 子ベスト=[" + a_childrenBest + "] 自点=[" + a_myScore + "]");
#endif
                        if (alpha_cut)
                        {
#if DEBUG_ALPHA_METHOD
                        errH.AppendLine_AddMemo("アルファ・カット☆！");
#endif
                            //----------------------------------------
                            // 次の「子の弟」要素はもう読みません。
                            //----------------------------------------

                            //*TODO:
                            break;
                            //toBreak1 = true;
                            // */
                        }
                    }

                    exceptionArea = 10000;

                    //gt_NextLoop:
                    //    ;
                }


            }
            catch (Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "棋譜ツリーで例外です(B)。exceptionArea=" + exceptionArea); throw ex;
            }

            return a_childrenBest;
        }
#if DEBUG
        public static void Log1(
            Tansaku_Genjo genjo,
            Move move_forLog,
            Sky src_Sky,
            out MmLogGenjoImpl out_mm_log,
            out KaisetuBoard out_logBrd_move1,
            KwLogger errH
        )
        {
            out_logBrd_move1 = new KaisetuBoard();// 盤１個分のログの準備

            try
            {
                out_mm_log = new MmLogGenjoImpl(
                        genjo.YomikaisiTemezumi,
                        out_logBrd_move1,//ログ？
                        src_Sky.Temezumi,//手済み
                        move_forLog,//指し手
                        errH//ログ
                    );
            }
            catch (Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "棋譜ツリーの読みループの作成次ノードの前半２０です。"); throw ex;
            }
        }
        private static void Log2(
            Tansaku_Genjo genjo,
            Node node_yomi,
            KaisetuBoard logBrd_move1,
            KwLogger errH
        )
        {
            try
            {
                logBrd_move1.Move = node_yomi.Key;

                SyElement srcMasu = Conv_Move.ToSrcMasu(logBrd_move1.Move);
                SyElement dstMasu = Conv_Move.ToDstMasu(logBrd_move1.Move);

                // ログ試し
                logBrd_move1.Arrow.Add(new Gkl_Arrow(Conv_SyElement.ToMasuNumber(srcMasu),
                    Conv_SyElement.ToMasuNumber(dstMasu)));
                genjo.Args.LogF_moveKiki.boards.Add(logBrd_move1);
            }
            catch (Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "棋譜ツリーの読みループの作成次ノードの前半４０です。"); throw ex;
            }
        }
#endif
    }

}
