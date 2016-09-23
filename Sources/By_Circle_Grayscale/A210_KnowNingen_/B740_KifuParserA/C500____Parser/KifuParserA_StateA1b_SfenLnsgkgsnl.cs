using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B520_SeizaStartp.C500____Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using System;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser
{
    /// <summary>
    /// 指定局面から始める配置です。
    /// 
    /// 「lnsgkgsnl/1r5b1/ppppppppp/9/9/6P2/PPPPPP1PP/1B5R1/LNSGKGSNL w - 1」といった文字の読込み
    /// </summary>
    public class KifuParserA_StateA1b_SfenLnsgkgsnl : KifuParserA_State
    {


        public static KifuParserA_StateA1b_SfenLnsgkgsnl GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1b_SfenLnsgkgsnl();
            }

            return instance;
        }
        private static KifuParserA_StateA1b_SfenLnsgkgsnl instance;



        private KifuParserA_StateA1b_SfenLnsgkgsnl()
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

                errH.AppendLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　さて、どんな内容なんだぜ☆？");
                errH.Flush(LogTypes.Error);

                StartposImporter startposImporter1;
                string restText;

                bool successful = StartposImporter.TryParse(
                    genjo.InputLine,
                    out startposImporter1,
                    out restText
                    );
                genjo.StartposImporter_OrNull = startposImporter1;
                errH.AppendLine("（＾△＾）restText=「" + restText + "」 successful=【" + successful + "】");
                errH.Flush(LogTypes.Error);

                if (successful)
                {
                    genjo.InputLine = restText;

                    nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
                }
                else
                {
                    // 解析に失敗しました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    genjo.ToBreak_Abnormal();
                }

            }
            catch (Exception ex) {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "SFEN解析中☆");
                throw ex;
            }

            return genjo.InputLine;
        }

    }
}
