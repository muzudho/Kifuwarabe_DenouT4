using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{
    public abstract class Util_Tree
    {

        /// <summary>
        /// 本譜だけ。
        /// </summary>
        /// <param name="endNode">葉側のノード。</param>
        /// <param name="delegate_Foreach"></param>
        public static void ForeachHonpu1(Node endNode, DELEGATE_Foreach1 delegate_Foreach)
        {
            bool toBreak = false;

            // 本譜（ノードのリスト）
            List<Node> honpu = new List<Node>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            while (null != endNode)//ルートを含むところまで遡ります。
            {
                honpu.Add(endNode); // リスト作成

                endNode = endNode.GetParentNode();
            }
            honpu.Reverse();

            //
            // 手済みを数えます。
            //
            int temezumi = 0;//初期局面が[0]

            foreach (Node item in honpu)//正順になっています。
            {
                delegate_Foreach(temezumi, item.Key, item.GetValue(), item, ref toBreak);
                if (toBreak)
                {
                    break;
                }

                temezumi++;
            }
        }
        /// <summary>
        /// 本譜だけ。
        /// </summary>
        /// <param name="endNode">葉側のノード。</param>
        /// <param name="delegate_Foreach"></param>
        public static void ForeachHonpu2(Node endNode, DELEGATE_Foreach2 delegate_Foreach)
        {
            bool toBreak = false;

            // 本譜（ノードのリスト）
            List<Node> honpu = new List<Node>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            while (null != endNode)//ルートを含むところまで遡ります。
            {
                honpu.Add(endNode); // リスト作成

                endNode = endNode.GetParentNode();
            }
            honpu.Reverse();

            //
            // 手済みを数えます。
            //
            int temezumi = 0;//初期局面が[0]

            foreach (Node item in honpu)//正順になっています。
            {
                delegate_Foreach(temezumi, item.Key, ref toBreak);
                if (toBreak)
                {
                    break;
                }

                temezumi++;
            }
        }
        /// <summary>
        /// 全て。
        /// </summary>
        /// <param name="endNode"></param>
        /// <param name="delegate_Foreach"></param>
        public static void ForeachZenpuku(Node startNode, DELEGATE_Foreach1 delegate_Foreach)
        {

            List<Node> list8 = new List<Node>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            int temezumi = 0;//※指定局面が0。
            bool toFinish_ZenpukuTansaku = false;

            Util_Tree.Recursive_Node_NextNode(
                temezumi, startNode, delegate_Foreach, ref toFinish_ZenpukuTansaku
                );
            if (toFinish_ZenpukuTansaku)
            {
                goto gt_EndMetdhod;
            }

            gt_EndMetdhod:
            ;
        }
        private static void Recursive_Node_NextNode(
            int temezumi1,

            //Sky position,
            Node node1,

            DELEGATE_Foreach1 delegate_Foreach1, ref bool toFinish_ZenpukuTansaku
        )
        {
            bool toBreak1 = false;

            // このノードを、まず報告。
            delegate_Foreach1(temezumi1, node1.Key, node1.GetValue(), node1, ref toBreak1);
            if (toBreak1)
            {
                //この全幅探索を終わらせる指示が出ていた場合
                toFinish_ZenpukuTansaku = true;
                goto gt_EndMetdhod;
            }

            // 次のノード
            node1.Children1.Foreach_ChildNodes((Move key2, Node node2, Sky sky, ref bool toBreak2) =>
            {
                bool toFinish_ZenpukuTansaku2 = false;
                Util_Tree.Recursive_Node_NextNode(
                    temezumi1 + 1, node2, delegate_Foreach1, ref toFinish_ZenpukuTansaku2
                    );
                if (toFinish_ZenpukuTansaku2)//この全幅探索を終わらせる指示が出ていた場合
                {
                    toBreak2 = true;
                    goto gt_EndBlock;
                }

                gt_EndBlock:
                ;
            });

            gt_EndMetdhod:
            ;
        }

    }
}
