﻿using System;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A060Application.B510ConvSy.C500Converter;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A060Application.B620ConvText.C500Converter;
using Grayscale.A210KnowNingen.B170WordShogi.C250Masu;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B310Shogiban.C500Util;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B350SfenTransla.C500Util;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B600UtilSky.C500Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter
{
    public abstract class ConvSfenMoveTokens
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 符号１「7g7f」を元に、move を作ります。
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
            Playerside psideA,
            ISky positionA,
            string hint,
            ILogTag logger
            )
        {
            move = Move.Empty;

            //kifu.AssertPside(kifu.CurNode, "str1=" + str1, errH);
            //Playerside pside1 = positionA.KaisiPside;

#if DEBUG
            Debug.Assert(!Conv_Masu.OnKomabukuro(Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(positionA.BusstopIndexOf((Finger)0)))), "[" + positionA.Temezumi + "]手目、駒が駒袋にあった。");
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
                    koma = UtilSkyFingerQuery.InOkibaSyuruiNow_IgnoreCase(
                        positionA,
                        Conv_Playerside.ToKomadai(psideA),//FIXME:
                        uttaSyurui,
                        logger);
                    if (Fingers.Error_1 == koma)
                    {
                        string message = "Conv_SfenMoveTokens#ToMove：[" + Conv_Playerside.ToLog_Kanji(psideA) + "]駒台から種類[" + uttaSyurui + "]の駒を掴もうとしましたが、エラーでした。\n" +
                            Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(psideA, positionA, logger)) + "\n" +
                            "hint=[" + hint + "]\n" +
                            "str1=[" + str1 + "]\n" +
                            "str2=[" + str2 + "]\n" +
                            "str3=[" + str3 + "]\n" +
                            "str4=[" + str4 + "]\n" +
                            "strNari=[" + strNari + "]\n" +
                            "";
                        Exception ex1 = new Exception(message);
                        Logger.Panic(LogTags.ProcessNoneError,ex1, "moves解析中☆");
                        throw ex1;
                    }


                    //// FIXME: 打のとき、srcSuji、srcDan が Int.Min
                }
                else
                {
                    //>>>>> 打ではないとき
                    SyElement masu1 = Conv_Masu.ToMasu_FromBanjoSujiDan(srcSuji, srcDan);
                    Fingers komas1 = UtilSkyFingersQuery.InMasuNow_Old(//これが空っぽになるときがある。
                        positionA, masu1
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
                        sb.AppendLine("将棋盤データの[" + Conv_Sy.Query_Word(masu1.Bitfield) + "]マスには、（駒が全て駒袋に入っているのか）駒がありませんでした。");
                        sb.AppendLine();

                        sb.AppendLine("hint=[" + hint + "]");
                        sb.AppendLine();

                        if (masu1 is INewBasho)
                        {
                            sb.AppendLine("masu1.masuNumber=[" + ((INewBasho)masu1).MasuNumber + "]");
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

                        sb.AppendLine("src_Sky.Temezumi=[" + positionA.Temezumi + "]");

                        // どんな局面なのか？
                        {
                            sb.AppendLine("局面=sfen " + Util_StartposExporter.ToSfenstring(
                                Conv_Sky.ToShogiban(psideA, positionA, logger), true));
                        }

                        sb.Append(UtilSky307.Json_1Sky(positionA, "エラー駒になったとき",
                            hint + "_SF解3",
                            positionA.Temezumi));

                        Exception ex1 = new Exception(sb.ToString());
                        Logger.Panic(LogTags.ProcessNoneError,ex1, "SFEN解析中の失敗");
                        throw ex1;
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
                    switch (psideA)
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


                    Finger srcKoma = UtilSkyFingerQuery.InOkibaSyuruiNow_IgnoreCase(
                        positionA,// siteiNode.Value.Kyokumen,
                        srcOkiba, srcSyurui, logger);

                    positionA.AssertFinger(srcKoma);
                    Busstop dstKoma = positionA.BusstopIndexOf(srcKoma);

                    srcMasu = Conv_Busstop.ToMasu(dstKoma);
                }
                else
                {
                    //>>>>> 盤上の駒を指した場合

                    positionA.AssertFinger(koma);
                    Busstop dstKoma = positionA.BusstopIndexOf(koma);


                    dstSyurui = Conv_Busstop.ToKomasyurui(dstKoma);
                    srcSyurui = dstSyurui; //駒は「元・種類」を記憶していませんので、「現・種類」を指定します。

                    srcOkiba = Okiba.ShogiBan;
                    srcMasu = Conv_Masu.ToMasu_FromBanjoSujiDan(srcSuji, srcDan);
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
                move = ConvMove.ToMove(
                    srcMasu,//FIXME:升ハンドルにしたい
                    Conv_Masu.ToMasu_FromBanjoSujiDan(suji, dan),//符号は将棋盤の升目です。 FIXME:升ハンドルにしたい
                    srcSyurui,//dstSyurui
                    Komasyurui14.H00_Null___,//符号からは、取った駒は分からない
                    promotion,
                    drop,
                    psideA,
                    false
                );
            }
            catch (Exception ex)
            {
                Logger.Panic(LogTags.ProcessNoneError,ex, "moves解析中☆　str1=「" + str1 + "」　str2=「" + str2 + "」　str3=「" + str3 + "」　str4=「" + str4 + "」　strNari=「" + strNari + "」　");
                throw;
            }
        }
    }
}
