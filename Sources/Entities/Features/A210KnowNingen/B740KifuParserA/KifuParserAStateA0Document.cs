using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;

#if DEBUG
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
#endif

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
{

    /// <summary>
    /// 変化なし
    /// </summary>
    public class KifuParserAStateA0Document : IKifuParserAState
    {


        public static KifuParserAStateA0Document GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserAStateA0Document();
            }

            return instance;
        }
        private static KifuParserAStateA0Document instance;



        private KifuParserAStateA0Document()
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
                    logTag.AppendLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ﾌﾑﾌﾑ... SFEN形式か...☆");
                    logTag.Flush(LogTypes.Plain);
#endif
                    genjo.InputLine = genjo.InputLine.Substring("position".Length);
                    genjo.InputLine = genjo.InputLine.Trim();


                    nextState = KifuParserAStateA1SfenPosition.GetInstance();
                }
                else if ("" == genjo.InputLine)
                {
                    // 異常時。
                    Logger.AppendLine(logTag,"＼（＾ｏ＾）／「" + genjo.InputLine + "」入力がない2☆！　終わるぜ☆");
                    Logger.Flush(logTag,LogTypes.Error);
                    genjo.ToBreak_Abnormal();
                }
                else
                {
#if DEBUG
                    Playerside pside = positionA.GetKaisiPside();//.KaisiPside;
                    logTag.AppendLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ﾌﾑﾌﾑ... positionじゃなかったぜ☆　日本式か☆？　SFENでmovesを読んだあとのプログラムに合流させるぜ☆　：　先後＝[" + pside + "]");
                    logTag.Flush(LogTypes.Plain);
#endif
                    nextState = KifuParserAStateA2SfenMoves.GetInstance();
                }

            }
            catch (Exception ex)
            {
                Logger.Panic(LogTags.ProcessNoneError,ex, "棋譜ドキュメント解析中☆");
                throw;
            }


            return genjo.InputLine;
        }

    }
}
