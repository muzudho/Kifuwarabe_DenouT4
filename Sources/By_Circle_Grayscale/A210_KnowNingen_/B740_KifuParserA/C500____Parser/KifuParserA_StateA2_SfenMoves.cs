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
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C400____Conv;

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

            //bool isHonshogi = true;//FIXME:暫定

            
            nextState = this;

            try
            {
                if (0 < genjo.InputLine.Trim().Length)
                {
                    bool abnormal;
                    string rest;
                    Move nextMove = Conv_StringMove.ToMove(
                        out abnormal,
                        out rest,
                        genjo.InputLine,
                        model_Taikyoku.Kifu.CurNode.Key,
                        model_Taikyoku.Kifu.CurNode.Value.Kyokumen,
                        errH
                        );
                    genjo.InputLine = rest;
                    if (abnormal)
                    {
                        genjo.ToBreak_Abnormal();
                        goto gt_EndMethod;
                    }


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
                                nextMove,
                                model_Taikyoku.Kifu.CurNode.Value.Kyokumen,
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
