using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Features;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
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
            IFeatureVector featureVector
            );

    }
}
