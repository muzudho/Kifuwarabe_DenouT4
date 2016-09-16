using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter
{
    public abstract class Conv_NextNodes
    {

        /// <summary>
        /// 変換『「指し手→局面」のコレクション』→『「「指し手→局面」のリスト』
        /// </summary>
        public static List<Node> ToList(
            Node hubNode
            )
        {
            List<Node> list = new List<Node>();

            // TODO:
            hubNode.Children1.Foreach_ChildNodes((Move key, Node node, Sky sky, ref bool toBreak) =>
            {
                list.Add((Node)node);
            });

            return list;
        }

    }
}
