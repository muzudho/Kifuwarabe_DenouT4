using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;

using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A210_KnowNingen_.B770_Conv_Sasu__.C500____Converter
{

    /// <summary>
    /// 星別指し手ユーティリティー。
    /// </summary>
    public abstract class Conv_StarbetuSasites
    {

        /// <summary>
        /// 変換：星別指し手一覧　→　次の局面の一覧をもった、入れ物ノード。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="pside_genTeban"></param>
        /// <returns>次の局面一覧を持った、入れ物ノード（ハブ・ノード）</returns>
        public static MoveNode ToNextNodes_AsHubNode(
            Maps_OneAndMulti<Finger,Move> komabetuAllMoves,
            Sky src_Sky,
            KwLogger logger
            )
        {
            MoveNode hubNode = new MoveNodeImpl(Conv_Move.GetErrorMove());//蝶番

#if DEBUG
            string dump = komabetuAllMoves.Dump();
#endif

            foreach (KeyValuePair<Finger, List<Move>> entry1 in komabetuAllMoves.Items)
            {
                Finger figKoma = entry1.Key;// 動かす駒

                if (figKoma==Fingers.Error_1)
                {
                    logger.DonimoNaranAkirameta("駒番号が記載されていない駒があるぜ☆（＾～＾）");
                    continue;
                }

                foreach (Move moveA in entry1.Value)// 駒の動ける升
                {
                    if (hubNode.Children1.ContainsKey(moveA))
                    {
                        // 既存の指し手なら無視
                        System.Console.WriteLine("既存の指し手なので無視します1。sfenText=[" + Conv_Move.ToSfen(moveA) + "]");
                    }
                    else
                    {

                        Move moveB = moveA;
                        hubNode.Children1.AddItem(moveB,
                            new MoveNodeImpl(moveB),
                            hubNode);
                    }
                }
            }

            return hubNode;
        }

    }
}
