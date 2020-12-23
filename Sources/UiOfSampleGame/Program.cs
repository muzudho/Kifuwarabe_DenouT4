using System;
using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Searching;
using Grayscale.Kifuwaragyoku.UseCases;
using Grayscale.Kifuwaragyoku.UseCases.Features;

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

            // 将棋指しオブジェクト
            var fv = new FeatureVector();
            IShogisasi shogisasi = new ShogisasiImpl(playing, fv);

            // 棋譜
            Earth earth1 = new EarthImpl();
            ISky positionA = UtilSkyCreator.New_Hirate();//日本の符号読取時;
            Tree kifu1 = new TreeImpl(positionA);

            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[Playing.SEARCHED_PV_LENGTH];
            MoveEx bestmoveNode = shogisasi.WA_Bestmove(
                ref searchedMaxDepth,
                ref searchedNodes,
                searchedPv,
                true,

                earth1,
                kifu1,// ツリーを伸ばしているぜ☆（＾～＾）
                positionA.GetKaisiPside(),
                positionA);

            Move move = bestmoveNode.Move;
            string sfenText = ConvMove.ToSfen(move);
            Logger.Trace("sfenText=" + sfenText + " move=" + Convert.ToString((int)move, 2));


            //bool isTimeoutShutdown_temp;
            //kifuWarabe.AtBody(out isTimeoutShutdown_temp);    // 将棋サーバーからのメッセージの受信や、
            // 思考は、ここで行っています。
        }
    }
}
