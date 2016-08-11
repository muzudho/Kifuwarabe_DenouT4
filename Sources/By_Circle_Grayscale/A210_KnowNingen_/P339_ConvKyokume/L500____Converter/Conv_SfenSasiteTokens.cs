using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P055_Conv_Sy.L500____Converter;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P062_ConvText___.L500____Converter;
using Grayscale.P211_WordShogi__.L___250_Masu;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P212_ConvPside__.L500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P214_Masu_______.L500____Util;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P226_Tree_______.L___500_Struct;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P239_ConvWords__.L500____Converter;
using Grayscale.P245_SfenTransla.L500____Util;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using Grayscale.P307_UtilSky____.L500____Util;
using Grayscale.P324_KifuTree___.L___250_Struct;
using System;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.P339_ConvKyokume.L500____Converter
{
    public abstract class Conv_SfenSasiteTokens
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 符号１「7g7f」を元に、sasite を作ります。
        /// ************************************************************************************************************************
        /// 
        /// ＜[再生]、[コマ送り]で呼び出されます＞
        /// </summary>
        /// <returns></returns>
        public static void ToMove(
            bool isHonshogi,
            string str1, //123456789 か、 PLNSGKRB
            string str2, //abcdefghi か、 *
            string str3, //123456789
            string str4, //abcdefghi
            string strNari, //+
            out Move move,
            KifuTree kifu,
            string hint,
            KwErrorHandler errH
            )
        {
            move = Move.Empty;

            Node<Move, KyokumenWrapper> siteiNode = kifu.CurNode;
            SkyConst src_Sky = siteiNode.Value.KyokumenConst;
            //kifu.AssertPside(kifu.CurNode, "str1=" + str1, errH);
            Playerside pside1 = src_Sky.KaisiPside;

#if DEBUG
            Debug.Assert(!Conv_MasuHandle.OnKomabukuro(Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu(src_Sky.BusstopIndexOf((Finger)0)))), "[" + src_Sky.Temezumi + "]手目、駒が駒袋にあった。");
#endif

            try
            {
                Komasyurui14 uttaSyurui; // 打った駒の種類

                int srcSuji = Util_Koma.CTRL_NOTHING_PROPERTY_SUJI;
                int srcDan = Util_Koma.CTRL_NOTHING_PROPERTY_DAN;

                if ("*" == str2)
                {
                    //>>>>>>>>>> 「打」でした。

                    Conv_String268.SfenUttaSyurui(str1, out uttaSyurui);

                }
                else
                {
                    //>>>>>>>>>> 指しました。
                    uttaSyurui = Komasyurui14.H00_Null___;//打った駒はない☆

                    //------------------------------
                    // 1
                    //------------------------------
                    if (!int.TryParse(str1, out srcSuji))
                    {
                    }

                    //------------------------------
                    // 2
                    //------------------------------
                    srcDan = Conv_Alphabet.ToInt(str2);
                }

                //------------------------------
                // 3
                //------------------------------
                int suji;
                if (!int.TryParse(str3, out suji))
                {
                }

                //------------------------------
                // 4
                //------------------------------
                int dan;
                dan = Conv_Alphabet.ToInt(str4);



                Finger koma;

                if ("*" == str2)
                {
                    //>>>>> 「打」でした。

                    // 駒台から、打った種類の駒を取得
                    koma = Util_Sky_FingerQuery.InOkibaSyuruiNow_IgnoreCase(
                        siteiNode.Value.KyokumenConst,
                        Conv_Playerside.ToKomadai(pside1),//FIXME:
                        uttaSyurui, errH);
                    if (Fingers.Error_1 == koma)
                    {
                        string message = "TuginoItte_Sfen#GetData_FromTextSub：駒台から種類[" + uttaSyurui + "]の駒を掴もうとしましたが、エラーでした。";
                        Exception ex = new Exception(message);
                        Util_OwataMinister.ERROR.DonimoNaranAkirameta(ex, "moves解析中☆"); throw ex;
                    }


                    //// FIXME: 打のとき、srcSuji、srcDan が Int.Min
                }
                else
                {
                    //>>>>> 打ではないとき
                    SyElement masu1 = Util_Masu10.OkibaSujiDanToMasu(Okiba.ShogiBan, srcSuji, srcDan);
                    Fingers komas1 = Util_Sky_FingersQuery.InMasuNow_Old(//これが空っぽになるときがある。
                        src_Sky, masu1
                        );
                    koma = komas1.ToFirst();

                    if (Fingers.Error_1 == koma)
                    {
                        //
                        // エラーの理由：
                        // 0手目、平手局面を想定していたが、駒がすべて駒袋に入っているときなど
                        //

                        StringBuilder sb = new StringBuilder();
                        sb.Append("TuginoItte_Sfen#GetData_FromTextSub：SFEN解析中の失敗：");
                        sb.Append("SFENでは [");
                        sb.Append(srcSuji);
                        sb.Append("]筋、[");
                        sb.Append(srcDan);
                        sb.AppendLine("]段 にある駒を掴めと指示がありましたが、");
                        sb.AppendLine("将棋盤データの[" + Conv_Sy.Query_Word( masu1.Bitfield) + "]マスには、（駒が全て駒袋に入っているのか）駒がありませんでした。");
                        sb.AppendLine();

                        sb.AppendLine("hint=[" + hint + "]");
                        sb.AppendLine();

                        if (masu1 is New_Basho)
                        {
                            sb.AppendLine("masu1.masuNumber=[" + ((New_Basho)masu1).MasuNumber + "]");
                            sb.AppendLine("komas1.Count=[" + komas1.Count + "]");
                        }
                        else
                        {
                            sb.AppendLine("masu1.masuNumber=New_Basho型じゃない。");
                        }
                        sb.AppendLine();


                        sb.AppendLine("isHonshogi=[" + isHonshogi + "]");
                        sb.AppendLine("str1=[" + str1 + "]");
                        sb.AppendLine("str2=[" + str2 + "]");
                        sb.AppendLine("str3=[" + str3 + "]");
                        sb.AppendLine("str4=[" + str4 + "]");
                        sb.AppendLine("strNari=[" + strNari + "]");

                        sb.AppendLine("src_Sky.Temezumi=[" + src_Sky.Temezumi + "]");

                        // どんな局面なのか？
                        {
                            StartposExporterImpl se = new StartposExporterImpl(src_Sky);
                            sb.AppendLine("局面=sfen " + Util_StartposExporter.ToSfenstring(se, true));
                        }

                        sb.Append(Util_Sky307.Json_1Sky(src_Sky, "エラー駒になったとき",
                            hint + "_SF解3",
                            src_Sky.Temezumi));

                        Exception ex = new Exception(sb.ToString());
                        Util_OwataMinister.ERROR.DonimoNaranAkirameta(ex, "SFEN解析中の失敗"); throw ex;
                    }
                }


                Komasyurui14 dstSyurui;
                Komasyurui14 srcSyurui;
                Okiba srcOkiba;
                SyElement srcMasu;

                bool drop = false;
                if ("*" == str2)
                {
                    //>>>>> 打った駒の場合
                    drop = true;

                    dstSyurui = uttaSyurui;
                    srcSyurui = uttaSyurui;
                    switch (pside1)
                    {
                        case Playerside.P2:
                            srcOkiba = Okiba.Gote_Komadai;
                            break;
                        case Playerside.P1:
                            srcOkiba = Okiba.Sente_Komadai;
                            break;
                        default:
                            srcOkiba = Okiba.Empty;
                            break;
                    }


                    Finger srcKoma = Util_Sky_FingerQuery.InOkibaSyuruiNow_IgnoreCase(siteiNode.Value.KyokumenConst, srcOkiba, srcSyurui, errH);

                    src_Sky.AssertFinger(srcKoma);
                    Busstop dstKoma = src_Sky.BusstopIndexOf(srcKoma);

                    srcMasu = Conv_Busstop.ToMasu( dstKoma);
                }
                else
                {
                    //>>>>> 盤上の駒を指した場合

                    src_Sky.AssertFinger(koma);
                    Busstop dstKoma = src_Sky.BusstopIndexOf(koma);


                    dstSyurui = Conv_Busstop.ToKomasyurui(dstKoma);
                    srcSyurui = dstSyurui; //駒は「元・種類」を記憶していませんので、「現・種類」を指定します。
                    srcOkiba = Okiba.ShogiBan;
                    srcMasu = Util_Masu10.OkibaSujiDanToMasu(srcOkiba, srcSuji, srcDan);
                }


                //------------------------------
                // 5
                //------------------------------
                bool promotion = false;
                if ("+" == strNari)
                {
                    // 成りました
                    promotion = true;
                    dstSyurui = Util_Komasyurui14.NariCaseHandle[(int)dstSyurui];
                }


                //------------------------------
                // 結果
                //------------------------------
                // 棋譜
                move = Conv_Move.ToMove(
                    srcMasu,//FIXME:升ハンドルにしたい
                    Util_Masu10.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan),//符号は将棋盤の升目です。 FIXME:升ハンドルにしたい
                    srcSyurui,//dstSyurui
                    Komasyurui14.H00_Null___,//符号からは、取った駒は分からない
                    promotion,
                    drop,
                    pside1,
                    false
                );
            }
            catch (Exception ex) { Util_OwataMinister.ERROR.DonimoNaranAkirameta(ex, "moves解析中☆　str1=「" + str1 + "」　str2=「" + str2 + "」　str3=「" + str3 + "」　str4=「" + str4 + "」　strNari=「" + strNari + "」　"); throw ex; }
        }
    }
}
