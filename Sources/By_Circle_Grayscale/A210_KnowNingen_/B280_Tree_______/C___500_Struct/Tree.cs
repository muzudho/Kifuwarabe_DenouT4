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
        void LogPvList(Tree kifu1, KwLogger logger);

        void RemoveLastPv(KwLogger logger);
        void ClearAllPv(KwLogger logger);
        void AppendPv(Move tail, KwLogger logger);
        Move GetLatestPv();
        Move GetPv(int index);
        int CountPv();
        List<Move> ToPvList();
        bool IsRoot();

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        MoveNode CurrentNode { get; }
        void RemoveCurrentChildren( KwLogger logger);
        void SetCurrentSetAndAdd(Move move, MoveNode newNode, KwLogger logger);
        /// <summary>
        /// 棋譜を空っぽにします。
        /// 
        /// ルートは残します。
        /// </summary>
        MoveNode OnDoCurrentMove(MoveNode node, Sky sky);
        /// <summary>
        /// 局面編集中
        /// </summary>
        /// <param name="sky"></param>
        /// <returns></returns>
        MoveNode OnEditCurrentMove(MoveNode node, Sky sky);
        Sky PositionA { get; }
        void SetPositionA(Sky positionA);
        void SetCurrentNode(MoveNode curNode);
    }
}
