﻿using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B820_KyokuParser.C___500_Parser
{
    public interface ParsedKyokumen
    {
        /// <summary>
        /// 初期局面の先後。
        /// </summary>
        Playerside FirstPside { get; set; }

        /// <summary>
        /// 棋譜ノード。
        /// </summary>
        Node KifuNode { get; set; }

        /// <summary>
        /// 持ち駒リスト。
        /// </summary>
        List<MotiItem> MotiList { get; set; }

        Sky Sky { get; set; }

    }
}
