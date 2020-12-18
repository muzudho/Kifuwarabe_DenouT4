﻿using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A690FvLearn.B110FvLearn.C250Learn;
using Grayscale.A690FvLearn.B110FvLearn.C260View;
using Grayscale.A690FvLearn.B110FvLearn.C450____Tyoseiryo;
using Grayscale.A690FvLearn.B110FvLearn.C470____StartZero;
using Grayscale.A690FvLearn.B110FvLearn.C480Functions;
using Grayscale.A690FvLearn.B110FvLearn.C506AutoMoveSort;

namespace Grayscale.A690FvLearn.B110FvLearn.C508AutoMoveRush
{
    /// <summary>
    /// 自動で棋譜を読み取ります。
    /// </summary>
    public abstract class UtilAutoMoveRush
    {
        /// <summary>
        /// 評価更新繰り返し回数。1以上の数字が必要。
        /// </summary>
        public const int RENZOKU_KAISU = 3; // 100の前提で作ってあるが、やるほど弱くなるので数回にしておく。
        // 60だと 34 ぐらいで終わる。
        // 150 だと 400 はすぐカンスト。
        // 256意味ない

        /// <summary>
        /// 繰り返し、指し手を進めます。
        /// </summary>
        public static void DoMoveRush(
            out bool out_isEndKifuread,
            ref bool isRequest_ShowGohosyu,
            ref bool isRequest_ChangeKyokumenPng,
            int renzokuTe,
            UcMain uc_Main,
            ILogTag errH)
        {
            out_isEndKifuread = false;

            bool isRequestDoEvents = false;
            bool isSaved = false;

            //
            // N手を連続で自動実行。
            // 本譜の手が残っている間。
            //
            for (int pushedButton = 0; pushedButton < renzokuTe && 0 < uc_Main.LstMove.Items.Count; pushedButton++)
            { // 指し手ループ
                isSaved = false;//リセット
                float tyoseiryo;
                float.TryParse(uc_Main.TxtTyoseiryo.Text, out tyoseiryo);

                HonpuMoveListItemImpl moveItem = (HonpuMoveListItemImpl)uc_Main.LstMove.Items[0];
                Move move1 = moveItem.Move;


                if (move1 == Move.Empty)
                {
                    goto gt_EndMoveList;
                }



                if (uc_Main.ChkIgnoreThink.Checked)
                {
                    // 最初の20手は学習しない。
                    int tesu;
                    if (!int.TryParse(uc_Main.TxtIgnoreLearn.Text, out tesu))
                    {
                        goto gt_EndTesu;
                    }

                    if (uc_Main.LearningData.PositionA.Temezumi < tesu + 1)
                    {
                        goto gt_EndLearn;
                    }
                gt_EndTesu:
                    ;
                }

                //----------------------------------------
                // FIXME: 暫定：ボタン連打は可能に。
                //----------------------------------------
                //this.btnUpdateKyokumenHyoka.Enabled = false;//ボタン連打防止。

                //----------------------------------------
                // 指し手の順位を変える回数
                //----------------------------------------
                int loopLimit = 1; // 通常1回
                if (uc_Main.ChkHyakuretuken.Checked)
                {
                    loopLimit = UtilAutoMoveRush.RENZOKU_KAISU;
                }

                //----------------------------------------
                // 指し手の順位を変えるループです。
                //----------------------------------------
                int pushCount;
                bool isEndAutoLearn;
                Util_AutoSortingRush.DoSortMoveRush(
                    out pushCount,
                    out isEndAutoLearn,
                    ref isRequest_ShowGohosyu,
                    ref isRequest_ChangeKyokumenPng,
                    ref isRequestDoEvents,
                    loopLimit,
                    ref tyoseiryo,
                    move1,
                    uc_Main, errH
                    );

                if (isEndAutoLearn)
                {
                    //棋譜の自動読取の終了
                    out_isEndKifuread = true;
                    goto gt_EndMethod;
                }

                // 調整量の自動調整
                if (uc_Main.ChkTyoseiryoAuto.Checked)
                {
                    Util_Tyoseiryo.Up_Bairitu_AtEnd(ref isRequestDoEvents, uc_Main, pushCount, ref tyoseiryo);
                }

                if (uc_Main.ChkStartZero.Checked)// 自動で、平手初期局面の点数を 0 点に近づけるよう調整します。
                {
                    Util_StartZero.Adjust_HirateSyokiKyokumen_0ten_AndFvParamRange(ref isRequestDoEvents, uc_Main.LearningData.Fv, errH);
                }

                if (uc_Main.ChkAutoParamRange.Checked)
                {
                    //// 自動で -999～999 に矯正。
                    UtilLearnFunctions.FvParamRange_PP(uc_Main.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。

                    // 合法手表示の更新を要求
                    isRequest_ShowGohosyu = true;
                }
            gt_EndLearn:
                ;


                //----------------------------------------
                // 投了図まで自動実行
                //----------------------------------------
                if (uc_Main.ChkRenzokuAutoRun.Checked)
                {
                    // [一手指す]ボタン押下
                    UtilLearningView.Ittesasu_ByBtnClick(
                        ref isRequest_ShowGohosyu,
                        ref isRequest_ChangeKyokumenPng,
                        uc_Main.LearningData, uc_Main, errH);

                    // 局面PNG画像の更新を要求
                    //isRequest_ChangeKyokumenPng = true;

                }
                else
                {
                    // 自動ループしないなら、終了。
                    break;
                }

                if (isRequest_ShowGohosyu)
                {
                    // 合法手一覧を更新
                    UtilLearningView.Aa_ShowGohosyu2(uc_Main.LearningData, uc_Main, errH);
                    isRequest_ShowGohosyu = false;

                    // 重い処理のあとは。
                    isRequestDoEvents = true;
                }

                if (isRequest_ChangeKyokumenPng)
                {
                    uc_Main.LearningData.ChangeKyokumenPng(
                        uc_Main,
                        uc_Main.LearningData.GetMove(),
                        uc_Main.LearningData.PositionA
                        );
                    isRequest_ChangeKyokumenPng = false;

                    // 重い処理のあとは。
                    isRequestDoEvents = true;
                }

                //----------------------------------------
                // N手で学習終了
                //----------------------------------------
                if (uc_Main.ChkEndLearnTesu.Checked)
                {
                    // N手で学習を終了します。
                    int tesu;
                    if (!int.TryParse(uc_Main.TxtEndLearnTesu.Text, out tesu))
                    {
                        goto gt_EndTesu;
                    }

                    if (tesu <= uc_Main.LearningData.PositionA.Temezumi)
                    {
                        // 自動ループしないなら、終了。
                        break;
                    }

                gt_EndTesu:
                    ;
                }

                // オートセーブ
                //
                // 20手間隔で。
                //
                if (
                    uc_Main.ChkAutosave.Checked && uc_Main.LearningData.PositionA.Temezumi % 20 == 0
                )
                {
                    UtilLearnFunctions.Do_Save(uc_Main, errH);
                    isSaved = true;
                }

                if (isRequestDoEvents)
                {
                    Application.DoEvents();
                    isRequestDoEvents = false;
                }
            }//指し手ループ

        gt_EndMoveList:
            ;

            // 終局時は、オートセーブ
            // ※20手で終局した場合は、20手で保存されたあと、2連続で終局時として保存されることになる。
            if (uc_Main.ChkAutosave.Checked)
            {
                if (!isSaved)
                {
                    UtilLearnFunctions.Do_Save(uc_Main, errH);
                    isSaved = true;
                }
            }

            if (isRequestDoEvents)
            {
                Application.DoEvents();
                isRequestDoEvents = false;
            }

            if (uc_Main.ChkAutoKifuNext.Checked)
            {
                //----------------------------------------
                // 棋譜がなくなるまで、繰り返すなら
                //----------------------------------------

                // 今読んだ棋譜を移す先（成功時）
                uc_Main.SeikoIdo();

                bool isEmptyKifu;
                uc_Main.Do_NextKifuSet(out isEmptyKifu, ref isRequest_ShowGohosyu, ref isRequest_ChangeKyokumenPng, errH);

                if (isEmptyKifu)
                {
                    //棋譜の自動読取の終了
                    out_isEndKifuread = true;
                    goto gt_EndMethod;
                }

                if (isRequest_ShowGohosyu)
                {
                    // 合法手一覧を更新
                    UtilLearnFunctions.FvParamRange_PP(uc_Main.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。
                    UtilLearningView.Aa_ShowGohosyu2(uc_Main.LearningData, uc_Main, errH);
                    isRequest_ShowGohosyu = false;
                }

                if (isRequest_ChangeKyokumenPng)
                {
                    uc_Main.LearningData.ChangeKyokumenPng(
                        uc_Main,
                        uc_Main.LearningData.GetMove(),
                        uc_Main.LearningData.PositionA
                        );
                    isRequest_ChangeKyokumenPng = false;
                }
            }
            else
            {
                //棋譜の自動読取の終了
                out_isEndKifuread = true;
                goto gt_EndMethod;
            }


        gt_EndMethod:
            ;
        }

    }
}