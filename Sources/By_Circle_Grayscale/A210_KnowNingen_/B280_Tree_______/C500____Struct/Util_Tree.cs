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
        public static void ForeachHonpu2(Tree kifu1,
            //MoveEx endNode,
            DELEGATE_Foreach2 delegate_Foreach)
        {
            bool toBreak = false;

            List<Move> pvList = kifu1.Pv_ToList();
            /*
            // 本譜（ノードのリスト）
            List<MoveEx> honpu = new List<MoveEx>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            while (null != endNode)//ルートを含むところまで遡ります。
            {
                honpu.Add(endNode); // リスト作成

                endNode = ((MoveExImpl)endNode).m_parentNode_;
            }
            honpu.Reverse();
            */

            //
            // 手済みを数えます。
            //
            int temezumi = 0;//初期局面が[0]

            //foreach (MoveEx item in honpu)//正順になっています。
            foreach (Move move in pvList)//正順になっています。
            {
                delegate_Foreach(temezumi,
                    move,//item.Key,
                    ref toBreak);
                if (toBreak)
                {
                    break;
                }

                temezumi++;
            }
        }
    }
}
