using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P324_KifuTree___.C___250_Struct;
using Grayscale.P324_KifuTree___.C250____Struct;
using Grayscale.P339_ConvKyokume.C500____Converter;
using Grayscale.P341_Ittesasu___.L510____OperationB;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P360_Conv_Sasu__.C500____Converter
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
        public static KifuNode ToNextNodes_AsHubNode(
            Maps_OneAndMulti<Finger,Move> komabetuAllMoves,
            SkyConst src_Sky,
            KwErrorHandler errH
            )
        {
            KifuNode hubNode = new KifuNodeImpl(
                Conv_Move.GetErrorMove(),
                null);//蝶番

#if DEBUG
            string dump = komabetuAllMoves.Dump();
#endif

            foreach (KeyValuePair<Finger, List<Move>> entry1 in komabetuAllMoves.Items)
            {
                Finger figKoma = entry1.Key;// 動かす駒

                foreach (Move move in entry1.Value)// 駒の動ける升
                {
                    if (hubNode.ContainsKey_ChildNodes(move))
                    {
                        // 既存の指し手なら無視
                        System.Console.WriteLine("既存の指し手なので無視します1。sfenText=[" + Conv_Move.ToSfen(move) + "]");
                    }
                    else
                    {
                        SyElement dstMasu = Conv_Move.ToDstMasu(move);

                        // 指したあとの次の局面を作るだけ☆
                        hubNode.PutAdd_ChildNode(move,
                            new KifuNodeImpl(
                                move,
                                new KyokumenWrapper(
                            Util_Sasu341.Sasu(
                                src_Sky,// to_parentNode.Value.ToKyokumenConst,//指定局面
                                figKoma,//動かす駒
                                dstMasu,//移動先升
                                false,//成りません。
                                errH
                        ))));
                    }
                }
            }

            return hubNode;
        }

    }
}
