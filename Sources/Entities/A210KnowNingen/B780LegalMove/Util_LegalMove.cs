using System;
using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B410Collection.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C250Masu;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B410SeizaFinger.C250Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B430Play.C500Query;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA;
using Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA;
using Grayscale.A210KnowNingen.B690Ittesasu.C510OperationB;
using Grayscale.A210KnowNingen.B770ConvSasu.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C505ConvLogJson;
#endif

namespace Grayscale.A210KnowNingen.B780LegalMove.C500Util
{


    /// <summary>
    /// 基本的に、本将棋でなければ　正しく使えません。
    /// </summary>
    public abstract class Util_LegalMove
    {

        /// <summary>
        /// 指定された手の中から、王手局面を除外します。
        /// 
        /// 王手回避漏れを防ぎたいんだぜ☆
        /// </summary>
        /// <param name="km_available">自軍の各駒の移動できる升セット</param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logTag"></param>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> LA_RemoveMate(
            int yomikaisiTemezumi,
            bool isHonshogi,
            Maps_OneAndMulti<Finger, Move> genTeban_komabetuAllMoves1,// 指定局面で、どの駒が、どんな手を指すことができるか
            Playerside psideA,
            ISky positionA,//指定局面。

#if DEBUG
            KaisetuBoards logF_kiki,
#endif

            string hint,
            ILogTag logTag)
        {
            Maps_OneAndOne<Finger, SySet<SyElement>> starbetuSusumuMasus = new Maps_OneAndOne<Finger, SySet<SyElement>>();// 「どの星を、どこに進める」の一覧

            long exception_area = 1000130;
            try
            {
                List<Move> inputMovelist;
                ConvStarbetuMoves.ToNextNodes_AsHubNode(
                    out inputMovelist,
                    genTeban_komabetuAllMoves1,
                    positionA,
                    logTag
                    );// ハブ・ノード自身はダミーノードなんだが、子ノードに、次のノードが入っている。

                exception_area = 20000;

                List<Move> restMovelist;
                if (isHonshogi)
                {
                    // 王手が掛かっている局面を除きます。

                    restMovelist = Util_LegalMove.LAA_RemoveNextNode_IfMate(
                        yomikaisiTemezumi,
                        inputMovelist,
                        positionA.Temezumi,
                        psideA,//positionA.GetKaisiPside(),
                        positionA,
#if DEBUG
                    logF_kiki,
#endif
                    logTag);
                }
                else
                {
                    restMovelist = new List<Move>();
                }

                exception_area = 30000;

                // 「指し手一覧」を、「星別の全指し手」に分けます。
                Maps_OneAndMulti<Finger, Move> starbetuAllMoves2 = Util_Sky258A.SplitMoveByStar(positionA,
                    restMovelist,//hubNode1.ToMovelist(),
                    logTag);

                exception_area = 40000;

                //
                // 「星別の指し手一覧」を、「星別の進むマス一覧」になるよう、データ構造を変換します。
                //
                foreach (KeyValuePair<Finger, List<Move>> entry in starbetuAllMoves2.Items)
                {
                    Finger finger = entry.Key;
                    List<Move> moveList = entry.Value;

                    exception_area = 500011;

                    // ポテンシャル・ムーブを調べます。
                    SySet<SyElement> masus_PotentialMove = new SySet_Default<SyElement>("ポテンシャルムーブ");
                    try
                    {
                        foreach (Move move in moveList)
                        {
                            SyElement dstMasu = ConvMove.ToDstMasu(move);
                            masus_PotentialMove.AddElement(dstMasu);
                        }
                    }
                    catch (Exception ex2)
                    {
                        Logger.Panic(logTag, ex2, "ポテンシャルムーブを調べているときだぜ☆（＾▽＾）");
                        throw;
                    }


                    exception_area = 60000;

                    if (!masus_PotentialMove.IsEmptySet())
                    {
                        // 空でないなら
                        Util_Sky258A.AddOverwrite(starbetuSusumuMasus, finger, masus_PotentialMove);
                    }
                }

                exception_area = 70000;

                // FIXME: デバッグ用
                foreach (Finger key in starbetuSusumuMasus.ToKeyList())
                {
                    positionA.AssertFinger(key);
                }

            }
            catch (Exception ex)
            {
                Logger.Panic(logTag, ex, "王手回避漏れを除外しているときだぜ☆（＾▽＾） exception_area=" + exception_area);
                throw;
            }

            return starbetuSusumuMasus;
        }

