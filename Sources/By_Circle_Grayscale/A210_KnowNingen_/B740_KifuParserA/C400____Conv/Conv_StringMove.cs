using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A120_KifuSfen___.B160_ConvSfen___.C500____Converter;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using System;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C400____Conv
{
    public abstract class Conv_StringMove
    {
        public static Move ToMove(
            out string out_restString,
            string sfenMove1,
            Move previous_Move,  // 「同」を調べるためのもの。
            Playerside pside,
            Sky previous_Sky,
            KwLogger errH
            )
        {
            bool isHonshogi = true;
            Move nextMove = Move.Empty;

            out_restString = sfenMove1;
            string sfenMove2 = sfenMove1.Trim();
            if (0 < sfenMove2.Length)
            {
                try
                {
                    //「6g6f」形式と想定して、１手だけ読込み
                    string str1;
                    string str2;
                    string str3;
                    string str4;
                    string str5;
                    string str6;
                    string str7;
                    string str8;
                    string str9;
                    if (Conv_Sfen.ToTokens_FromMove(
                        sfenMove2, out str1, out str2, out str3, out str4, out str5,
                        out out_restString, errH)
                        &&
                        !(str1 == "" && str2 == "" && str3 == "" && str4 == "" && str5 == "")
                        )
                    {

                        Conv_SfenSasiteTokens.ToMove(
                            isHonshogi,
                            str1,  //123456789 か、 PLNSGKRB
                            str2,  //abcdefghi か、 *
                            str3,  //123456789
                            str4,  //abcdefghi
                            str5,  //+
                            out nextMove,

                            pside,
                            //Conv_Playerside.Reverse( Conv_Move.ToPlayerside(previous_Move)),
                            //previous_Sky.KaisiPside,

                            previous_Sky,
                            "棋譜パーサーA_SFENパース1",
                            errH
                            );
                    }
                    else
                    {
                        //>>>>> 「6g6f」形式ではなかった☆

                        //「▲６六歩」形式と想定して、１手だけ読込み
                        if (Conv_JsaFugoText.ToTokens(
                            sfenMove2,
                            out str1, out str2, out str3, out str4, out str5, out str6, out str7, out str8, out str9,
                            out out_restString,
                            errH))
                        {
                            if (!(str1 == "" && str2 == "" && str3 == "" && str4 == "" && str5 == "" && str6 == "" && str7 == "" && str8 == "" && str9 == ""))
                            {
                                Conv_JsaFugoTokens.ToMove(
                                    str1,  //▲△
                                    str2,  //123…9、１２３…９、一二三…九
                                    str3,  //123…9、１２３…９、一二三…九
                                    str4,  // “同”
                                    str5,  //(歩|香|桂|…
                                    str6,           // 右|左…
                                    str7,  // 上|引
                                    str8, //成|不成
                                    str9,  //打
                                    out nextMove,
                                    previous_Move,
                                    previous_Sky,
                                    errH
                                    );
                            }

                        }
                        else
                        {
                            //「6g6f」形式でもなかった☆

                            string message = "（＾△＾）「" + sfenMove1 + "」！？　次の一手が読めない☆";
                            errH.AppendLine(message);
                            errH.Flush(LogTypes.Error);
                            
                            Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(message);
                            goto gt_EndMethod;
                        }

                    }
                }
                catch (Exception ex) {
                    Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "moves解析中☆");
                    throw ex;
                }
            }

        gt_EndMethod:
            return nextMove;
        }
    }
}
