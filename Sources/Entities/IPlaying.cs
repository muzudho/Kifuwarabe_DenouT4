using Grayscale.Kifuwaragyoku.Entities.Searching;

namespace Grayscale.Kifuwaragyoku.Entities
{
    public interface IPlaying
    {
        /// <summary>
        /// 時間管理
        /// </summary>
        ITimeManager TimeManager { get; set; }
    }
}
