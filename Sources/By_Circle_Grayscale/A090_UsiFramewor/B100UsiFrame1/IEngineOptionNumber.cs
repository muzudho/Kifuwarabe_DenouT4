namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option
{
    /// <summary>
    /// long型に対応。
    /// </summary>
    public interface IEngineOptionNumber : IEngineOption
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
