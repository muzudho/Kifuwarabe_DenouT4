using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using System;

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
            ref KifuParserA_Result result,
            Model_Taikyoku model_Taikyoku,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KwLogger errH
            )
        {
            nextState = this;

            try
            {
                if (genjo.InputLine.StartsWith("startpos"))
                {
                    // 平手の初期配置です。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

#if DEBUG
                    errH.WriteLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　平手のようなんだぜ☆", LogTypes.Memo);
#endif

                    genjo.InputLine = genjo.InputLine.Substring("startpos".Length);
                    genjo.InputLine = genjo.InputLine.Trim();

                    //----------------------------------------
                    // 棋譜を空っぽにし、平手初期局面を与えます。
                    //----------------------------------------
                    {
                        model_Taikyoku.Kifu.Clear();// 棋譜を空っぽにします。

                        model_Taikyoku.Kifu.GetRoot().Value.SetKyokumen(
                            Util_SkyWriter.New_Hirate());//SFENのstartpos解析時
                        model_Taikyoku.Kifu.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面
                    }

                    nextState = KifuParserA_StateA1a_SfenStartpos.GetInstance();
                }
                else
                {
//#if DEBUG
                    errH.WriteLine("（＾△＾）ここはスルーして次に状態遷移するんだぜ☆\n「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】",LogTypes.Error);//　：　局面の指定のようなんだぜ☆　対応していない☆？
                    //errH.WriteLine_Error("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　局面の指定のようなんだぜ☆　対応していない☆？");
//#endif
                    nextState = KifuParserA_StateA1b_SfenLnsgkgsnl.GetInstance();
                }
            }
            catch (Exception ex) { Util_Loggers.ERROR.DonimoNaranAkirameta(ex, "positionの解析中。"); throw ex; }

            return genjo.InputLine;
        }

    }
}
