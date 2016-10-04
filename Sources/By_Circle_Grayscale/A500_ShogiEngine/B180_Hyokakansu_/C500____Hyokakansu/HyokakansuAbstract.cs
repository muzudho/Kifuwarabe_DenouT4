using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C___500_Hyokakansu;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

namespace Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu
{

    /// <summary>
    /// 局面の得点計算。
    /// </summary>
    public abstract class HyokakansuAbstract : Hyokakansu
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
        public HyokakansuAbstract( HyokakansuName name)
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
            Sky positionA,
            FeatureVector featureVector,
            KwLogger errH
            );

    }
}
