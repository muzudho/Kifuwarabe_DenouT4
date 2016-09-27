﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using System;
using System.Runtime.CompilerServices;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser
{


    /// <summary>
    /// 変化なし
    /// </summary>
    public class KifuParserA_Impl : KifuParserA
    {

        public KifuParserA_State State { get; set; }


        public KifuParserA_Impl()
        {
            // 初期状態＝ドキュメント
            this.State = KifuParserA_StateA0_Document.GetInstance();
        }

        /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        public string Execute_Step_CurrentMutable(
            ref KifuParserA_Result result,
            Earth earth1,
            Tree kifu1_mutable,
            KifuParserA_Genjo genjo,
            KwLogger errH
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
                errH.AppendLine("┏━━━━━┓(^o^)");
                errH.AppendLine("わたしは　" + this.State.GetType().Name + "　の　Execute_Step　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
                errH.Flush(LogTypes.Plain);
#endif

                KifuParserA_State nextState;
                MoveNode curNode1 = kifu1_mutable.CurNode3okok;

                MoveNodeType moveNodeType;
                genjo.InputLine = this.State.Execute(
                    out moveNodeType,
                    ref result,
                    earth1,
                    curNode1.Key,
                    kifu1_mutable.PositionA,// curNode1.GetNodeValue(),
                    out nextState,
                    this,
                    genjo, errH);
                if (MoveNodeType.Clear == moveNodeType)
                {
                    earth1.Clear();

                    curNode1 = kifu1_mutable.OnClearMove(result.NewSky);// 棋譜を空っぽにします。

                    earth1.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面
                }

                if (null!=result.Out_newNode_OrNull)
                {
                    Util_IttesasuRoutine.BeforeUpdateKifuTree(
                        earth1,
                        kifu1_mutable,
                        result.Out_newNode_OrNull.Key,
                        result.NewSky
                        );
                    curNode1 = kifu1_mutable.CurNode3okok;
                    // ■■■■■■■■■■カレント・チェンジ■■■■■■■■■■
                    result.SetNode(curNode1,
                        result.NewSky
                        );
                }
                this.State = nextState;

            }
            catch (Exception ex) {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "棋譜解析中☆");
                throw ex;
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
            ref KifuParserA_Result result,
            Earth earth1,
            Tree kifu1_mutable,
            KifuParserA_Genjo genjo,
            KwLogger errH
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            try
            {
#if DEBUG
                errH.AppendLine("┏━━━━━━━━━━┓");
                errH.AppendLine("わたしは　" + this.State.GetType().Name + "　の　Execute_All　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
                errH.Flush(LogTypes.Plain);
#endif

                KifuParserA_State nextState = this.State;

                MoveNode curNode1 = kifu1_mutable.CurNode3okok;
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
                        curNode1.Key,
                        kifu1_mutable.PositionA,// curNode1.GetNodeValue(), //
                        out nextState,
                        this,
                        genjo, errH);
                    if (MoveNodeType.Clear == moveNodeType)
                    {
                        Sky positionInit = Util_SkyCreator.New_Hirate();
                        earth1.Clear();

                        curNode1 = kifu1_mutable.OnClearMove(positionInit);// 棋譜を空っぽにします。

                        earth1.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面
                    }


                    if (null != result.Out_newNode_OrNull)
                    {
                        Util_IttesasuRoutine.BeforeUpdateKifuTree(
                            earth1,
                            kifu1_mutable,
                            result.Out_newNode_OrNull.Key,
                            result.NewSky
                            );
                        MoveNode newNodeB = kifu1_mutable.CurNode3okok;
                        // ■■■■■■■■■■カレント・チェンジ■■■■■■■■■■
                        result.SetNode(newNodeB,
                            result.NewSky
                            );
                    }


                    this.State = nextState;

                gt_NextLoop1:
                    ;
                }



                //if (null != genjo.StartposImporter_OrNull)
                //{
                //    // SFENの解析結果を渡すので、
                //    // その解析結果をどう使うかは、委譲します。
                //    this.Delegate_OnChangeSky_Im(
                //        model_PnlTaikyoku,
                //        genjo,
                //        errH
                //        );
                //}


            }
            catch (Exception ex) {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "棋譜解析中☆");
                throw ex;
            }
        }

    }
}
