﻿using System;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.A120KifuSfen;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C400Conv
{
    public abstract class ConvStringMove
    {
        public static Move ToMove(
            out string out_restString,
            string sfenMove1,
            Move previous_Move,  // 「同」を調べるためのもの。
            Playerside pside,
            ISky previous_Sky,
            ILogger errH
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
                    if (SfenConf.ToTokens_FromMove(
                        sfenMove2, out str1, out str2, out str3, out str4, out str5,
                        out out_restString, errH)
                        &&
                        !(str1 == "" && str2 == "" && str3 == "" && str4 == "" && str5 == "")
                        )
                    {

                        ConvSfenMoveTokens.ToMove(
                            isHonshogi,
                            str1,  //123456789 か、 PLNSGKRB
                            str2,  //abcdefghi か、 *
                            str3,  //123456789
                            str4,  //abcdefghi
                            str5,  //+
                            out nextMove,

                            pside,
                            //Conv_Playerside.Reverse( ConvMove.ToPlayerside(previous_Move)),
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
                                ConvJsaFugoTokens.ToMove(
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

                            ErrorControllerReference.ProcessNoneError.DonimoNaranAkirameta(message);
                            goto gt_EndMethod;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ErrorControllerReference.ProcessNoneError.DonimoNaranAkirameta(ex, "moves解析中☆");
                    throw;
                }
            }

        gt_EndMethod:
            return nextMove;
        }
    }
}
