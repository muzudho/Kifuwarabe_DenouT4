namespace Grayscale.A090_UsiFramewor.B100_usiFrame1__.C___490_Option__
{
    /// <summary>
    /// long型に対応。
    /// </summary>
    public interface EngineOption_Number : EngineOption
    {

        /// <summary>
        /// 既定値
        /// </summary>
        long Default { get; set; }

        /// <summary>
        /// 現在値
        /// </summary>
        long Value { get; set; }

    }
}
