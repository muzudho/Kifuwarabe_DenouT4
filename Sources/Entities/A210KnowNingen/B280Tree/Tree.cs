﻿using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;

namespace Grayscale.A210KnowNingen.B280Tree.C500Struct
{

    /// <summary>
    /// 記録係が利用します。
    /// </summary>
    /// <param name="temezumi">手目済</param>
    /// <param name="node">ノードのかたまりのまま。</param>
    /// <param name="toBreak"></param>
    public delegate void DELEGATE_Foreach2(int temezumi, Move move, ref bool toBreak);


    public interface Tree
    {
        void LogPv(string message, ILogger logger);

        void Pv_RemoveLast(ILogger logger);
        void Pv_ClearAll(ILogger logger);
        void Pv_Append(Move tail, ILogger logger);
        Move Pv_GetLatest();
        Move Pv_Get(int index);
        int Pv_Count();
        List<Move> Pv_ToList();
        bool Pv_IsRoot();

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        MoveEx MoveEx_Current { get; }
        /// <summary>
        /// 局面編集中
        /// </summary>
        /// <param name="sky"></param>
        /// <returns></returns>
        MoveEx MoveEx_OnEditCurrent(MoveEx node, ISky sky);
        ISky PositionA { get; }
        void SetPositionA(ISky positionA);
        void MoveEx_SetCurrent(MoveEx curNode);
    }
}
