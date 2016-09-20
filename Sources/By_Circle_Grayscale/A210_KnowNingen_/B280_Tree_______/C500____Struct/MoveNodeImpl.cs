using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B600_UtilSky____.C500____Util;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{

    /// <summary>
    /// ノードです。
    /// </summary>
    public class MoveNodeImpl : KifuNode
    {
        public MoveNodeImpl(Move move)
        {
            this.MoveEx = new MoveExImpl(move);

            this.SetParentNode(null);
            this.m_key_ = move;
            this.SetValue(null);

            this.Children1 = new ChildrenImpl();
        }

        public MoveEx MoveEx { get; set; }



        /// <summary>
        /// </summary>
        public Sky GetNodeValue()
        {
            return null;
        }
        public void SetValue(Sky sky)
        {
        }


        public KifuNode GetParentNode()
        {
            return this.parentNode;
        }
        public void SetParentNode(KifuNode parent)
        {
            this.parentNode = parent;
        }
        private KifuNode parentNode;


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

        public Children Children1 { get; set; }
    }
}
