using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
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
        /// <param name="kifu1"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public MoveEx WA_Bestmove(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            bool isHonshogi,

            Earth earth1,
            Tree kifu1,

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
                earth1.GetSennititeCounter(),
                this.FeatureVector,
                this,
                Util_KifuTreeLogWriter.REPORT_ENVIRONMENT
#if DEBUG
                ,
                logF_kiki
#endif
                );

            float alphabeta_otherBranchDecidedValue;
            switch (kifu1.CurNode.Value.KaisiPside)
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

                    kifu1.CurNode.Value.Temezumi,
                    kifu1.CurNode,
                    
                    isHonshogi, Mode_Tansaku.Shogi_ENgine, alphabeta_otherBranchDecidedValue, args, errH);
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "棋譜ツリーを作っていたときです。"); throw ex; }


#if DEBUG
            //
            // 評価明細ログの書出し
            //
            Util_KifuTreeLogWriter.A_Write_KifuTreeLog(
                logF_kiki,
                kifu1,
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

            // ヌルになることがある？
            MoveEx bestNode = this.ChoiceBest(isHonshogi, kifu1.CurNode, errH);


            this.TimeManager.Stopwatch.Stop();
            return bestNode;
        }

        private MoveEx ChoiceBest(
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
                    if (rootNode.Count_ChildNodes < 2)
                    {
                        goto gt_EndSort;
                    }


                    exception_area = 1000;

                    // ソートしたいので、リスト構造に移し変えます。
                    List<MoveEx> rankedMoveExs = new List<MoveEx>();
                    {
                        try
                        {
                            rootNode.Foreach_ChildNodes((Move key, Node node, ref bool toBreak) =>
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
                    if (rootNode.Value.KaisiPside == Playerside.P2)
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
    }
}