        /// <summary>
        /// ハブノードの次手番の局面のうち、王手がかかった局面は取り除きます。
        /// </summary>
        public static List<Move> LAA_RemoveNextNode_IfMate(
            int yomikaisiTemezumi,
            List<Move> inputMovelist,
            int temezumi_yomiGenTeban_forLog,//読み進めている現在の手目
            Playerside pside_genTeban,
            ISky positionA,

#if DEBUG
            KaisetuBoards logF_kiki,
#endif

            ILogTag logTag
            )
        {
            // 残す指し手☆
            List<Move> restMovelist = new List<Move>();

            foreach (Move moveA in inputMovelist)
            {
                Move moveB = moveA;
                long exception_area = 1000120;
                try
                {
                    bool successful = Util_IttesasuSuperRoutine.DoMove_Super1(
                        ConvMove.ToPlayerside(moveB),
                        ref positionA,//指定局面
                        ref moveB,
                        "A100_IfMate",
                        logTag
                    );
                    if (!successful)
                    {
                        // 将棋盤と指し手の不一致があるとき
                        goto gt_EndLoop;
                    }

                    exception_area = 30000;

                    // 王様が利きに飛び込んだか？
                    bool kingSuicide = Util_LegalMove.LAAA_KingSuicide(
                        yomikaisiTemezumi,
                        positionA,
                        temezumi_yomiGenTeban_forLog,
                        pside_genTeban,//現手番＝攻め手視点
#if DEBUG
                    logF_kiki,
#endif
                        moveB,
                        logTag
                        );

                    exception_area = 40000;

                    if (!kingSuicide)
                    {
                        // 王様が利きに飛び込んでいない局面だけ、残します。
                        restMovelist.Add(moveB);
                    }

                    exception_area = 5000110;

                    IIttemodosuResult ittemodosuResult;
                    UtilIttemodosuRoutine.UndoMove(
                        out ittemodosuResult,
                        moveB,//この関数が呼び出されたときの指し手☆（＾～＾）
                        ConvMove.ToPlayerside(moveB),
                        positionA,
                        "A900_IfMate",
                        logTag
                        );
                    positionA = ittemodosuResult.SyuryoSky;
                }
                catch (Exception ex)
                {
                    Logger.Panic(logTag,ex,
                        "ノードを削除しているときだぜ☆（＾▽＾） exception_area=" + exception_area +
                        "\nmove=" + ConvMove.ToLog(moveB));
                    throw;
                }

            gt_EndLoop:
                ;
            }

            return restMovelist;
        }


        /// <summary>
        /// 利きに飛び込んでいないか（王手されていないか）、調べます。
        /// 
        /// GetAvailableMove()の中では使わないでください。循環してしまいます。
        /// </summary>
        public static bool LAAA_KingSuicide(
            int yomikaisiTemezumi,
            ISky src_Sky,//調べたい局面
            int temezumi_yomiCur_forLog,//読み進めている現在の手目
            Playerside pside_genTeban,//現手番側

#if DEBUG
            KaisetuBoards logF_kiki,
#endif
            Move move_forLog,
            ILogTag errH
            )
        {
            bool isHonshogi = true;

            System.Diagnostics.Debug.Assert(src_Sky.Count == Masu_Honshogi.HONSHOGI_KOMAS);

            // 「相手の駒を動かしたときの利き」リスト
            // 持ち駒はどう考える？「駒を置けば、思い出王手だってある」
            List_OneAndMulti<Finger, SySet<SyElement>> sMs_effect_aiTeban = Util_LegalMove.LAAAA_GetEffect(
                yomikaisiTemezumi,
                isHonshogi,
                src_Sky,
                pside_genTeban,
                true,// 相手盤の利きを調べます。
#if DEBUG
                logF_kiki,
#endif
                "玉自殺ﾁｪｯｸ",
                temezumi_yomiCur_forLog,
                move_forLog,
                errH);


            // 現手番側が受け手に回ったとします。現手番の、王の座標
            int genTeban_kingMasuNumber;

            if (Playerside.P2 == pside_genTeban)
            {
                // 現手番は、後手

                src_Sky.AssertFinger(Finger_Honshogi.GoteOh);
                Busstop koma = src_Sky.BusstopIndexOf(Finger_Honshogi.GoteOh);

                genTeban_kingMasuNumber = Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma));
            }
            else
            {
                // 現手番は、先手
                src_Sky.AssertFinger(Finger_Honshogi.SenteOh);
                Busstop koma = src_Sky.BusstopIndexOf(Finger_Honshogi.SenteOh);

                genTeban_kingMasuNumber = Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma));
            }


            // 相手の利きに、自分の王がいるかどうか確認します。
            bool mate = false;
            sMs_effect_aiTeban.Foreach_Entry((Finger koma, SySet<SyElement> kikis, ref bool toBreak) =>
            {
                foreach (INewBasho kiki in kikis.Elements)
                {
                    if (genTeban_kingMasuNumber == kiki.MasuNumber)
                    {
                        mate = true;
                        toBreak = true;
                    }
                }
            });

            return mate;
        }


        /// <summary>
        /// 指定された局面で、指定された手番の駒の、利きマスを算出します。
        /// 持ち駒は盤上にないので、利きを調べる必要はありません。
        /// 
        /// 「手目」は判定できません。
        /// 
        /// </summary>
        /// <param name="kouho"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logger"></param>
        public static List_OneAndMulti<Finger, SySet<SyElement>> LAAAA_GetEffect(
            int yomikaisiTemezumi,
            bool isHonshogi,
            ISky src_Sky,
            Playerside pside_genTeban3,
            bool isAiteban,
#if DEBUG
            KaisetuBoards logF_kiki,
#endif
            string logBrd_caption,
            int temezumi_yomiCur_forLog,
            Move move_forLog,
            ILogTag logTag
            )
        {
#if DEBUG
            KaisetuBoard logBrd_kiki = new KaisetuBoard();
            logBrd_kiki.Caption = logBrd_caption;// "利き_" 
            logBrd_kiki.Temezumi = temezumi_yomiCur_forLog;
            logBrd_kiki.YomikaisiTemezumi = yomikaisiTemezumi;
            //logBrd_kiki.Score = 0.0d;
            logBrd_kiki.GenTeban = pside_genTeban3;// 現手番
            logF_kiki.boards.Add(logBrd_kiki);
#endif

            // 《１》
            List_OneAndMulti<Finger, SySet<SyElement>> sMs_effect = new List_OneAndMulti<Finger, SySet<SyElement>>();//盤上の駒の利き
            {
                // 《１．１》
                Playerside tebanSeme;//手番（利きを調べる側）
                Playerside tebanKurau;//手番（喰らう側）
                {
                    if (isAiteban)
                    {
                        tebanSeme = Conv_Playerside.Reverse(pside_genTeban3);
                        tebanKurau = pside_genTeban3;
                    }
                    else
                    {
                        tebanSeme = pside_genTeban3;
                        tebanKurau = Conv_Playerside.Reverse(pside_genTeban3);
                    }

#if DEBUG
                    if (Playerside.P1 == tebanSeme)
                    {
                        logBrd_kiki.NounaiSeme = Gkl_NounaiSeme.Sente;
                    }
                    else if(Playerside.P2 == tebanSeme)
                    {
                        logBrd_kiki.NounaiSeme = Gkl_NounaiSeme.Gote;
                    }
#endif
                }


                // 《１．２》
                Fingers fingers_seme_BANJO;//盤上駒（利きを調べる側）
                Fingers fingers_kurau_BANJO;//盤上駒（喰らう側）
                Fingers dust1;
                Fingers dust2;

                UtilSkyFingersQueryFx.Split_BanjoSeme_BanjoKurau_MotiSeme_MotiKurau(
                        out fingers_seme_BANJO,
                        out fingers_kurau_BANJO,
                        out dust1,
                        out dust2,
                        src_Sky,
                        tebanSeme,
                        tebanKurau,
                        logTag
                    );


                // 攻め手の駒の位置
#if DEBUG
                KaisetuBoard boardLog_clone = new KaisetuBoard(logBrd_kiki);
                foreach (Finger finger in fingers_seme_BANJO.Items)
                {
                    Busstop koma = src_Sky.BusstopIndexOf(finger);


                        Gkl_KomaMasu km = new Gkl_KomaMasu(
                            Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanSeme, Conv_Busstop.ToKomasyurui(koma), ""),
                            Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu( koma))
                            );
                        boardLog_clone.KomaMasu1.Add(km);
                }
                foreach (Finger finger in fingers_kurau_BANJO.Items)
                {
                    Busstop koma = src_Sky.BusstopIndexOf(finger);


                        logBrd_kiki.KomaMasu2.Add(new Gkl_KomaMasu(
                            Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanKurau, Conv_Busstop.ToKomasyurui(koma), ""),
                            Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma))
                            ));
                }
                logBrd_kiki = boardLog_clone;
