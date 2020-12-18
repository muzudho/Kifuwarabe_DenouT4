using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B410Collection.C500Struct;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B770ConvSasu.C500Converter
{

    /// <summary>
    /// 星別指し手ユーティリティー。
    /// </summary>
    public abstract class ConvStarbetuMoves
    {

        /// <summary>
        /// 変換：星別指し手一覧　→　次の局面の一覧をもった、入れ物ノード。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="pside_genTeban"></param>
        /// <returns>次の局面一覧を持った、入れ物ノード（ハブ・ノード）</returns>
        public static void ToNextNodes_AsHubNode(
            out List<Move> out_inputMovelist,
            Maps_OneAndMulti<Finger, Move> komabetuAllMoves,
            ISky src_Sky,
            ILogTag logTag
            )
        {
            out_inputMovelist = new List<Move>();

#if DEBUG
            string dump = komabetuAllMoves.Dump();
#endif

            foreach (KeyValuePair<Finger, List<Move>> entry1 in komabetuAllMoves.Items)
            {
                Finger figKoma = entry1.Key;// 動かす駒

                if (figKoma == Fingers.Error_1)
                {
                    Logger.Panic(logTag,"駒番号が記載されていない駒があるぜ☆（＾～＾）");
                    continue;
                }

                foreach (Move moveA in entry1.Value)// 駒の動ける升
                {
                    if (out_inputMovelist.Contains(moveA))
                    {
                        // 既存の指し手なら無視
                        System.Console.WriteLine("既存の指し手なので無視します1。sfenText=[" + ConvMove.ToSfen(moveA) + "]");
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
