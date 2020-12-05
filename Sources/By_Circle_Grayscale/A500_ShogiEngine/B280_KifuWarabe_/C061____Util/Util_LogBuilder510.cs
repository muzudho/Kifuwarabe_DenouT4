#if DEBUG
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B410Collection.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A150LogKyokuPng.B100KyokumenPng.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B460KyokumMoves.C250Log;
using Grayscale.A210KnowNingen.B460KyokumMoves.C500Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C061____Util
{
    public abstract class Util_LogBuilder510
    {

        /// <summary>
        /// 盤１個分のログ。
        /// </summary>
        public static void Build_LogBoard(
            MoveEx node_forLog,
            string nodePath,
            MoveEx niniNode,//任意のノード
            Tree kifu1,
            KyokumenPngEnvironment reportEnvironment,
            KaisetuBoards logF_kiki,
            KwLogger errH
            )
        {
            //
            // HTMLﾛｸﾞ
            //
            if (logF_kiki.boards.Count < 30)//出力件数制限
            {
                KaisetuBoard logBrd_move1 = new KaisetuBoard();

                List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;
                Playerside pside = kifu1.PositionA.GetKaisiPside();//.KaisiPside;
                Util_KyokumenMoves.LA_Split_KomaBETUSusumeruMasus(
                    2,
                    //node_forLog,
                    out komaBETUSusumeruMasus,
                    true,//本将棋
                    kifu1.PositionA,//現在の局面
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

                logBrd_move1.YomikaisiTemezumi = kifu1.PositionA.Temezumi;//読み開始手目済み    // int.MinValue;
                logBrd_move1.Temezumi = int.MinValue;
                logBrd_move1.Score = (int)niniNode.Score;

                logF_kiki.boards.Add(logBrd_move1);
            }
        }


    }
}
#endif
