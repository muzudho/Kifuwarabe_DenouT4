﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C___500_struct__;

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi
{

    /// <summary>
    /// 将棋指し。
    /// </summary>
    public interface Shogisasi
    {
        /// <summary>
        /// 右脳。
        /// </summary>
        FeatureVector FeatureVector { get; set; }

        /// <summary>
        /// 対局開始のとき。
        /// </summary>
        void OnTaikyokuKaisi();

        /// <summary>
        /// 時間管理
        /// </summary>
        TimeManager TimeManager { get; set; }


        /// <summary>
        /// 指し手を決めます。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="kifu"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        KifuNode WA_Bestmove(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            bool isHonshogi,
            KifuTree kifu,
            KwErrorHandler errH
            );

    }

}