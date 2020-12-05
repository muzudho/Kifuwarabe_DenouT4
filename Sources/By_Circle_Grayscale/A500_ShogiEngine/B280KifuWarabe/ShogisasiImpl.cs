using System;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A240_KifuTreeLog.B110KifuTreeLog.C500Struct;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B200Scoreing.C005UsiLoop;
using Grayscale.A500ShogiEngine.B200Scoreing.C240Shogisasi;
using Grayscale.A500ShogiEngine.B200Scoreing.C250Args;
using Grayscale.A500ShogiEngine.B210TimeMan.C500struct;
using Grayscale.A500ShogiEngine.B210TimeMan.C500Struct;
using Grayscale.A500ShogiEngine.B240_TansaFukasa.C500Struct;

#if DEBUG
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
#endif

namespace Grayscale.A500ShogiEngine.B280KifuWarabe.C100Shogisasi
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
            Tree kifu1,// ツリーを伸ばしているぜ☆（＾～＾）
            Playerside psideA,
            ISky positionA,

            KwLogger errH
            )
        {
            MoveEx bestNode = null;

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


            try
            {
                //
                // 指し手生成ルーチンで、棋譜ツリーを作ります。
                //
                // 指し手は１つに絞ること。
                //
                bestNode = Tansaku_FukasaYusen_Routine.WAA_Yomu_Start(
                    ref searchedMaxDepth,
                    ref searchedNodes,
                    searchedPv,

                    kifu1,// ツリーを伸ばしているぜ☆（＾～＾）
                    psideA,//positionA.GetKaisiPside(),
                    positionA,

                    isHonshogi, Mode_Tansaku.Shogi_ENgine,
                    args, errH);
            }
            catch (Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "棋譜ツリーを作っていたときです。");
                throw ex;
            }


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

            this.TimeManager.Stopwatch.Stop();
            return bestNode;
        }

    }
}
