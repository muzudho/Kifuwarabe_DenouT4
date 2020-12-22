using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.Kifuwaragyoku.Entities.Features
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

            //kifu.AssertPside(kifu.CurNode, "str1=" + str1, logTag);
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
                        throw new Exception($@"Conv_SfenMoveTokens#ToMove：[{Conv_Playerside.ToLog_Kanji(psideA)}]駒台から種類[{uttaSyurui}]の駒を掴もうとしましたが、エラーでした。
{Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(psideA, positionA, logger))}
hint=[{hint}]
str1=[{str1}]
str2=[{str2}]
str3=[{str3}]
str4=[{str4}]
strNari=[{strNari}]");
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

                        string text2;
                        if (masu1 is INewBasho)
                        {
                            text2 = $@"masu1.masuNumber=[{((INewBasho)masu1).MasuNumber}]
komas1.Count=[{komas1.Count}]";
                        }
                        else
                        {
                            text2 = $"masu1.masuNumber=New_Basho型じゃない。";
                        }

                        var sky2 = UtilSky307.Json_1Sky(positionA, "エラー駒になったとき", $"{hint}_SF解3", positionA.Temezumi);

                        throw new Exception($@"TuginoItte_Sfen#GetData_FromTextSub：SFEN解析中の失敗：SFENでは [{srcSuji}]筋、[{srcDan}]段 にある駒を掴めと指示がありましたが、
将棋盤データの[{Conv_Sy.Query_Word(masu1.Bitfield)}]マスには、（駒が全て駒袋に入っているのか）駒がありませんでした。
hint=[{hint}]
{text2}
isHonshogi=[{isHonshogi}]
str1=[{str1}]
str2=[{str2}]
str3=[{str3}]
str4=[{str4}]
strNari=[{strNari}]
src_Sky.Temezumi=[{positionA.Temezumi}]
局面=sfen {Util_StartposExporter.ToSfenstring(Conv_Sky.ToShogiban(psideA, positionA, logger), true)}
{sky2}");
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
                Logger.Panic(LogTags.ProcessNoneError, ex, "moves解析中☆　str1=「" + str1 + "」　str2=「" + str2 + "」　str3=「" + str3 + "」　str4=「" + str4 + "」　strNari=「" + strNari + "」　");
                throw;
            }
        }
    }
}
