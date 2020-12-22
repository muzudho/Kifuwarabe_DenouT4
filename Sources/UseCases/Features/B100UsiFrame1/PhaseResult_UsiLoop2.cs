
namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public enum PhaseResultUsiLoop2
    {
        /// <summary>
        /// 何もしません。
        /// </summary>
        None,

        /// <summary>
        /// ループを抜けます。
        /// </summary>
        Break,

        /// <summary>
        /// タイムアウトによる強制終了
        /// </summary>
        TimeoutShutdown,
    }
}
