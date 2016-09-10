﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C125____UtilB;
using System.Text;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;

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
            KifuTree src_kifu,
            string hint,
            KwLogger errH
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");

            sb.Append(src_kifu.GetProperty(Word_KifuTree.PropName_Startpos));
            sb.Append(" moves ");

            // 採譜用に、新しい対局を用意します。
            KifuTree saifuKifu;
            {
                Move move = Conv_Move.GetErrorMove();

                saifuKifu = new KifuTreeImpl(
                        new KifuNodeImpl(
                            move,
                            new SkyImpl(Util_SkyCreator.New_Hirate())//日本の符号読取時
                        )
                );
                saifuKifu.Clear();// 棋譜を空っぽにします。
                saifuKifu.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面 // FIXME:平手とは限らないのでは？
            }

            src_kifu.ForeachHonpu(src_kifu.CurNode, (int temezumi, Sky kWrap, Node<Move, Sky> node, ref bool toBreak) =>
            {
                if (0 == temezumi)
                {
                    // 初期局面はスキップします。
                    goto gt_EndLoop;
                }

                //------------------------------
                // 符号の追加（記録係）
                //------------------------------
                Sky saifu_kWrap = saifuKifu.CurNode.Value;


                // 採譜用新ノード
                KifuNode saifu_newChild = new KifuNodeImpl(
                    node.Key,
                    new SkyImpl(saifu_kWrap)
                );
                saifu_newChild.Value.SetKaisiPside(Conv_Playerside.Reverse(saifu_kWrap.KaisiPside));
                saifu_newChild.Value.SetTemezumi(temezumi);


                // 記録係り用棋譜（採譜）
                Util_KifuTree282.AppendChild_And_ChangeCurrentToChild(saifuKifu, saifu_newChild, hint+"/ToJsaKifuText", errH);// 新しい次ノードを追加。次ノードを、これからカレントとする。

                // 後手の符号がまだ含まれていない。
                string jsaFugoStr = Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(saifu_newChild,
                    //saifu_newChild.Value,
                    errH);
                //sb.Append(Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(node, saifu_kWrap, errH));
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
        public static string ToSfen_PositionCommand(KifuTree src_kifu)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");
            sb.Append(src_kifu.GetProperty(Word_KifuTree.PropName_Startpos));
            sb.Append(" moves ");

            // 本譜
            int count = 0;
            src_kifu.ForeachHonpu(src_kifu.CurNode, (int temezumi, Sky kWrap, Node<Move, Sky> node, ref bool toBreak) =>
            {
                if (0 == temezumi)
                {
                    // 初期局面はスキップします。
                    goto gt_EndLoop;
                }

                sb.Append(Conv_Move.ToSfen(node.Key));

                //// TODO:デバッグ用
                //switch (sasite.TottaKoma)
                //{
                //    case KomaSyurui.UNKNOWN:
                //    case KomaSyurui.TOTTA_KOMA_NASI:
                //        break;
                //    default:
                //        sb.Append("(");
                //        sb.Append(Converter.SyuruiToSfen(sasite.Pside,sasite.TottaKoma));
                //        sb.Append(")");
                //        break;
                //}

                sb.Append(" ");


            gt_EndLoop:
                count++;
            });

            return sb.ToString();
        }

    }
}
