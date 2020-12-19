﻿using System.Collections.Generic;

namespace Grayscale.A500ShogiEngine.B200Scoreing.C005UsiLoop
{
    public interface ShogiEngine
    {
        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        bool GoPonder { get; set; }

        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// </summary>
        Dictionary<string, string> GameoverProperties { get; set; }
    }
}
