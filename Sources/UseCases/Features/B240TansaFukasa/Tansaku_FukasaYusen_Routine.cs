// アルファ法のデバッグ出力をする場合。
//#define DEBUG_ALPHA_METHOD

using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.Kifuwaragyoku.Entities.Configuration;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Grayscale.Kifuwaragyoku.Entities.Searching;

#if DEBUG
// using Grayscale.Kifuwaragyoku.Entities.Features;
#else
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{

    /// <summary>
    /// 探索ルーチン
    /// </summary>
    public class Tansaku_FukasaYusen_Routine
    {

        public static ICurrentSearch CreateGenjo(
            int temezumi,
            bool isHonshogi,
            Mode_Tansaku mode_Tansaku)
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

                //* ２手の読み。（学習）
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                };
                // */

                /* ３手の読み。（学習）
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
                //* ２手の読み。（対局）
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                };
                // */

                /* ３手の読み。（対局）
                yomuLimitter = new int[]{
                    0,   // 現局面は無視します。
                    600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                    600, // 読みの2手目の横幅
                    600, // 読みの3手目の横幅
                };
                // */

                // 読みを深くすると、玉の手しか読めなかった、ということがある。

                /* ４手の読み。（対局）
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


            ISearchArgs args = new SearchArgs(isHonshogi, yomuLimitter, logF_moveKiki);
            ICurrentSearch genjo = new CurrentSearch(
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
            IPlaying playing,
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            Playerside psideA,//Playerside psideA = positionA.GetKaisiPside();
            IPosition positionA,
            bool isHonshogi,
            Mode_Tansaku mode_Tansaku,
            EvaluationArgs args)
        {
            int temezumi = positionA.Temezumi;

            ICurrentSearch genjo = Tansaku_FukasaYusen_Routine.CreateGenjo(
                temezumi,
                isHonshogi, mode_Tansaku);

            // 最初は投了からスタートだぜ☆（*＾～＾*）
            MoveEx a_bestmoveEx_Children = new MoveExImpl(
                Move.Empty,
                //最悪点からスタートだぜ☆（＾～＾）
                // プレイヤー1ならmax値、プレイヤー2ならmin値。
                Util_Scoreing.GetWorstScore(psideA)
                );

            int wideCount2 = 0;

            // 
            // （１）合法手に一対一対応した子ノードを作成し、ハブ・ノードにぶら下げます。
            //
            int yomiDeep;
            List<Move> movelist = UtilMovePicker.CreateMovelist_BeforeLoop(
                playing.EngineConf,
                genjo,

                psideA,//TODO:
                positionA,

                ref searchedMaxDepth,
                out yomiDeep);

            if (Tansaku_FukasaYusen_Routine.CanNotNextLoop(yomiDeep, wideCount2, movelist.Count, genjo, playing.TimeManager))
            {
                // 1手も読まないのなら。
                // FIXME: エラー？
                //----------------------------------------
                // もう深くよまないなら
                //----------------------------------------

                // 局面に評価を付けます。
                float score = Tansaku_FukasaYusen_Routine.Do_Leaf(
                    playing,
                    genjo,

                    psideA,// positionA.GetKaisiPside(),
                    positionA,

                    args);

                a_bestmoveEx_Children = Util_Scoreing.GetHighScore(
                    a_bestmoveEx_Children.Move,
                    score,
                    a_bestmoveEx_Children,
                    psideA//positionA.GetKaisiPside()
                    );
            }
            else
            {
                // ここが再帰のスタート地点☆（＾▽＾）
                a_bestmoveEx_Children = Tansaku_FukasaYusen_Routine.WAAA_Yomu_Loop(
                    playing,
                    ref searchedMaxDepth,
                    ref searchedNodes,
                    searchedPv,
                    genjo,

                    positionA.Temezumi,
                    psideA,//positionA.GetKaisiPside(),
                    positionA,//この局面から合法手を作成☆（＾～＾）
                    a_bestmoveEx_Children.Score,
                    playing.Kifu.MoveEx_Current,// ツリーを伸ばしているぜ☆（＾～＾）

                    movelist.Count,
                    args);
            }

#if DEBUG
                if (0 < genjo.Args.LogF_moveKiki.boards.Count)//ﾛｸﾞが残っているなら
                {
                    ////
                    //// ログの書き出し
                    ////
                    //Util_GraphicalLog.WriteHtml5(
                    //    true,//enableLog,
                    //    "MoveRoutine#Yomi_NextNodes(00)新ログ",
                    //    "[" + Util_GraphicalLog.BoardFileLog_ToJsonStr(genjo.Args.LogF_moveKiki) + "]"
                    //);

                    // 書き出した分はクリアーします。
                    genjo.Args.LogF_moveKiki.boards.Clear();
                }
#endif

            return a_bestmoveEx_Children;
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
            ICurrentSearch genjo,
            ITimeManager timeManager
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
        private static float Do_Leaf(
            IPlaying playing,
            ICurrentSearch genjo,

            Playerside psideA,
            IPosition positionA,

            EvaluationArgs args)
        {
            float score = 0.0f;

            // 局面に評価値を付けます。
            score += Util_Scoreing.DoScoreing_Kyokumen(
                playing,
                psideA,
                positionA,

                args);

#if DEBUG_ALPHA_METHOD
                    logTag.AppendLine_AddMemo("1. 手(" + node_yomi.Value.ToKyokumenConst.Temezumi + ")読(" + yomiDeep + ") 兄弟最善=[" + a_siblingDecidedValue + "] 子ベスト=[" + a_childrenBest + "]");
#endif

#if DEBUG
            bool enableLog = false;
            //
            // ログの書き出し
            //
            Util_GraphicalLog.WriteHtml5(
                playing.EngineConf,
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
            //                        logTag
            //                    );
            //#endif

            return score;
        }

        /// <summary>
        /// 棋譜ツリーに、ノードを追加していきます。再帰します。
        /// </summary>
        /// <param name="genjo"></param>
        /// <param name="alphabeta_otherBranchDecidedValue"></param>
        /// <param name="args"></param>
        /// <param name="logTag"></param>
        /// <returns>子の中で最善の点</returns>
        private static MoveEx WAAA_Yomu_Loop(
            IPlaying playing,
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            ICurrentSearch genjo,
            int kaisiTemezumi,
            Playerside psideA,
            IPosition positionA,//この局面から、合法手を作成☆（＾～＾）
            float parentsiblingBestScore,
            MoveEx baseNod1,// ツリーを伸ばしているぜ☆（＾～＾）
            int movelist_count,
            EvaluationArgs args)
        {
            MoveEx result_thisDepth;

            //
            // まず前提として、
            // 現手番の「被王手の局面」だけがピックアップされます。
            // これはつまり、次の局面がないときは、その枝は投了ということです。
            //


            // 
            // （１）合法手に一対一対応した子ノードを作成し、ハブ・ノードにぶら下げます。
            //
            int yomiDeep2;
            List<Move> movelist2 = UtilMovePicker.CreateMovelist_BeforeLoop(
                playing.EngineConf,
                genjo,

                psideA,//TODO:
                positionA,

                ref searchedMaxDepth,
                out yomiDeep2);

            // 空っぽにして用意しておくぜ☆
            result_thisDepth = new MoveExImpl(Move.Empty);
            result_thisDepth.SetScore(Util_Scoreing.GetWorstScore(
                positionA.GetKaisiPside() //× psideA//
                ));// プレイヤー1ならmax値、プレイヤー2ならmin値。

            int wideCount1 = 0;
            foreach (Move iMov_child_const in movelist2)//次に読む手
            {
                Move iMov_child_variable = iMov_child_const;
                MoveEx iNod_child = new MoveExImpl(iMov_child_variable);

                if (Tansaku_FukasaYusen_Routine.CanNotNextLoop(
                    yomiDeep2,
                    wideCount1,
                    movelist2.Count,
                    genjo,
                    playing.TimeManager
                ))
                {
                    //----------------------------------------
                    // もう深くよまないなら
                    //----------------------------------------
                    float baseDepth_score = Tansaku_FukasaYusen_Routine.Do_Leaf(
                        playing,
                        genjo,

                        psideA,//positionA.GetKaisiPside(),
                        positionA,//改造前

                        args);

                    //result_movEx3 = new MoveExImpl(nod1.Key, this_score);
                    //*
                    result_thisDepth = Util_Scoreing.GetHighScore(
                        baseNod1.Move,
                        baseDepth_score,
                        result_thisDepth,
                        positionA.GetKaisiPside()//× psideA//
                        );

                    //*/
                    wideCount1++;
                    break;
                }

                //────────────────────────────────────────
                // 葉以外の探索中なら
                //────────────────────────────────────────

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

                // 局面
                Util_IttesasuSuperRoutine.DoMove_Super1(
                    ConvMove.ToPlayerside(iMov_child_variable),
                    ref positionA,//指定局面
                    ref iMov_child_variable,
                    "C100");
                //Playerside psideB = positionA.GetKaisiPside();//反転している☆（*＾～＾*）？
                iNod_child.SetMove(iMov_child_variable);

                // 自分を親要素につなげたあとで、子を検索するぜ☆（＾～＾）
                playing.Kifu.MoveEx_SetCurrent(TreeImpl.OnDoCurrentMove(iNod_child, playing.Kifu, positionA));

                // これを呼び出す回数を減らすのが、アルファ法。
                // 枝か、葉か、確定させにいきます。
                // （＾▽＾）再帰☆
                MoveEx iMovEx_child_temp = Tansaku_FukasaYusen_Routine.WAAA_Yomu_Loop(
                    playing,
                    ref searchedMaxDepth,
                    ref searchedNodes,
                    searchedPv,
                    genjo,
                    positionA.Temezumi,
                    ConvMove.ToPlayerside(iMov_child_variable),// positionA.GetKaisiPside(),
                    positionA,//この局面から合法手を作成☆（＾～＾）
                    result_thisDepth.Score,
                    playing.Kifu.MoveEx_Current,// ツリーを伸ばしているぜ☆（＾～＾）
                    movelist2.Count,
                    args);

                //*
                // １手戻したいぜ☆（＾～＾）

                IIttemodosuResult ittemodosuResult;
                UtilIttemodosuRoutine.UndoMove(
                    out ittemodosuResult,
                    iMov_child_variable,//この関数が呼び出されたときの指し手☆（＾～＾）
                    ConvMove.ToPlayerside(iMov_child_variable),
                    positionA,
                    "C900");
                positionA = ittemodosuResult.SyuryoSky;
                //*/

                playing.Kifu.MoveEx_SetCurrent(
                    TreeImpl.OnUndoCurrentMove(playing.Kifu, ittemodosuResult.SyuryoSky, "WAAA_Yomu_Loop20000")
                );

                //----------------------------------------
                // 子要素の検索が終わった時点で、読み筋を格納☆
                //----------------------------------------
                searchedPv[yomiDeep2] = ConvMove.ToSfen(iMov_child_variable); //FIXME:
                searchedPv[yomiDeep2 + 1] = "";//後ろの１手を消しておいて 終端子扱いにする。

                //----------------------------------------
                // 子要素の検索が終わった時点
                //----------------------------------------
                //
                // 子の点数を、自分に反映させるぜ☆
                bool alpha_cut;
                result_thisDepth = Util_Scoreing.Update_BestScore_And_Check_AlphaCut(
                    result_thisDepth,// これを更新する

                    yomiDeep2,

                    psideA,// positionA.GetKaisiPside(),

                    parentsiblingBestScore,
                    iMovEx_child_temp,

                    out alpha_cut
                    );

                wideCount1++;

