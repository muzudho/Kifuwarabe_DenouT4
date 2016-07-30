using System.Collections.Generic;

namespace Grayscale.P571_usiFrame1__.L___490_Option__
{
    public interface EngineOption_Combo : EngineOption
    {

        /// <summary>
        /// 既定値
        /// </summary>
        string Default { get; set; }

        /// <summary>
        /// 現在値
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// 値のリスト
        /// </summary>
        List<string> ValueVars { get; set; }

    }
}
