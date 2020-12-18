﻿using System;
using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.A060Application.B410Collection.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B460KyokumMoves.C500Util;
using Grayscale.A210KnowNingen.B730UtilSasuEx.C500Util;
using Grayscale.A210KnowNingen.B770ConvSasu.C500Converter;
using Grayscale.A210KnowNingen.B780LegalMove.C500Util;
using Grayscale.A500ShogiEngine.B220Tansaku.C500Tansaku;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
using Grayscale.A210KnowNingen.B110GraphicLog.C500Util;
using Grayscale.A210KnowNingen.B460KyokumMoves.C250Log;
using Grayscale.A210KnowNingen.B810LogGraphiEx.C500Util;
// using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
#else
#endif

namespace Grayscale.A500ShogiEngine.B240_TansaFukasa.C500Struct
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
        /// <param name="errH"></param>
        public static List<Move> CreateMovelist_BeforeLoop(
            Tansaku_Genjo genjo,

            Playerside psideA,
            ISky positionA,//この局面から合法手を作成☆（＾～＾）

            ref int searchedMaxDepth,
            out int out_yomiDeep,
            ILogger errH
            )
        {
            List<Move> result_movelist = UtilMovePicker.WAAAA_Create_ChildNodes(
                genjo,
                psideA,//× Conv_Playerside.Reverse( psideA),
                positionA,
                //move_ForLog,//ログ用
                errH);

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
            ILogger logger
            )
        {
            int exceptionArea = 0;

            //----------------------------------------
            // ハブ・ノードとは
            //----------------------------------------
            //
            // このハブ・ノード自身は空っぽで、ハブ・ノードの次ノードが、次局面のリストになっています。
            //
            List<Move> movelist;

            try
            {
                //----------------------------------------
                // ①現手番の駒の移動可能場所_被王手含む
                //----------------------------------------

                //----------------------------------------
                // 盤１個分のログの準備
                //----------------------------------------
                exceptionArea = 20000;
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
                            logger.DonimoNaranAkirameta("カップルリストに駒番号が入っていないデータが含まれているぜ☆（＾～＾）");
                        }
                    }
                }

                //#if DEBUG
                //                System.Console.WriteLine("komaBETUSusumeruMasusの全要素＝" + Util_List_OneAndMultiEx<Finger, SySet<SyElement>>.CountAllElements(komaBETUSusumeruMasus));
                //#endif
                //#if DEBUG
                //                string jsaMoveStr = Util_Translator_Move.ToMove(genjo.Node_yomiNext, genjo.Node_yomiNext.Value, errH);
                //                System.Console.WriteLine("[" + jsaMoveStr + "]の駒別置ける升 調べ\n" + Util_List_OneAndMultiEx<Finger, SySet<SyElement>>.Dump(komaBETUSusumeruMasus, genjo.Node_yomiNext.Value.ToKyokumenConst));
                //#endif
                //Moveseisei_FukasaYusen_Routine.Log2(genjo, logBrd_move1, errH);//ログ試し

                exceptionArea = 29000;



                //----------------------------------------
                // ②利きから、被王手の局面を除いたハブノード
                //----------------------------------------
                if (genjo.Args.IsHonshogi)
                {
                    //----------------------------------------
                    // 本将棋
                    //----------------------------------------

                    exceptionArea = 300011;
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
                                logger.DonimoNaranAkirameta("駒番号が入っていないデータが含まれているぜ☆（＾～＾）");
                            }
                        }
                    }

                    //#if DEBUG
                    //                    System.Console.WriteLine("komaBETUAllMovesの全要素＝" + Util_Maps_OneAndMultiEx<Finger, SySet<SyElement>>.CountAllElements(komaBETUAllMoves));
                    //#endif


                    exceptionArea = 300012;
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
                        logger);

                    exceptionArea = 40000;

                    //----------------------------------------
                    // 『駒別升ズ』を、ハブ・ノードへ変換。
                    //----------------------------------------
                    //成り以外の手
                    movelist = Conv_Movelist1.ToMovelist_NonPromotion(
                        starbetuSusumuMasus,
                        psideA,
                        positionA,
                        logger
                    );

                    exceptionArea = 42000;

                    //----------------------------------------
                    // 成りの指し手を作成します。（拡張）
                    //----------------------------------------
                    //成りの手
                    List<Move> b_movelist = Util_SasuEx.CreateNariMove(positionA,
                        movelist,
                        logger);

                    exceptionArea = 44000;

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
                    exceptionArea = 50000;

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
                        logger
                        );

                    //#if DEBUG
                    //                    System.Console.WriteLine("駒別置ける升="+komaBETUSusumeruMasus.Items.Count+"件。　指し手別局面="+ss.Count+"件。");
                    //                    Debug.Assert(komaBETUSusumeruMasus.Items.Count == ss.Count, "変換後のデータ件数が異なります。[" + komaBETUSusumeruMasus.Items.Count + "]→["+ss.Count+"]");
                    //#endif
                }

                exceptionArea = 1000000;

            }
            catch (Exception ex)
            {
                logger.DonimoNaranAkirameta(ex, "探索深さルーチンでエラー☆ exceptionArea=" + exceptionArea);
                throw ;
            }


            return movelist;
        }

    }
}
