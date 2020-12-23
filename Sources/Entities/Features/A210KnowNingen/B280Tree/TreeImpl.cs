using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;

#if DEBUG
#endif

namespace Grayscale.Kifuwaragyoku.Entities.Features
{

    /// <summary>
    /// 棋譜。
    /// </summary>
    public class TreeImpl : ITree
    {
        public TreeImpl(ISky sky)
        {
            this.m_moveEx_ = new MoveExImpl();
            this.m_positionA_ = sky;
            this.m_pv_ = new List<Move>();
            this.m_pv_.Add(Move.Empty);
        }
        public TreeImpl(MoveEx root, ISky sky)
        {
            this.m_moveEx_ = root;
            this.m_positionA_ = sky;
            this.m_pv_ = new List<Move>();
            this.m_pv_.Add(Move.Empty);
        }

        #region PV関連

        public void LogPv(string message)
        {
            var buf = new StringBuilder();
            int index = 0;
            buf.AppendLine($"┌──────────┐{message}");
            foreach (Move move in this.m_pv_)
            {
                buf.AppendLine($"({index}){ConvMove.ToLog(move)}");
                index++;
            }
            buf.AppendLine("└──────────┘");
            Logger.Trace(buf.ToString());

            //this.LogPvList(this);
        }
        /*
        public void LogPvList(Tree kifu1, ILogger logger)
        {
            List<Move> pvList = kifu1.ToPvList();

            logger.AppendLine("┌──────────┐ToPvList 旧・新");
            for (int index = 0; ; index++)
            {
                if (pvList.Count <= index && kifu1.CountPv() <= index)
                {
                    break;
                }
                logger.AppendLine("[" + ConvMove.ToLog(
                    index < pvList.Count ? pvList[index] : Move.Empty
                    ) + "] [" + ConvMove.ToLog(kifu1.GetPv(index)) + "]");
            }
            logger.AppendLine("└──────────┘");
        }
        */
        public void Pv_RemoveLast()
        {
            if (1 < this.m_pv_.Count)//[0]はルート☆（*＾～＾*）
            {
                this.m_pv_.RemoveAt(this.m_pv_.Count - 1);
                this.LogPv("RemoveLastPv後");
            }
        }
        public void Pv_ClearAll()
        {
            this.m_pv_.Clear();
            this.m_pv_.Add(Move.Empty);
            this.LogPv("ClearAll後");
        }
        public void Pv_Append(Move tail)
        {
            this.m_pv_.Add(tail);
            this.LogPv("Append後");
        }
        public Move Pv_GetLatest()
        {
            if (0 < this.m_pv_.Count)
            {
                return this.m_pv_[this.m_pv_.Count - 1];
            }
            return Move.Empty;
        }
        public Move Pv_Get(int index)
        {
            if (index < this.m_pv_.Count)
            {
                return this.m_pv_[index];
            }
            return Move.Empty;
        }
        public int Pv_Count()
        {
            return this.m_pv_.Count;
        }
        public List<Move> Pv_ToList()
        {
            return new List<Move>(this.m_pv_);
        }
        public bool Pv_IsRoot()
        {
            return this.m_pv_.Count == 1;
        }
        private List<Move> m_pv_;

        #endregion



        #region MoveEx関連

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        public MoveEx MoveEx_Current { get { return this.m_moveEx_; } }
        public void MoveEx_SetCurrent(MoveEx curNode)
        {
            this.m_moveEx_ = curNode;
        }
        private MoveEx m_moveEx_;


        public static Playerside MoveEx_ClearAllCurrent(ITree tree, ISky positionA)
        {
            tree.MoveEx_SetCurrent(new MoveExImpl());

            Playerside rootPside = Playerside.P2;
            if (1 < ((TreeImpl)tree).m_pv_.Count)
            {
                rootPside = Conv_Playerside.Reverse(ConvMove.ToPlayerside(((TreeImpl)tree).m_pv_[1]));
            }

            ((TreeImpl)tree).m_pv_.Clear();
            ((TreeImpl)tree).m_pv_.Add(Move.Empty);
            tree.SetPositionA(positionA);
            return rootPside;
        }
        /// <summary>
        /// 局面編集中
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sky"></param>
        /// <returns></returns>
        public MoveEx MoveEx_OnEditCurrent(MoveEx node, ISky sky)
        {
            this.m_moveEx_ = node;
            this.m_positionA_ = sky;
            return this.m_moveEx_;
        }

        #endregion


        public static MoveEx OnDoCurrentMove(MoveEx curNode, ITree kifu1, ISky positionA)
        {
            kifu1.MoveEx_SetCurrent(curNode);
            kifu1.Pv_Append(curNode.Move);

            kifu1.SetPositionA(positionA);
            return kifu1.MoveEx_Current;
        }
        public static MoveEx OnUndoCurrentMove(ITree kifu1, ISky positionA, string hint)
        {
            if (kifu1.Pv_IsRoot())
            {
                // やってはいけない操作は、例外を返すようにします。
                throw new Exception($"ルート局面を削除しようとしました。hint={hint}");
            }

            kifu1.Pv_RemoveLast();
            kifu1.SetPositionA(positionA);
            return kifu1.MoveEx_Current;
        }



        public ISky PositionA
        {
            get { return this.m_positionA_; }
        }
        public void SetPositionA(ISky positionA)
        {
            this.m_positionA_ = positionA;
        }
        private ISky m_positionA_;
    }
}
