using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Features;

#if DEBUG || LEARN
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public abstract class UtilHyokakansuCollection
    {

        /// <summary>
        /// 「千日手」評価関数１個。
        /// </summary>
        public static IHyokakansu Hyokakansu_Sennichite { get; set; }

        /// <summary>
        /// 「駒割」「二駒関係ＰＰ」の評価関数が入ったリスト。
        /// </summary>
        public static List<IHyokakansu> Hyokakansu_Normal { get; set; }

        static UtilHyokakansuCollection()
        {
            UtilHyokakansuCollection.Hyokakansu_Sennichite = new Hyokakansu_Sennitite();

            UtilHyokakansuCollection.Hyokakansu_Normal = new List<IHyokakansu>()
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
        /// <param name="logTag"></param>
        public static float EvaluateAll_Normal(
            Playerside psideA,
            ISky positionA,
            IFeatureVector fv
            )
        {
            float score = 0.0f;

            // 妄想と、指定のノードを比較し、点数付けします。
            foreach (IHyokakansu hyokakansu in UtilHyokakansuCollection.Hyokakansu_Normal)
            {
                score += hyokakansu.Evaluate(
                    psideA,
                    positionA,
                    fv
                );
            }

            return score;
        }
    }
}
