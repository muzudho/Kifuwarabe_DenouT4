using System;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class Util_IttesasuSuperRoutine
    {
        public static bool DoMove_Super1(
            Playerside psideA,
            ref IPosition positionA,//指定局面
            ref Move move,//TODO:取った駒があると、上書きされる
            string hint)
        {
            bool successful = true;
            bool log = false;
            //*
            if (log)
            {
                Logger.Trace($"進める前 {hint}\n{Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(psideA, positionA), positionA, move)}");
            }
            //*/

            // 動かす駒
            Fingers fingers = UtilSkyFingersQuery.InMasuNow_New(positionA, move);

            if (fingers.Count < 1)
            {
                string message = "Util_IttesasuSuperRoutine#DoMove_Super:指し手に該当する駒が無かったぜ☆（＾～＾） hint=" +
                    hint +
                    " move=" + ConvMove.ToLog(move);

                throw new Exception(message);
            }
            else
            {
                Util_IttesasuSuperRoutine.DoMove_Super2(
                        ref positionA,
                        ref move,

                        // フィンガー
                        fingers.ToFirst(),// マス

                        ConvMove.ToDstMasu(move),//移動先升
                        ConvMove.ToPromotion(move)//成るか。
                    );

                if (log)
                {
                    Logger.Trace($"進めた後 {hint}\n{Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(psideA, positionA), positionA, move)}");
                }
            }

            return successful;
        }

        //*
        /// <summary>
        /// 指したあとの、次の局面を作るだけ☆
        /// </summary>
        /// <param name="positionA"></param>
        /// <param name="figKoma"></param>
        /// <param name="dstMasu"></param>
        /// <param name="pside_genTeban"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static void DoMove_Super2(
            ref IPosition positionA,//指定局面
            ref Move moveA,
            Finger figKoma,//動かす駒
            SyElement dstMasu,//移動先マス
            bool toNaru//成るなら真
            )
        {
            // 移動先に相手の駒がないか、確認します。
            Finger tottaKomaFig = UtilSkyFingersQuery.InMasuNow_Old(positionA, dstMasu).ToFirst();

            if (tottaKomaFig != Fingers.Error_1)
            {
                // なにか駒を取ったら

                // 駒台の空いているマス１つ。
                SyElement akiMasu;
                if (positionA.GetKaisiPside(moveA) == Playerside.P1)
                {
                    akiMasu = UtilIttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, positionA);
                }
                else
                {
                    akiMasu = UtilIttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, positionA);
                }

                positionA.AssertFinger(tottaKomaFig);
                Busstop tottaKomaBus = positionA.BusstopIndexOf(tottaKomaFig);

                // 駒台の空いているマスへ移動☆
                positionA.PutOverwriteOrAdd_Busstop(tottaKomaFig,
                    Conv_Busstop.ToBusstop(positionA.GetKaisiPside(moveA), akiMasu, Conv_Busstop.ToKomasyurui(tottaKomaBus))
                    );

                if (Conv_Busstop.ToKomasyurui(tottaKomaBus) != Komasyurui14.H00_Null___)
                {
                    // 元のキーの、取った駒の種類だけを差替えます。
                    moveA = ConvMove.SetCaptured(
                        moveA,
                        Conv_Busstop.ToKomasyurui(tottaKomaBus)
                        );
                }
            }

            // 駒を１個動かします。
            {
                positionA.AssertFinger(figKoma);
                Komasyurui14 komaSyurui = Conv_Busstop.ToKomasyurui(positionA.BusstopIndexOf(figKoma));

                if (toNaru)
                {
                    komaSyurui = Util_Komasyurui14.ToNariCase(komaSyurui);
                }

                positionA.PutOverwriteOrAdd_Busstop(figKoma,
                    Conv_Busstop.ToBusstop(positionA.GetKaisiPside(moveA), dstMasu, komaSyurui)
                    );
            }

            // 動かしたあとに、先後を逆転させて、手目済カウントを増やします。
            positionA.IncreasePsideTemezumi();
        }
        //*/
    }
}
