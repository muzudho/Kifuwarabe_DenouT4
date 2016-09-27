using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B290_Komahaiyaku.C250____Word;
using Grayscale.A210_KnowNingen_.B300_KomahaiyaTr.C500____Table;
using Grayscale.A210_KnowNingen_.B390_KomahaiyaEx.C500____Util;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System;
using System.Diagnostics;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky
{
    public static class Util_Sky258A
    {
        /// <summary>
        /// 成ケース
        /// </summary>
        /// <returns></returns>
        public static Komasyurui14 ToNariCase(Move move)
        {
            return Util_Komasyurui14.NariCaseHandle[(int)Conv_Move.ToDstKomasyurui(move)];
        }

        /// <summary>
        /// 外字を利用した、デバッグ用の駒の名前１文字だぜ☆
        /// </summary>
        /// <returns></returns>
        public static char ToGaiji(Move move)
        {
            Komasyurui14 dstKs = Conv_Move.ToDstKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);

            return Util_Komasyurui14.ToGaiji(dstKs, pside);
        }

        public static void Assert_Honshogi(Sky src_Sky)
        {
            Debug.Assert(src_Sky.Count == 40, "siteiSky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            ////デバッグ
            //{
            //    StringBuilder sb = new StringBuilder();

            //    for (int i = 0; i < 40; i++)
            //    {
            //        sb.Append("駒" + i + ".種類=[" + ((RO_Star_KomaKs)siteiSky.StarlightIndexOf(i)).Syurui + "]\n");
            //    }

            //    MessageBox.Show(sb.ToString());
            //}


            // 王
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(0)) == Komasyurui14.H06_Gyoku__, "駒0.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(0)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(1)) == Komasyurui14.H06_Gyoku__, "駒1.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(1)) + "]");

            // 飛車
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(2)) == Komasyurui14.H07_Hisya__ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(2)) == Komasyurui14.H09_Ryu____, "駒2.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(2)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(3)) == Komasyurui14.H07_Hisya__ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(3)) == Komasyurui14.H09_Ryu____, "駒3.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(3)) + "]");

            // 角
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(4)) == Komasyurui14.H08_Kaku___ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(4)) == Komasyurui14.H10_Uma____, "駒4.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(4)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(5)) == Komasyurui14.H08_Kaku___ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(5)) == Komasyurui14.H10_Uma____, "駒5.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(5)) + "]");

            // 金
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(6)) == Komasyurui14.H05_Kin____, "駒6.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(6)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(7)) == Komasyurui14.H05_Kin____, "駒7.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(7)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(8)) == Komasyurui14.H05_Kin____, "駒8.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(8)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(9)) == Komasyurui14.H05_Kin____, "駒9.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(9)) + "]");

            // 銀
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(10)) == Komasyurui14.H04_Gin____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(10)) == Komasyurui14.H14_NariGin, "駒10.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(10)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(11)) == Komasyurui14.H04_Gin____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(11)) == Komasyurui14.H14_NariGin, "駒11.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(11)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(12)) == Komasyurui14.H04_Gin____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(12)) == Komasyurui14.H14_NariGin, "駒12.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(12)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(13)) == Komasyurui14.H04_Gin____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(13)) == Komasyurui14.H14_NariGin, "駒13.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(13)) + "]");

            // 桂
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(14)) == Komasyurui14.H03_Kei____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(14)) == Komasyurui14.H13_NariKei, "駒14.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(14)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(15)) == Komasyurui14.H03_Kei____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(15)) == Komasyurui14.H13_NariKei, "駒15.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(15)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(16)) == Komasyurui14.H03_Kei____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(16)) == Komasyurui14.H13_NariKei, "駒16.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(16)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(17)) == Komasyurui14.H03_Kei____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(17)) == Komasyurui14.H13_NariKei, "駒17.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(17)) + "]");

            // 香
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(18)) == Komasyurui14.H02_Kyo____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(18)) == Komasyurui14.H12_NariKyo, "駒18.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(18)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(19)) == Komasyurui14.H02_Kyo____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(19)) == Komasyurui14.H12_NariKyo, "駒19.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(19)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(20)) == Komasyurui14.H02_Kyo____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(20)) == Komasyurui14.H12_NariKyo, "駒20.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(20)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(21)) == Komasyurui14.H02_Kyo____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(21)) == Komasyurui14.H12_NariKyo, "駒21.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(21)) + "]");

            // 歩
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(22)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(22)) == Komasyurui14.H11_Tokin__, "駒22.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(22)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(23)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(23)) == Komasyurui14.H11_Tokin__, "駒23.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(23)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(24)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(24)) == Komasyurui14.H11_Tokin__, "駒24.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(24)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(25)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(25)) == Komasyurui14.H11_Tokin__, "駒25.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(25)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(26)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(26)) == Komasyurui14.H11_Tokin__, "駒26.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(26)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(27)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(27)) == Komasyurui14.H11_Tokin__, "駒27.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(27)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(28)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(28)) == Komasyurui14.H11_Tokin__, "駒28.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(28)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(29)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(29)) == Komasyurui14.H11_Tokin__, "駒29.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(29)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(30)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(30)) == Komasyurui14.H11_Tokin__, "駒30.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(30)) + "]");

            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(31)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(31)) == Komasyurui14.H11_Tokin__, "駒31.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(31)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(32)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(32)) == Komasyurui14.H11_Tokin__, "駒32.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(32)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(33)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(33)) == Komasyurui14.H11_Tokin__, "駒33.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(33)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(34)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(34)) == Komasyurui14.H11_Tokin__, "駒34.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(34)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(35)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(35)) == Komasyurui14.H11_Tokin__, "駒35.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(35)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(36)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(36)) == Komasyurui14.H11_Tokin__, "駒36.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(36)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(37)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(37)) == Komasyurui14.H11_Tokin__, "駒37.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(37)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(38)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(38)) == Komasyurui14.H11_Tokin__, "駒38.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(38)) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(39)) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(39)) == Komasyurui14.H11_Tokin__, "駒39.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(39)) + "]");



            for (int i = 0; i < 40; i++)
            {
                Busstop koma = src_Sky.BusstopIndexOf(0);
                Komahaiyaku185 haiyaku = Data_KomahaiyakuTransition.ToHaiyaku(Conv_Busstop.ToKomasyurui(koma), Conv_Busstop.ToMasu(koma), Conv_Busstop.ToPlayerside(koma));

                if (Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma))
                {
                    Debug.Assert(!Util_KomahaiyakuEx184.IsKomabukuro(haiyaku), "将棋盤の上に、配役：駒袋　があるのはおかしい。");
                }


                //if(
                //    haiyaku==Kh185.n164_歩打
                //    )
                //{
                //}
                //koma.Syurui
                //Debug.Assert((.Syurui == Ks14.H06_Oh, "駒0.種類=[" + ((RO_Star_Koma)siteiSky.StarlightIndexOf(0)).Syurui + "]");
                //sb.Append("駒" + i + ".種類=[" + ((RO_Star_KomaKs)siteiSky.StarlightIndexOf(i)).Syurui + "]\n");
            }


        }
        



        /// <summary>
        /// 無ければ追加、あれば上書き。
        /// </summary>
        /// <param name="hKoma"></param>
        /// <param name="masus"></param>
        public static void AddOverwrite(
            Maps_OneAndOne<Finger, SySet<SyElement>> komabetuMasus,
            Finger finger,
            SySet<SyElement> masus)
        {
            if ((int)finger<0)
            {
                throw new ApplicationException("fingerに負数が指定されましたが、間違いです(A)。 finger="+ finger);
            }
            else
            if (komabetuMasus.Items.ContainsKey(finger))
            {
                komabetuMasus.Items[finger].AddSupersets(masus);//追加します。
            }
            else
            {
                // 無かったので、新しく追加します。
                komabetuMasus.Items.Add(finger, masus);
            }
        }

        /// <summary>
        /// 指し手一覧を、駒毎に分けます。
        /// TODO: これ、SkyConstに移動できないか☆？
        /// </summary>
        /// <param name="hubNode">指し手一覧</param>
        /// <param name="logger"></param>
        /// <returns>駒毎の、全指し手</returns>
        public static Maps_OneAndMulti<Finger, Move> SplitSasite_ByStar(
            Sky positionA,
            List<Move> siblingMoves,
            KwLogger logger
            )
        {
            Maps_OneAndMulti<Finger, Move> enable_moveMap = new Maps_OneAndMulti<Finger, Move>();

            foreach (Move move in siblingMoves)
            {
                Finger figKoma = Util_Sky_FingersQuery.InMasuNow_New(
                    positionA,
                    move,
                    logger
                    ).ToFirst();
                if ((int)figKoma < 0)
                {
                    throw new ApplicationException("駒のハンドルが負数でしたが、間違いです(B)。figKoma=" + (int)figKoma +
                        " move=" + Convert.ToString((int)move,2)+
                        "\n Log=" + Conv_Move.ToLog(move));
                }

                enable_moveMap.Put_NewOrOverwrite(
                    figKoma,
                    move
                    );
            }

            return enable_moveMap;
        }

    }
}
