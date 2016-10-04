using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using System;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using System.Collections.Generic;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

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
            this.m_moveEx_ = new MoveExImpl();
            this.m_positionA_ = sky;
            this.m_pv_ = new List<Move>();
            this.m_pv_.Add(Move.Empty);
        }
        public TreeImpl(MoveEx root, Sky sky)
        {
            this.m_moveEx_ = root;
            this.m_positionA_ = sky;
            this.m_pv_ = new List<Move>();
            this.m_pv_.Add(Move.Empty);
        }

        #region PV関連

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

            //this.LogPvList(this, logger);
        }
        /*
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
        */
        public void Pv_RemoveLast(KwLogger logger)
        {
            if (1 < this.m_pv_.Count)//[0]はルート☆（*＾～＾*）
            {
                this.m_pv_.RemoveAt(this.m_pv_.Count - 1);
                this.LogPv("RemoveLastPv後", logger);
            }
        }
        public void Pv_ClearAll( KwLogger logger)
        {
            this.m_pv_.Clear();
            this.m_pv_.Add(Move.Empty);
            this.LogPv("ClearAll後", logger);
        }
        public void Pv_Append(Move tail,KwLogger logger)
        {
            this.m_pv_.Add(tail);
            this.LogPv("Append後", logger);
        }
        public Move Pv_GetLatest()
        {
            if (0<this.m_pv_.Count)
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


        public static Playerside MoveEx_ClearAllCurrent(Tree tree, Sky positionA, KwLogger logger)
        {
            tree.MoveEx_SetCurrent(new MoveExImpl());

            Playerside rootPside = Playerside.P2;
            if (1<((TreeImpl)tree).m_pv_.Count)
            {
                rootPside = Conv_Playerside.Reverse(Conv_Move.ToPlayerside(((TreeImpl)tree).m_pv_[1]));
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
        public MoveEx MoveEx_OnEditCurrent(MoveEx node, Sky sky)
        {
            this.m_moveEx_ = node;
            this.m_positionA_ = sky;
            return this.m_moveEx_;
        }

        #endregion


        public static MoveEx OnDoCurrentMove(MoveEx curNode, Tree kifu1, Sky positionA, KwLogger logger)
        {
            kifu1.MoveEx_SetCurrent(curNode);
            kifu1.Pv_Append(curNode.Move, logger);

            kifu1.SetPositionA(positionA);
            return kifu1.MoveEx_Current;
        }
        public static MoveEx OnUndoCurrentMove(Tree kifu1, Sky positionA, KwLogger logger, string hint)
        {
            if (kifu1.Pv_IsRoot())
            {
                // やってはいけない操作は、例外を返すようにします。
                string message = "ルート局面を削除しようとしました。hint=" + hint;
                throw new Exception(message);
            }

            kifu1.Pv_RemoveLast(logger);
            kifu1.SetPositionA(positionA);
            return kifu1.MoveEx_Current;
        }



        public Sky PositionA
        {
            get { return this.m_positionA_; }
        }
        public void SetPositionA(Sky positionA)
        {
            this.m_positionA_ = positionA;
        }
        private Sky m_positionA_;
    }
}
