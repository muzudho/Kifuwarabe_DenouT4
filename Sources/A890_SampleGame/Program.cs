﻿using System;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A500ShogiEngine.B200Scoreing.C240Shogisasi;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C100Shogisasi;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe;

namespace P930_SampleGame
{
    class Program
    {
        /// <summary>
        /// プロファイラを使って、実行時速度を計測するためのもの。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ILogger errH = ErrorControllerReference.ProcessEngineDefault;


            // 将棋エンジン　きふわらべ
            KifuWarabeImpl kifuWarabe = new KifuWarabeImpl(new UsiFrameworkImpl());
            kifuWarabe.OnApplicationBegin();


            // 将棋指しオブジェクト
            Shogisasi shogisasi = new ShogisasiImpl(kifuWarabe);

            // 棋譜
            Earth earth1 = new EarthImpl();
            ISky positionA = UtilSkyCreator.New_Hirate();//日本の符号読取時;
            Tree kifu1 = new TreeImpl(positionA);

            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[KifuWarabeImpl.SEARCHED_PV_LENGTH];
            MoveEx bestmoveNode = shogisasi.WA_Bestmove(
                ref searchedMaxDepth,
                ref searchedNodes,
                searchedPv,
                true,

                earth1,
                kifu1,// ツリーを伸ばしているぜ☆（＾～＾）
                positionA.GetKaisiPside(),
                positionA,

                errH);

            Move move = bestmoveNode.Move;
            string sfenText = ConvMove.ToSfen(move);
            System.Console.WriteLine("sfenText=" + sfenText + " move=" + Convert.ToString((int)move, 2));


            //bool isTimeoutShutdown_temp;
            //kifuWarabe.AtBody(out isTimeoutShutdown_temp, errH);    // 将棋サーバーからのメッセージの受信や、
            // 思考は、ここで行っています。
            kifuWarabe.OnApplicationEnd();
        }
    }
}