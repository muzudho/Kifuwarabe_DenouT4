using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using System;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser
{
    /// <summary>
    /// 平手の初期配置です。
    /// </summary>
    public class KifuParserA_StateA1a_SfenStartpos : KifuParserA_State
    {


        public static KifuParserA_StateA1a_SfenStartpos GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1a_SfenStartpos();
            }

            return instance;
        }
        private static KifuParserA_StateA1a_SfenStartpos instance;



        private KifuParserA_StateA1a_SfenStartpos()
        {
        }


        public string Execute(
            out MoveNodeType out_moveNodeType,
            ref KifuParserA_Result result,

            Earth earth1_notUse,
            Move move1_notUse,
            Sky positionA,

            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KwLogger errH
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


                    nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
                }
                else if (""==genjo.InputLine)
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
            catch (Exception ex) {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "SFEN文字列の解析中。");
                throw ex;
            }

            return genjo.InputLine;
        }
    }
}
