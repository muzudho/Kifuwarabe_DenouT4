using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C400____Conv;
using System;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

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
        /// <param name="kifu1_notUse"></param>
        /// <param name="nextState"></param>
        /// <param name="owner"></param>
        /// <param name="genjo"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public string Execute(
            out MoveNodeType out_moveNodeType,
            ref KifuParserA_Result result,

            Earth earth1,
            Move moveA,
            Sky positionA,

            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KwLogger errH
            )
        {
            out_moveNodeType = MoveNodeType.None;
            int exceptionArea = 0;

            //bool isHonshogi = true;//FIXME:暫定

            
            nextState = this;

            try
            {
                if (0 < genjo.InputLine.Trim().Length)
                {
                    string rest;
                    Move nextMove = Conv_StringMove.ToMove(
                        out rest,
                        genjo.InputLine,
                        moveA,

                        //Conv_Move.ToPlayerside(move1),
                        positionA.GetKaisiPside(),

                        positionA,
                        errH
                        );
                    genjo.InputLine = rest;


                    if (Move.Empty != nextMove)
                    {
                        exceptionArea = 1000;

                        IttesasuResult ittesasuResult = new IttesasuResultImpl(
                            Fingers.Error_1, Fingers.Error_1, null, Komasyurui14.H00_Null___);

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
                            Util_IttesasuRoutine.DoMove_Normal(
                                out ittesasuResult,
                                ref nextMove,
                                positionA,
                                errH
                                );
                            // 棋譜ツリーのカレントを変更します。
                            result.SetNode( new MoveExImpl(nextMove),
                                ittesasuResult.SyuryoKyokumenW
                                );
                            out_moveNodeType = MoveNodeType.Do;

                            exceptionArea = 1080;
                            //↑↑一手指し
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

                // サーバーを止めるフラグ☆（＾▽＾）
                genjo.ToBreak_Abnormal();
                goto gt_EndMethod;
            }

            gt_EndMethod:
            return genjo.InputLine;
        }

    }
}
