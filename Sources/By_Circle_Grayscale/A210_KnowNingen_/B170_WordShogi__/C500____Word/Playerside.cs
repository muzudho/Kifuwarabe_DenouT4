﻿using System;

namespace Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word
{


    /// <summary>
    /// ************************************************************************************************************************
    /// 先後です。
    /// ************************************************************************************************************************
    /// 
    /// ・黒手（先手）
    /// ・白手（後手）
    /// ・どちらも持っていない
    /// 
    /// の３つです。
    /// 
    /// これをビットフィールドで表現します。
    /// 
    /// [0]００００　どちらも持っていない
    /// [1]０００１　黒手（先手）
    /// [2]００１０　白手（後手）
    /// 
    /// </summary>
    [Flags]// Enum型をビット・フィールド化
    public enum Playerside
    {


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// なんの働きもしない値(*1)として。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Empty = 0,

        /// <summary>
        /// 黒手（先手）
        /// </summary>
        P1 = 1,

        /// <summary>
        /// 白手（後手）
        /// </summary>
        P2 = 2,
    }
}
