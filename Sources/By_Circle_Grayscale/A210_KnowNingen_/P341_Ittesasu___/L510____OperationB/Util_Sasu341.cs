using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P212_ConvPside__.L500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using Grayscale.P341_Ittesasu___.L500____UtilA;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;

namespace Grayscale.P341_Ittesasu___.L510____OperationB
{
    public abstract class Util_Sasu341
    {

        /// <summary>
        /// 指したあとの、次の局面を作るだけ☆
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="finger"></param>
        /// <param name="masu"></param>
        /// <param name="pside_genTeban"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static SkyConst Sasu(
            SkyConst src_Sky,//指定局面
            Finger finger,//動かす駒
            SyElement masu,//移動先マス
            bool toNaru,//成るなら真
            KwErrorHandler errH
            )
        {
            SkyBuffer sky_buf = new SkyBuffer(src_Sky); // 現局面を元に、新規局面を書き換えます。
            sky_buf.SetKaisiPside(Conv_Playerside.Reverse(src_Sky.KaisiPside));// 開始先後を逆転させます。
            sky_buf.SetTemezumi(sky_buf.Temezumi+1);// 1手進めます。
            SkyConst src_Sky2 = SkyConst.NewInstance(sky_buf,
                -1//sky_bufでもう変えてあるので、そのまま。
                );

            // 移動先に相手の駒がないか、確認します。
            Finger tottaKoma = Util_Sky_FingersQuery.InMasuNow_Old(src_Sky2, masu).ToFirst();

            if (tottaKoma != Fingers.Error_1)
            {
                // なにか駒を取ったら
                SyElement akiMasu;

                if (src_Sky.KaisiPside == Playerside.P1)
                {
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, src_Sky2);
                }
                else
                {
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, src_Sky2);
                }

                sky_buf.AssertFinger(tottaKoma);
                Busstop koma = sky_buf.BusstopIndexOf(tottaKoma);

                    // FIXME:配役あってるか？
                sky_buf.PutOverwriteOrAdd_Busstop(tottaKoma, Conv_Busstop.ToBusstop(src_Sky.KaisiPside, akiMasu, Conv_Busstop.ToKomasyurui( koma)));
            }

            // 駒を１個動かします。
            // FIXME: 取った駒はどうなっている？
            {
                sky_buf.AssertFinger(finger);
                Busstop koma = sky_buf.BusstopIndexOf(finger);
                Komasyurui14 komaSyurui = Conv_Busstop.ToKomasyurui( koma);

                if (toNaru)
                {
                    komaSyurui = Util_Komasyurui14.ToNariCase(komaSyurui);
                }

                sky_buf.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(src_Sky.KaisiPside, masu, komaSyurui));
            }

            return SkyConst.NewInstance( sky_buf,
                -1//sky_bufでもう進めてあるので、そのまま。
                );
        }

    }
}
