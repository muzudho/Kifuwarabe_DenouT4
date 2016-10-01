using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

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
        public static void ToNextNodes_AsHubNode(
            out List<Move> out_inputMovelist,
            Maps_OneAndMulti<Finger,Move> komabetuAllMoves,
            Sky src_Sky,
            KwLogger logger
            )
        {
            out_inputMovelist = new List<Move>();

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
                    if (out_inputMovelist.Contains(moveA))
                    {
                        // 既存の指し手なら無視
                        System.Console.WriteLine("既存の指し手なので無視します1。sfenText=[" + Conv_Move.ToSfen(moveA) + "]");
                    }
                    else
                    {
                        out_inputMovelist.Add(moveA);
                    }
                }
            }
        }

    }
}
