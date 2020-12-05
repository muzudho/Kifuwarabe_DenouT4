using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500Struct;

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
            Playerside psideA,
            ISky positionA,
            FeatureVector featureVector,
            KwLogger errH
            );

    }
}
