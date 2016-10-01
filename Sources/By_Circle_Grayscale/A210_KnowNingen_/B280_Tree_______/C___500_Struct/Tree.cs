using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
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
        void LogPv(string message, KwLogger logger);

        void Pv_RemoveLast(KwLogger logger);
        void Pv_ClearAll(KwLogger logger);
        void Pv_Append(Move tail, KwLogger logger);
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
        MoveEx MoveEx_OnEditCurrent(MoveEx node, Sky sky);
        Sky PositionA { get; }
        void SetPositionA(Sky positionA);
        void MoveEx_SetCurrent(MoveEx curNode);
    }
}
