using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A500_ShogiEngine.B220_Tansaku____.C___500_Tansaku;

namespace Grayscale.A500_ShogiEngine.B220_Tansaku____.C500____Struct
{

    /// <summary>
    /// 探索が終わるまで、途中で変更されない設定。
    /// </summary>
    public class Tansaku_ArgsImpl : Tansaku_Args
    {
        /// <summary>
        /// 本将棋なら真。
        /// </summary>
        public bool IsHonshogi { get { return this.m_isHonshogi_; } }
        private bool m_isHonshogi_;

        /// <summary>
        /// 読みの上限の様々な設定☆（深さ優先探索で使用☆）
        /// </summary>
        public int[] YomuLimitter { get { return this.m_yomuLimitter_; } }
        private int[] m_yomuLimitter_;

        /// <summary>
        /// ログ用☆
        /// </summary>
        public KaisetuBoards LogF_moveKiki { get { return this.m_logF_moveKiki_; } }
        private KaisetuBoards m_logF_moveKiki_;

        public Tansaku_ArgsImpl(
            bool isHonshogi,
            int[] yomuLimitter,
            KaisetuBoards logF_moveKiki
            )
        {
            this.m_isHonshogi_ = isHonshogi;
            this.m_yomuLimitter_ = yomuLimitter;
            this.m_logF_moveKiki_ = logF_moveKiki;
        }
    }

}
