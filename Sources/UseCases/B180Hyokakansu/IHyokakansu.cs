using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;

namespace Grayscale.A500ShogiEngine.B180Hyokakansu.C500Hyokakansu
{

    /// <summary>
    /// 局面評価の判断。
    /// </summary>
    public interface IHyokakansu
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
            FeatureVector featureVector
            );

    }
}
