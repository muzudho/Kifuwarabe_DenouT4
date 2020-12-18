using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA;
using Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA;
using Grayscale.A210KnowNingen.B740KifuParserA.C400Conv;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
{

    /// <summary>
    /// 「moves」を読込みました。
    /// 
    /// 処理の中で、一手指すルーチンを実行します。
    /// </summary>
    public class KifuParserAStateA2SfenMoves : IKifuParserAState
    {

        public static KifuParserAStateA2SfenMoves GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserAStateA2SfenMoves();
            }

            return instance;
        }
        private static KifuParserAStateA2SfenMoves instance;


        private KifuParserAStateA2SfenMoves()
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
        /// <param name="logTag"></param>
        /// <returns></returns>
        public string Execute(
            out MoveNodeType out_moveNodeType,
            ref IKifuParserAResult result,

            Earth earth1,
            Move moveA,
            ISky positionA,

            out IKifuParserAState nextState,
            IKifuParserA owner,
            IKifuParserAGenjo genjo,
            ILogTag logTag
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
                    Move nextMove = ConvStringMove.ToMove(
                        out rest,
                        genjo.InputLine,
                        moveA,

                        //ConvMove.ToPlayerside(move1),
                        positionA.GetKaisiPside(),

                        positionA,
                        logTag
                        );
                    genjo.InputLine = rest;


                    if (Move.Empty != nextMove)
                    {
                        exceptionArea = 1000;

                        IIttesasuResult ittesasuResult = new IttesasuResultImpl(
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
                            UtilIttesasuRoutine.DoMove_Normal(
                                out ittesasuResult,
                                ref nextMove,
                                positionA,
                                logTag
                                );
                            // 棋譜ツリーのカレントを変更します。
                            result.SetNode(new MoveExImpl(nextMove),
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
                            Logger.AppendLine(logTag,message);
                            Logger.Flush(logTag,LogTypes.Error);
                        }

                    }
                    else
                    {
                        genjo.ToBreak_Abnormal();
                        string message = "＼（＾ｏ＾）／Moveオブジェクトがない☆！　inputLine=[" + genjo.InputLine + "]";
                        Logger.AppendLine(logTag,message);
                        Logger.Flush(logTag,LogTypes.Error);
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
                Logger.AppendLine(logTag,message);
                Logger.Flush(logTag,LogTypes.Error);

                // サーバーを止めるフラグ☆（＾▽＾）
                genjo.ToBreak_Abnormal();
                goto gt_EndMethod;
            }

        gt_EndMethod:
            return genjo.InputLine;
        }

    }
}
