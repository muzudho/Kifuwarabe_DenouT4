﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using System;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;

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
            ref KifuParserA_Result result,
            Model_Taikyoku model_Taikyoku,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KwErrorHandler errH
            )
        {
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
                    errH.Logger.WriteLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ﾌﾑﾌﾑ... SFEN形式か...☆", LogTypes.Memo);
#endif
                    genjo.InputLine = genjo.InputLine.Substring("position".Length);
                    genjo.InputLine = genjo.InputLine.Trim();


                    nextState = KifuParserA_StateA1_SfenPosition.GetInstance();
                }
                else if (""==genjo.InputLine)
                {
                    // 異常時。
                    errH.Logger.WriteLine("＼（＾ｏ＾）／「" + genjo.InputLine + "」入力がない2☆！　終わるぜ☆", LogTypes.Error);
                    genjo.ToBreak_Abnormal();
                }
                else
                {
#if DEBUG
                    Playerside pside = model_Taikyoku.Kifu.CurNode.Value.Kyokumen.KaisiPside;
                    errH.Logger.WriteLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ﾌﾑﾌﾑ... positionじゃなかったぜ☆　日本式か☆？　SFENでmovesを読んだあとのプログラムに合流させるぜ☆　：　先後＝[" + pside + "]", LogTypes.Memo);
#endif
                    nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
                }

            }
            catch (Exception ex) { Util_Loggers.ERROR.Logger.DonimoNaranAkirameta(ex, "棋譜ドキュメント解析中☆"); throw ex; }


            return genjo.InputLine;
        }

    }
}
