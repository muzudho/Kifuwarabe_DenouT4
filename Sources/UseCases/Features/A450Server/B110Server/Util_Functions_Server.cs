using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

#if DEBUG
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{


    public class Util_Functions_Server
    {
        /// <summary>
        /// 「棋譜ツリーのカレントノード」の差替え、
        /// および
        /// 「ＧＵＩ用局面データ」との同期。
        /// 
        /// (1) 駒をつまんでいるときに、マウスの左ボタンを放したとき。
        /// (2) 駒の移動先の升の上で、マウスの左ボタンを放したとき。
        /// (3) 成る／成らないダイアログボックスが出たときに、マウスの左ボタンを押下したとき。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="newNode"></param>
        public static void AfterSetCurNode_Srv(
            SkyWrapper_Gui model_Manual,
            MoveEx newNodeA,
            Move move,
            ISky positionA,
            out string jsaFugoStr,
            Tree kifu1)
        {
            model_Manual.SetGuiSky(positionA);

            jsaFugoStr = ConvMoveStrJsa.ToMoveStrJsa(
                move,
                kifu1.Pv_ToList(),
                positionA);
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// １手進む
        /// ************************************************************************************************************************
        /// 
        /// ＜棋譜読込用＞＜マウス入力非対応＞
        /// 
        /// 「棋譜並べモード」と「vsコンピューター対局」でも、これを使いますが、
        /// 「棋譜並べモード」では送られてくる SFEN が「position startpos moves 8c8d」とフルであるのに対し、
        /// 「vsコンピューター対局」では、送られてくる SFEN が「8c8d」だけです。
        /// 
        /// それにより、処理の流れが異なります。
        /// 
        /// </summary>
        public static bool ReadLine_TuginoItteSusumu_Srv_CurrentMutable(
            ref string inputLine,

            Earth earth1,
            Tree kifu1,//SetCurNodeがある。[コマ送り][再生]などで使用。

            SkyWrapper_Gui model_Manual,
            out bool toBreak,
            string hint,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            //ILogger logTag = OwataMinister.SERVER_KIFU_YOMITORI;

            bool successful = false;
            KifuParserAImpl kifuParserA_Impl = new KifuParserAImpl();
            IKifuParserAResult result = new KifuParserA_ResultImpl();
            IKifuParserAGenjo genjo = new KifuParserA_GenjoImpl(inputLine);

            try
            {

                if (kifuParserA_Impl.State is KifuParserAStateA0Document)
                {
                    // 最初はここ

#if DEBUG
                    logger.AppendLine("(^o^)... ...");
                    logger.AppendLine("ｻｲｼｮﾊｺｺ☆　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
                    logger.Flush(LogTypes.Plain);
#endif
                    inputLine = kifuParserA_Impl.Execute_Step_CurrentMutable(
                        ref result,
                        earth1,
                        kifu1,
                        genjo);

                    Debug.Assert(result.Out_newNode_OrNull == null, "ここでノードに変化があるのはおかしい。");

                    if (genjo.IsBreak())
                    {
                        goto gt_EndMethod;
                    }
                    // （１）position コマンドを処理しました。→startpos
                    // （２）日本式棋譜で、何もしませんでした。→moves
                }

                if (kifuParserA_Impl.State is KifuParserAStateA1SfenPosition)
                {
                    //------------------------------------------------------------
                    // このブロックでは「position ～ moves 」まで一気に(*1)を処理します。
                    //------------------------------------------------------------
                    //
                    //          *1…初期配置を作るということです。
                    // 

                    {
#if DEBUG
                        string message = "(^o^)ﾂｷﾞﾊ　ﾋﾗﾃ　ﾏﾀﾊ　ｼﾃｲｷｮｸﾒﾝ　ｦ　ｼｮﾘｼﾀｲ☆ inputLine=[" + inputLine + "]";
                        logger.AppendLine(message);
                        logger.Flush(LogTypes.Plain);
#endif

                        inputLine = kifuParserA_Impl.Execute_Step_CurrentMutable(
                            ref result,
                            earth1,
                            kifu1,
                            genjo);
                        Debug.Assert(result.Out_newNode_OrNull == null, "ここでノードに変化があるのはおかしい。");


                        if (genjo.IsBreak())
                        {
                            goto gt_EndMethod;
                        }
                    }


                    {
#if DEBUG
                        logger.AppendLine("(^o^)ﾂｷﾞﾊ　ﾑｰﾌﾞｽ　ｦ　ｼｮﾘｼﾀｲ☆");
                        logger.Flush(LogTypes.Plain);
#endif

                        inputLine = kifuParserA_Impl.Execute_Step_CurrentMutable(
                            ref result,
                            earth1,
                            kifu1,
                            genjo);
                        Debug.Assert(result.Out_newNode_OrNull == null, "ここでノードに変化があるのはおかしい。");


                        if (genjo.IsBreak())
                        {
                            goto gt_EndMethod;
                        }
                        // moves を処理しました。
                        // ここまでで、「position ～ moves 」といった記述が入力されていたとすれば、入力欄から削除済みです。
                    }


                    // →moves
                }

                //
                // 対COMP戦で関係があるのはここです。
                //
                if (kifuParserA_Impl.State is KifuParserAStateA2SfenMoves)
                {
#if DEBUG
                    logTag.AppendLine("ﾂｷﾞﾊ　ｲｯﾃ　ｼｮﾘｼﾀｲ☆");
                    logTag.Flush(LogTypes.Plain);
#endif

                    inputLine = kifuParserA_Impl.Execute_Step_CurrentMutable(
                        ref result,
                        earth1,
                        kifu1,
                        genjo);

                    if (null != result.Out_newNode_OrNull)
                    {
                        string jsaFugoStr;

                        //× kifu1.Pv_Append(result.Out_newNode_OrNull.Move);
                        //kifu1.MoveEx_SetCurrent(TreeImpl.DoCurrentMove(result.Out_newNode_OrNull, kifu1, result.NewSky,logger));
                        //kifu1.OnDoCurrentMove(result.Out_newNode_OrNull, result.NewSky);

                        Util_Functions_Server.AfterSetCurNode_Srv(
                            model_Manual,
                            result.Out_newNode_OrNull,
                            result.Out_newNode_OrNull.Move,
                            result.NewSky,
                            out jsaFugoStr,
                            kifu1);
                    }

                    if (genjo.IsBreak())
                    {
                        goto gt_EndMethod;
                    }


                    // １手を処理した☆？
                }


                if (null != genjo.StartposImporter_OrNull)
                {
                    // 初期配置が平手でないとき。
                    // ************************************************************************************************************************
                    // SFENの初期配置の書き方(*1)を元に、駒を並べます。
                    // ************************************************************************************************************************
                    // 
                    //     *1…「position startpos moves 7g7f 3c3d 2g2f」といった書き方。
                    //     
                    ParsedKyokumen parsedKyokumen = Conv_StartposImporter.ToParsedKyokumen(
                        model_Manual,
                        genjo.StartposImporter_OrNull,//指定されているはず。
                        genjo);

                    //------------------------------
                    // 駒の配置
                    //------------------------------
                    string jsaFugoStr;

                    MoveEx curNode1 = new MoveExImpl(parsedKyokumen.NewMove);

                    Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1, parsedKyokumen.NewSky);
                    curNode1 = kifu1.MoveEx_Current;

                    Util_Functions_Server.AfterSetCurNode_Srv(
                        model_Manual,
                        curNode1,
                        parsedKyokumen.NewMove,
                        parsedKyokumen.NewSky,
                        out jsaFugoStr,
                        kifu1);// GUIに通知するだけ。
                }


                successful = true;
            }
            catch (Exception ex)
            {
                Util_Message.Show(ex.GetType().Name + "：" + ex.Message);
                toBreak = true;
                successful = false;
            }

        gt_EndMethod:
            toBreak = genjo.IsBreak();
            return successful;
        }






        /// <summary>
        /// ************************************************************************************************************************
        /// [巻戻し]ボタン
        /// ************************************************************************************************************************
        /// </summary>
        public static bool Makimodosi_Srv(
            out Finger movedKoma,
            out Finger foodKoma,
            out string jsaFugoStr,
            MoveEx curNode1,//削るノード
            Tree kifu1_mutable)
        {
            bool successful = false;

            //------------------------------
            // 棋譜から１手削ります
            //------------------------------
            ISky positionA = kifu1_mutable.PositionA;// curNode1.GetNodeValue();
            int korekaranoTemezumi = positionA.Temezumi - 1;//１手前へ。

            if (kifu1_mutable.Pv_IsRoot())// curNode1.IsRoot(kifu1_mutable,logger)
            {
                // ルート
                jsaFugoStr = "×";
                movedKoma = Fingers.Error_1;
                foodKoma = Fingers.Error_1;
                goto gt_EndMethod;
            }


            //------------------------------
            // 符号
            //------------------------------
            // [巻戻し]ボタン
            jsaFugoStr = ConvMoveStrJsa.ToMoveStrJsa(
                curNode1.Move,
                kifu1_mutable.Pv_ToList(),
                positionA);




            //------------------------------
            // 前の手に戻します
            //------------------------------
            IIttemodosuResult ittemodosuResult;
            UtilIttemodosuRoutine.UndoMove(
                out ittemodosuResult,
                curNode1.Move,
                ConvMove.ToPlayerside(curNode1.Move),
                positionA,
                "B");

            kifu1_mutable.MoveEx_SetCurrent(
                TreeImpl.OnUndoCurrentMove(kifu1_mutable, ittemodosuResult.SyuryoSky, "Makimodosi_Srv30000")
            );

            movedKoma = ittemodosuResult.FigMovedKoma;
            foodKoma = ittemodosuResult.FigFoodKoma;

            successful = true;


        gt_EndMethod:
            return successful;
        }





        /// <summary>
        /// ************************************************************************************************************************
        /// [コマ送り]ボタン
        /// ************************************************************************************************************************
        /// 
        /// vsコンピューター対局でも、タイマーによって[コマ送り]が実行されます。
        /// 
        /// </summary>
        public static bool Komaokuri_Srv(
            ref string inputLine,

            Earth earth1,
            Tree kifu1,

            SkyWrapper_Gui model_Manual
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {

            if ("" == inputLine)
            {
                goto gt_EndMethod;
            }

            bool toBreak = false;
            Util_Functions_Server.ReadLine_TuginoItteSusumu_Srv_CurrentMutable(
                ref inputLine,
                earth1,
                kifu1,//SetCurNodeがある。
                model_Manual,
                out toBreak,
                "hint");

        gt_EndMethod:
            ;
            return true;
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// 駒を動かします(1)。マウスボタンが押下されたとき。
        /// ************************************************************************************************************************
        /// 
        /// 成る、成らない関連。
        /// 
        /// </summary>
        public static void Komamove1a_50Srv(
            out bool torareruKomaAri,
            out Busstop koma_Food_after,
            Busstop dst,
            Finger fig_btnTumandeiruKoma,
            Busstop foodee_koma,//取られる対象の駒
            SkyWrapper_Gui model_Manual)
        {

            Finger btnKoma_Food_Koma;



            // 取られることになる駒のボタン
            btnKoma_Food_Koma = UtilSkyFingersQuery.InMasuNow_Old(model_Manual.GuiSky, Conv_Busstop.ToMasu(foodee_koma)).ToFirst();
            if (Fingers.Error_1 == btnKoma_Food_Koma)
            {
                koma_Food_after = Busstop.Empty;
                torareruKomaAri = false;
                btnKoma_Food_Koma = Fingers.Error_1;
                goto gt_EndBlock1;
            }
            else
            {
                //>>>>> 取る駒があったとき
                torareruKomaAri = true;
            }




            model_Manual.GuiSky.AssertFinger(btnKoma_Food_Koma);
            Komasyurui14 koma_Food_pre_Syurui = Conv_Busstop.ToKomasyurui(model_Manual.GuiSky.BusstopIndexOf(btnKoma_Food_Koma));


            // その駒は、駒置き場に移動させます。
            SyElement akiMasu;
            switch (Conv_Busstop.ToPlayerside(foodee_koma))
            {
                case Playerside.P2:

                    akiMasu = UtilIttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, model_Manual.GuiSky);
                    if (!Masu_Honshogi.IsErrorBasho(akiMasu))
                    {
                        // 駒台に空きスペースがありました。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        koma_Food_after = Conv_Busstop.ToBusstop(
                            Playerside.P2,
                            akiMasu,//駒台へ
                            Util_Komasyurui14.NarazuCaseHandle(koma_Food_pre_Syurui)
                        );
                    }
                    else
                    {
                        // エラー：　駒台に空きスペースがありませんでした。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        koma_Food_after = Conv_Busstop.ToBusstop(
                            Playerside.P2,
                            Conv_Masu.ToMasu_FromBangaiSujiDan(
                                Okiba.Gote_Komadai,
                                Util_Koma.CTRL_NOTHING_PROPERTY_SUJI,
                                Util_Koma.CTRL_NOTHING_PROPERTY_DAN
                            ),
                            Util_Komasyurui14.NarazuCaseHandle(koma_Food_pre_Syurui)
                        );
                    }

                    break;

                case Playerside.P1://thru
                default:

                    akiMasu = UtilIttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, model_Manual.GuiSky);
                    if (!Masu_Honshogi.IsErrorBasho(akiMasu))
                    {
                        // 駒台に空きスペースがありました。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        koma_Food_after = Conv_Busstop.ToBusstop(
                            Playerside.P1,
                            akiMasu,//駒台へ
                            Util_Komasyurui14.NarazuCaseHandle(koma_Food_pre_Syurui)
                        );
                    }
                    else
                    {
                        // エラー：　駒台に空きスペースがありませんでした。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        koma_Food_after = Conv_Busstop.ToBusstop(
                            Playerside.P1,
                            Conv_Masu.ToMasu_FromBangaiSujiDan(
                                Okiba.Sente_Komadai,
                                Util_Koma.CTRL_NOTHING_PROPERTY_SUJI,
                                Util_Koma.CTRL_NOTHING_PROPERTY_DAN
                            ),
                            Util_Komasyurui14.NarazuCaseHandle(koma_Food_pre_Syurui)
                        );
                    }

                    break;
            }



        gt_EndBlock1:


            if (btnKoma_Food_Koma != Fingers.Error_1)
            {
                //------------------------------
                // 取られる駒があった場合
                //------------------------------
                model_Manual.GuiSky.AddObjects(
                    //
                    // 指した駒と、取られた駒
                    //
                    new Finger[] { fig_btnTumandeiruKoma, btnKoma_Food_Koma },
                    new Busstop[] { dst, koma_Food_after }
                    );
            }
            else
            {
                //------------------------------
                // 取られる駒がなかった場合
                //------------------------------
                model_Manual.GuiSky.AssertFinger(fig_btnTumandeiruKoma);
                Busstop movedKoma = model_Manual.GuiSky.BusstopIndexOf(fig_btnTumandeiruKoma);

                model_Manual.GuiSky.AddObjects(
                    //
                    // 指した駒
                    //
                    new Finger[] { fig_btnTumandeiruKoma }, new Busstop[] { dst }
                    );
            }

            model_Manual.SetGuiSky(model_Manual.GuiSky);
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            // 棋譜は変更された。
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        }


    }


}
