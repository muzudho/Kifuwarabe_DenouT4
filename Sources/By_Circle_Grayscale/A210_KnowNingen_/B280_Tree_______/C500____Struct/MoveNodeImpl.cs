using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{

    /// <summary>
    /// ノードです。
    /// </summary>
    public class MoveNodeImpl : MoveNode
    {
        public MoveNodeImpl()
        {
            this.Score = 0.0f;

            this.m_parentNode_ = null;
            this.m_key_ = Move.Empty;

            this.m_childMove_ = Move.Empty;
            this.m_childNode_ = null;
        }
        public MoveNodeImpl(Move move)
        {
            this.Score = 0.0f;

            this.m_parentNode_ = null;
            this.m_key_ = move;

            this.m_childMove_ = Move.Empty;
            this.m_childNode_ = null;
        }







        public MoveNode m_parentNode_;


        /// <summary>
        /// ルート・ノードなら真。
        /// </summary>
        /// <returns></returns>
        public bool IsRoot()
        {
            return this.m_parentNode_ == null;
        }


        public Move Key
        {
            get
            {
                return this.m_key_;
            }
        }
        private Move m_key_;
        public float Score { get; set; }





        public List<Move> ToPvList()
        {
            // 本譜（ノードのリスト）
            List<Move> pvList = new List<Move>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            MoveNode cursor = this;
            while (null != cursor)//ルートを含むところまで遡ります。
            {
                pvList.Add(cursor.Key); // リスト作成

                cursor = ((MoveNodeImpl)cursor).m_parentNode_;
            }
            pvList.Reverse();

            return pvList;
        }





        public bool Child_Exists
        {
            get
            {
                if (this.m_childMove_ == Move.Empty)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 次の局面への全ての候補手
        /// </summary>
        private Move m_childMove_;
        private MoveNode m_childNode_;

        public void Child_Clear(Tree kifu1, KwLogger logger)
        {
            this.m_childMove_ = Move.Empty;
            kifu1.ClearPv(logger);

            this.m_childNode_ = null;
        }
        public void Child_SetChild(Move move, MoveNode newChildNode, Tree kifu1, KwLogger logger)
        {
            this.m_childMove_ = move;
            this.m_childNode_ = newChildNode;
            kifu1.AppendPv(move,logger);

            ((MoveNodeImpl)newChildNode).m_parentNode_ = this;
        }
        public Move Child_GetItem(Tree kifu1)
        {
            return this.m_childMove_;
        }
    }
}
