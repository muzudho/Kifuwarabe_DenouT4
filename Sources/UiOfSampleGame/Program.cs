using System;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Grayscale.Kifuwaragyoku.UseCases;

namespace Grayscale.Kifuwaragyoku.CliOfSampleGame
{
    class Program
    {
        /// <summary>
        /// プロファイラを使って、実行時速度を計測するためのもの。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // 将棋エンジン　きふわらべ
            Playing playing = new Playing();

            // 棋譜
            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[Playing.SEARCHED_PV_LENGTH];
            MoveEx bestmoveNode = playing.WA_Bestmove(
                ref searchedMaxDepth,
                ref searchedNodes,
                searchedPv,
                true,
                playing.StartingPosition.GetKaisiPside(),
                playing.StartingPosition);

            Move move = bestmoveNode.Move;
            string sfenText = ConvMove.ToSfen(move);
            Logger.Trace("sfenText=" + sfenText + " move=" + Convert.ToString((int)move, 2));


            //bool isTimeoutShutdown_temp;
            //kifuWarabe.AtBody(out isTimeoutShutdown_temp);    // 将棋サーバーからのメッセージの受信や、
            // 思考は、ここで行っています。
        }
    }
}
