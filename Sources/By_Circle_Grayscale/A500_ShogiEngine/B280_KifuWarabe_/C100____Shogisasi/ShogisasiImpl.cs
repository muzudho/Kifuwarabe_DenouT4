﻿using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C___490_Option__;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A240_KifuTreeLog.B110_KifuTreeLog.C500____Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___005_Usi_Loop;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C250____Args;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C___500_struct__;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C500____struct__;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct;
using System;
using System.Collections.Generic;

#if DEBUG
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
#endif

namespace Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C100____Shogisasi
{
    /// <summary>
    /// 将棋指しエンジン。
    /// 
    /// 指し手を決めるエンジンです。
    /// </summary>
    public class ShogisasiImpl : Shogisasi
    {
        private ShogiEngine Owner { get { return this.owner; } }
        private ShogiEngine owner;

        /// <summary>
        /// 右脳。
        /// </summary>
        public FeatureVector FeatureVector { get; set; }

        /// <summary>
        /// 時間管理
        /// </summary>
        public TimeManager TimeManager { get; set; }

        public ShogisasiImpl(ShogiEngine owner)
        {
            this.owner = owner;
            this.FeatureVector = new FeatureVectorImpl();
            this.TimeManager = new TimeManagerImpl(owner.EngineOptions.GetOption(EngineOptionNames.THINKING_MILLI_SECOND).GetNumber());
        }


        /// <summary>
        /// 対局開始のとき。
        /// </summary>
        public void OnTaikyokuKaisi()
        {
        }

        /// <summary>
        /// 指し手を決めます。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="kifu"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public KifuNode WA_Bestmove(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            bool isHonshogi,
            KifuTree kifu,
            KwLogger errH
            )
        {
            //────────────────────────────────────────
            // ストップウォッチ
            //────────────────────────────────────────
            this.TimeManager.Stopwatch.Restart();

#if DEBUG
            KaisetuBoards logF_kiki = new KaisetuBoards();// デバッグ用だが、メソッドはこのオブジェクトを必要としてしまう。
#endif
            EvaluationArgs args = new EvaluationArgsImpl(
                kifu.GetSennititeCounter(),
                this.FeatureVector,
                this,
                Util_KifuTreeLogWriter.REPORT_ENVIRONMENT
#if DEBUG
                ,
                logF_kiki
#endif
                );

            float alphabeta_otherBranchDecidedValue;
            switch (((KifuNode)kifu.CurNode).Value.KaisiPside)
            {
                case Playerside.P1:
                    // 2プレイヤーはまだ、小さな数を見つけていないという設定。
                    alphabeta_otherBranchDecidedValue = float.MaxValue;
                    break;
                case Playerside.P2:
                    // 1プレイヤーはまだ、大きな数を見つけていないという設定。
                    alphabeta_otherBranchDecidedValue = float.MinValue;
                    break;
                default: throw new Exception("探索直前、プレイヤーサイドのエラー");
            }

            try
            {
                //
                // 指し手生成ルーチンで、棋譜ツリーを作ります。
                //
                new Tansaku_FukasaYusen_Routine().WAA_Yomu_Start(
                    ref searchedMaxDepth,
                    ref searchedNodes,
                    searchedPv,
                    kifu, isHonshogi, Mode_Tansaku.Shogi_ENgine, alphabeta_otherBranchDecidedValue, args, errH);
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "棋譜ツリーを作っていたときです。"); throw ex; }


#if DEBUG
            //
            // 評価明細ログの書出し
            //
            Util_KifuTreeLogWriter.A_Write_KifuTreeLog(
                logF_kiki,
                kifu,
                errH
                );
            //Util_LogWriter500_HyokaMeisai.Log_Html5(
            //    this,
            //    logF_kiki,
            //    kifu,
            //    playerInfo,
            //    errH
            //    );
#endif

            KifuNode bestKifuNode = this.ChoiceBest(isHonshogi, kifu.CurNode, errH);


            this.TimeManager.Stopwatch.Stop();
            return bestKifuNode;
        }

        private KifuNode ChoiceBest(
            bool isHonshogi,
            Node<Move, Sky> rootNode,
            KwLogger errH
            )
        {
            // 同着もいる☆
            List<KifuNode> bestNodes = new List<KifuNode>();

            // 評価値の高いノードだけを残します。（同点が残る）
            try
            {
                int exception_area = 0;

                try
                {
                    //
                    // ノードが２つもないようなら、スキップします。
                    //
                    if (rootNode.Count_ChildNodes < 2)
                    {
                        goto gt_EndSort;
                    }


                    exception_area = 1000;

                    // ソートしたいので、リスト構造に移し変えます。
                    List<Node<Move, Sky>> rankedNodes = new List<Node<Move, Sky>>();
                    {
                        try
                        {
                            rootNode.Foreach_ChildNodes((Move key, Node<Move, Sky> node, ref bool toBreak) =>
                            {
                                rankedNodes.Add(node);
                            });

                            exception_area = 1000;

                            // ソートします。
                            rankedNodes.Sort((a, b) =>
                            {
                                float bScore;
                                float aScore;

                                // 比較できないものは 0 にしておく必要があります。
                                if (!(a is KifuNode) || !(b is KifuNode))
                                {
                                    return 0;
                                }

                                bScore = ((KifuNode)b).NodeEx.Score;
                                aScore = ((KifuNode)a).NodeEx.Score;

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
                    if (rootNode.Value.KaisiPside == Playerside.P2)
                    {
                        // 1番高いスコアを調べます。
                        goodestScore = ((KifuNode)rankedNodes[0]).NodeEx.Score;
                        for (int iNode = 0; iNode < rankedNodes.Count; iNode++)
                        {
                            if (goodestScore == ((KifuNode)rankedNodes[iNode]).NodeEx.Score)
                            {
                                bestNodes.Add((KifuNode)rankedNodes[iNode]);
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
                        goodestScore = ((KifuNode)rankedNodes[rankedNodes.Count - 1]).NodeEx.Score;
                        for (int iNode = rankedNodes.Count - 1; -1 < iNode; iNode--)
                        {
                            if (goodestScore == ((KifuNode)rankedNodes[iNode]).NodeEx.Score)
                            {
                                bestNodes.Add((KifuNode)rankedNodes[iNode]);
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
            KifuNode bestKifuNode = null;
            try
            {
                {
                    // 次のノードをシャッフル済みリストにします。
                    LarabeShuffle<KifuNode>.Shuffle_FisherYates(ref bestNodes);

                    // シャッフルした最初のノードを選びます。
                    if (0 < bestNodes.Count)
                    {
                        bestKifuNode = bestNodes[0];
                    }
                }
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "ベストムーブ後半３０：同点決勝"); throw ex; }

            return bestKifuNode;
        }
    }
}