#if DEBUG_ALPHA_METHOD
                logTag.AppendLine_AddMemo("3. 手(" + node_yomi.Value.ToKyokumenConst.Temezumi + ")読(" + yomiDeep + ") 兄弟最善=[" + a_siblingDecidedValue + "] 子ベスト=[" + a_childrenBest + "] 自点=[" + a_myScore + "]");
#endif
                if (alpha_cut)
                {
#if DEBUG_ALPHA_METHOD
                    logTag.AppendLine_AddMemo("アルファ・カット☆！");
#endif
                    //----------------------------------------
                    // 次の「子の弟」要素はもう読みません。
                    //----------------------------------------
                    break;
                }
            }

            return result_thisDepth;
        }
#if DEBUG
        //public static void Log1(
        //    Tansaku_Genjo genjo,
        //    IPosition src_Sky,
        //    out MmLogGenjoImpl out_mm_log,
        //    out KaisetuBoard out_logBrd_move1,
        //    ILogger logTag
        //)
        //{
        //    Move move_forLog = Move.Empty;//ログ出力しないことにした☆（＞＿＜）
        //    out_logBrd_move1 = new KaisetuBoard();// 盤１個分のログの準備

        //        out_mm_log = new MmLogGenjoImpl(
        //                genjo.YomikaisiTemezumi,
        //                out_logBrd_move1,//ログ？
        //                src_Sky.Temezumi,//手済み
        //                move_forLog,//指し手
        //                logTag//ログ
        //            );
        //}
        //private static void Log2(
        //    Tansaku_Genjo genjo,
        //    MoveEx node_yomi,
        //    KaisetuBoard logBrd_move1,
        //    ILogger logTag
        //)
        //{
        //        logBrd_move1.Move = node_yomi.Key;

        //        SyElement srcMasu = ConvMove.ToSrcMasu(logBrd_move1.Move);
        //        SyElement dstMasu = ConvMove.ToDstMasu(logBrd_move1.Move);

        //        // ログ試し
        //        logBrd_move1.Arrow.Add(new Gkl_Arrow(Conv_Masu.ToMasuHandle(srcMasu),
        //            Conv_Masu.ToMasuHandle(dstMasu)));
        //        genjo.Args.LogF_moveKiki.boards.Add(logBrd_move1);
        //}
#endif
    }

}