#endif




                // 《１．３》
                SySet<SyElement> masus_seme_BANJO = ConvFingers.ToMasus(fingers_seme_BANJO, src_Sky);// 盤上のマス（利きを調べる側の駒）
                SySet<SyElement> masus_kurau_BANJO = ConvFingers.ToMasus(fingers_kurau_BANJO, src_Sky);// 盤上のマス（喰らう側の駒）

                // 駒のマスの位置は、特にログに取らない。

                // 《１．４》
                Maps_OneAndOne<Finger, SySet<SyElement>> kmEffect_seme_BANJO = Query_FingersMasusSky.To_KomabetuKiki_OnBanjo(
                    fingers_seme_BANJO,//この中身がおかしい。
                    masus_seme_BANJO,
                    masus_kurau_BANJO,
                    src_Sky,
                    //Conv_Move.Move_To_KsString_ForLog(Move_forLog, pside_genTeban3),
                    logTag
                    );// 利きを調べる側の利き（戦駒）

                // 盤上駒の利き
#if DEBUG
                logBrd_kiki = new KaisetuBoard(logBrd_kiki);
                kmEffect_seme_BANJO.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
                {
                    Busstop koma = src_Sky.BusstopIndexOf(key);


                    string komaImg = Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanSeme, Conv_Busstop.ToKomasyurui(koma), "");

                    foreach (New_Basho masu in value.Elements)
                    {
                        boardLog_clone.Masu_theEffect.Add(masu.MasuNumber);
                    }
                });
                logBrd_kiki = boardLog_clone;
#endif


                try
                {
                    // 《１》　＝　《１．４》の盤上駒＋持駒
                    sMs_effect.AddRange_New(kmEffect_seme_BANJO);

                }
                catch (Exception ex)
                {
                    Logger.Panic(logTag,ex, "ランダムチョイス(50)");
                    throw;
                }

            }

            return sMs_effect;
        }




    }
}
