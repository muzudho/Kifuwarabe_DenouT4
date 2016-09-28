using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C___500_Hyokakansu
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
        float Evaluate(
            Sky src_Sky,
            FeatureVector featureVector,
            KwLogger errH
            );

    }
}
