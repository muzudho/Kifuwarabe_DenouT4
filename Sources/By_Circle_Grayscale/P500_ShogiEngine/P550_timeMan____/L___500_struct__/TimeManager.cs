﻿using System.Diagnostics;

namespace Grayscale.P550_timeMan____.L___500_struct__
{
    public interface TimeManager
    {
        /// <summary>
        /// 思考時間。
        /// </summary>
        long ThinkableMilliSeconds { get; set; }

        /// <summary>
        /// ストップウォッチ
        /// </summary>
        Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// 思考の時間切れ
        /// </summary>
        /// <returns></returns>
        bool IsTimeOver();
    }
}
