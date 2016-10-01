using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using System;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using System.Collections.Generic;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{

    /// <summary>
    /// 棋譜。
    /// </summary>
    public class TreeImpl : Tree
    {
        public TreeImpl(Sky sky)
        {
            this.m_currentNode_ = new MoveNodeImpl();
            this.m_sky_ = sky;
            this.m_pv_ = new List<Move>();
            this.m_pv_.Add(Move.Empty);
        }
        public TreeImpl(
            MoveNode root, Sky sky
        )
        {
            this.m_currentNode_ = root;
            this.m_sky_ = sky;
            this.m_pv_ = new List<Move>();
            this.m_pv_.Add(Move.Empty);
        }

        public void LogPv(string message, KwLogger logger)
        {
            int index = 0;
            logger.AppendLine("┌──────────┐"+message);
            foreach(Move move in this.m_pv_)
            {
                logger.AppendLine("("+ index+")" +Conv_Move.ToLog(move));
                index++;
            }
            logger.AppendLine("└──────────┘");
        }

        public void ClearPv( KwLogger logger)
        {
            this.m_pv_.Clear();
            this.m_pv_.Add(Move.Empty);
            this.LogPv("Clear後", logger);
        }
        public void AppendPv(Move tail,KwLogger logger)
        {
            this.m_pv_.Add(tail);
            this.LogPv("Append後", logger);
        }
        public Move GetLatestPv()
        {
            if (0<this.m_pv_.Count)
            {
                return this.m_pv_[this.m_pv_.Count - 1];
            }
            return Move.Empty;
        }
        private List<Move> m_pv_;



        #region プロパティ類

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        public MoveNode CurrentNode { get { return this.m_currentNode_; } }
        public MoveNode ParentNode1 { get { return ((MoveNodeImpl)this.m_currentNode_).m_parentNode_; } }
        public void ClearCurrentChildren( KwLogger logger)
        {
            this.CurrentNode.Child_Clear(this,logger);
        }
        public void SetCurrentSetAndAdd(Move move, MoveNode newChildNode, KwLogger logger)
        {
            this.CurrentNode.Child_SetChild(move, newChildNode, this,logger);
        }
        /// <summary>
        /// 棋譜を空っぽにします。
        /// 
        /// ルートは残します。
        /// </summary>
        /*
        public MoveNode OnClearCurrentMove(Sky sky)
        {
            {
                this.m_currentNode_ = TreeImpl.ClearCurrentMove(this.CurrentNode, this, sky);
            }

            return this.m_currentNode_;
        }
        */
        public static MoveNode ClearCurrentMove(MoveNode curNode, Tree tree, Sky positionA, KwLogger logger)
        {
            {
                // ルートまで遡ります。
                while (!curNode.IsRoot())
                {
                    curNode = ((MoveNodeImpl)curNode).m_parentNode_;
                }

                // ルートの次の手を全クリアーします。
                curNode.Child_Clear(tree,logger);
            }

            tree.SetPositionA(positionA);

            return curNode;
        }
        public MoveNode OnDoCurrentMove(MoveNode node, Sky sky)
        {
            return TreeImpl.DoCurrentMove(node, this, sky);
        }
        public static MoveNode DoCurrentMove(MoveNode curNode, Tree kifu1, Sky positionA)
        {
            kifu1.SetCurrentNode( curNode);
            kifu1.SetPositionA( positionA);
            return kifu1.CurrentNode;
        }
        /*
        public MoveNode OnUndoCurrentMove(MoveNode node, Sky sky)
        {
            {
                this.m_currentNode_ = TreeImpl.UndoCurrentMove(this.CurrentNode, this, sky);
            }

            return this.m_currentNode_;
        }
        }*/
        public static MoveNode UndoCurrentMove(MoveNode curNode, Tree kifu1, Sky positionA)
        {
            //一手戻した後処理に必要
            // 現在の要素を切り取って返します。なければヌル。
            // カレントは、１手前に戻ります。
            {
                if (curNode.IsRoot())
                {
                    // やってはいけない操作は、例外を返すようにします。
                    string message = "ルート局面を削除しようとしました。";
                    throw new Exception(message);
                }

                //>>>>> ラスト要素がルートでなかったら

                // カレントを、１つ前の要素に替えます。
                curNode = ((MoveNodeImpl)curNode).m_parentNode_;
            }

            kifu1.SetPositionA(positionA);

            return curNode;
        }
        /// <summary>
        /// 局面編集中
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sky"></param>
        /// <returns></returns>
        public MoveNode OnEditCurrentMove(MoveNode node, Sky sky)
        {
            this.m_currentNode_ = node;
            this.m_sky_ = sky;
            return this.m_currentNode_;
        }
        public Sky PositionA {
            get { return this.m_sky_; }
        }
        public void SetPositionA(Sky positionA)
        {
            this.m_sky_ = positionA;
        }
        private Sky m_sky_;
        public void SetCurrentNode(MoveNode curNode)
        {
            this.m_currentNode_ = curNode;
        }
        private MoveNode m_currentNode_;

        #endregion
    }
}
