using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using System;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG || LEARN
#endif

namespace Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu
{


    /// <summary>
    /// 千日手。
    /// </summary>
    public class Hyokakansu_Sennitite : HyokakansuAbstract
    {

        public Hyokakansu_Sennitite()
            : base(HyokakansuName.N01_Sennitite________)
        {
        }


        /// <summary>
        /// 評価値を返します。先手が有利ならプラス、後手が有利ならマイナス、互角は 0.0 です。
        /// </summary>
        /// <param name="input_node"></param>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
        public override void Evaluate(
            out float out_score,
            Sky src_Sky,
            FeatureVector featureVector,
            KwLogger errH
            )
        {
            out_score = 0.0f;//互角

            switch (src_Sky.KaisiPside)
            {
                case Playerside.P1: out_score = float.MinValue; break;
                case Playerside.P2: out_score = float.MaxValue; break;
                default: throw new Exception("千日手判定をしようとしましたが、先後の分からない局面データがありました。");
            }
        }
    }
}
