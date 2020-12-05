namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option
{
    public interface IEngineOptionBool : IEngineOption
    {

        /// <summary>
        /// 既定値
        /// </summary>
        bool Default { get; set; }

        /// <summary>
        /// 現在値
        /// </summary>
        bool Value { get; set; }

    }
}
