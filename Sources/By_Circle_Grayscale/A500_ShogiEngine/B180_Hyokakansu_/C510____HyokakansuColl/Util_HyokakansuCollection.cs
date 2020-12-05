using System.Collections.Generic;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500Struct;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C___500_Hyokakansu;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu;

#if DEBUG || LEARN
using Grayscale.A210KnowNingen.B620KyokumHyoka.C250Struct;
#endif

namespace Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C510____HyokakansuColl
{
    public abstract class Util_HyokakansuCollection
    {

        /// <summary>
        /// 「千日手」評価関数１個。
        /// </summary>
        public static Hyokakansu Hyokakansu_Sennichite { get; set; }

        /// <summary>
        /// 「駒割」「二駒関係ＰＰ」の評価関数が入ったリスト。
        /// </summary>
        public static List<Hyokakansu> Hyokakansu_Normal { get; set; }

        static Util_HyokakansuCollection()
        {
            Util_HyokakansuCollection.Hyokakansu_Sennichite = new Hyokakansu_Sennitite();

            Util_HyokakansuCollection.Hyokakansu_Normal = new List<Hyokakansu>()
            {
                new Hyokakansu_Komawari(),
                new Hyokakansu_NikomaKankeiPp(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_mutable_KAIZOMAE">この評価シートに明細項目を追加します。</param>
        /// <param name="fv"></param>
        /// <param name="errH"></param>
        public static float EvaluateAll_Normal(
            Playerside psideA,
            ISky positionA,
            FeatureVector fv,
            KwLogger errH
            )
        {
            float score = 0.0f;

            // 妄想と、指定のノードを比較し、点数付けします。
            foreach (Hyokakansu hyokakansu in Util_HyokakansuCollection.Hyokakansu_Normal)
            {
                score += hyokakansu.Evaluate(
                    psideA,
                    positionA,
                    fv,
                    errH
                );
            }

            return score;
        }
    }
}
