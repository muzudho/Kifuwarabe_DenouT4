using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C___500_Hyokakansu;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
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
        public static void EvaluateAll_Normal(
            MoveEx moveEx,
            Sky position,
            FeatureVector fv,
            KwLogger errH
            )
        {
            // 妄想と、指定のノードを比較し、点数付けします。
            foreach (Hyokakansu hyokakansu in Util_HyokakansuCollection.Hyokakansu_Normal)
            {
                float score;
#if DEBUG || LEARN
                KyHyokaMeisai_Koumoku meisai;
#endif
                hyokakansu.Evaluate(
                    out score,
#if DEBUG || LEARN
                    out meisai,
#endif
                    position,
                    fv,
                    errH
                );

                moveEx.AddScore(score);
#if DEBUG || LEARN
                moveEx.KyHyokaSheet_Mutable.Add(
                    hyokakansu.Name.ToString(),
                    meisai
                );
#endif
            }
        }
    }
}
