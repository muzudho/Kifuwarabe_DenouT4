using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Diagnostics;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter
{
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
    public abstract class Conv_Masu
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


        public static SyElement ToMasu(int masuHandle)
        {
            SyElement masu;

            if (
                !Conv_Masu.Yuko(masuHandle)
            )
            {
                masu = Masu_Honshogi.Query_Basho( Masu_Honshogi.nError);
            }
            else
            {
                masu = Masu_Honshogi.Masus_All[masuHandle];
            }

            return masu;
        }


        #region 範囲妥当性チェック

        public static bool Yuko(int masuHandle)
        {
            return 0 <= masuHandle && masuHandle <= 201;
        }

        #endregion

        public static bool OnAll(int masuHandle)
        {
            return Masu_Honshogi.nban11_１一 <= masuHandle && masuHandle <= Masu_Honshogi.nError;
        }

        public static bool OnShogiban(int masuHandle)
        {
            return Masu_Honshogi.nban11_１一 <= masuHandle && masuHandle <= Masu_Honshogi.nban99_９九;
        }

        /// <summary>
        /// 駒台の上なら真。
        /// </summary>
        /// <param name="masuHandle"></param>
        /// <returns></returns>
        public static bool OnKomadai(int masuHandle)
        {
            return Masu_Honshogi.nsen01 <= masuHandle && masuHandle <= Masu_Honshogi.ngo40;
        }

        public static bool OnSenteKomadai(int masuHandle)
        {
            return Masu_Honshogi.nsen01 <= masuHandle && masuHandle <= Masu_Honshogi.nsen40;
        }

        public static bool OnGoteKomadai(int masuHandle)
        {
            return Masu_Honshogi.ngo01 <= masuHandle && masuHandle <= Masu_Honshogi.ngo40;
        }

        public static bool OnKomabukuro(int masuHandle)
        {
            Debug.Assert(Masu_Honshogi.nfukuro01 == 161, "fukuro01=[" + Masu_Honshogi.nfukuro01.ToString() + "]");

            return Masu_Honshogi.nfukuro01 <= masuHandle && masuHandle <= Masu_Honshogi.nfukuro40;
        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <returns></returns>
        public static int ToMasuHandle_FromBanjoSujiDan(int suji, int dan)
        {
            if (1 <= suji && suji <= Conv_Masu.SHOGIBAN_LAST_SUJI && 1 <= dan && dan <= Conv_Masu.SHOGIBAN_LAST_DAN)
            {
                return (suji - 1) * Conv_Masu.SHOGIBAN_LAST_DAN + (dan - 1);
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
                    if (1 <= suji && suji <= Conv_Masu.KOMADAI_LAST_SUJI && 1 <= dan && dan <= Conv_Masu.KOMADAI_LAST_DAN)
                    {
                        masuHandle = (suji - 1) * Conv_Masu.KOMADAI_LAST_DAN + (dan - 1);
                        masuHandle += Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(okiba));
                    }
                    break;

                default:
                    break;
            }

            return masuHandle;
        }
        public static int ToMasuHandle_FromKomadaiKomasyurui_First(Playerside pside, Komasyurui14 ks14, Sky positionA)
        {
            Fingers figKomas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                positionA.AssertFinger(figKoma);
                Busstop koma = positionA.BusstopIndexOf(figKoma);

                if (Conv_Busstop.ToKomadai(koma) && pside == Conv_Busstop.ToPlayerside(koma))
                {
                    SyElement masu = Conv_Busstop.ToMasu(koma);
                    return Conv_Masu.ToMasuHandle(masu);
                }
            }

            return Masu_Honshogi.nError;

            /*
            switch (okiba)
            {
                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                case Okiba.KomaBukuro:
                    if (ks14 != Komasyurui14.H00_Null___)
                    {
                        SyElement masu = Conv_Masu.ToMasu_FromDokodemoKomasyurui(ks14, positionA);
                        return Conv_Masu.ToMasuHandle(masu);
                    }
                    break;
                default:
                    break;
            }
            return ERROR_MASU_HANDLE;
            */
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
            int masuHandle = Conv_Masu.ToMasuHandle_FromBanjoSujiDan(suji, dan);

            SyElement masu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);//範囲外が指定されることもあります。

            if (Conv_Masu.Yuko(masuHandle))
            {
                masu = Masu_Honshogi.Masus_All[masuHandle];
            }

            return masu;
        }
        public static SyElement ToMasu_FromBangaiSujiDan(Okiba okiba, int suji, int dan)
        {
            int masuHandle = Conv_Masu.ToMasuHandle_FromBangaiSujiDan(okiba, suji, dan);

            if (Conv_Masu.Yuko(masuHandle))
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
        public static SyElement ToMasu_FromDokodemoKomasyurui(Komasyurui14 syurui, Sky positionA)
        {
            Fingers figKomas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                positionA.AssertFinger(figKoma);
                Busstop koma = positionA.BusstopIndexOf(figKoma);
                return Conv_Busstop.ToMasu(koma);
            }

            return Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);
        }
        public static SyElement ToMasu_FromKomadaiKomasyurui(Playerside pside, Komasyurui14 ks14, Sky positionA)
        {
            int masuHandle = Conv_Masu.ToMasuHandle_FromKomadaiKomasyurui_First(pside, ks14, positionA);

            if (Conv_Masu.Yuko(masuHandle))
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
        public static SyElement BanjoOffset(SyElement masu, Playerside pside, Hogaku muki)
        {
            int offsetSuji;
            int offsetDan;
            Conv_Muki.MukiToOffsetSujiDan(muki, pside, out offsetSuji, out offsetDan);

            int suji;
            int dan;
            Conv_Masu.ToSuji_FromBanjoMasu(masu, out suji);
            Conv_Masu.ToDan_FromBanjoMasu(masu, out dan);

            return Conv_Masu.ToMasu_FromBanjoSujiDan(
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
            Conv_Masu.ToSuji_FromBanjoMasu(masu, out suji);
            Conv_Masu.ToDan_FromBanjoMasu(masu, out dan);

            return Conv_Masu.ToMasu_FromBanjoSujiDan(
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
            if (Okiba.ShogiBan == Conv_Masu.ToOkiba(masu) && pside == Playerside.P2)
            {
                result = Masu_Honshogi.Masus_All[80 - Conv_Masu.ToMasuHandle(masu)];
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
            Conv_Masu.ToDan_FromBanjoMasu(masu, out dan);

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
            return Conv_Masu.YUKO_SUJI_MIN <= suji && suji <= Conv_Masu.YUKO_SUJI_MAX;
        }

        public static bool YukoDan(int dan)
        {
            return Conv_Masu.YUKO_DAN_MIN <= dan && dan <= Conv_Masu.YUKO_DAN_MAX;
        }
        #endregion



        /// <summary>
        /// 盤上の位置を「２八」といった表記にして返します。
        /// 
        /// Conv_Masu使用。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns></returns>
        public static string ToBanjoArabiaAndKanji_FromMasu(SyElement masu)
        {
            StringBuilder sb = new StringBuilder();

            int suji;
            int dan;
            Conv_Masu.ToSuji_FromBanjoMasu(masu, out suji);
            Conv_Masu.ToDan_FromBanjoMasu(masu, out dan);

            sb.Append(Conv_Int.ToArabiaSuji(suji));
            sb.Append(Conv_Int.ToKanSuji(dan));

            return sb.ToString();
        }










        #region 整数変換(基礎)

        /// <summary>
        /// 将棋盤、駒台に筋があります。番号は1～。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool ToSuji_FromBanjoMasu(SyElement masu, out int result)
        {
            int masuNumber = Conv_Masu.ToMasuHandle(masu);
            return Conv_Masu.ToSuji_FromBanjoMasu(masuNumber, out result);
        }
        /// <summary>
        /// TODO: 廃止予定
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ToSuji_FromBangaiMasu(SyElement masu, out int result)
        {
            int masuNumber = Conv_Masu.ToMasuHandle(masu);
            return Conv_Masu.ToSuji_FromBangaiMasu(masuNumber, out result);
        }
        /// <summary>
        /// TODO: 今後こちらに置き換えていく予定。まだ使えない。
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ToPiece_FromBangaiMasu(SyElement masu, out Pieces result)
        {
            int masuNumber = Conv_Masu.ToMasuHandle(masu);
            return Conv_Masu.ToPiece_FromBangaiMasu(masuNumber, out result);
        }

        public static int FirstMasu_Shogiban { get { return Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.ShogiBan)); } }
        public static int FirstMasu_SenteKomadai { get { return Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.Sente_Komadai)); } }
        public static int FirstMasu_GoteKomadai { get { return Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.Gote_Komadai)); } }
        public static int FirstMasu_Komabukuro { get { return Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.KomaBukuro)); } }

        /// <summary>
        /// 将棋盤、駒台に筋があります。番号は1～。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool ToSuji_FromBanjoMasu(int masuNumber, out int result)
        {
            result = (masuNumber - Conv_Masu.FirstMasu_Shogiban) / 9 + 1;
            return true;
        }
        /// <summary>
        /// TODO: 廃止予定。
        /// </summary>
        /// <param name="masuNumber"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ToSuji_FromBangaiMasu(int masuNumber, out int result)
        {
            bool successful = true;

            Okiba okiba = Conv_Masu.ToOkiba(masuNumber);

            switch (okiba)
            {
                /*case Okiba.ShogiBan: result = (masuNumber - Util_MasuNum.FirstMasu_Shogiban) / 9 + 1; break;*/
                case Okiba.Sente_Komadai: result = (masuNumber - Conv_Masu.FirstMasu_SenteKomadai) / 10 + 1; break;
                case Okiba.Gote_Komadai: result = (masuNumber - Conv_Masu.FirstMasu_GoteKomadai) / 10 + 1; break;
                case Okiba.KomaBukuro: result = (masuNumber - Conv_Masu.FirstMasu_Komabukuro) / 10 + 1; break;
                default:
                    // エラー
                    result = -1;
                    successful = false;
                    goto gt_EndMethod;
            }

            gt_EndMethod:
            return successful;
        }
        /// <summary>
        /// TODO: これに置き換え予定。
        /// </summary>
        /// <param name="masuNumber"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ToPiece_FromBangaiMasu(int masuNumber, out Pieces result)
        {
            bool successful = true;

            Okiba okiba = Conv_Masu.ToOkiba(masuNumber);

            switch (okiba)
            {
                // TODO: まだ使えない☆
                case Okiba.Sente_Komadai: result = (Pieces)(masuNumber - Conv_Masu.FirstMasu_SenteKomadai); break;
                case Okiba.Gote_Komadai: result = (Pieces)(masuNumber - Conv_Masu.FirstMasu_GoteKomadai); break;
                case Okiba.KomaBukuro: result = (Pieces)(masuNumber - Conv_Masu.FirstMasu_Komabukuro); break;
                default:
                    // エラー
                    result = Pieces.None;
                    successful = false;
                    goto gt_EndMethod;
            }

            gt_EndMethod:
            return successful;
        }

        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool ToDan_FromBanjoMasu(SyElement masu, out int result)
        {
            int masuNumber = Conv_Masu.ToMasuHandle(masu);
            return Conv_Masu.ToDan_FromBanjoMasu(masuNumber, out result);
        }
        public static bool ToDan_FromBangaiMasu(SyElement masu, out int result)
        {
            int masuNumber = Conv_Masu.ToMasuHandle(masu);
            return Conv_Masu.ToDan_FromBangaiMasu(masuNumber, out result);
        }

        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool ToDan_FromBanjoMasu(int masuNumber, out int result)
        {
            result = (masuNumber - Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.ShogiBan))) % 9 + 1;
            return true;
        }
        public static bool ToDan_FromBangaiMasu(int masuNumber, out int result)
        {
            bool successful = true;

            Okiba okiba = Conv_Masu.ToOkiba(masuNumber);

            switch (okiba)
            {
                /*
                case Okiba.ShogiBan:
                    result = (masuNumber - Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.ShogiBan))) % 9 + 1;
                    break;
                */
                case Okiba.Sente_Komadai:
                    result = (masuNumber - Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.Sente_Komadai))) % 10 + 1;
                    break;

                case Okiba.Gote_Komadai:
                    result = (masuNumber - Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.Gote_Komadai))) % 10 + 1;
                    break;

                case Okiba.KomaBukuro:
                    result = (masuNumber - Conv_Masu.ToMasuHandle(Conv_Okiba.GetFirstMasuFromOkiba(Okiba.KomaBukuro))) % 10 + 1;
                    break;

                default:
                    // エラー
                    result = -1;
                    successful = false;
                    goto gt_EndMethod;
            }

            gt_EndMethod:
            return successful;
        }

        #endregion






        public static int ToMasuHandle(SyElement syElm)
        {
            if (syElm is New_Basho)
            {
                return ((New_Basho)syElm).MasuNumber;
            }

            return Masu_Honshogi.nError;
        }

        public static Okiba ToOkiba(SyElement masu)
        {
            int masuHandle = Conv_Masu.ToMasuHandle(masu);
            return Conv_Masu.ToOkiba(masuHandle);
        }

        public static Okiba ToOkiba(int masuNumber)
        {
            Okiba result;

            if ((int)Masu_Honshogi.nban11_１一 <= masuNumber && masuNumber <= (int)Masu_Honshogi.nban99_９九)
            {
                // 将棋盤
                result = Okiba.ShogiBan;
            }
            else if ((int)Masu_Honshogi.nsen01 <= masuNumber && masuNumber <= (int)Masu_Honshogi.nsen40)
            {
                // 先手駒台
                result = Okiba.Sente_Komadai;
            }
            else if ((int)Masu_Honshogi.ngo01 <= masuNumber && masuNumber <= (int)Masu_Honshogi.ngo40)
            {
                // 後手駒台
                result = Okiba.Gote_Komadai;
            }
            else if ((int)Masu_Honshogi.nfukuro01 <= masuNumber && masuNumber <= (int)Masu_Honshogi.nfukuro40)
            {
                // 駒袋
                result = Okiba.KomaBukuro;
            }
            else
            {
                // 該当なし
                result = Okiba.Empty;
            }

            return result;
        }

        public static string ToLog(SyElement masu)
        {
            StringBuilder sb = new StringBuilder();

            Okiba okiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(masu));
            sb.Append("okiba=" + Conv_Okiba.ToLog(okiba));

            if (okiba==Okiba.ShogiBan)
            {
                int suji;
                if (!Conv_Masu.ToSuji_FromBanjoMasu(masu, out suji))
                {
                    suji = -1;
                }
                sb.Append(" suji="+suji);

                int dan;
                if (!Conv_Masu.ToDan_FromBanjoMasu(masu, out dan))
                {
                    dan = -1;
                }
                sb.Append(" dan=" + dan);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将棋盤、駒台に筋があります。番号は1～。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static string ToLog_FromBanjo(SyElement masu)
        {
            int suji;
            if (!Conv_Masu.ToSuji_FromBanjoMasu(masu, out suji))
            {
                suji = -1;
            }

            int dan;
            if (!Conv_Masu.ToDan_FromBanjoMasu(masu, out dan))
            {
                dan = -1;
            }

            return suji + "," + dan;
        }

    }
}
