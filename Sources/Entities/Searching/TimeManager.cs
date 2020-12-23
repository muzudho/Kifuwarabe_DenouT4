using System.Diagnostics;
using Grayscale.Kifuwaragyoku.Entities.Searching;

namespace Grayscale.Kifuwaragyoku.Entities.Searching
{
    /// <summary>
    /// 時間管理。
    /// </summary>
    public class TimeManager : ITimeManager
    {
        public TimeManager(long thinkableMilliSeconds)
        {
            this.Stopwatch = new Stopwatch();
            this.ThinkableMilliSeconds = thinkableMilliSeconds;
        }

        /// <summary>
        /// 思考用に配分した使っていい時間。
        /// </summary>
        public long ThinkableMilliSeconds { get; set; }

        /// <summary>
        /// ストップウォッチ
        /// </summary>
        public Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// 思考の時間切れ
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver()
        {
            return this.ThinkableMilliSeconds < this.Stopwatch.ElapsedMilliseconds;
        }

    }
}
