using Grayscale.A150_LogKyokuPng.B100_KyokumenPng.C___500_Struct;
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.P323_Sennitite__.C___500_Struct;
using Grayscale.P521_FeatureVect.C___500_Struct;
using Grayscale.P542_Scoreing___.L___240_Shogisasi;

namespace Grayscale.P542_Scoreing___.L___250_Args
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
