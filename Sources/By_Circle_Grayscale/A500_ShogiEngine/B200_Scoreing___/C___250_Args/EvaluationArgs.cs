using Grayscale.A150LogKyokuPng.B100KyokumenPng.C500Struct;
using Grayscale.A210KnowNingen.B630Sennitite.C500Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi;

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args
{
    public interface EvaluationArgs
    {
        /// <summary>
        /// 千日手になるかどうかの判定だけを行うクラスです。
        /// </summary>
        SennititeConfirmer SennititeConfirmer { get; }

        /// <summary>
        /// フィーチャー・ベクター。
        /// </summary>
        FeatureVector FeatureVector { get; }

        Shogisasi Shogisasi { get; }

        KyokumenPngEnvironment ReportEnvironment { get; }

#if DEBUG
        /// <summary>
        /// デバッグ用。
        /// </summary>
        KaisetuBoards KaisetuBoards_orNull { get; }
#endif
    }
}
