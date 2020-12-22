using System;
using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
// using Grayscale.Kifuwaragyoku.Entities.Features;
#else
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public abstract class UtilMovePicker
    {

        /// <summary>
        /// ループに入る前に。
        /// </summary>
        /// <param name="genjo"></param>
        /// <param name="node_yomi"></param>
        /// <param name="out_movelist"></param>
        /// <param name="out_yomiDeep"></param>
        /// <param name="out_a_childrenBest"></param>
        /// <param name="logTag"></param>
        public static List<Move> CreateMovelist_BeforeLoop(
            Tansaku_Genjo genjo,

            Playerside psideA,
            ISky positionA,//この局面から合法手を作成☆（＾～＾）

            ref int searchedMaxDepth,
            out int out_yomiDeep,
            ILogTag logTag
            )
        {
            List<Move> result_movelist = UtilMovePicker.WAAAA_Create_ChildNodes(
                genjo,
                psideA,//× Conv_Playerside.Reverse( psideA),
                positionA,
                //move_ForLog,//ログ用
                logTag);

            out_yomiDeep = positionA.Temezumi - genjo.YomikaisiTemezumi + 1;
            if (searchedMaxDepth < out_yomiDeep - 1)//これから探索する分をマイナス1しているんだぜ☆（＾～＾）
            {
                searchedMaxDepth = out_yomiDeep - 1;
            }

            return result_movelist;
        }


        /// <summary>
        /// 指し手をぶら下げます。
        /// 
        /// ぶらさがるのは、現手番から見た「被王手の次の一手の局面」だけです。
        /// ぶらさがりがなければ、「投了」を選んでください。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="yomuDeep"></param>
        /// <param name="pside_yomiCur"></param>
        /// <param name="node_yomiCur"></param>
        /// <param name="logF_moveKiki"></param>
        /// <param name="logTag"></param>
        /// <returns>複数のノードを持つハブ・ノード</returns>
        private static List<Move> WAAAA_Create_ChildNodes(
            Tansaku_Genjo genjo,
            Playerside psideA,
            ISky positionA,
            //Move move_ForLog,
            ILogTag logTag
            )
        {
            //----------------------------------------
            // ハブ・ノードとは
            //----------------------------------------
            //
            // このハブ・ノード自身は空っぽで、ハブ・ノードの次ノードが、次局面のリストになっています。
            //
            List<Move> movelist;

            //----------------------------------------
            // ①現手番の駒の移動可能場所_被王手含む
            //----------------------------------------

            //----------------------------------------
            // 盤１個分のログの準備
            //----------------------------------------
#if DEBUG
                MmLogGenjoImpl mm_log_orNull = null;
                KaisetuBoard logBrd_move1;
                Tansaku_FukasaYusen_Routine.Log1(genjo, positionA, out mm_log_orNull, out logBrd_move1, logger);
#endif

            //----------------------------------------
            // 進めるマス
            //----------------------------------------
            List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;
            Util_KyokumenMoves.LA_Split_KomaBETUSusumeruMasus(
                1,
                out komaBETUSusumeruMasus,
                genjo.Args.IsHonshogi,//本将棋か
                positionA,//現在の局面  // FIXME:Lockすると、ここでヌルになる☆

                //手番
                positionA.GetKaisiPside(),// × psideA,

                false//相手番か
#if DEBUG
                    ,
                    mm_log_orNull
#endif
                );
            bool test = true;
            if (test)
            {
                foreach (Couple<Finger, SySet<SyElement>> couple in komaBETUSusumeruMasus.Items)
                {
                    if (couple.A == Fingers.Error_1)
                    {
                        throw new Exception("カップルリストに駒番号が入っていないデータが含まれているぜ☆（＾～＾）");
                    }
                }
            }

            //#if DEBUG
            //                Logger.Trace("komaBETUSusumeruMasusの全要素＝" + Util_List_OneAndMultiEx<Finger, SySet<SyElement>>.CountAllElements(komaBETUSusumeruMasus));
            //#endif
            //#if DEBUG
            //                string jsaMoveStr = Util_Translator_Move.ToMove(genjo.Node_yomiNext, genjo.Node_yomiNext.Value, logTag);
            //                Logger.Trace("[" + jsaMoveStr + "]の駒別置ける升 調べ\n" + Util_List_OneAndMultiEx<Finger, SySet<SyElement>>.Dump(komaBETUSusumeruMasus, genjo.Node_yomiNext.Value.ToKyokumenConst));
            //#endif
            //Moveseisei_FukasaYusen_Routine.Log2(genjo, logBrd_move1, logTag);//ログ試し

            //----------------------------------------
            // ②利きから、被王手の局面を除いたハブノード
            //----------------------------------------
            if (genjo.Args.IsHonshogi)
            {
                //----------------------------------------
                // 本将棋
                //----------------------------------------

                //----------------------------------------
                // 指定局面での全ての指し手。
                //----------------------------------------
                Maps_OneAndMulti<Finger, Move> komaBETUAllMoves = Conv_KomabetuSusumeruMasus.ToKomaBETUAllMoves(
                    komaBETUSusumeruMasus, positionA);
                if (test)
                {
                    foreach (Finger fig in komaBETUAllMoves.Items.Keys)
                    {
                        if (fig == Fingers.Error_1)
                        {
                            throw new Exception("駒番号が入っていないデータが含まれているぜ☆（＾～＾）");
                        }
                    }
                }

                //#if DEBUG
                //                    Logger.Trace("komaBETUAllMovesの全要素＝" + Util_Maps_OneAndMultiEx<Finger, SySet<SyElement>>.CountAllElements(komaBETUAllMoves));
                //#endif

                //----------------------------------------
                // 本将棋の場合、王手されている局面は削除します。
                //----------------------------------------
                Maps_OneAndOne<Finger, SySet<SyElement>> starbetuSusumuMasus = Util_LegalMove.LA_RemoveMate(
                    genjo.YomikaisiTemezumi,
                    genjo.Args.IsHonshogi,
                    komaBETUAllMoves,//駒別の全ての指し手
                    psideA,
                    positionA,
#if DEBUG
                        genjo.Args.LogF_moveKiki,//利き用
#endif
                        "読みNextルーチン",
                    logTag);

                //----------------------------------------
                // 『駒別升ズ』を、ハブ・ノードへ変換。
                //----------------------------------------
                //成り以外の手
                movelist = Conv_Movelist1.ToMovelist_NonPromotion(
                    starbetuSusumuMasus,
                    psideA,
                    positionA,
                    logTag
                );

                //----------------------------------------
                // 成りの指し手を作成します。（拡張）
                //----------------------------------------
                //成りの手
                List<Move> b_movelist = Util_SasuEx.CreateNariMove(positionA,
                    movelist,
                    logTag);

                // マージ
                foreach (Move move in b_movelist)
                {
                    if (!movelist.Contains(move))
                    {
                        movelist.Add(move);
                    }
                }
            }
            else
            {
                //----------------------------------------
                // 本将棋じゃないもの
                //----------------------------------------

                //----------------------------------------
                // 駒別置ける升　→　指し手別局面
                //----------------------------------------
                //
                // １対１変換
                //
                movelist = Conv_Movelist1.ToMovelist_NonPromotion(
                    komaBETUSusumeruMasus,
                    psideA,
                    positionA,
                    logTag
                    );

                //#if DEBUG
                //                    Logger.Trace("駒別置ける升="+komaBETUSusumeruMasus.Items.Count+"件。　指し手別局面="+ss.Count+"件。");
                //                    Debug.Assert(komaBETUSusumeruMasus.Items.Count == ss.Count, "変換後のデータ件数が異なります。[" + komaBETUSusumeruMasus.Items.Count + "]→["+ss.Count+"]");
                //#endif
            }

            return movelist;
        }

    }
}
