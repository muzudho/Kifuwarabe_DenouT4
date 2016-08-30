﻿#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A150_LogKyokuPng.B100_KyokumenPng.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C250____Log;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C500____Util;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C061____Util
{
    public abstract class Util_LogBuilder510
    {

        /// <summary>
        /// 盤１個分のログ。
        /// </summary>
        public static void Build_LogBoard(
            Node<Move, KyokumenWrapper> node_forLog,
            string nodePath,
            KifuNode niniNode,//任意のノード
            //KifuTree kifu_forAssert,
            KyokumenPngEnvironment reportEnvironment,
            KaisetuBoards logF_kiki,
            KwErrorHandler errH
            )
        {
            //
            // HTMLﾛｸﾞ
            //
            if (logF_kiki.boards.Count < 30)//出力件数制限
            {
                KaisetuBoard logBrd_move1 = new KaisetuBoard();

                List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;
                Playerside pside = niniNode.Value.KyokumenConst.KaisiPside;
                Util_KyokumenMoves.LA_Split_KomaBETUSusumeruMasus(
                    2,
                    //node_forLog,
                    out komaBETUSusumeruMasus,
                    true,//本将棋
                    niniNode.Value.KyokumenConst,//現在の局面
                    pside,
                    false
//#if DEBUG
                    ,
                    new MmLogGenjoImpl(
                        0,//読み開始手目済み
                        logBrd_move1,
                        0,//現在の手済み
                        niniNode.Key,
                        errH
                    )
//#endif
                );

                logBrd_move1.Move = niniNode.Key;

                logBrd_move1.YomikaisiTemezumi = niniNode.Value.KyokumenConst.Temezumi;//読み開始手目済み    // int.MinValue;
                logBrd_move1.Temezumi = int.MinValue;
                logBrd_move1.Score = (int)niniNode.Score;

                logF_kiki.boards.Add(logBrd_move1);
            }
        }


    }
}
#endif