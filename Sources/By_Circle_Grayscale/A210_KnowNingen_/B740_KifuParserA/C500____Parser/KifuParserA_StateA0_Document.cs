using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using System;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

#if DEBUG
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
#endif

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser
{

    /// <summary>
    /// 変化なし
    /// </summary>
    public class KifuParserA_StateA0_Document : KifuParserA_State
    {


        public static KifuParserA_StateA0_Document GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA0_Document();
            }

            return instance;
        }
        private static KifuParserA_StateA0_Document instance;



        private KifuParserA_StateA0_Document()
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

                if (genjo.InputLine.StartsWith("position"))
                {
                    // SFEN形式の「position」コマンドが、入力欄に入っていました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    //------------------------------------------------------------
                    // まずこのブロックで「position ～ moves 」まで(*1)を処理します。
                    //------------------------------------------------------------
                    //
                    //          *1…初期配置を作るということです。
                    // 

#if DEBUG
                    errH.AppendLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ﾌﾑﾌﾑ... SFEN形式か...☆");
                    errH.Flush(LogTypes.Plain);
#endif
                    genjo.InputLine = genjo.InputLine.Substring("position".Length);
                    genjo.InputLine = genjo.InputLine.Trim();


                    nextState = KifuParserA_StateA1_SfenPosition.GetInstance();
                }
                else if (""==genjo.InputLine)
                {
                    // 異常時。
                    errH.AppendLine("＼（＾ｏ＾）／「" + genjo.InputLine + "」入力がない2☆！　終わるぜ☆");
                    errH.Flush(LogTypes.Error);
                    genjo.ToBreak_Abnormal();
                }
                else
                {
#if DEBUG
                    Playerside pside = positionA.KaisiPside;
                    errH.AppendLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ﾌﾑﾌﾑ... positionじゃなかったぜ☆　日本式か☆？　SFENでmovesを読んだあとのプログラムに合流させるぜ☆　：　先後＝[" + pside + "]");
                    errH.Flush(LogTypes.Plain);
#endif
                    nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
                }

            }
            catch (Exception ex) {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "棋譜ドキュメント解析中☆");
                throw ex;
            }


            return genjo.InputLine;
        }

    }
}
