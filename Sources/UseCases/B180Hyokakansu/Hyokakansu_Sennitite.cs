using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
// using Grayscale.Kifuwaragyoku.Entities.Features;

#if DEBUG || LEARN
#endif

namespace Grayscale.A500ShogiEngine.B180Hyokakansu.C500Hyokakansu
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
        public override float Evaluate(
            Playerside psideA,
            ISky positionA,
            FeatureVector featureVector
            )
        {
            switch (psideA)//positionA.GetKaisiPside()
            {
                case Playerside.P1: return float.MinValue;
                case Playerside.P2: return float.MaxValue;
                default: throw new Exception("千日手判定をしようとしましたが、先後の分からない局面データがありました。");
            }
        }
    }
}
