using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Searching;

namespace Grayscale.Kifuwaragyoku.Entities
{
    public interface IPlaying
    {
        /// <summary>
        /// 右脳。
        /// </summary>
        IFeatureVector FeatureVector { get; set; }

        /// <summary>
        /// 時間管理
        /// </summary>
        ITimeManager TimeManager { get; set; }
    }
}
