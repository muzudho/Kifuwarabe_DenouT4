﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using System;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB
{
    public abstract class Util_IttesasuSuperRoutine
    {
        public static bool DoMove_Super1(
            ref Sky positionA,//指定局面
            ref Move move,//TODO:取った駒があると、上書きされる
            string hint,
            KwLogger logger
            )
        {
            bool successful = true;
            bool log = false;
            //*
            if (log)
            {
                logger.AppendLine("進める前 "+ hint);
                logger.Append(Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(positionA,logger), positionA, move));
                logger.Flush(LogTypes.Plain);
            }
            //*/

            // 動かす駒
            Fingers fingers = Util_Sky_FingersQuery.InMasuNow_New(positionA, move, logger);

            if (fingers.Count < 1)
            {
                string message = "Util_IttesasuSuperRoutine#DoMove_Super:指し手に該当する駒が無かったぜ☆（＾～＾） hint=" +
                    hint +
                    " move=" + Conv_Move.ToLog(move);

                throw new Exception(message);
            }
            else
            {
                Util_IttesasuSuperRoutine.DoMove_Super2(
                        ref positionA,
                        ref move,

                        // フィンガー
                        fingers.ToFirst(),// マス

                        Conv_Move.ToDstMasu(move),//移動先升
                        Conv_Move.ToPromotion(move),//成るか。
                        logger
                    );

                if (log)
                {
                    logger.AppendLine("進めた後 " + hint);
                    logger.Append(Conv_Shogiban.ToLog_Type2(Conv_Sky.ToShogiban(positionA, logger), positionA, move));
                    logger.Flush(LogTypes.Plain);
                }
            }

            return successful;
        }

        //*
        /// <summary>
        /// 指したあとの、次の局面を作るだけ☆
        /// </summary>
        /// <param name="position"></param>
        /// <param name="figKoma"></param>
        /// <param name="dstMasu"></param>
        /// <param name="pside_genTeban"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static void DoMove_Super2(
            ref Sky position,//指定局面
            ref Move move,
            Finger figKoma,//動かす駒
            SyElement dstMasu,//移動先マス
            bool toNaru,//成るなら真
            KwLogger errH
            )
        {
            // 移動先に相手の駒がないか、確認します。
            Finger tottaKomaFig = Util_Sky_FingersQuery.InMasuNow_Old(position, dstMasu).ToFirst();

            if (tottaKomaFig != Fingers.Error_1)
            {
                // なにか駒を取ったら

                // 駒台の空いているマス１つ。
                SyElement akiMasu;
                if (position.KaisiPside == Playerside.P1)
                {
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, position);
                }
                else
                {
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, position);
                }

                position.AssertFinger(tottaKomaFig);
                Busstop tottaKomaBus = position.BusstopIndexOf(tottaKomaFig);

                // 駒台の空いているマスへ移動☆
                position.PutOverwriteOrAdd_Busstop(tottaKomaFig, Conv_Busstop.ToBusstop(position.KaisiPside, akiMasu, Conv_Busstop.ToKomasyurui(tottaKomaBus)));

                if (Conv_Busstop.ToKomasyurui(tottaKomaBus) != Komasyurui14.H00_Null___)
                {
                    // 元のキーの、取った駒の種類だけを差替えます。
                    move = Conv_Move.SetCaptured(
                        move,
                        Conv_Busstop.ToKomasyurui(tottaKomaBus)
                        );
                }
            }

            // 駒を１個動かします。
            {
                position.AssertFinger(figKoma);
                Komasyurui14 komaSyurui = Conv_Busstop.ToKomasyurui(position.BusstopIndexOf(figKoma));

                if (toNaru)
                {
                    komaSyurui = Util_Komasyurui14.ToNariCase(komaSyurui);
                }

                position.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(position.KaisiPside, dstMasu, komaSyurui));
            }

            // 動かしたあとに、先後を逆転させて、手目済カウントを増やします。
            position.IncreasePsideTemezumi();
        }
        //*/
    }
}
