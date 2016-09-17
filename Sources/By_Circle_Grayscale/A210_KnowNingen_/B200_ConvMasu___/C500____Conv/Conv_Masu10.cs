using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using System.Text;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C500____Util;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

/*
     /// <summary>
    /// ------------------------------------------------------------------------------------------------------------------------
    /// 枡ハンドルの一覧。
    /// ------------------------------------------------------------------------------------------------------------------------
    /// 
    /// ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
    /// │72│63│54│45│36│27│18│ 9│ 0│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │73│64│55│46│37│28│19│10│ 1│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │74│65│56│47│38│29│20│11│ 2│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │75│66│57│48│39│30│21│12│ 3│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │76│67│58│49│40│31│22│13│ 4│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │77│68│59│50│41│32│23│14│ 5│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │78│69│60│51│42│33│24│15│ 6│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │79│70│61│52│43│34│25│16│ 7│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │80│71│62│53│44│35│26│17│ 8│
    /// └─┴─┴─┴─┴─┴─┴─┴─┴─┘
    /// 先手駒台：81～120
    /// 後手駒台：121～160
    /// 駒袋：161～200
    /// エラー：201
    /// の、計202。
    /// 
    /// 将棋盤上の枡のリスト。
    /// 
    /// ・Add、Removeといった、データ構造に縛られるメソッドは持たせません。
    ///   変わりに、Minus といった汎用的に操作できるメソッドを持たせます。
    /// 
    /// ・Clearメソッドは持たせません。インスタンスを作り直して親要素にセットし直してください。
    ///   空にすることができないオブジェクト（線分など）があることが理由です。
    /// </summary>

 */

namespace Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv
{
    public abstract class Conv_Masu10
    {

        /// <summary>
        /// 先手駒台は40マス、後手駒台は40マス、駒袋は40マスです。
        /// </summary>
        public const int KOMADAI_KOMABUKURO_SPACE_LENGTH = 40;

        public const int KOMADAI_LAST_SUJI = 4;
        public const int KOMADAI_LAST_DAN = 10;

        public const int SHOGIBAN_LAST_SUJI = 9;
        public const int SHOGIBAN_LAST_DAN = 9;

        public const int ERROR_DAN = -1;

        public const int ERROR_MASU_HANDLE = -1;


        public static SyElement GetKomasyuruiMasu(Sky src_Sky, Komasyurui14 syurui)
        {
            Fingers figKomas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                src_Sky.AssertFinger(figKoma);
                Busstop koma = src_Sky.BusstopIndexOf(figKoma);
                return Conv_Busstop.ToMasu(koma);
            }

