using System.Collections.Generic;

namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option
{
    public interface IEngineOptionCombo : IEngineOption
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
