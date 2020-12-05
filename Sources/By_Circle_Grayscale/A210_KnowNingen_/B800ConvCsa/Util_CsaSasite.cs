using System.Diagnostics;
using System.Text;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A060Application.B620ConvText.C500Converter;
using Grayscale.A120KifuSfen.B140SfenStruct.C250Struct;
using Grayscale.A180KifuCsa.B120KifuCsa.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;

namespace Grayscale.A210KnowNingen.B800ConvCsa.C500Converter
{
    public abstract class Util_CsaSasite
    {

        /// <summary>
        /// CSA符号→元位置
        /// </summary>
        /// <param name="csa"></param>
        /// <returns></returns>
        public static SyElement ToSrcMasu(CsaKifuSasite csa)
        {
            int suji;
            int dan;
            int.TryParse(csa.SourceMasu[0].ToString(), out suji);
            int.TryParse(csa.SourceMasu[1].ToString(), out dan);

            return Conv_Masu.ToMasu_FromBanjoSujiDan(suji, dan);
        }

        /// <summary>
        /// CSA符号→先位置
        /// </summary>
        /// <param name="csa"></param>
        /// <returns></returns>
        public static SyElement ToDstMasu(CsaKifuSasite csa)
        {
            int suji;
            int dan;
            int.TryParse(csa.DestinationMasu[0].ToString(), out suji);
            int.TryParse(csa.DestinationMasu[1].ToString(), out dan);

            return Conv_Masu.ToMasu_FromBanjoSujiDan(suji, dan);
        }


        /// <summary>
        /// CSA符号→先位置での駒種類
        /// </summary>
        /// <param name="csa"></param>
        /// <returns></returns>
        public static Komasyurui14 ToKomasyurui(CsaKifuSasite csa)
        {
            Komasyurui14 result_kifuwarabe;

            switch (csa.Syurui)
            {
                case Word_Csa.FU_FU_____: result_kifuwarabe = Komasyurui14.H01_Fu_____; break;
                case Word_Csa.KY_KYO____: result_kifuwarabe = Komasyurui14.H02_Kyo____; break;
                case Word_Csa.KE_KEI____: result_kifuwarabe = Komasyurui14.H03_Kei____; break;
                case Word_Csa.GI_GIN____: result_kifuwarabe = Komasyurui14.H04_Gin____; break;
                case Word_Csa.KI_KIN____: result_kifuwarabe = Komasyurui14.H05_Kin____; break;
                case Word_Csa.KA_KAKU___: result_kifuwarabe = Komasyurui14.H08_Kaku___; break;
                case Word_Csa.HI_HISYA__: result_kifuwarabe = Komasyurui14.H07_Hisya__; break;
                case Word_Csa.OU_OU_____: result_kifuwarabe = Komasyurui14.H06_Gyoku__; break;
                case Word_Csa.TO_TOKIN__: result_kifuwarabe = Komasyurui14.H11_Tokin__; break;
                case Word_Csa.NY_NARIKYO: result_kifuwarabe = Komasyurui14.H12_NariKyo; break;
                case Word_Csa.NK_NARIKEI: result_kifuwarabe = Komasyurui14.H13_NariKei; break;
                case Word_Csa.NG_NARIGIN: result_kifuwarabe = Komasyurui14.H14_NariGin; break;
                case Word_Csa.UM_UMA____: result_kifuwarabe = Komasyurui14.H10_Uma____; break;
                case Word_Csa.RY_RYU____: result_kifuwarabe = Komasyurui14.H09_Ryu____; break;
                default: result_kifuwarabe = Komasyurui14.H00_Null___; break;
            }

            return result_kifuwarabe;
        }

        /// <summary>
        /// CSA符号→プレイヤーサイド
        /// </summary>
        /// <param name="csa"></param>
        /// <returns></returns>
        public static Playerside ToPside(CsaKifuSasite csa)
        {
            Playerside result;

            switch (csa.Sengo)
            {
                case "+": result = Playerside.P1; break;
                case "-": result = Playerside.P2; break;
                default: result = Playerside.Empty; break;
            }

            return result;
        }

