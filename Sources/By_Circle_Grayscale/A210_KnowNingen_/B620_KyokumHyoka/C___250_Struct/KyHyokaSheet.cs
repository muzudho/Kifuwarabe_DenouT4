using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct
{

    /// <summary>
    /// 局面の評価明細書。
    /// </summary>
    public interface KyHyokaSheet
    {

        /// <summary>
        /// 各項目。
        /// </summary>
        Dictionary<string, KyHyokaMeisai_Koumoku> Items { get; }

        void Add(string name, KyHyokaMeisai_Koumoku item);
        KyHyokaMeisai_Koumoku Get(string name);
        void Clear();

    }
}
