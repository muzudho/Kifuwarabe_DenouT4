using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using System;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser
{

    /// <summary>
    /// 「position」を読込みました。
    /// </summary>
    public class KifuParserA_StateA1_SfenPosition : KifuParserA_State
    {


        public static KifuParserA_StateA1_SfenPosition GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1_SfenPosition();
            }

            return instance;
        }
        private static KifuParserA_StateA1_SfenPosition instance;


        private KifuParserA_StateA1_SfenPosition()
        {
        }


        public string Execute(
            out MoveNodeType out_moveNodeType,
            ref KifuParserA_Result result,

            Earth earth1,
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
                if (genjo.InputLine.StartsWith("startpos"))
                {
                    // 平手の初期配置です。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

#if DEBUG
                    errH.AppendLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　平手のようなんだぜ☆");
                    errH.Flush(LogTypes.Plain);
#endif

                    genjo.InputLine = genjo.InputLine.Substring("startpos".Length);
                    genjo.InputLine = genjo.InputLine.Trim();

                    //----------------------------------------
                    // 棋譜を空っぽにし、平手初期局面を与えます。
                    //----------------------------------------
                    out_moveNodeType = MoveNodeType.Clear;

                    nextState = KifuParserA_StateA1a_SfenStartpos.GetInstance();
                }
                else
                {
//#if DEBUG
                    errH.AppendLine("（＾△＾）ここはスルーして次に状態遷移するんだぜ☆\n「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】");//　：　局面の指定のようなんだぜ☆　対応していない☆？
                    errH.Flush(LogTypes.Error);
                    //errH.AppendLine_Error("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　局面の指定のようなんだぜ☆　対応していない☆？");
                    //#endif
                    nextState = KifuParserA_StateA1b_SfenLnsgkgsnl.GetInstance();
                }
            }
            catch (Exception ex) {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "positionの解析中。");
                throw ex;
            }

            return genjo.InputLine;
        }

    }
}
