﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.P324_KifuTree___.C___250_Struct;
using Grayscale.P542_Scoreing___.L___250_Args;
using Grayscale.P551_Tansaku____.L___500_Tansaku;
using Grayscale.P554_TansaFukasa.C___500_Struct;

namespace Grayscale.P553_TansakuHaba.C500____Struct
{
    public class Tansaku_HabaYusen_Routine : Tansaku_Routine
    {


        
        /// <summary>
        /// 読む。
        /// 
        /// 棋譜ツリーを作成します。
        /// </summary>
        /// <param name="kifu">この棋譜ツリーの現局面に、次局面をぶら下げて行きます。</param>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public void WAA_Yomu_Start(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            KifuTree kifu,
            bool isHonshogi,
            Mode_Tansaku mode_Tansaku,
            float alphabeta_otherBranchDecidedValue,
            EvaluationArgs args,
            KwErrorHandler log
            )
        {
        }

    }
}
