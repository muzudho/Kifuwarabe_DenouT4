using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Features;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{

    /// <summary>
    /// 局面の得点計算。
    /// </summary>
    public abstract class HyokakansuAbstract : IHyokakansu
    {

        /// <summary>
        /// 評価関数名
        /// </summary>
        public HyokakansuName Name
        {
            get
            {
                return this.name;
            }
        }
        private HyokakansuName name;

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="name"></param>
        public HyokakansuAbstract(HyokakansuName name)
        {
            this.name = name;
        }

        /// <summary>
        /// 評価値を返します。先手が有利ならプラス、後手が有利ならマイナス、互角は 0.0 です。
        /// </summary>
        /// <param name="keisanArgs"></param>
        /// <returns></returns>
        abstract public float Evaluate(
            Playerside psideA,
            ISky positionA,
            IFeatureVector featureVector
            );

    }
}
