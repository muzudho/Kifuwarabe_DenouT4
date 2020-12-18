using System;
using System.Runtime.CompilerServices;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B640_KifuTree___.C250Struct;
using Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
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
            Earth earth1,
            Tree kifu1_mutable,
            IKifuParserAGenjo genjo,
            ILogger logger
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            //shogiGui_Base.Model_PnlTaikyoku.Kifu.AssertPside(shogiGui_Base.Model_PnlTaikyoku.Kifu.CurNode, "Execute_Step",errH);

            try
            {
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
                    earth1,
                    kifu1_mutable.MoveEx_Current.Move,
                    kifu1_mutable.PositionA,
                    out nextState,
                    this,
                    genjo, logger);
                if (MoveNodeType.Clear == moveNodeType)
                {
                    earth1.Clear();

                    // 棋譜を空っぽにします。
                    Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1_mutable, result.NewSky, logger);

                    earth1.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面
                }

                if (null != result.Out_newNode_OrNull)
                {
                    UtilIttesasuRoutine.BeforeUpdateKifuTree(
                        earth1,
                        kifu1_mutable,
                        result.Out_newNode_OrNull.Move,
                        result.NewSky,
                        logger
                        );
                    // ■■■■■■■■■■カレント・チェンジ■■■■■■■■■■
                    result.SetNode(kifu1_mutable.MoveEx_Current, result.NewSky);
                }
                this.State = nextState;
            }
            catch (Exception ex)
            {
                ErrorControllerReference.ProcessNoneError.DonimoNaranAkirameta(ex, "棋譜解析中☆");
                throw;
            }

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
            Earth earth1,
            Tree kifu1_mutable,
            IKifuParserAGenjo genjo,
            ILogger logger
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            try
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
                        //FIXME: errH.AppendLine_Error("＼（＾ｏ＾）／「" + genjo.InputLine + "」入力がない3☆！　終わるぜ☆");
                        genjo.ToBreak_Abnormal();
                        goto gt_NextLoop1;
                    }

                    MoveNodeType moveNodeType;
                    genjo.InputLine = this.State.Execute(
                        out moveNodeType,
                        ref result,
                        earth1,
                        kifu1_mutable.MoveEx_Current.Move,
                        kifu1_mutable.PositionA,
                        out nextState,
                        this,
                        genjo, logger);
                    if (MoveNodeType.Clear == moveNodeType)
                    {
                        ISky positionInit = UtilSkyCreator.New_Hirate();
                        earth1.Clear();

                        // 棋譜を空っぽにします。
                        Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1_mutable, positionInit, logger);

                        earth1.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面
                    }


                    if (null != result.Out_newNode_OrNull)
                    {
                        UtilIttesasuRoutine.BeforeUpdateKifuTree(
                            earth1,
                            kifu1_mutable,
                            result.Out_newNode_OrNull.Move,
                            result.NewSky,
                            logger
                            );
                        // ■■■■■■■■■■カレント・チェンジ■■■■■■■■■■
                        result.SetNode(kifu1_mutable.MoveEx_Current, result.NewSky);
                    }

                    this.State = nextState;

                gt_NextLoop1:
                    ;
                }
            }
            catch (Exception ex)
            {
                ErrorControllerReference.ProcessNoneError.DonimoNaranAkirameta(ex, "棋譜解析中☆");
                throw;
            }
        }
    }
}
