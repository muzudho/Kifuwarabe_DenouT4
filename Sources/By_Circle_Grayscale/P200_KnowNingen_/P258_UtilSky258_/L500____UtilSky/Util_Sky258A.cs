using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P035_Collection_.L500____Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P211_WordShogi__.L250____Masu;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P212_ConvPside__.L500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P226_Tree_______.L___500_Struct;
using Grayscale.P234_Komahaiyaku.L250____Word;
using Grayscale.P234_Komahaiyaku.L500____Util;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P250_KomahaiyaEx.L500____Util;
using System.Diagnostics;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;
using System;
using Grayscale.P236_KomahaiyaTr.L500____Table;

namespace Grayscale.P258_UtilSky258_.L500____UtilSky
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

        public static void Assert_Honshogi(SkyConst src_Sky)
        {
            Debug.Assert(src_Sky.Count == 40, "siteiSky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            ////デバッグ
            //{
            //    StringBuilder sb = new StringBuilder();

            //    for (int i = 0; i < 40; i++)
            //    {
            //        sb.Append("駒" + i + ".種類=[" + ((RO_Star_KomaKs)siteiSky.StarlightIndexOf(i).Now).Syurui + "]\n");
            //    }

            //    MessageBox.Show(sb.ToString());
            //}


            // 王
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(0).Now) == Komasyurui14.H06_Gyoku__, "駒0.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(0).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(1).Now) == Komasyurui14.H06_Gyoku__, "駒1.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(1).Now) + "]");

            // 飛車
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(2).Now) == Komasyurui14.H07_Hisya__ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(2).Now) == Komasyurui14.H09_Ryu____, "駒2.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(2).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(3).Now) == Komasyurui14.H07_Hisya__ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(3).Now) == Komasyurui14.H09_Ryu____, "駒3.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(3).Now) + "]");

            // 角
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(4).Now) == Komasyurui14.H08_Kaku___ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(4).Now) == Komasyurui14.H10_Uma____, "駒4.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(4).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(5).Now) == Komasyurui14.H08_Kaku___ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(5).Now) == Komasyurui14.H10_Uma____, "駒5.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(5).Now) + "]");

            // 金
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(6).Now) == Komasyurui14.H05_Kin____, "駒6.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(6).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(7).Now) == Komasyurui14.H05_Kin____, "駒7.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(7).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(8).Now) == Komasyurui14.H05_Kin____, "駒8.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(8).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(9).Now) == Komasyurui14.H05_Kin____, "駒9.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(9).Now) + "]");

            // 銀
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(10).Now) == Komasyurui14.H04_Gin____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(10).Now) == Komasyurui14.H14_NariGin, "駒10.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(10).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(11).Now) == Komasyurui14.H04_Gin____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(11).Now) == Komasyurui14.H14_NariGin, "駒11.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(11).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(12).Now) == Komasyurui14.H04_Gin____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(12).Now) == Komasyurui14.H14_NariGin, "駒12.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(12).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(13).Now) == Komasyurui14.H04_Gin____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(13).Now) == Komasyurui14.H14_NariGin, "駒13.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(13).Now) + "]");

            // 桂
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(14).Now) == Komasyurui14.H03_Kei____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(14).Now) == Komasyurui14.H13_NariKei, "駒14.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(14).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(15).Now) == Komasyurui14.H03_Kei____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(15).Now) == Komasyurui14.H13_NariKei, "駒15.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(15).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(16).Now) == Komasyurui14.H03_Kei____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(16).Now) == Komasyurui14.H13_NariKei, "駒16.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(16).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(17).Now) == Komasyurui14.H03_Kei____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(17).Now) == Komasyurui14.H13_NariKei, "駒17.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(17).Now) + "]");

            // 香
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(18).Now) == Komasyurui14.H02_Kyo____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(18).Now) == Komasyurui14.H12_NariKyo, "駒18.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(18).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(19).Now) == Komasyurui14.H02_Kyo____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(19).Now) == Komasyurui14.H12_NariKyo, "駒19.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(19).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(20).Now) == Komasyurui14.H02_Kyo____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(20).Now) == Komasyurui14.H12_NariKyo, "駒20.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(20).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(21).Now) == Komasyurui14.H02_Kyo____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(21).Now) == Komasyurui14.H12_NariKyo, "駒21.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(21).Now) + "]");

            // 歩
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(22).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(22).Now) == Komasyurui14.H11_Tokin__, "駒22.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(22).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(23).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(23).Now) == Komasyurui14.H11_Tokin__, "駒23.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(23).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(24).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(24).Now) == Komasyurui14.H11_Tokin__, "駒24.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(24).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(25).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(25).Now) == Komasyurui14.H11_Tokin__, "駒25.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(25).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(26).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(26).Now) == Komasyurui14.H11_Tokin__, "駒26.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(26).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(27).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(27).Now) == Komasyurui14.H11_Tokin__, "駒27.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(27).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(28).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(28).Now) == Komasyurui14.H11_Tokin__, "駒28.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(28).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(29).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(29).Now) == Komasyurui14.H11_Tokin__, "駒29.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(29).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(30).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(30).Now) == Komasyurui14.H11_Tokin__, "駒30.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(30).Now) + "]");

            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(31).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(31).Now) == Komasyurui14.H11_Tokin__, "駒31.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(31).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(32).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(32).Now) == Komasyurui14.H11_Tokin__, "駒32.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(32).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(33).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(33).Now) == Komasyurui14.H11_Tokin__, "駒33.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(33).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(34).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(34).Now) == Komasyurui14.H11_Tokin__, "駒34.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(34).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(35).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(35).Now) == Komasyurui14.H11_Tokin__, "駒35.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(35).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(36).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(36).Now) == Komasyurui14.H11_Tokin__, "駒36.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(36).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(37).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(37).Now) == Komasyurui14.H11_Tokin__, "駒37.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(37).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(38).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(38).Now) == Komasyurui14.H11_Tokin__, "駒38.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(38).Now) + "]");
            Debug.Assert(Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(39).Now) == Komasyurui14.H01_Fu_____ || Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(39).Now) == Komasyurui14.H11_Tokin__, "駒39.種類=[" + Conv_Busstop.ToKomasyurui(src_Sky.StarlightIndexOf(39).Now) + "]");



            for (int i = 0; i < 40; i++)
            {
                Busstop koma = src_Sky.StarlightIndexOf(0).Now;
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
                //Debug.Assert((.Syurui == Ks14.H06_Oh, "駒0.種類=[" + ((RO_Star_Koma)siteiSky.StarlightIndexOf(0).Now).Syurui + "]");
                //sb.Append("駒" + i + ".種類=[" + ((RO_Star_KomaKs)siteiSky.StarlightIndexOf(i).Now).Syurui + "]\n");
            }


        }

        //public static Playerside GetReverseTebanside(Playerside tebanside1)
        //{
        //    Playerside side2;
        //    switch (tebanside1)
        //    {
        //        case Playerside.P1: side2 = Playerside.P2; break;
        //        case Playerside.P2: side2 = Playerside.P1; break;
        //        case Playerside.Empty: side2 = Playerside.Empty; break;
        //        default: throw new Exception("未定義のプレイヤーサイド [" + tebanside1 + "]");
        //    }

        //    return side2;
        //}



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
        /// <param name="errH"></param>
        /// <returns>駒毎の、全指し手</returns>
        public static Maps_OneAndMulti<Finger, Move> SplitSasite_ByStar(
            SkyConst src_Sky,
            Node<Move, KyokumenWrapper> hubNode, KwErrorHandler errH)
        {
            Maps_OneAndMulti<Finger, Move> enable_moveMap = new Maps_OneAndMulti<Finger, Move>();


            hubNode.Foreach_ChildNodes((string key, Node<Move, KyokumenWrapper> nextNode, ref bool toBreak) =>
            {
                Finger figKoma = Util_Sky_FingersQuery.InMasuNow_New(
                    src_Sky,
                    nextNode.Key
                    ).ToFirst();
                if ((int)figKoma<0)
                {
                    throw new ApplicationException("駒のハンドルが負数でしたが、間違いです(B)。figKoma="+ (int)figKoma+
                        " nextNode.Key="+Convert.ToString((int)nextNode.Key,2)+
                        "\n Log="+Conv_Move.ToLog(nextNode.Key));
                }

                enable_moveMap.Put_NewOrOverwrite(
                    figKoma,
                    nextNode.Key
                    );
            });

            return enable_moveMap;
        }

    }
}
