#if DEBUG
using Grayscale.Kifuwaragyoku.Entities.Features;
#endif

namespace Grayscale.Kifuwaragyoku.Entities.Searching
{

    /// <summary>
    /// 探索が終わるまで、途中で変更されない設定。
    /// </summary>
    public interface ISearchArgs
    {

        bool IsHonshogi { get; }
        int[] YomuLimitter { get; }

#if DEBUG
        KaisetuBoards LogF_moveKiki { get; }
#endif
    }
}
