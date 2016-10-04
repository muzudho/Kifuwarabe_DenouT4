using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___400_54List;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C400____54List;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C430____Zooming;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C440____Ranking;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C460____Scoreing;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C470____StartZero
{
    /// <summary>
    /// 平手初期局面を 0 点に近づける機能です。
    /// </summary>
    public abstract class Util_StartZero
    {

        /// <summary>
        /// 平手初期局面
        /// </summary>
        private static Sky src_Sky_hirateSyokikyokumen;

        /// <summary>
        /// 平手初期局面の54要素のリスト。
        /// </summary>
        private static N54List n54List_hirateSyokikyokumen;

        private static int[] tyoseiryo;

        static Util_StartZero()
        {
            Util_StartZero.tyoseiryo = new int[]{
                64,
                32,
                16,
                8,
                4,
                2,
                1
            };
        }

        /// <summary>
        /// 平手初期局面が -100点～+100点　に収まるように調整します。
        /// 
        /// 7回だけ調整します。
        /// 
        /// [0]回目：　順位を　64 ずらす。
        /// [1]回目：　順位を　32 ずらす。
        /// [2]回目：　順位を　16 ずらす。
        /// [3]回目：　順位を　8 ずらす。
        /// [4]回目：　順位を　4 ずらす。
        /// [5]回目：　順位を　2 ずらす。
        /// [6]回目：　順位を　1 ずらす。
        /// 
        /// これで、１方向に最長で（順位換算で） 130 ほどずれます。
        /// 
        /// </summary>
        public static void Adjust_HirateSyokiKyokumen_0ten_AndFvParamRange(
            ref bool ref_isRequestDoEvents,
            FeatureVector fv, KwLogger errH)
        {
            if (null == Util_StartZero.src_Sky_hirateSyokikyokumen)
            {
                // 平手初期局面
                Util_StartZero.src_Sky_hirateSyokikyokumen = Util_SkyCreator.New_Hirate();
            }

            if (null == Util_StartZero.n54List_hirateSyokikyokumen)
            {
                //----------------------------------------
                // ４０枚の駒、または１４種類の持駒。多くても５４要素。
                //----------------------------------------
                Util_StartZero.n54List_hirateSyokikyokumen = Util_54List.Calc_54List(Util_StartZero.src_Sky_hirateSyokikyokumen, errH);
            }

            Hyokakansu_NikomaKankeiPp kansu = new Hyokakansu_NikomaKankeiPp();



            //--------------------------------------------------------------------------------
            // Check
            //--------------------------------------------------------------------------------
            //
            // 平手初期局面の点数を調べます。
            //
            float score = kansu.Evaluate(
                Util_StartZero.src_Sky_hirateSyokikyokumen.GetKaisiPside(),
                Util_StartZero.src_Sky_hirateSyokikyokumen,
                fv,
                errH
                );

            if (-100 <= score && score <= 100)
            {
                // 目標達成。
                goto gt_Goal;
            }

            for (int iCount = 0; iCount < 7; iCount++)//最大で7回調整します。
            {
                // 初期局面の評価値が、-100～100 よりも振れていれば、0 になるように調整します。

                //--------------------------------------------------------------------------------
                // 点数を、順位に変換します。
                //--------------------------------------------------------------------------------
                Util_Ranking.Perform_Ranking(fv);

                //
                // 調整量
                //
                int chosei_offset = Util_StartZero.tyoseiryo[iCount];// 調整量を、調整します。どんどん幅が広くなっていきます。

                if (-100 <= score && score <= 100)
                {
                    // 目標達成。
                    goto gt_Goal;
                }
                else if (100 < score)// ±0か、マイナスに転じさせたい。
                {
                    chosei_offset *= - 1;
                }

                int changedCells;
                Util_FvScoreing.Fill54x54_Add(out changedCells, chosei_offset, src_Sky_hirateSyokikyokumen, fv,
                    Util_StartZero.n54List_hirateSyokikyokumen, errH);

                // 順位を、点数に変換します。
                Util_Zooming.ZoomTo_FvParamRange(fv, errH);

                // フォームの更新を要求します。
                ref_isRequestDoEvents = true;

                //--------------------------------------------------------------------------------
                // Check
                //--------------------------------------------------------------------------------
                //
                // 平手初期局面の点数を調べます。
                //
                score = kansu.Evaluate(
                    Util_StartZero.src_Sky_hirateSyokikyokumen.GetKaisiPside(),
                    Util_StartZero.src_Sky_hirateSyokikyokumen,
                    fv,
                    errH
                    );

                if (-100 <= score && score <= 100)
                {
                    // 目標達成。
                    goto gt_Goal;
                }

                // 目標を達成していないなら、ループを繰り返します。
            }

        gt_Goal:
            ;
        }
    }
}
