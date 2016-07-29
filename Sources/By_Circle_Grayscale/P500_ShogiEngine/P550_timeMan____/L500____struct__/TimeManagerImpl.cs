using Grayscale.P550_timeMan____.L___500_struct__;
using System.Diagnostics;

namespace Grayscale.P550_timeMan____.L500____struct__
{
    /// <summary>
    /// 時間管理。
    /// </summary>
    public class TimeManagerImpl : TimeManager
    {
        public TimeManagerImpl(long thinkingMilliSeconds)
        {
            this.Stopwatch = new Stopwatch();
            this.ThinkingMilliSeconds = thinkingMilliSeconds;
        }

        /// <summary>
        /// 思考時間。
        /// </summary>
        public long ThinkingMilliSeconds { get; set; }

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
            return this.ThinkingMilliSeconds < this.Stopwatch.ElapsedMilliseconds;
        }

    }
}
