using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A690FvLearn.B110FvLearn.C250Learn;
using Grayscale.A690FvLearn.B110FvLearn.C260View;
using Grayscale.A690FvLearn.B110FvLearn.C450____Tyoseiryo;
using Grayscale.A690FvLearn.B110FvLearn.C470____StartZero;
using Grayscale.A690FvLearn.B110FvLearn.C480Functions;

#if DEBUG
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
// using Grayscale.Kifuwaragyoku.Entities;
#endif

namespace Grayscale.A690FvLearn.B110FvLearn.C506AutoMoveSort
{
    public abstract class Util_AutoSortingRush
    {

        /// <summary>
        /// 指し手の順位を変えるループです。
        /// </summary>
        /// <param name="out_pushCount"></param>
        /// <param name="out_isEndAutoLearn"></param>
        /// <param name="ref_isRequest_ShowGohosyu"></param>
        /// <param name="ref_isRequest_ChangeKyokumenPng"></param>
        /// <param name="ref_isRequestDoEvents"></param>
        /// <param name="loopLimit"></param>
        /// <param name="ref_tyoseiryo"></param>
        /// <param name="move1"></param>
        /// <param name="uc_Main"></param>
        /// <param name="errH"></param>
        public static void DoSortMoveRush(
            out int out_pushCount,
            out bool out_isEndAutoLearn,
            ref bool ref_isRequest_ShowGohosyu,
            ref bool ref_isRequest_ChangeKyokumenPng,
            ref bool ref_isRequestDoEvents,
            int loopLimit,
            ref float ref_tyoseiryo,
            Move move1,
            UcMain uc_Main, ILogger errH
            )
        {
            out_isEndAutoLearn = false;
            out_pushCount = 0;

            for (; out_pushCount < loopLimit; out_pushCount++)
            { //指し手順位更新ループ
                //----------------------------------------
                // 強制中断（ループの最初のうちに）
                //----------------------------------------
                //
                // 「Stop_learning.txt」という名前のファイルが .exe と同じフォルダーに置いてあると
                // 学習を終了することにします。
                //
                if (uc_Main.StopLearning.IsStop())
                {
                    out_isEndAutoLearn = true;
                    goto gt_EndMethod;
                }

                // 順位確認
                if (0 < uc_Main.LstGohosyu.Items.Count)
                {
                    GohosyuListItem gohosyuItem = (GohosyuListItem)uc_Main.LstGohosyu.Items[0];

                    if (move1 == gohosyuItem.Move)
                    {
                        // 1位なら終了
#if DEBUG
                        string sfen = ConvMove.ToSfen(gohosyuItem.Move);
                        errH.AppendLine("items.Count=[" + uc_Main.LstGohosyu.Items.Count + "] sfen=[" + sfen + "]");
                        errH.Flush(LogTypes.Plain);
#endif
                        break;
                    }
                }

                // １位ではないのでランクアップ。
                UtilLearnFunctions.Do_RankUpHonpu(ref ref_isRequest_ShowGohosyu, uc_Main, move1, ref_tyoseiryo);

                // 調整量の自動調整
                if (uc_Main.ChkTyoseiryoAuto.Checked)
                {
                    Util_Tyoseiryo.Up_Bairitu_AtStep(ref ref_isRequestDoEvents, uc_Main, out_pushCount, ref ref_tyoseiryo);
                }

                if (uc_Main.ChkStartZero.Checked)// 自動で、平手初期局面の点数を 0 点に近づけるよう調整します。
                {
                    Util_StartZero.Adjust_HirateSyokiKyokumen_0ten_AndFvParamRange(ref ref_isRequestDoEvents, uc_Main.LearningData.Fv, errH);
                }

                // 局面の表示を更新します。
                if (ref_isRequest_ShowGohosyu)
                {
                    // 合法手一覧を更新
                    UtilLearningView.Aa_ShowGohosyu2(uc_Main.LearningData, uc_Main, errH);
                    // 局面PNG画像の更新は、ここでは行いません。
                    ref_isRequest_ShowGohosyu = false;

                    ref_isRequestDoEvents = true;
                }

                if (ref_isRequestDoEvents)
                {
                    Application.DoEvents();
                    ref_isRequestDoEvents = false;
                }

            }// 指し手順位更新ループ
             //----------------------------------------
             // 連打終わり
             //----------------------------------------

        gt_EndMethod:
            ;
        }

    }
}