        /// <summary>
        /// CSAの指し手を、SFENの指し手に変換します。
        /// </summary>
        /// <param name="csa"></param>
        /// <param name="ittemae_Sky">1手前の局面。ルート局面などの理由で１手前の局面がない場合はヌル。</param>
        /// <returns></returns>
        public static string ToSfen(CsaKifuSasite csa, ISky ittemae_Sky_orNull)
        {
            StringBuilder sb = new StringBuilder();

            int dstSuji;
            int.TryParse(csa.DestinationMasu[0].ToString(), out dstSuji);

            string dstDan = Conv_Suji.ToAlphabet(csa.DestinationMasu[1]);

            // 元位置の筋と段は、あとで必ず使う。（成りの判定）
            int srcSuji;
            int.TryParse(csa.SourceMasu[0].ToString(), out srcSuji);
            int srcDan;
            int.TryParse(csa.SourceMasu[1].ToString(), out srcDan);

            if ("00" == csa.SourceMasu)
            {
                // 打

                string syurui;
                switch (csa.Syurui)
                {
                    case Word_Csa.FU_FU_____: syurui = Word_Sfen.P_PAWN__; break;
                    case Word_Csa.KY_KYO____: syurui = Word_Sfen.L_LANCE_; break;
                    case Word_Csa.KE_KEI____: syurui = Word_Sfen.N_KNIGHT; break;
                    case Word_Csa.GI_GIN____: syurui = Word_Sfen.S_SILVER; break;
                    case Word_Csa.KI_KIN____: syurui = Word_Sfen.G_GOLD__; break;
                    case Word_Csa.KA_KAKU___: syurui = Word_Sfen.B_BISHOP; break;
                    case Word_Csa.HI_HISYA__: syurui = Word_Sfen.R_ROOK__; break;
                    case Word_Csa.OU_OU_____: syurui = Word_Sfen.K_KING__; break;//おまけ
                    default: syurui = Word_Sfen.ERROR___; break;//エラー
                }

                sb.Append(syurui);
                sb.Append("*");
            }
            else
            {
                string srcDan_alphabet = Conv_Suji.ToAlphabet(csa.SourceMasu[1]);
                sb.Append(srcSuji);
                sb.Append(srcDan_alphabet);
            }




            sb.Append(dstSuji);
            sb.Append(dstDan);

            bool nari = false;
            switch (csa.Syurui)
            {
                case Word_Csa.TO_TOKIN__: nari = true; break;
                case Word_Csa.NY_NARIKYO: nari = true; break;
                case Word_Csa.NK_NARIKEI: nari = true; break;
                case Word_Csa.NG_NARIGIN: nari = true; break;
                case Word_Csa.UM_UMA____: nari = true; break;
                case Word_Csa.RY_RYU____: nari = true; break;
            }

            //
            // 「成り」をしたのかどうかを、調べます。
            //
            {
                if (null != ittemae_Sky_orNull && "00" != csa.SourceMasu)
                {
                    // ルート局面ではなく、かつ、打ではないとき。

                    //ittemae_Sky_orNull.Foreach_Starlights((Finger finger, Starlight light, ref bool toBreak) =>
                    //{
                    //    RO_Star_Koma koma = Util_Starlightable.AsKoma(light.Now);
                    //    System.Console.WriteLine("[" + finger + "] " + koma.Masu.Word + "　" + koma.Pside + "　" + KomaSyurui14Array.Ichimoji[(int)koma.Syurui]);
                    //});

                    SyElement srcMasu = Conv_Masu.ToMasu_FromBanjoSujiDan(srcSuji, srcDan);
                    Busstop srcKoma = UtilSkyKomaQuery.InMasuNow(ittemae_Sky_orNull, srcMasu);
                    Debug.Assert(Busstop.Empty != srcKoma, "元位置の駒を取得できなかった。1");

                    if (!Util_Komasyurui14.IsNari(Conv_Busstop.ToKomasyurui(srcKoma)) && nari)//移動元で「成り」でなかった駒が、移動後に「成駒」になっていた場合。
                    {
                        sb.Append("+");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
