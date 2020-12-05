namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option
{
    public interface IEngineOptionSpin : IEngineOption
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
