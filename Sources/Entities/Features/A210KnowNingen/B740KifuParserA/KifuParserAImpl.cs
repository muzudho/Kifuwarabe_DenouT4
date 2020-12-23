using System;
using System.Runtime.CompilerServices;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{


    /// <summary>
    /// 変化なし
    /// </summary>
    public class KifuParserAImpl : IKifuParserA
    {

        public IKifuParserAState State { get; set; }


        public KifuParserAImpl()
        {
            // 初期状態＝ドキュメント
            this.State = KifuParserAStateA0Document.GetInstance();
        }

        /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        public string Execute_Step_CurrentMutable(
            ref IKifuParserAResult result,
            IPlaying playing,
            Tree kifu1_mutable,
            IKifuParserAGenjo genjo
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            //shogiGui_Base.Model_PnlTaikyoku.Kifu.AssertPside(shogiGui_Base.Model_PnlTaikyoku.Kifu.CurNode, "Execute_Step",logTag);

#if DEBUG
                logger.AppendLine("┏━━━━━┓(^o^)");
                logger.AppendLine("わたしは　" + this.State.GetType().Name + "　の　Execute_Step　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
                logger.Flush(LogTypes.Plain);
#endif

            IKifuParserAState nextState;

            MoveNodeType moveNodeType;
            genjo.InputLine = this.State.Execute(
                out moveNodeType,
                ref result,
                playing,
                kifu1_mutable.MoveEx_Current.Move,
                kifu1_mutable.PositionA,
                out nextState,
                this,
                genjo);
            if (MoveNodeType.Clear == moveNodeType)
            {
                playing.ClearEarth();

                // 棋譜を空っぽにします。
                Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1_mutable, result.NewSky);

                playing.SetEarthProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面
            }

            if (null != result.Out_newNode_OrNull)
            {
                UtilIttesasuRoutine.BeforeUpdateKifuTree(
                    playing,
                    kifu1_mutable,
                    result.Out_newNode_OrNull.Move,
                    result.NewSky);
                // ■■■■■■■■■■カレント・チェンジ■■■■■■■■■■
                result.SetNode(kifu1_mutable.MoveEx_Current, result.NewSky);
            }
            this.State = nextState;

            return genjo.InputLine;
        }

        /// <summary>
        /// 最初から最後まで実行します。（きふわらべCOMP用）
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        public void Execute_All_CurrentMutable(
            ref IKifuParserAResult result,
            IPlaying playing,
            Tree kifu1_mutable,
            IKifuParserAGenjo genjo
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
#if DEBUG
                logger.AppendLine("┏━━━━━━━━━━┓");
                logger.AppendLine("わたしは　" + this.State.GetType().Name + "　の　Execute_All　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
                logger.Flush(LogTypes.Plain);
#endif

            IKifuParserAState nextState = this.State;

            while (!genjo.IsBreak())//breakするまでくり返し。
            {
                if ("" == genjo.InputLine)
                {
                    // FIXME: コンピューターが先手のとき、ここにくる？

                    // 異常時。
                    //FIXME: logTag.AppendLine_Error("＼（＾ｏ＾）／「" + genjo.InputLine + "」入力がない3☆！　終わるぜ☆");
                    genjo.ToBreak_Abnormal();
                    goto gt_NextLoop1;
                }

                MoveNodeType moveNodeType;
                genjo.InputLine = this.State.Execute(
                    out moveNodeType,
                    ref result,
                    playing,
                    kifu1_mutable.MoveEx_Current.Move,
                    kifu1_mutable.PositionA,
                    out nextState,
                    this,
                    genjo);
                if (MoveNodeType.Clear == moveNodeType)
                {
                    ISky positionInit = UtilSkyCreator.New_Hirate();
                    playing.ClearEarth();

                    // 棋譜を空っぽにします。
                    Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1_mutable, positionInit);

                    playing.SetEarthProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面
                }


                if (null != result.Out_newNode_OrNull)
                {
                    UtilIttesasuRoutine.BeforeUpdateKifuTree(
                        playing,
                        kifu1_mutable,
                        result.Out_newNode_OrNull.Move,
                        result.NewSky);
                    // ■■■■■■■■■■カレント・チェンジ■■■■■■■■■■
                    result.SetNode(kifu1_mutable.MoveEx_Current, result.NewSky);
                }

                this.State = nextState;

            gt_NextLoop1:
                ;
            }
        }
    }
}
