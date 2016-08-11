namespace Grayscale.P091_usiFrame1__.L___490_Option__
{
    public interface EngineOption_Spin : EngineOption
    {

        /// <summary>
        /// 既定値
        /// </summary>
        int Default { get; set; }

        /// <summary>
        /// 現在値
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// 最小値
        /// </summary>
        int Min { get; set; }

        /// <summary>
        /// 最大値
        /// </summary>
        int Max { get; set; }

    }
}
