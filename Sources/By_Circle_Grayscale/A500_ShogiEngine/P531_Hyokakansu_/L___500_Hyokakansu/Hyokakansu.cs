﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.P321_KyokumHyoka.C___250_Struct;
using Grayscale.P521_FeatureVect.C___500_Struct;

namespace Grayscale.P531_Hyokakansu_.L___500_Hyokakansu
{

    /// <summary>
    /// 局面評価の判断。
    /// </summary>
    public interface Hyokakansu
    {

        /// <summary>
        /// 評価関数名。
        /// </summary>
        HyokakansuName Name { get; }

        /// <summary>
        /// 評価値を返します。先手が有利ならプラス、後手が有利ならマイナス、互角は 0.0 です。
        /// </summary>
        /// <param name="keisanArgs"></param>
        /// <returns></returns>
        void Evaluate(
            out float score,
#if DEBUG || LEARN
            out KyHyokaMeisai_Koumoku kyokumenScore,
#endif
            SkyConst src_Sky,
            FeatureVector featureVector,
            KwErrorHandler errH
            );

    }
}
