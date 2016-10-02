using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C490____UtilFvFormat;
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C491____UtilFvIo;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C___400_54List;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C400____54List;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C420____Inspection;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C430____Zooming;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C440____Ranking;
using Grayscale.A690_FvLearn____.B110_FvLearn____.C460____Scoreing;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

namespace Grayscale.A690_FvLearn____.B110_FvLearn____.C480____Functions
{
    public abstract class Util_LearnFunctions
    {
        /// <summary>
        /// FVを、-999.0～999.0(*bairitu)に矯正。
        /// </summary>
        public static void FvParamRange_PP(FeatureVector fv, KwLogger errH)
        {
            //--------------------------------------------------------------------------------
            // 変換前のデータを確認。 
            //--------------------------------------------------------------------------------
            Util_Inspection.Inspection1(fv, errH);

            //--------------------------------------------------------------------------------
            // 点数を、順位に変換します。
            //--------------------------------------------------------------------------------
            Util_Ranking.Perform_Ranking(fv);

            //--------------------------------------------------------------------------------
            // トポロジー的に加工したあとのデータを確認。 
            //--------------------------------------------------------------------------------
            Util_Zooming.ZoomTo_FvParamRange(fv, errH);

        }
        /// <summary>
        /// FVの保存。
        /// </summary>
        /// <param name="uc_Main"></param>
        public static void Do_Save(Uc_Main uc_Main, KwLogger errH)
        {
            FeatureVector fv = uc_Main.LearningData.Fv;


            // ファイルチューザーで指定された、fvフォルダーのパス
            string fvFolderPath = Path.GetDirectoryName(uc_Main.TxtFvFilepath.Text);

            // ファイルチューザーで指定された、Dataフォルダーのパス（fvフォルダーの親）
            string dataFolderPath = Directory.GetParent(fvFolderPath).FullName;
            //MessageBox.Show(
            //    "fvフォルダーのパス=[" + fvFolderPath + "]\n"+
            //    "dataフォルダーのパス=[" + dataFolderPath + "]"
            //, "Do_Save");

            //----------------------------------------
            // 時間
            //----------------------------------------
            string ymd;
            string hms;
            {
                DateTime dt = DateTime.Now;

                // 年月日
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dt.Year);
                    sb.Append("-");
                    sb.Append(dt.Month);
                    sb.Append("-");
                    sb.Append(dt.Day);
                    ymd = sb.ToString();
                    uc_Main.TxtAutosaveYMD.Text = ymd;
                }

