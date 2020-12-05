using System;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
{
    /// <summary>
    /// 平手の初期配置です。
    /// </summary>
    public class KifuParserA_StateA1aSfenStartpos : IKifuParserAState
    {


        public static KifuParserA_StateA1aSfenStartpos GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1aSfenStartpos();
            }

            return instance;
        }
        private static KifuParserA_StateA1aSfenStartpos instance;



        private KifuParserA_StateA1aSfenStartpos()
        {
        }


        public string Execute(
            out MoveNodeType out_moveNodeType,
            ref IKifuParserAResult result,

            Earth earth1_notUse,
            Move move1_notUse,
            ISky positionA,

            out IKifuParserAState nextState,
            IKifuParserA owner,
            IKifuParserAGenjo genjo,
            ILogger errH
            )
        {
            out_moveNodeType = MoveNodeType.None;
            nextState = this;

            try
            {
                if (genjo.InputLine.StartsWith("moves"))
                {
                    //>>>>> 棋譜が始まります。
#if DEUBG
                    errH.AppendLine_AddMemo("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ｳﾑ☆　moves 分かるぜ☆");
#endif

                    genjo.InputLine = genjo.InputLine.Substring("moves".Length);
                    genjo.InputLine = genjo.InputLine.Trim();


                    nextState = KifuParserAStateA2SfenMoves.GetInstance();
                }
                else if ("" == genjo.InputLine)
                {
                    // FIXME: コンピューターが先手のとき、ここにくる？

                    // 異常時。
                    errH.AppendLine("＼（＾ｏ＾）／「" + genjo.InputLine + "」入力がない1☆！　終わるぜ☆");
                    errH.Flush(LogTypes.Error);
                    genjo.ToBreak_Abnormal();
                }
                else
                {
                    // 異常時。
                    errH.AppendLine("＼（＾ｏ＾）／「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　movesがない☆！　終わるぜ☆");
                    errH.Flush(LogTypes.Error);
                    genjo.ToBreak_Abnormal();
                }
            }
            catch (Exception ex)
            {
                ErrorControllerReference.ProcessNoneError.DonimoNaranAkirameta(ex, "SFEN文字列の解析中。");
                throw ex;
            }

            return genjo.InputLine;
        }
    }
}
