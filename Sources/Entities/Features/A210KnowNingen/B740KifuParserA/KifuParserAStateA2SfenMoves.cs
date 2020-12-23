using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.Entities.Features
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
            IKifuParserAGenjo genjo)
        {
            out_moveNodeType = MoveNodeType.None;

            //bool isHonshogi = true;//FIXME:暫定


            nextState = this;

            if (0 < genjo.InputLine.Trim().Length)
            {
                string rest;
                Move nextMove = ConvStringMove.ToMove(
                    out rest,
                    genjo.InputLine,
                    moveA,

                    //ConvMove.ToPlayerside(move1),
                    positionA.GetKaisiPside(),

                    positionA);
                genjo.InputLine = rest;


                if (Move.Empty != nextMove)
                {
                    IIttesasuResult ittesasuResult = new IttesasuResultImpl(
                        Fingers.Error_1, Fingers.Error_1, null, Komasyurui14.H00_Null___);

                    //
                    //FIXME: これが悪さをしていないか☆？
                    //FIXME: スピードが必要なので省略。
                    //Application.DoEvents(); // 時間のかかる処理の間にはこれを挟みます。
                    //

                    //------------------------------
                    // ★棋譜読込専用  駒移動
                    //------------------------------
                    //logTag.AppendLine_AddMemo("一手指し開始　：　残りの符号つ「" + genjo.InputLine + "」");

                    //
                    //↓↓将棋エンジンが一手指し（進める）
                    //
                    UtilIttesasuRoutine.DoMove_Normal(
                        out ittesasuResult,
                        ref nextMove,
                        positionA);
                    // 棋譜ツリーのカレントを変更します。
                    result.SetNode(new MoveExImpl(nextMove),
                        ittesasuResult.SyuryoKyokumenW
                        );
                    out_moveNodeType = MoveNodeType.Do;
                    //↑↑一手指し
                }
                else
                {
                    genjo.ToBreak_Abnormal();
                    throw new Exception($"＼（＾ｏ＾）／Moveオブジェクトがない☆！　inputLine=[{genjo.InputLine}]");
                }
            }
            else
            {
                //logTag.AppendLine_AddMemo("（＾△＾）現局面まで進んだのかだぜ☆？\n" + Util_Sky307.Json_1Sky(
                //    src_Sky,
                //    "棋譜パース",
                //    "SFENパース3",
                //    src_Sky.Temezumi//読み進めている現在の手目
                //    ));
                genjo.ToBreak_Normal();//棋譜パーサーＡの、唯一の正常終了のしかた。
            }

        // gt_EndMethod:
            return genjo.InputLine;
        }

    }
}
