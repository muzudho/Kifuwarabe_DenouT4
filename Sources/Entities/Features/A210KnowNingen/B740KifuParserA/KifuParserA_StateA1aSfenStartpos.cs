using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.Entities.Features
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
            ILogTag logTag
            )
        {
            out_moveNodeType = MoveNodeType.None;
            nextState = this;

            if (genjo.InputLine.StartsWith("moves"))
            {
                //>>>>> 棋譜が始まります。
#if DEUBG
                    logTag.AppendLine_AddMemo("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ｳﾑ☆　moves 分かるぜ☆");
#endif

                genjo.InputLine = genjo.InputLine.Substring("moves".Length);
                genjo.InputLine = genjo.InputLine.Trim();


                nextState = KifuParserAStateA2SfenMoves.GetInstance();
            }
            else if ("" == genjo.InputLine)
            {
                // TODO: コンピューターが先手のとき、ここにくる？
                // FIXME: コンピューターが先手のとき、ここにくる？

                // 異常時。
                throw new Exception($"＼（＾ｏ＾）／「{genjo.InputLine}」入力がない1☆！　終わるぜ☆");
            }
            else
            {
                // 異常時。
                throw new Exception($"＼（＾ｏ＾）／「{genjo.InputLine}」vs【{this.GetType().Name}】　：　movesがない☆！　終わるぜ☆");
            }

            return genjo.InputLine;
        }
    }
}
