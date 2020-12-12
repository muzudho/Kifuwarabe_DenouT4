#if DEBUG
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B410Collection.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;

namespace Grayscale.A210KnowNingen.B460KyokumMoves.C250Log
{
    /// <summary>
    /// ログを取るためのもの。
    /// </summary>
    public interface MmLogGenjo
    {
        KaisetuBoard BrdMove { get; set; }

        /// <summary>
        /// 読み開始手目済み
        /// </summary>
        int YomikaisiTemezumi { get; }

        int Temezumi_yomiCur { get; }

        Move Move { get; }

        KwLogger ErrH { get; }

        void Log1(Playerside pside_genTeban3);

        void Log2(
            Playerside tebanSeme,//手番（利きを調べる側）
            Playerside tebanKurau//手番（喰らう側）
        );

        void Log3(
            Sky src_Sky,
            Playerside tebanKurau,//手番（喰らう側）
            Playerside tebanSeme,//手番（利きを調べる側）
            Fingers fingers_kurau_IKUSA,//戦駒（喰らう側）
            Fingers fingers_kurau_MOTI,// 持駒（喰らう側）
            Fingers fingers_seme_IKUSA,//戦駒（利きを調べる側）
            Fingers fingers_seme_MOTI// 持駒（利きを調べる側）
        );

        void Log4(
            Sky src_Sky,
            Playerside tebanSeme,//手番（利きを調べる側）
            Maps_OneAndOne<Finger, SySet<SyElement>> kmMove_seme_IKUSA
        );

    }
}
#endif
