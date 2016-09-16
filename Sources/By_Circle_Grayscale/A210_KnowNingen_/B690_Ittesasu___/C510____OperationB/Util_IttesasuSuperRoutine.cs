using Grayscale.A060_Application.B110_Log________.C___500_Struct;
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

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB
{
    public abstract class Util_IttesasuSuperRoutine
    {
        //*
        /// <summary>
        /// 指したあとの、次の局面を作るだけ☆
        /// </summary>
        /// <param name="position"></param>
        /// <param name="finger"></param>
        /// <param name="dstMasu"></param>
        /// <param name="pside_genTeban"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static void DoMove_Super(
            ref Sky position,//指定局面
            Finger finger,//動かす駒
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
            }

            // 駒を１個動かします。
            {
                position.AssertFinger(finger);
                Komasyurui14 komaSyurui = Conv_Busstop.ToKomasyurui(position.BusstopIndexOf(finger));

                if (toNaru)
                {
                    komaSyurui = Util_Komasyurui14.ToNariCase(komaSyurui);
                }

                position.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(position.KaisiPside, dstMasu, komaSyurui));
            }

            // 動かしたあとに、先後を逆転させて、手目済カウントを増やします。
            position.IncreasePsideTemezumi();
        }
        //*/

        /*
        /// <summary>
        /// 指したあとの、次の局面を作るだけ☆
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="finger"></param>
        /// <param name="dstMasu"></param>
        /// <param name="pside_genTeban"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static Sky DoMove_Super(
            Sky src_Sky,//指定局面
            Finger finger,//動かす駒
            SyElement dstMasu,//移動先マス
            bool toNaru,//成るなら真
            KwLogger errH
            )
        {
            Sky newSky = new SkyImpl(src_Sky); // 現局面を元に、新規局面を書き換えます。
            newSky.SetKaisiPside(Conv_Playerside.Reverse(src_Sky.KaisiPside));// 開始先後を逆転させます。
            newSky.SetTemezumi(newSky.Temezumi+1);// 1手進めます。

            // 移動先に相手の駒がないか、確認します。
            Finger tottaKoma = Util_Sky_FingersQuery.InMasuNow_Old(newSky, dstMasu).ToFirst();

            if (tottaKoma != Fingers.Error_1)
            {
                // なにか駒を取ったら

                // 駒台の空いているマス１つ。
                SyElement akiMasu;
                if (src_Sky.KaisiPside == Playerside.P1)
                {
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, newSky);
                }
                else
                {
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, newSky);
                }

                newSky.AssertFinger(tottaKoma);
                Busstop koma = newSky.BusstopIndexOf(tottaKoma);

                // 駒台の空いているマスへ移動☆
                newSky.PutOverwriteOrAdd_Busstop(tottaKoma, Conv_Busstop.ToBusstop(src_Sky.KaisiPside, akiMasu, Conv_Busstop.ToKomasyurui( koma)));
            }

            // 駒を１個動かします。
            {
                newSky.AssertFinger(finger);
                Komasyurui14 komaSyurui = Conv_Busstop.ToKomasyurui(newSky.BusstopIndexOf(finger));

                if (toNaru)
                {
                    komaSyurui = Util_Komasyurui14.ToNariCase(komaSyurui);
                }

                newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(src_Sky.KaisiPside, dstMasu, komaSyurui));
            }

            return newSky;
        }
        //*/
    }
}
