namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
#if DEBUG
    using System;
    using Grayscale.Kifuwaragyoku.Entities.Features;
    using Grayscale.Kifuwaragyoku.Entities.Logging;
    using Grayscale.Kifuwaragyoku.UseCases;
#else
    using Grayscale.Kifuwaragyoku.Entities.Evaluation;
    using Grayscale.Kifuwaragyoku.Entities.Features;
    using Grayscale.Kifuwaragyoku.Entities.Searching;
    using Grayscale.Kifuwaragyoku.UseCases;
#endif

    /// <summary>
    /// 将棋指しエンジン。
    /// 
    /// 指し手を決めるエンジンです。
    /// </summary>
    public class ShogisasiImpl : IShogisasi
    {
        /// <summary>
        /// 右脳。
        /// </summary>
        public IFeatureVector FeatureVector { get; set; }

        /// <summary>
        /// 時間管理
        /// </summary>
        public ITimeManager TimeManager { get; set; }

        public ShogisasiImpl(Playing playing, IFeatureVector fv)
        {
            this.FeatureVector = fv; //  new FeatureVectorImpl();
            this.TimeManager = new TimeManager(playing.EngineOptions.GetOption(EngineOptionNames.THINKING_MILLI_SECOND).GetNumber());
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
            ISky positionA)
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
                UtilKifuTreeLogWriter.REPORT_ENVIRONMENT
#if DEBUG
                ,
                logF_kiki
#endif
                );


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
                args);


#if DEBUG
            //
            // 評価明細ログの書出し
            //
            Util_KifuTreeLogWriter.A_Write_KifuTreeLog(
                logF_kiki,
                kifu1,
                logTag
                );
            //Util_LogWriter500_HyokaMeisai.Log_Html5(
            //    this,
            //    logF_kiki,
            //    kifu,
            //    playerInfo,
            //    logTag
            //    );
#endif

            this.TimeManager.Stopwatch.Stop();
            return bestNode;
        }

    }
}
