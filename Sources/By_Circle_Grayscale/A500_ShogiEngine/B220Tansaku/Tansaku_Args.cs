﻿#if DEBUG
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
#endif

namespace Grayscale.A500ShogiEngine.B220Tansaku.C500Tansaku
{

    /// <summary>
    /// 探索が終わるまで、途中で変更されない設定。
    /// </summary>
    public interface Tansaku_Args
    {

        bool IsHonshogi { get; }
        int[] YomuLimitter { get; }

#if DEBUG
        KaisetuBoards LogF_moveKiki { get; }
#endif
    }
}