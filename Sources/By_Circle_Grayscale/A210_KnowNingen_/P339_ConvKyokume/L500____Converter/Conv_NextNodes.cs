﻿using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.P324_KifuTree___.C___250_Struct;
using System.Collections.Generic;

namespace Grayscale.P339_ConvKyokume.C500____Converter
{
    public abstract class Conv_NextNodes
    {

        /// <summary>
        /// 変換『「指し手→局面」のコレクション』→『「「指し手→局面」のリスト』
        /// </summary>
        public static List<KifuNode> ToList(
            Node<Move, KyokumenWrapper> hubNode
            )
        {
            List<KifuNode> list = new List<KifuNode>();

            // TODO:
            hubNode.Foreach_ChildNodes((Move key, Node<Move, KyokumenWrapper> node, ref bool toBreak) =>
            {
                list.Add((KifuNode)node);
            });

            return list;
        }

    }
}
