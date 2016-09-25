using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{

    /// <summary>
    /// ノードです。
    /// </summary>
    public class MoveNodeImpl : MoveNode
    {
        public MoveNodeImpl(Move move)
        {
            this.MoveEx = new MoveExImpl(move);

            this.SetParentNode(null);
            this.m_key_ = move;

            this.Children1 = new ChildrenImpl();
        }




        public MoveEx MoveEx { get; set; }





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



        public List<Move> ToMovelist()
        {
            return this.Children1.ToMovelist();
        }


        public Children Children1 { get; set; }
    }
}
