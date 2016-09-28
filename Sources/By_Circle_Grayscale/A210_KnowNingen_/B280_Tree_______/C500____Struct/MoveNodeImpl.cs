using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

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

            this.SetParentNode(null);
            this.m_key_ = Conv_Move.GetErrorMove();

            this.m_move_ = Move.Empty;
            this.m_childNode_ = null;
        }
        public MoveNodeImpl(Move move)
        {
            this.Score = 0.0f;

            this.SetParentNode(null);
            this.m_key_ = move;

            this.m_move_ = Move.Empty;
            this.m_childNode_ = null;
        }







        public MoveNode GetParentNode()
        {
            return this.parentNode;
        }
        public void SetParentNode(MoveNode parent)
        {
            this.parentNode = parent;
        }
        private MoveNode parentNode;


        /// <summary>
        /// ルート・ノードなら真。
        /// </summary>
        /// <returns></returns>
        public bool IsRoot()
        {
            return this.GetParentNode() == null;
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

                cursor = cursor.GetParentNode();
            }
            pvList.Reverse();

            return pvList;
        }





        public bool Child_HasItem
        {
            get
            {
                if (this.m_move_ == Move.Empty)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 次の局面への全ての候補手
        /// </summary>
        private Move m_move_;
        private MoveNode m_childNode_;

        public bool Child_ContainsKey(Move key)
        {
            return this.m_move_ == key && key != Move.Empty;
        }
        public void Child_Clear()
        {
            this.m_move_ = Move.Empty;
            this.m_childNode_ = null;
        }
        public void Child_SetItem(Move move, MoveNode newNode)
        {
            this.m_move_ = move;
            this.m_childNode_ = newNode;
            newNode.SetParentNode(this);
        }

        public Move Child_GetItem()
        {
            return this.m_move_;
        }
    }
}
