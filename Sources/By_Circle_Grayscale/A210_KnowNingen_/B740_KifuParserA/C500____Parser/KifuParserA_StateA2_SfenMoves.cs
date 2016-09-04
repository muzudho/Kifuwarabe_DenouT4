using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A120_KifuSfen___.B160_ConvSfen___.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using System;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser
{

    /// <summary>
    /// 「moves」を読込みました。
    /// 
    /// 処理の中で、一手指すルーチンを実行します。
    /// </summary>
    public class KifuParserA_StateA2_SfenMoves : KifuParserA_State
    {

        public static KifuParserA_StateA2_SfenMoves GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA2_SfenMoves();
            }

            return instance;
        }
        private static KifuParserA_StateA2_SfenMoves instance;


        private KifuParserA_StateA2_SfenMoves()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="model_Taikyoku"></param>
        /// <param name="nextState"></param>
        /// <param name="owner"></param>
        /// <param name="genjo"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public string Execute(
            ref KifuParserA_Result result,
            Model_Taikyoku model_Taikyoku,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KwLogger errH,
            KwDisplayer kd
            )
        {
            int exceptionArea = 0;

            bool isHonshogi = true;//FIXME:暫定

            
            nextState = this;

            try
            {
                if (0 < genjo.InputLine.Trim().Length)
                {
                    Move nextMove = Move.Empty;
                    string rest;

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
                            genjo.InputLine, out str1, out str2, out str3, out str4, out str5, out rest, errH)
                            &&
                            !(str1=="" && str2=="" && str3=="" && str4=="" && str5=="")
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
                                model_Taikyoku.Kifu,
                                "棋譜パーサーA_SFENパース1",
                                errH
                                );
                        }
                        else
                        {
                            //>>>>> 「6g6f」形式ではなかった☆

                            //「▲６六歩」形式と想定して、１手だけ読込み
                            if (Conv_JsaFugoText.ToTokens(
                                genjo.InputLine, out str1, out str2, out str3, out str4, out str5, out str6, out str7, out str8, out str9, out rest, model_Taikyoku.Kifu, errH))
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
                                        model_Taikyoku.Kifu,
                                        errH
                                        );
                                }

                            }
                            else
                            {
                                //「6g6f」形式でもなかった☆

                                errH.AppendLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　！？　次の一手が読めない☆　inputLine=[" + genjo.InputLine + "]");
                                errH.Flush(LogTypes.Error);
                                genjo.ToBreak_Abnormal();
                                goto gt_EndMethod;
                            }

                        }

                        genjo.InputLine = rest;
                    }
                    catch (Exception ex) { Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "moves解析中☆"); throw ex; }




                    if (Move.Empty != nextMove)
                    {
                        exceptionArea = 1000;

                        IttesasuResult ittesasuResult = new IttesasuResultImpl(
                            Fingers.Error_1, Fingers.Error_1, Move.Empty, null, Komasyurui14.H00_Null___);

                        try
                        {
                            exceptionArea = 1010;

                            //
                            //FIXME: これが悪さをしていないか☆？
                            //FIXME: スピードが必要なので省略。
                            //Application.DoEvents(); // 時間のかかる処理の間にはこれを挟みます。
                            //

                            exceptionArea = 1020;
                            //------------------------------
                            // ★棋譜読込専用  駒移動
                            //------------------------------
                            //errH.AppendLine_AddMemo("一手指し開始　：　残りの符号つ「" + genjo.InputLine + "」");


                            exceptionArea = 1030;
                            //
                            //↓↓将棋エンジンが一手指し（進める）
                            //
                            Util_IttesasuRoutine.DoMove(
                                out ittesasuResult,
                                model_Taikyoku.Kifu.CurNode.Value,
                                nextMove,//FIXME: if文で分けているので、これがヌルなはずはないと思うが。
                                errH,
                                kd,
                                "KifuParserA_StateA2_SfenMoves#Execute"
                                );
                            // 棋譜ツリーのカレントを変更します。
                            Util_IttesasuRoutine.UpdateKifuTree(model_Taikyoku.Kifu, ittesasuResult);

                            exceptionArea = 1080;
                            result.Out_newNode_OrNull = model_Taikyoku.Kifu.CurNode;// ittesasuResult.Get_SyuryoNode_OrNull;
                            //↑↑一手指し

                            //exceptionArea = 1090;
                            //errH.AppendLine_AddMemo(Util_Sky307.Json_1Sky(
                            //    src_Sky,
                            //    "一手指し終了",
                            //    "SFENパース2",
                            //    src_Sky.Temezumi//読み進めている現在の手目
                            //    ));


                        }
                        catch (Exception ex)
                        {
                            //>>>>> エラーが起こりました。

                            // どうにもできないので  ログだけ取って無視します。
                            string message = this.GetType().Name + "#Execute（B）： exceptionArea=" + exceptionArea + "\n" + ex.GetType().Name + "：" + ex.Message;
                            errH.AppendLine(message);
                            errH.Flush(LogTypes.Error);
                        }

                    }
                    else
                    {
                        genjo.ToBreak_Abnormal();
                        string message = "＼（＾ｏ＾）／teSasiteオブジェクトがない☆！　inputLine=[" + genjo.InputLine + "]";
                        errH.AppendLine(message);
                        errH.Flush(LogTypes.Error);
                        throw new Exception(message);
                    }
                }
                else
                {
                    //errH.AppendLine_AddMemo("（＾△＾）現局面まで進んだのかだぜ☆？\n" + Util_Sky307.Json_1Sky(
                    //    src_Sky,
                    //    "棋譜パース",
                    //    "SFENパース3",
                    //    src_Sky.Temezumi//読み進めている現在の手目
                    //    ));
                    genjo.ToBreak_Normal();//棋譜パーサーＡの、唯一の正常終了のしかた。
                }
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute：" + ex.GetType().Name + "：" + ex.Message;
                errH.AppendLine(message);
                errH.Flush(LogTypes.Error);
            }

        gt_EndMethod:
            return genjo.InputLine;
        }

    }
}
