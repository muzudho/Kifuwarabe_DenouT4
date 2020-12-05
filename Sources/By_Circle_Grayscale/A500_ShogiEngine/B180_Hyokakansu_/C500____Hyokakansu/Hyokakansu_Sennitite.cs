﻿using System;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500Struct;
// using Grayscale.A210KnowNingen.B170WordShogi.C500Word;

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
        public override float Evaluate(
            Playerside psideA,
            ISky positionA,
            FeatureVector featureVector,
            KwLogger errH
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
