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
        public static List<Move> CreateChlidMoves(KifuNode hubNode)
        {
            List<Move> childMoves = new List<Move>();

            hubNode.Children1.Foreach_ChildNodes5((Move move, ref bool toBreak) =>
            {
                childMoves.Add(move);
            });

            return childMoves;
        }

        public static List<Move> CreateHonpu2List(MoveNode endNode)
        {
            // 本譜（ノードのリスト）
            List<Move> honpu = new List<Move>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            while (null != endNode)//ルートを含むところまで遡ります。
            {
                honpu.Add(endNode.Key); // リスト作成

                endNode = endNode.GetParentNode();
            }
            honpu.Reverse();

            return honpu;
        }

        public static List<KifuNode> CreateHonpu1List(KifuNode endNode)
        {
            // 本譜（ノードのリスト）
            List<KifuNode> honpu = new List<KifuNode>();

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

            return honpu;
        }

        /// <summary>
        /// 本譜だけ。
        /// </summary>
        /// <param name="endNode">葉側のノード。</param>
        /// <param name="delegate_Foreach"></param>
        public static void ForeachHonpu1(KifuNode endNode, DELEGATE_Foreach1 delegate_Foreach)
        {
            bool toBreak = false;

            // 本譜（ノードのリスト）
            List<KifuNode> honpu = Util_Tree.CreateHonpu1List(endNode);

            //
            // 手済みを数えます。
            //
            int temezumi = 0;//初期局面が[0]

            foreach (KifuNode item in honpu)//正順になっています。
            {
                delegate_Foreach(temezumi, item.Key, item.GetNodeValue(), item, ref toBreak);
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
        public static void ForeachHonpu2(MoveNode endNode, DELEGATE_Foreach2 delegate_Foreach)
        {
            bool toBreak = false;

            // 本譜（ノードのリスト）
            List<MoveNode> honpu = new List<MoveNode>();

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

            foreach (MoveNode item in honpu)//正順になっています。
            {
                delegate_Foreach(temezumi, item.Key, ref toBreak);
                if (toBreak)
                {
                    break;
                }

                temezumi++;
            }
        }
    }
}
