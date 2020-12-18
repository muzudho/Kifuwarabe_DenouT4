﻿using System;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
{

    /// <summary>
    /// 「position」を読込みました。
    /// </summary>
    public class KifuParserAStateA1SfenPosition : IKifuParserAState
    {


        public static KifuParserAStateA1SfenPosition GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserAStateA1SfenPosition();
            }

            return instance;
        }
        private static KifuParserAStateA1SfenPosition instance;


        private KifuParserAStateA1SfenPosition()
        {
        }


        public string Execute(
            out MoveNodeType out_moveNodeType,
            ref IKifuParserAResult result,

            Earth earth1,
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

                    nextState = KifuParserA_StateA1aSfenStartpos.GetInstance();
                }
                else
                {
                    //#if DEBUG
                    errH.AppendLine("（＾△＾）ここはスルーして次に状態遷移するんだぜ☆\n「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】");//　：　局面の指定のようなんだぜ☆　対応していない☆？
                    errH.Flush(LogTypes.Error);
                    //errH.AppendLine_Error("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　局面の指定のようなんだぜ☆　対応していない☆？");
                    //#endif
                    nextState = KifuParserAStateA1bSfenLnsgkgsnl.GetInstance();
                }
            }
            catch (Exception ex)
            {
                ErrorControllerReference.ProcessNoneError.DonimoNaranAkirameta(ex, "positionの解析中。");
                throw;
            }

            return genjo.InputLine;
        }

    }
}
