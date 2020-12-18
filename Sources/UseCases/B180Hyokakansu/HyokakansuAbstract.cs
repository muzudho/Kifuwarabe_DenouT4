using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B180Hyokakansu.C500Hyokakansu;

namespace Grayscale.A500ShogiEngine.B180Hyokakansu.C500Hyokakansu
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
            FeatureVector featureVector,
            ILogger errH
            );

    }
}
