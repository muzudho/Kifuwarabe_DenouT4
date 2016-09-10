using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;

namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct
{
    public class NodeExImpl : NodeEx
    {
        /// <summary>
        /// スコア
        /// </summary>
        public float Score { get { return this.m_score_; } }
        public void AddScore(float offset) { this.m_score_ += offset; }
        public void SetScore(float score) { this.m_score_ = score; }
        private float m_score_;

    }
}
