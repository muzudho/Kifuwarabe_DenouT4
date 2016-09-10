using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct
{
    public interface NodeEx
    {
        /// <summary>
        /// スコア
        /// </summary>
        float Score { get; }
        void AddScore(float offset);
        void SetScore(float score);

    }
}
