using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Diagnostics;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky
{
    /// <summary>
    /// 局面データを作成します。
    /// </summary>
    public abstract class Util_SkyCreator
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒を、平手の初期配置に並べます。
        /// ************************************************************************************************************************
        /// </summary>
        public static Sky New_Hirate()
        {
            Sky newSky = new SkyImpl();
            Finger figKoma;

            figKoma = Finger_Honshogi.SenteOh;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban59_５九), Komasyurui14.H06_Gyoku__));//先手王
            figKoma = (int)Finger_Honshogi.GoteOh;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban51_５一), Komasyurui14.H06_Gyoku__));//後手王

            figKoma = (int)Finger_Honshogi.Hi1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban28_２八), Komasyurui14.H07_Hisya__));//飛
            figKoma = (int)Finger_Honshogi.Hi2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban82_８二), Komasyurui14.H07_Hisya__));

            figKoma = (int)Finger_Honshogi.Kaku1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban88_８八), Komasyurui14.H08_Kaku___));//角
            figKoma = (int)Finger_Honshogi.Kaku2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban22_２二), Komasyurui14.H08_Kaku___));

            figKoma = (int)Finger_Honshogi.Kin1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban49_４九), Komasyurui14.H05_Kin____));//金
            figKoma = (int)Finger_Honshogi.Kin2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban69_６九), Komasyurui14.H05_Kin____));
            figKoma = (int)Finger_Honshogi.Kin3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban41_４一), Komasyurui14.H05_Kin____));
            figKoma = (int)Finger_Honshogi.Kin4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban61_６一), Komasyurui14.H05_Kin____));

            figKoma = (int)Finger_Honshogi.Gin1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban39_３九), Komasyurui14.H04_Gin____));//銀
            figKoma = (int)Finger_Honshogi.Gin2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban79_７九), Komasyurui14.H04_Gin____));
            figKoma = (int)Finger_Honshogi.Gin3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban31_３一), Komasyurui14.H04_Gin____));
            figKoma = (int)Finger_Honshogi.Gin4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban71_７一), Komasyurui14.H04_Gin____));

            figKoma = (int)Finger_Honshogi.Kei1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban29_２九), Komasyurui14.H03_Kei____));//桂
            figKoma = (int)Finger_Honshogi.Kei2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban89_８九), Komasyurui14.H03_Kei____));
            figKoma = (int)Finger_Honshogi.Kei3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban21_２一), Komasyurui14.H03_Kei____));
            figKoma = (int)Finger_Honshogi.Kei4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban81_８一), Komasyurui14.H03_Kei____));

            figKoma = (int)Finger_Honshogi.Kyo1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban19_１九), Komasyurui14.H02_Kyo____));//香
            figKoma = (int)Finger_Honshogi.Kyo2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban99_９九), Komasyurui14.H02_Kyo____));
            figKoma = (int)Finger_Honshogi.Kyo3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一), Komasyurui14.H02_Kyo____));
            figKoma = (int)Finger_Honshogi.Kyo4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban91_９一), Komasyurui14.H02_Kyo____));

            figKoma = (int)Finger_Honshogi.Fu1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban17_１七), Komasyurui14.H01_Fu_____));//歩
            figKoma = (int)Finger_Honshogi.Fu2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban27_２七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban37_３七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban47_４七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu5;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban57_５七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu6;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban67_６七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu7;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban77_７七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu8;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban87_８七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu9;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban97_９七), Komasyurui14.H01_Fu_____));

            figKoma = (int)Finger_Honshogi.Fu10;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban13_１三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu11;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban23_２三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu12;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban33_３三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu13;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban43_４三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu14;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban53_５三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu15;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban63_６三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu16;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban73_７三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu17;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban83_８三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu18;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban93_９三), Komasyurui14.H01_Fu_____));


            //LarabeLogger.GetInstance().WriteAddMemo(logTag, "平手局面にセットしたぜ☆");

            return newSky;
        }


        public static Sky New_Komabukuro()
        {
            Sky newSky = new SkyImpl();

            Finger finger = 0;

            newSky.PutOverwriteOrAdd_Busstop(finger, /*finger,*/ Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro01), Komasyurui14.H06_Gyoku__));// Kh185.n051_底奇王
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro02), Komasyurui14.H06_Gyoku__));// Kh185.n051_底奇王
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro03), Komasyurui14.H07_Hisya__));// Kh185.n061_飛
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro04), Komasyurui14.H07_Hisya__));// Kh185.n061_飛
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro05), Komasyurui14.H08_Kaku___));// Kh185.n072_奇角
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro06), Komasyurui14.H08_Kaku___));// Kh185.n072_奇角
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro07), Komasyurui14.H05_Kin____));// Kh185.n038_底偶金
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro08), Komasyurui14.H05_Kin____));// Kh185.n038_底偶金
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro09), Komasyurui14.H05_Kin____));// Kh185.n038_底偶金
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro10), Komasyurui14.H05_Kin____));// Kh185.n038_底偶金
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro11), Komasyurui14.H04_Gin____));// Kh185.n023_底奇銀
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro12), Komasyurui14.H04_Gin____));// Kh185.n023_底奇銀
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro13), Komasyurui14.H04_Gin____));// Kh185.n023_底奇銀
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro14), Komasyurui14.H04_Gin____));// Kh185.n023_底奇銀
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro15), Komasyurui14.H03_Kei____));// Kh185.n007_金桂
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro16), Komasyurui14.H03_Kei____));// Kh185.n007_金桂
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro17), Komasyurui14.H03_Kei____));// Kh185.n007_金桂
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro18), Komasyurui14.H03_Kei____));// Kh185.n007_金桂
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro19), Komasyurui14.H02_Kyo____));// Kh185.n002_香
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro20), Komasyurui14.H02_Kyo____));// Kh185.n002_香
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro21), Komasyurui14.H02_Kyo____));// Kh185.n002_香
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro22), Komasyurui14.H02_Kyo____));// Kh185.n002_香
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro23), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro24), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro25), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro26), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro27), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro28), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro29), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro30), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro31), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro32), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro33), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro34), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro35), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro36), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro37), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro38), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro39), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newSky.PutOverwriteOrAdd_Busstop(finger, Conv_Busstop.ToBusstop(Playerside.P2,Masu_Honshogi.Query_Basho( Masu_Honshogi.nfukuro40), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            // 以上、全40駒。
            Debug.Assert(newSky.Busstops.Count == 40);

            return newSky;
        }

    }
}
