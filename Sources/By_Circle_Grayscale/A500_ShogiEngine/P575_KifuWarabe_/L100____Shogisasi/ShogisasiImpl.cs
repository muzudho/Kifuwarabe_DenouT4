using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A240_KifuTreeLog.B110_KifuTreeLog.C500____Struct;
using Grayscale.P521_FeatureVect.C___500_Struct;
using Grayscale.P521_FeatureVect.C500____Struct;
using Grayscale.P542_Scoreing___.L___005_Usi_Loop;
using Grayscale.P542_Scoreing___.L___240_Shogisasi;
using Grayscale.P542_Scoreing___.L___250_Args;
using Grayscale.P542_Scoreing___.L125____ScoreSibori;
using Grayscale.P542_Scoreing___.L250____Args;
using Grayscale.P550_timeMan____.L___500_struct__;
using Grayscale.P550_timeMan____.L500____struct__;
using Grayscale.P554_TansaFukasa.C___500_Struct;
using Grayscale.P554_TansaFukasa.C500____Struct;
using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C___490_Option__;
using System;
using System.Collections.Generic;

#if DEBUG
using Grayscale.A210_KnowNingen_.B400_Log_Kaisetu.C250____Struct;
#endif

namespace Grayscale.P575_KifuWarabe_.L100____Shogisasi
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
        /// 枝狩りエンジン。
        /// </summary>
        public ScoreSiboriEngine EdagariEngine { get; set; }

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
            this.EdagariEngine = new ScoreSiboriEngine();
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
            KwErrorHandler errH
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
            switch (((KifuNode)kifu.CurNode).Value.KyokumenConst.KaisiPside)
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

            try
            {
                // 評価値の高いノードだけを残します。
                this.EdagariEngine.EdaSibori_HighScore(kifu, this, errH);
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "ベストムーブ後半２０：ハイスコア抽出"); throw ex; }


            // 評価値の同点があれば、同点決勝をして　1手に決めます。
            KifuNode bestKifuNode = null;
            try
            {
                bestKifuNode = this.ChoiceNode_DoutenKessyou(kifu, isHonshogi, errH);
            }
            catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "ベストムーブ後半３０：同点決勝"); throw ex; }


            this.TimeManager.Stopwatch.Stop();
            return bestKifuNode;
        }

        /// <summary>
        /// 同点決勝。
        /// 
        /// 評価値が同点のノード（指し手）の中で、ランダムに１つ選びます。
        /// </summary>
        /// <param name="kifu">ツリー構造になっている棋譜</param>
        /// <param name="logTag">ログ</param>
        /// <returns></returns>
        private KifuNode ChoiceNode_DoutenKessyou(
            KifuTree kifu,
            bool isHonshogi, KwErrorHandler errH)
        {
            KifuNode bestKifuNode = null;

            {
                // 次のノードをリストにします。
                //List<KifuNode> nextNodes = Util_Converter280.NextNodes_ToList(kifu.CurNode);

                // 次のノードをシャッフル済みリストにします。
                List<KifuNode> nextNodes_shuffled = Conv_NextNodes.ToList(kifu.CurNode);
                LarabeShuffle<KifuNode>.Shuffle_FisherYates(ref nextNodes_shuffled);

                // シャッフルした最初のノードを選びます。
                if (0 < nextNodes_shuffled.Count)
                {
                    bestKifuNode = nextNodes_shuffled[0];
                }
            }

            return bestKifuNode;
        }



    }
}