                // 時分秒
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dt.Hour);
                    sb.Append("-");
                    sb.Append(dt.Minute);
                    sb.Append("-");
                    sb.Append(dt.Second);
                    hms = sb.ToString();
                    uc_Main.TxtAutosaveHMS.Text = hms;
                }
            }

            //----------------------------------------
            // バックアップ
            //----------------------------------------
            //
            // 失敗した場合、バックアップせず続行します
            //
            {
                // バックアップの失敗判定
                bool backup_failuer = false;

                // フォルダーのリネーム
                try
                {
                    string srcPath = Path.Combine(dataFolderPath, "fv");
                    string dstPath = Path.Combine(dataFolderPath, "fv_" + ymd + "_" + hms);
                    //MessageBox.Show(
                    //    "リネーム前のフォルダーのパス=[" + srcPath + "]\n" +
                    //    "リネーム後のフォルダーのパス=[" + dstPath + "]", "Do_Save");

                    Directory.Move(srcPath, dstPath);
                }
                catch (IOException)
                {
                    // フォルダーを、Windowsのファイル・エクスプローラーで開いているなどすると、失敗します。
                    backup_failuer = true;
                }

                if (!backup_failuer)
                {
                    // fvフォルダーの新規作成
                    Directory.CreateDirectory(fvFolderPath);
                }
            }

            //----------------------------------------
            // -999～999 に調整
            //----------------------------------------
            Util_LearnFunctions.FvParamRange_PP(uc_Main.LearningData.Fv, errH);// 自動で -999～999(*bairitu) に矯正。


            // 駒割
            File.WriteAllText(uc_Main.TxtFvFilepath.Text, Format_FeatureVector_Komawari.Format_Text(fv));
            // スケール
            Util_FeatureVectorOutput.Write_Scale(fv, fvFolderPath);
            // KK
            Util_FeatureVectorOutput.Write_KK(fv, fvFolderPath);
            // 1pKP,2pKP
            Util_FeatureVectorOutput.Write_KP(fv, fvFolderPath);
            // PP 盤上
            Util_FeatureVectorOutput.Write_PP_Banjo(fv, fvFolderPath);
            // PP １９枚の持駒
            Util_FeatureVectorOutput.Write_PP_19Mai(fv, fvFolderPath);
            // PP ５枚の持駒、３枚の持駒
            Util_FeatureVectorOutput.Write_PP_5Mai(fv, fvFolderPath);
            Util_FeatureVectorOutput.Write_PP_3Mai(fv, fvFolderPath);
        }

        /// <summary>
        /// 本譜の手をランクアップ。
        /// </summary>
        public static void Do_RankUpHonpu(
            ref bool ref_isRequestShowGohosyu, Uc_Main uc_Main, Move move1, float tyoseiryo)
        {
            KwLogger logger = Util_Loggers.ProcessLearner_DEFAULT;

            //----------------------------------------
            // 1P は正の数がグッド、2P は負の数がグッド。
            //----------------------------------------
            float tyoseiryo_bad = -tyoseiryo;//減点に使われる数字です。[局面評価更新]ボタンの場合。
            float tyoseiryo_good = 0.0f;//加点に使われる数字です。

            float badScore_temp = tyoseiryo_bad;
            Sky positionA = uc_Main.LearningData.PositionA;
            if (positionA.GetKaisiPside() == Playerside.P2)
            {
                tyoseiryo_bad *= -1.0f;//2Pは、負数の方が高得点です。
            }

            //
            // 合法手一覧
            //
            {
                Move moveB = uc_Main.LearningData.ToCurChildItem();
                // 本譜手はまだ計算しない。
                if (moveB == move1)
                {
                    goto gt_NextLoop1;
                }

                Util_IttesasuSuperRoutine.DoMove_Super1(
                    Conv_Move.ToPlayerside(moveB),
                    ref positionA,//指定局面
                    ref moveB,
                    "E100",
                    logger
                );


                // 盤上の駒、持駒を数えます。
                N54List childNode_n54List = Util_54List.Calc_54List(positionA, logger);

                float real_tyoseiryo; //実際に調整した量。
                Util_FvScoreing.UpdateKyokumenHyoka(
                    childNode_n54List,
                    positionA,
                    uc_Main.LearningData.Fv,
                    tyoseiryo_bad,
                    out real_tyoseiryo,
                    logger
                    );//相手が有利になる点
                tyoseiryo_good += -real_tyoseiryo;


                IttemodosuResult ittemodosuResult;
                Util_IttemodosuRoutine.UndoMove(
                    out ittemodosuResult,
                    moveB,//この関数が呼び出されたときの指し手☆（＾～＾）
                    Conv_Move.ToPlayerside(moveB),
                    positionA,
                    "E900",
                    logger
                    );
                positionA = ittemodosuResult.SyuryoSky;


                gt_NextLoop1:
                ;
            }

            //
            // 本譜手
            //
            /*
            if (uc_Main.LearningData.ContainsKeyCurChildNode(move1, uc_Main.LearningData.KifuA, logger))
            {
                // 進める
                Move moveD = move1;
                bool successful = Util_IttesasuSuperRoutine.DoMove_Super1(
                    ref positionA,//指定局面
                    ref moveD,
                    "H100_LearnFunc",
                    logger
                );

                // 盤上の駒、持駒を数えます。
                N54List currentNode_n54List = Util_54List.Calc_54List(positionA, logger);

                float real_tyoseiryo; //実際に調整した量。
                Util_FvScoreing.UpdateKyokumenHyoka(
                    currentNode_n54List,
                    positionA,
                    uc_Main.LearningData.Fv,
                    tyoseiryo_good,
                    out real_tyoseiryo,
                    logger
                    );//自分が有利になる点


                IttemodosuResult ittemodosuResult;
                Util_IttemodosuRoutine.UndoMove(
                    out ittemodosuResult,
                    moveD,//この関数が呼び出されたときの指し手☆（＾～＾）
                    positionA,
                    "H900_LearnFunc",
                    logger
                    );
                positionA = ittemodosuResult.SyuryoSky;
            }
            else
            {
            */
                Debug.Fail("指し手[" + move1 +
                    "]に対応する次ノードは作成されていませんでした。\n" +
                    uc_Main.LearningData.DumpToAllGohosyu(
                        uc_Main.LearningData.PositionA));
            /*
            }
            */

            // 局面の合法手表示の更新を要求します。
            ref_isRequestShowGohosyu = true;
        }

    }
}
