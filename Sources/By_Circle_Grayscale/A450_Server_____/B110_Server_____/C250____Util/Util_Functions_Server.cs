using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B210_Tushin_____.C500____Util;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_Masu_______.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C500____Util;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser;
using Grayscale.A210_KnowNingen_.B820_KyokuParser.C___500_Parser;
using Grayscale.A210_KnowNingen_.B830_ConvStartpo.C500____Converter;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A450_Server_____.B110_Server_____.C250____Util
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
        public static void SetCurNode_Srv(
            Model_Taikyoku model_Taikyoku,// Taikyokuの内容をManualへ移す。
            Model_Manual model_Manual,
            Node<Move, KyokumenWrapper> newNode,
            out string jsaFugoStr,
            KwErrorHandler errH
            )
        {
            Debug.Assert(null != newNode, "新規ノードがヌル。");

            model_Taikyoku.Kifu.SetCurNode(newNode);

            model_Manual.SetGuiSky(newNode.Value.Kyokumen);
            model_Manual.GuiTemezumi = model_Taikyoku.Kifu.CurNode.Value.Kyokumen.Temezumi;
            model_Manual.GuiPside = model_Taikyoku.Kifu.CurNode.Value.Kyokumen.KaisiPside;

            jsaFugoStr = Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(newNode, errH);
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
        public static bool ReadLine_TuginoItteSusumu_Srv(
            ref string inputLine,
            Model_Taikyoku model_Taikyoku,//SetCurNodeがある。[コマ送り][再生]などで使用。
            Model_Manual model_Manual,
            out bool toBreak,
            string hint,
            KwErrorHandler errH
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            //KwErrorHandler errH = OwataMinister.SERVER_KIFU_YOMITORI;

            bool successful = false;
            KifuParserA_Impl kifuParserA_Impl = new KifuParserA_Impl();
            KifuParserA_Result result = new KifuParserA_ResultImpl();
            KifuParserA_Genjo genjo = new KifuParserA_GenjoImpl( inputLine);

            try
            {

                if (kifuParserA_Impl.State is KifuParserA_StateA0_Document)
                {
                    // 最初はここ

#if DEBUG
                    errH.Logger.WriteLine_AddMemo("(^o^)... ...");
                    errH.Logger.WriteLine_AddMemo("ｻｲｼｮﾊｺｺ☆　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
#endif
                    inputLine = kifuParserA_Impl.Execute_Step(
                        ref result,
                        model_Taikyoku,
                        genjo,
                        errH
                        );

                    Debug.Assert(result.Out_newNode_OrNull == null, "ここでノードに変化があるのはおかしい。");

                    if (genjo.IsBreak())
                    {
                        goto gt_EndMethod;
                    }
                    // （１）position コマンドを処理しました。→startpos
                    // （２）日本式棋譜で、何もしませんでした。→moves
                }

                if (kifuParserA_Impl.State is KifuParserA_StateA1_SfenPosition)
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
                        errH.Logger.WriteLine_AddMemo(message);
#endif

                        inputLine = kifuParserA_Impl.Execute_Step(
                            ref result,
                            model_Taikyoku,
                            genjo,
                            errH
                            );
                        Debug.Assert(result.Out_newNode_OrNull == null, "ここでノードに変化があるのはおかしい。");


                        if (genjo.IsBreak())
                        {
                            goto gt_EndMethod;
                        }
                    }


                    {
#if DEBUG
                        errH.Logger.WriteLine_AddMemo("(^o^)ﾂｷﾞﾊ　ﾑｰﾌﾞｽ　ｦ　ｼｮﾘｼﾀｲ☆");
#endif

                        inputLine = kifuParserA_Impl.Execute_Step(
                            ref result,
                            model_Taikyoku,
                            genjo,
                            errH
                            );
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
                if (kifuParserA_Impl.State is KifuParserA_StateA2_SfenMoves)
                {
#if DEBUG
                    errH.Logger.WriteLine_AddMemo("ﾂｷﾞﾊ　ｲｯﾃ　ｼｮﾘｼﾀｲ☆");
#endif

                    inputLine = kifuParserA_Impl.Execute_Step(
                        ref result,
                        model_Taikyoku,
                        genjo,
                        errH
                        );

                    if (null != result.Out_newNode_OrNull)
                    {
                        string jsaFugoStr;
                        Util_Functions_Server.SetCurNode_Srv(model_Taikyoku, model_Manual, result.Out_newNode_OrNull, out jsaFugoStr, errH);
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
                        genjo,
                        errH
                        );

                    //------------------------------
                    // 駒の配置
                    //------------------------------
                    string jsaFugoStr;
                    Util_Functions_Server.SetCurNode_Srv(model_Taikyoku, model_Manual, parsedKyokumen.KifuNode, out jsaFugoStr, errH);// GUIに通知するだけ。

                    ////------------------------------
                    //// 駒を、駒袋から駒台に移動させます。
                    ////------------------------------
                    //model_Operating.Manual.SetGuiSky(
                    //    SkyConst.NewInstance(
                    //        parsedKyokumen.buffer_Sky,
                    //        -1//そのまま
                    //    )
                    //);
                }


                successful = true;
            }
            catch (Exception ex)
            {
                Util_Message.Show(ex.GetType().Name+"："+ ex.Message);
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
            Model_Taikyoku model_Taikyoku,
            KwErrorHandler errH
            )
        {
            bool successful = false;

            //------------------------------
            // 棋譜から１手削ります
            //------------------------------
            Node<Move, KyokumenWrapper> removeeLeaf = model_Taikyoku.Kifu.CurNode;
            int korekaranoTemezumi = removeeLeaf.Value.Kyokumen.Temezumi - 1;//１手前へ。

            if (removeeLeaf.IsRoot())
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
            jsaFugoStr = Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(removeeLeaf,errH);




            //------------------------------
            // 前の手に戻します
            //------------------------------
            IttemodosuResult ittemodosuResult = new IttemodosuResultImpl(Fingers.Error_1, Fingers.Error_1, null, Komasyurui14.H00_Null___, null);
            {
                //Ks14 foodKomaSyurui;//取った駒があれば、取った駒の種類。
                //SkyConst susunda_Sky_orNull;
                //
                // 一手巻き戻す
                //
                Util_IttemodosuRoutine.Before1(
                    model_Taikyoku.Kifu.CurNode,
                    new IttemodosuArgImpl(
                        removeeLeaf.Key,
                        korekaranoTemezumi
                    ),
                    out ittemodosuResult,
                    errH
                    );
                Util_IttemodosuRoutine.Before2(
                    ref ittemodosuResult,
                    errH
                    );
                Util_IttemodosuRoutine.After3_ChangeCurrent(
                    model_Taikyoku.Kifu
                    );
            }
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
            Model_Taikyoku model_Taikyoku,
            Model_Manual model_Manual,
            KwErrorHandler errH
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {

            if(""==inputLine)
            {
                goto gt_EndMethod;
            }

            bool toBreak = false;
            Util_Functions_Server.ReadLine_TuginoItteSusumu_Srv(
                ref inputLine,
                model_Taikyoku,//SetCurNodeがある。
                model_Manual,
                out toBreak,
                "hint",
                errH
                );

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
            Model_Manual model_Manual,
            KwErrorHandler errH
            )
        {

            Finger btnKoma_Food_Koma;



            // 取られることになる駒のボタン
            btnKoma_Food_Koma = Util_Sky_FingersQuery.InMasuNow_Old(model_Manual.GuiSky, Conv_Busstop.ToMasu( foodee_koma)).ToFirst();
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
            switch (Conv_Busstop.ToPlayerside( foodee_koma))
            {
                case Playerside.P2:

                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, model_Manual.GuiSky);
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
                            Util_Masu10.BangaiSujiDanToMasu(
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

                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, model_Manual.GuiSky);
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
                            Util_Masu10.BangaiSujiDanToMasu(
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
