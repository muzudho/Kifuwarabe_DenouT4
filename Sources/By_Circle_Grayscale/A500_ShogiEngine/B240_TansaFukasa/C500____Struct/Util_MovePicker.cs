using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C500____Util;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B730_Util_SasuEx.C500____Util;
using Grayscale.A210_KnowNingen_.B770_Conv_Sasu__.C500____Converter;
using Grayscale.A210_KnowNingen_.B780_LegalMove__.C500____Util;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C500____Util;
using Grayscale.A500_ShogiEngine.B220_Tansaku____.C___500_Tansaku;
using System;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;

#if DEBUG
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C250____Log;
using Grayscale.A210_KnowNingen_.B810_LogGraphiEx.C500____Util;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
#else
#endif

namespace Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct
{
    public abstract class Util_MovePicker
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
        public static void CreateMovelist_BeforeLoop(
            Tansaku_Genjo genjo,
            
            Move move_ForLog,
            Sky pos1,//この局面から合法手を作成☆（＾～＾）

            out List<Move> out_movelist,
            ref int searchedMaxDepth,
            out int out_yomiDeep,
            KwLogger errH
            )
        {
            out_movelist = Util_MovePicker.WAAAA_Create_ChildNodes(
                genjo,
                pos1,
                move_ForLog,//ログ用
                errH);

            out_yomiDeep = pos1.Temezumi - genjo.YomikaisiTemezumi + 1;
            if (searchedMaxDepth < out_yomiDeep - 1)//これから探索する分をマイナス1しているんだぜ☆（＾～＾）
            {
                searchedMaxDepth = out_yomiDeep - 1;
            }
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
            Sky src_Sky,
            Move move_ForLog,
            KwLogger errH
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
                exceptionArea = 10000;

                //----------------------------------------
                // 盤１個分のログの準備
                //----------------------------------------
                exceptionArea = 20000;
#if DEBUG
                MmLogGenjoImpl mm_log_orNull = null;
                KaisetuBoard logBrd_move1;
                Tansaku_FukasaYusen_Routine.Log1(genjo, move_ForLog, src_Sky, out mm_log_orNull, out logBrd_move1, errH);
#endif

                //----------------------------------------
                // 進めるマス
                //----------------------------------------
                List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;
                Util_KyokumenMoves.LA_Split_KomaBETUSusumeruMasus(
                    1,
                    out komaBETUSusumeruMasus,
                    genjo.Args.IsHonshogi,//本将棋か
                    src_Sky,//現在の局面  // FIXME:Lockすると、ここでヌルになる☆
                    src_Sky.KaisiPside,//手番
                    false//相手番か
#if DEBUG
                    ,
                    mm_log_orNull
#endif
                );

                //#if DEBUG
                //                System.Console.WriteLine("komaBETUSusumeruMasusの全要素＝" + Util_List_OneAndMultiEx<Finger, SySet<SyElement>>.CountAllElements(komaBETUSusumeruMasus));
                //#endif
                //#if DEBUG
                //                string jsaSasiteStr = Util_Translator_Sasite.ToSasite(genjo.Node_yomiNext, genjo.Node_yomiNext.Value, errH);
                //                System.Console.WriteLine("[" + jsaSasiteStr + "]の駒別置ける升 調べ\n" + Util_List_OneAndMultiEx<Finger, SySet<SyElement>>.Dump(komaBETUSusumeruMasus, genjo.Node_yomiNext.Value.ToKyokumenConst));
                //#endif
                //Sasiteseisei_FukasaYusen_Routine.Log2(genjo, logBrd_move1, errH);//ログ試し

                exceptionArea = 29000;



                //----------------------------------------
                // ②利きから、被王手の局面を除いたハブノード
                //----------------------------------------
                if (genjo.Args.IsHonshogi)
                {
                    //----------------------------------------
                    // 本将棋
                    //----------------------------------------

                    exceptionArea = 30000;
                    //----------------------------------------
                    // 指定局面での全ての指し手。
                    //----------------------------------------
                    Maps_OneAndMulti<Finger, Move> komaBETUAllSasites = Conv_KomabetuSusumeruMasus.ToKomaBETUAllSasites(
                        komaBETUSusumeruMasus, src_Sky);

                    //#if DEBUG
                    //                    System.Console.WriteLine("komaBETUAllSasitesの全要素＝" + Util_Maps_OneAndMultiEx<Finger, SySet<SyElement>>.CountAllElements(komaBETUAllSasites));
                    //#endif


                    //----------------------------------------
                    // 本将棋の場合、王手されている局面は削除します。
                    //----------------------------------------
                    Maps_OneAndOne<Finger, SySet<SyElement>> starbetuSusumuMasus = Util_LegalMove.LA_RemoveMate(
                        genjo.YomikaisiTemezumi,
                        genjo.Args.IsHonshogi,
                        komaBETUAllSasites,//駒別の全ての指し手
                        src_Sky,
#if DEBUG
                        genjo.Args.LogF_moveKiki,//利き用
#endif
                        "読みNextルーチン",
                        errH);

                    exceptionArea = 40000;

                    //----------------------------------------
                    // 『駒別升ズ』を、ハブ・ノードへ変換。
                    //----------------------------------------
                    //成り以外の手
                    movelist = Conv_Movelist1.ToMovelist_NonPromotion(
                        starbetuSusumuMasus,
                        src_Sky,
                        errH
                    );

                    exceptionArea = 42000;

                    //----------------------------------------
                    // 成りの指し手を作成します。（拡張）
                    //----------------------------------------
                    //成りの手
                    List<Move> b_movelist = Util_SasuEx.CreateNariSasite(src_Sky,
                        movelist,
                        errH);

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
                        src_Sky,
                        errH
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
                errH.DonimoNaranAkirameta(ex, "探索深さルーチンでエラー☆ exceptionArea=" + exceptionArea);
                throw ex;
            }


            return movelist;
        }

    }
}
