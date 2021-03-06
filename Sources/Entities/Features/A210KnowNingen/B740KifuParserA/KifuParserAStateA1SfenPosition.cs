﻿using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
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
            IPlaying playing,
            Move move1_notUse,
            IPosition positionA,

            out IKifuParserAState nextState,
            IKifuParserA owner,
            IKifuParserAGenjo genjo
            )
        {
            out_moveNodeType = MoveNodeType.None;
            nextState = this;

            if (genjo.InputLine.StartsWith("startpos"))
            {
                // 平手の初期配置です。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

#if DEBUG
                    //logTag.AppendLine("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　平手のようなんだぜ☆");
                    //logTag.Flush(LogTypes.Plain);
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
                //　：　局面の指定のようなんだぜ☆　対応していない☆？
                Logger.Error($"（＾△＾）ここはスルーして次に状態遷移するんだぜ☆\n「{genjo.InputLine}」vs【{this.GetType().Name}】");
                //logTag.AppendLine_Error("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　局面の指定のようなんだぜ☆　対応していない☆？");
                //#endif
                nextState = KifuParserAStateA1bSfenLnsgkgsnl.GetInstance();
            }

            return genjo.InputLine;
        }

    }
}