            return Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <returns></returns>
        public static int ToMasuHandle_FromBanjoSujiDan( int suji, int dan)
        {
            if (1 <= suji && suji <= Conv_Masu10.SHOGIBAN_LAST_SUJI && 1 <= dan && dan <= Conv_Masu10.SHOGIBAN_LAST_DAN)
            {
                return (suji - 1) * Conv_Masu10.SHOGIBAN_LAST_DAN + (dan - 1);
            }
            return ERROR_MASU_HANDLE;
        }
        public static int ToMasuHandle_FromBangaiSujiDan(Okiba okiba, int suji, int dan)
        {
            int masuHandle = ERROR_MASU_HANDLE;

            switch (okiba)
            {
                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                case Okiba.KomaBukuro:
                    if (1 <= suji && suji <= Conv_Masu10.KOMADAI_LAST_SUJI && 1 <= dan && dan <= Conv_Masu10.KOMADAI_LAST_DAN)
                    {
                        masuHandle = (suji - 1) * Conv_Masu10.KOMADAI_LAST_DAN + (dan - 1);
                        masuHandle += Conv_SyElement.ToMasuNumber(Conv_Okiba.GetFirstMasuFromOkiba(okiba));
                    }
                    break;

                default:
                    break;
            }

            return masuHandle;
        }
        public static int ToMasuHandle_FromBangaiKomasyurui(Okiba okiba, Komasyurui14 ks14, Sky positionA)
        {
            int masuHandle = ERROR_MASU_HANDLE;

            switch (okiba)
            {
                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                case Okiba.KomaBukuro:
                    if (ks14 != Komasyurui14.H00_Null___)
                    {
                        SyElement masu = Conv_Masu10.GetKomasyuruiMasu(positionA, ks14);
                        masuHandle = Conv_SyElement.ToMasuNumber(masu);
                    }
                    break;

                default:
                    break;
            }

            return masuHandle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <returns></returns>
        public static SyElement ToMasu_FromBanjoSujiDan(int suji, int dan)
        {
            int masuHandle = Conv_Masu10.ToMasuHandle_FromBanjoSujiDan(suji,dan);

            SyElement masu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);//範囲外が指定されることもあります。

            if (Conv_MasuHandle.Yuko(masuHandle))
            {
                masu = Masu_Honshogi.Masus_All[masuHandle];
            }

            return masu;
        }
        public static SyElement ToMasu_FromBangaiSujiDan(Okiba okiba, int suji, int dan)
        {
            int masuHandle = Conv_Masu10.ToMasuHandle_FromBangaiSujiDan(okiba, suji, dan);

            if (Conv_MasuHandle.Yuko(masuHandle))
            {
                // マス
                return Masu_Honshogi.Masus_All[masuHandle];
            }
            else
            {
                // エラー
                return Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);//範囲外が指定されることもあります。
            }
        }
        public static SyElement ToMasu_FromBangaiKomasyurui(Okiba okiba, Komasyurui14 ks14, Sky positionA)
        {
            int masuHandle = Conv_Masu10.ToMasuHandle_FromBangaiKomasyurui(okiba, ks14, positionA);

            if (Conv_MasuHandle.Yuko(masuHandle))
            {
                // マス
                return Masu_Honshogi.Masus_All[masuHandle];
            }
            else
            {
                // エラー
                return Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);//範囲外が指定されることもあります。
            }
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// １マス上、のように指定して、マスを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="offsetSuji"></param>
        /// <param name="offsetDan"></param>
        /// <returns></returns>
        public static SyElement BanjoOffset( SyElement masu, Playerside pside, Hogaku muki)
        {
            int offsetSuji;
            int offsetDan;
            Conv_Muki.MukiToOffsetSujiDan(muki, pside, out offsetSuji, out offsetDan);

            int suji;
            int dan;
            Conv_MasuNum.ToSuji_FromBanjoMasu(masu, out suji);
            Conv_MasuNum.ToDan_FromBanjoMasu(masu, out dan);

            return Conv_Masu10.ToMasu_FromBanjoSujiDan(
                suji + offsetSuji,
                dan + offsetDan
                );
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// １マス上、のように指定して、マスを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="offsetSuji"></param>
        /// <param name="offsetDan"></param>
        /// <returns></returns>
        public static SyElement BanjoOffset(SyElement masu, int offsetSuji, int offsetDan)
        {
            int suji;
            int dan;
            Conv_MasuNum.ToSuji_FromBanjoMasu(masu, out suji);
            Conv_MasuNum.ToDan_FromBanjoMasu(masu, out dan);

            return Conv_Masu10.ToMasu_FromBanjoSujiDan(
                    suji + offsetSuji,
                    dan + offsetDan
                );
        }


        /// <summary>
        /// 後手からも、先手のような座標で指示できるように変換します。
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static SyElement BothSenteView(SyElement masu, Playerside pside)
        {
            SyElement result = masu;

            // 将棋盤上で後手なら、180°回転します。
            if (Okiba.ShogiBan == Conv_SyElement.ToOkiba(masu) && pside == Playerside.P2)
            {
                result = Masu_Honshogi.Masus_All[80 - Conv_SyElement.ToMasuNumber(masu)];
            }

            // 将棋盤で先手、または　駒台か　駒袋なら、指定されたマスにそのまま入れます。

            return result;
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// 相手陣に入っていれば真。
        /// ************************************************************************************************************************
        /// 
        ///         後手は 7,8,9 段。
        ///         先手は 1,2,3 段。
        /// </summary>
        /// <returns></returns>
        public static bool InBanjoAitejin(SyElement masu, Playerside pside)
        {
            int dan;
            Conv_MasuNum.ToDan_FromBanjoMasu(masu, out dan);

            return (Playerside.P2 == pside && 7 <= dan)
                || (Playerside.P1 == pside && dan <= 3);
        }


        #region 定数
        //------------------------------------------------------------
        /// <summary>
        /// 筋は 1～9 だけ有効です。
        /// </summary>
        public const int YUKO_SUJI_MIN = 1;
        public const int YUKO_SUJI_MAX = 9;

        /// <summary>
        /// 段は 1～9 だけ有効です。
        /// </summary>
        public const int YUKO_DAN_MIN = 1;
        public const int YUKO_DAN_MAX = 9;
        //------------------------------------------------------------

        public static bool YukoSuji(int suji)
        {
            return Conv_Masu10.YUKO_SUJI_MIN <= suji && suji <= Conv_Masu10.YUKO_SUJI_MAX;
        }

        public static bool YukoDan(int dan)
        {
            return Conv_Masu10.YUKO_DAN_MIN <= dan && dan <= Conv_Masu10.YUKO_DAN_MAX;
        }
        #endregion



        /// <summary>
        /// 盤上の位置を「２八」といった表記にして返します。
        /// 
        /// Conv_SyElement使用。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns></returns>
        public static string ToBanjoArabiaAndKanji_FromMasu(SyElement masu)
        {
            StringBuilder sb = new StringBuilder();

            int suji;
            int dan;
            Conv_MasuNum.ToSuji_FromBanjoMasu(masu, out suji);
            Conv_MasuNum.ToDan_FromBanjoMasu(masu, out dan);

            sb.Append(Conv_Int.ToArabiaSuji(suji));
            sb.Append(Conv_Int.ToKanSuji(dan));

            return sb.ToString();
        }




    }
}
