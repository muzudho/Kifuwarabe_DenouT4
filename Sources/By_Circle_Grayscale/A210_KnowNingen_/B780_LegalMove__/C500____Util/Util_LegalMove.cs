using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C250____Struct;
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B430_Play_______.C500____Query;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B770_Conv_Sasu__.C500____Converter;
using System;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C505____ConvLogJson;
#endif

namespace Grayscale.A210_KnowNingen_.B780_LegalMove__.C500____Util
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
        /// <param name="errH"></param>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> LA_RemoveMate(
            int yomikaisiTemezumi,
            bool isHonshogi,
            Maps_OneAndMulti<Finger, Move> genTeban_komabetuAllMoves1,// 指定局面で、どの駒が、どんな手を指すことができるか
            Sky src_Sky,//指定局面。

#if DEBUG
            KaisetuBoards logF_kiki,
#endif

            string hint,
            KwLogger errH)
        {
            Node<Move, KyokumenWrapper> hubNode = Conv_StarbetuSasites.ToNextNodes_AsHubNode(
                genTeban_komabetuAllMoves1,
                src_Sky,
                errH
                );// ハブ・ノード自身はダミーノードなんだが、子ノードに、次のノードが入っている。
#if DEBUG
            Util_LegalMove.Log1(hubNode, src_Sky.Temezumi, hint, errH);
#endif

            if (isHonshogi)
            {
                // 王手が掛かっている局面を除きます。

                Util_LegalMove.LAA_RemoveNextNode_IfMate(
                    yomikaisiTemezumi,
                    hubNode,
                    src_Sky.Temezumi,
                    src_Sky.KaisiPside,

#if DEBUG
                    logF_kiki,
#endif
                    errH);
            }


            // 「指し手一覧」を、「星別の全指し手」に分けます。
            Maps_OneAndMulti<Finger, Move> starbetuAllSasites2 = Util_Sky258A.SplitSasite_ByStar(src_Sky, hubNode, errH);

            //
            // 「星別の指し手一覧」を、「星別の進むマス一覧」になるよう、データ構造を変換します。
            //
            Maps_OneAndOne<Finger, SySet<SyElement>> starbetuSusumuMasus = new Maps_OneAndOne<Finger, SySet<SyElement>>();// 「どの星を、どこに進める」の一覧
            foreach (KeyValuePair<Finger, List<Move>> entry in starbetuAllSasites2.Items)
            {
                Finger finger = entry.Key;
                List<Move> moveList = entry.Value;

                // ポテンシャル・ムーブを調べます。
                SySet<SyElement> masus_PotentialMove = new SySet_Default<SyElement>("ポテンシャルムーブ");
                foreach (Move move in moveList)
                {
                    SyElement dstMasu = Conv_Move.ToDstMasu(move);

                    masus_PotentialMove.AddElement(dstMasu);
                }

                if (!masus_PotentialMove.IsEmptySet())
                {
                    // 空でないなら
                    Util_Sky258A.AddOverwrite(starbetuSusumuMasus, finger, masus_PotentialMove);
                }
            }

            // FIXME: デバッグ用
            foreach (Finger key in starbetuSusumuMasus.ToKeyList())
            {
                src_Sky.AssertFinger(key);
            }

            return starbetuSusumuMasus;
        }
        private static void Log1(
            Node<Move, KyokumenWrapper> hubNode,
            int temezumi_yomiGenTeban,
            string hint,
            KwLogger errH
            )
        {
            bool enableLog = false;//errH.Enable
            Util_GraphicalLog.WriteHtml5(enableLog, "Util_LegalMove(王手回避漏れ02)王手を回避するかどうかに関わらず、ひとまず全ての次の手", "[" +
                ((KifuNode)hubNode).Json_NextNodes_MultiSky(
                    "(王手回避漏れ02." + temezumi_yomiGenTeban + "手目)",
                    hint + "_Lv3_RMHO",
                    temezumi_yomiGenTeban,
                    errH) + "]");// ログ出力
        }

        /// <summary>
        /// ハブノードの次手番の局面のうち、王手がかかった局面は取り除きます。
        /// </summary>
        public static void LAA_RemoveNextNode_IfMate(
            int yomikaisiTemezumi,
            Node<Move, KyokumenWrapper> hubNode,
            int temezumi_yomiGenTeban_forLog,//読み進めている現在の手目
            Playerside pside_genTeban,

#if DEBUG
            KaisetuBoards logF_kiki,
#endif

            KwLogger errH
            )
        {
            // Node<,>の形で。
            Dictionary<Move, Node<Move, KyokumenWrapper>> newNextNodes = new Dictionary<Move, Node<Move, KyokumenWrapper>>();

            hubNode.Foreach_ChildNodes((Move key, Node<Move, KyokumenWrapper> node, ref bool toBreak) =>
            {
                //System.Diagnostics.Debug.Assert(node.Key != null);//指し手がヌルなはず無いはず。

                // 王様が利きに飛び込んだか？
                bool kingSuicide = Util_LegalMove.LAAA_KingSuicide(
                    yomikaisiTemezumi,
                    node.Value.Kyokumen,
                    temezumi_yomiGenTeban_forLog,
                    pside_genTeban,//現手番＝攻め手視点
#if DEBUG
                    logF_kiki,
#endif
                    node.Key,
                    errH
                    );

                if (!kingSuicide)
                {
                    // 王様が利きに飛び込んでいない局面だけ、残します。
                    if (newNextNodes.ContainsKey(key))
                    {
                        newNextNodes[key] = node;
                    }
                    else
                    {
                        newNextNodes.Add(key, node);
                    }
                }
            });


            // 入替え
            hubNode.Set_ChildNodes(newNextNodes);
        }


        /// <summary>
        /// 利きに飛び込んでいないか（王手されていないか）、調べます。
        /// 
        /// GetAvailableMove()の中では使わないでください。循環してしまいます。
        /// </summary>
        public static bool LAAA_KingSuicide(
            int yomikaisiTemezumi,
            Sky src_Sky,//調べたい局面
            int temezumi_yomiCur_forLog,//読み進めている現在の手目
            Playerside pside_genTeban,//現手番側

#if DEBUG
            KaisetuBoards logF_kiki,
#endif
            Move move_forLog,
            KwLogger errH
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

                    genTeban_kingMasuNumber = Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu( koma));
            }
            else
            {
                // 現手番は、先手
                src_Sky.AssertFinger(Finger_Honshogi.SenteOh);
                Busstop koma = src_Sky.BusstopIndexOf(Finger_Honshogi.SenteOh);

                    genTeban_kingMasuNumber = Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu(koma));
            }


            // 相手の利きに、自分の王がいるかどうか確認します。
            bool mate = false;
            sMs_effect_aiTeban.Foreach_Entry((Finger koma, SySet<SyElement> kikis, ref bool toBreak) =>
            {
                foreach (New_Basho kiki in kikis.Elements)
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
            Sky src_Sky,
            Playerside pside_genTeban3,
            bool isAiteban,
#if DEBUG
            KaisetuBoards logF_kiki,
#endif
            string logBrd_caption,
            int temezumi_yomiCur_forLog,
            Move move_forLog,
            KwLogger errH
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
            List_OneAndMulti<Finger, SySet<SyElement>> sMs_effect = new List_OneAndMulti<Finger,SySet<SyElement>>();//盤上の駒の利き
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

                Util_Sky_FingersQueryFx.Split_BanjoSeme_BanjoKurau_MotiSeme_MotiKurau(
                        out fingers_seme_BANJO,
                        out fingers_kurau_BANJO,
                        out dust1,
                        out dust2,
                        src_Sky,
                        tebanSeme,
                        tebanKurau,
                        errH
                    );


                // 攻め手の駒の位置
#if DEBUG
                KaisetuBoard boardLog_clone = new KaisetuBoard(logBrd_kiki);
                foreach (Finger finger in fingers_seme_BANJO.Items)
                {
                    Busstop koma = src_Sky.BusstopIndexOf(finger);


                        Gkl_KomaMasu km = new Gkl_KomaMasu(
                            Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanSeme, Conv_Busstop.ToKomasyurui(koma), ""),
                            Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu( koma))
                            );
                        boardLog_clone.KomaMasu1.Add(km);
                }
                foreach (Finger finger in fingers_kurau_BANJO.Items)
                {
                    Busstop koma = src_Sky.BusstopIndexOf(finger);


                        logBrd_kiki.KomaMasu2.Add(new Gkl_KomaMasu(
                            Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanKurau, Conv_Busstop.ToKomasyurui(koma), ""),
                            Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu(koma))
                            ));
                }
                logBrd_kiki = boardLog_clone;
#endif




                // 《１．３》
                SySet<SyElement> masus_seme_BANJO = Conv_Fingers.ToMasus(fingers_seme_BANJO, src_Sky);// 盤上のマス（利きを調べる側の駒）
                SySet<SyElement> masus_kurau_BANJO = Conv_Fingers.ToMasus(fingers_kurau_BANJO, src_Sky);// 盤上のマス（喰らう側の駒）

                // 駒のマスの位置は、特にログに取らない。

                // 《１．４》
                Maps_OneAndOne<Finger, SySet<SyElement>> kmEffect_seme_BANJO = Query_FingersMasusSky.To_KomabetuKiki_OnBanjo(
                    fingers_seme_BANJO,//この中身がおかしい。
                    masus_seme_BANJO,
                    masus_kurau_BANJO,
                    src_Sky,
                    //Conv_Sasite.Sasite_To_KsString_ForLog(sasite_forLog, pside_genTeban3),
                    errH
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
                    sMs_effect.AddRange_New( kmEffect_seme_BANJO);

                }
                catch (Exception ex) { errH.DonimoNaranAkirameta(ex, "ランダムチョイス(50)"); throw ex; }

            }

            return sMs_effect;
        }




    }
}
