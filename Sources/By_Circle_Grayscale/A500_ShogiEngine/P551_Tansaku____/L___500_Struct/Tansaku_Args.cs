﻿#if DEBUG
using Grayscale.A210_KnowNingen_.B400_Log_Kaisetu.C250____Struct;
#endif

namespace Grayscale.P551_Tansaku____.L___500_Tansaku
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