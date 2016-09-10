using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter
{
    public abstract class Conv_NextNodes
    {

        /// <summary>
        /// 変換『「指し手→局面」のコレクション』→『「「指し手→局面」のリスト』
        /// </summary>
        public static List<KifuNode> ToList(
            Node<Move, Sky> hubNode
            )
        {
            List<KifuNode> list = new List<KifuNode>();

            // TODO:
            hubNode.Foreach_ChildNodes((Move key, Node<Move, Sky> node, ref bool toBreak) =>
            {
                list.Add((KifuNode)node);
            });

            return list;
        }

    }
}
