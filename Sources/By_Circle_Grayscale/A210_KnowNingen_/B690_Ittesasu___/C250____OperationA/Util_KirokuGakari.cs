﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Text;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 記録係
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class Util_KirokuGakari
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト１(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「▲２六歩△８四歩▲７六歩」といった書き方。
        /// 
        /// 
        /// FIXME: 将棋GUII には要るものの、将棋エンジンには要らないはず。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToJsaFugoListString(
            Earth earth1,

            //MoveNode curNode_base,
            Tree kifu1,

            string hint,
            KwLogger logger
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");

            sb.Append(earth1.GetProperty(Word_KifuTree.PropName_Startpos));
            sb.Append(" moves ");

            // 採譜用に、新しい対局を用意します。
            Earth saifuEarth2 = new EarthImpl();
            Tree saifuKifu2;//使い捨て☆
            {
                Sky positionInit = Util_SkyCreator.New_Hirate();//日本の符号読取時
                saifuKifu2 = new TreeImpl(positionInit);
                earth1.Clear();

                // 棋譜を空っぽにします。
                saifuKifu2.SetCurrentNode( TreeImpl.ClearAllCurrentMove(saifuKifu2.CurrentNode, saifuKifu2, positionInit,logger));
                //saifuKifu2.OnClearCurrentMove(positionInit);

                saifuEarth2.SetProperty(
                    Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面 // FIXME:平手とは限らないのでは？
            }

            MoveNode curNode = saifuKifu2.CurrentNode;
            Util_Tree.ForeachHonpu2(
                kifu1,//curNode_base,
                (int temezumi, Move move, ref bool toBreak) =>
            {
                if (0 == temezumi)
                {
                    // 初期局面はスキップします。
                    goto gt_EndLoop;
                }

                //------------------------------
                // 符号の追加（記録係）
                //------------------------------
                Sky saifu_PositionA = new SkyImpl(saifuKifu2.PositionA);


                // 採譜用新ノード
                MoveNode saifu_newChild = new MoveNodeImpl(move);
                saifu_PositionA.SetKaisiPside(Conv_Playerside.Reverse(saifu_PositionA.KaisiPside));
                saifu_PositionA.SetTemezumi(temezumi);


                // 記録係り用棋譜（採譜）
                // 新しい次ノードを追加。次ノードを、これからカレントとする。
                {
                    //----------------------------------------
                    // 次ノート追加
                    //----------------------------------------
                    earth1.GetSennititeCounter().CountUp_New(
                        Conv_Sky.ToKyokumenHash(saifu_PositionA),
                        hint + "/AppendChild_And_ChangeCurrentToChild");
                    saifuKifu2.SetCurrentSetAndAdd(saifu_newChild.Key, saifu_newChild, logger);
                }

                saifuKifu2.SetCurrentNode(TreeImpl.DoCurrentMove(saifu_newChild, saifuKifu2, saifu_PositionA));
                //saifuKifu2.OnDoCurrentMove(saifu_newChild, saifu_PositionA);//次ノードを、これからのカレントとします。

                // 後手の符号がまだ含まれていない。
                string jsaFugoStr = Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(
                    saifu_newChild.Key,
                    saifuKifu2.ToPvList(),
                    saifu_PositionA,
                    logger);
                sb.Append(jsaFugoStr);

            gt_EndLoop:
                ;
            });

            return sb.ToString();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト２(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「position startpos moves 7g7f 3c3d 2g2f」といった書き方。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToSfen_PositionCommand(
            Earth earth1,

            //MoveNode endNode1
            Tree kifu1
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");
            sb.Append(earth1.GetProperty(Word_KifuTree.PropName_Startpos));
            sb.Append(" moves ");

            // 本譜
            int count = 0;
            Util_Tree.ForeachHonpu2(
                //endNode1,
                kifu1,
                (int temezumi, Move move, ref bool toBreak) =>
            {
                if (0 == temezumi)
                {
                    // 初期局面はスキップします。
                    goto gt_EndLoop;
                }

                sb.Append(Conv_Move.ToSfen(move));
                sb.Append(" ");

            gt_EndLoop:
                count++;
            });

            return sb.ToString();
        }

    }
}
