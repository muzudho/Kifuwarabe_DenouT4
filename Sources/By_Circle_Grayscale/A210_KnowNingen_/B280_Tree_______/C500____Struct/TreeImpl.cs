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

            this.LogPvList(this, logger);
        }
        public void LogPvList(Tree kifu1, KwLogger logger)
        {
            List<Move> pvList = kifu1.ToPvList();

            logger.AppendLine("┌──────────┐ToPvList 旧・新");
            for (int index = 0; ; index++)
            {
                if (pvList.Count <= index && kifu1.CountPv() <= index)
                {
                    break;
                }
                logger.AppendLine("[" + Conv_Move.ToLog(
                    index < pvList.Count ? pvList[index] : Move.Empty
                    ) + "] [" + Conv_Move.ToLog(kifu1.GetPv(index)) + "]");
            }
            logger.AppendLine("└──────────┘");
        }
        public void RemoveLastPv(KwLogger logger)
        {
            if (1 < this.m_pv_.Count)//[0]はルート☆（*＾～＾*）
            {
                this.m_pv_.RemoveAt(this.m_pv_.Count - 1);
                this.LogPv("RemoveLastPv後", logger);
            }
        }
        public void ClearAllPv( KwLogger logger)
        {
            this.m_pv_.Clear();
            this.m_pv_.Add(Move.Empty);
            this.LogPv("ClearAll後", logger);
        }
        public void AppendPv(Move tail,KwLogger logger)
        {
            this.m_pv_.Add(tail);
            //this.m_pvIndex_++;
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
        public Move GetPv(int index)
        {
            if (index < this.m_pv_.Count)
            {
                return this.m_pv_[index];
            }
            return Move.Empty;
        }
        public int CountPv()
        {
            return this.m_pv_.Count;
        }
        public List<Move> ToPvList()
        {
            return new List<Move>(this.m_pv_);
        }
        public bool IsRoot()
        {
            return this.m_pv_.Count == 1;
        }
        private List<Move> m_pv_;



        #region プロパティ類

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        public MoveNode CurrentNode { get { return this.m_currentNode_; } }
        public void RemoveCurrentChildren( KwLogger logger)
        {
            this.CurrentNode.Child_RemoveThis(this,logger);
        }
        public void SetCurrentSetAndAdd(Move move, MoveNode newChildNode, KwLogger logger)
        {
            this.CurrentNode.Child_SetChild(move, newChildNode, this,logger);
        }
        public static MoveNode ClearAllCurrentMove(MoveNode curNode, Tree tree, Sky positionA, KwLogger logger)
        {
            tree.SetCurrentNode(new MoveNodeImpl());
            ((TreeImpl)tree).m_pv_.Clear();
            ((TreeImpl)tree).m_pv_.Add(Move.Empty);
            tree.SetPositionA(positionA);
            return tree.CurrentNode;
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
        public static MoveNode UndoCurrentMove(MoveNode curNode, Tree kifu1, Sky positionA, KwLogger logger)
        {
            if (kifu1.IsRoot())
            {
                // やってはいけない操作は、例外を返すようにします。
                string message = "ルート局面を削除しようとしました。";
                throw new Exception(message);
            }

            kifu1.RemoveLastPv(logger);
            kifu1.SetPositionA(positionA);
            return kifu1.CurrentNode;
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
