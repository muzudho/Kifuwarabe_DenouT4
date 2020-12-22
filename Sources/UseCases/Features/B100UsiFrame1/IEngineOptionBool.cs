namespace Grayscale.Kifuwaragyoku.UseCases.Features
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
