using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA;
using Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA;
using Grayscale.A210KnowNingen.B690Ittesasu.C510OperationB;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A500ShogiEngine.B523UtilFv.C490UtilFvFormat;
using Grayscale.A500ShogiEngine.B523UtilFv.C491UtilFvIo;
using Grayscale.A690FvLearn.B110FvLearn.C___400_54List;
using Grayscale.A690FvLearn.B110FvLearn.C400____54List;
using Grayscale.A690FvLearn.B110FvLearn.C420Inspection;
using Grayscale.A690FvLearn.B110FvLearn.C430Zooming;
using Grayscale.A690FvLearn.B110FvLearn.C440Ranking;
using Grayscale.A690FvLearn.B110FvLearn.C460____Scoreing;

namespace Grayscale.A690FvLearn.B110FvLearn.C480Functions
{
    public abstract class UtilLearnFunctions
    {
        /// <summary>
        /// FVを、-999.0～999.0(*bairitu)に矯正。
        /// </summary>
        public static void FvParamRange_PP(FeatureVector fv, ILogTag logTag)
        {
            //--------------------------------------------------------------------------------
            // 変換前のデータを確認。 
            //--------------------------------------------------------------------------------
            UtilInspection.Inspection1(fv, logTag);

            //--------------------------------------------------------------------------------
            // 点数を、順位に変換します。
            //--------------------------------------------------------------------------------
            UtilRanking.Perform_Ranking(fv);

            //--------------------------------------------------------------------------------
            // トポロジー的に加工したあとのデータを確認。 
            //--------------------------------------------------------------------------------
            UtilZooming.ZoomTo_FvParamRange(fv, logTag);

        }
        /// <summary>
        /// FVの保存。
        /// </summary>
        /// <param name="uc_Main"></param>
        public static void Do_Save(UcMain uc_Main, ILogTag logTag)
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
            UtilLearnFunctions.FvParamRange_PP(uc_Main.LearningData.Fv, logTag);// 自動で -999～999(*bairitu) に矯正。


            // 駒割
            File.WriteAllText(uc_Main.TxtFvFilepath.Text, Format_FeatureVector_Komawari.Format_Text(fv));
            // スケール
            UtilFeatureVectorOutput.Write_Scale(fv, fvFolderPath);
            // KK
            UtilFeatureVectorOutput.Write_KK(fv, fvFolderPath);
            // 1pKP,2pKP
            UtilFeatureVectorOutput.Write_KP(fv, fvFolderPath);
            // PP 盤上
            UtilFeatureVectorOutput.Write_PP_Banjo(fv, fvFolderPath);
            // PP １９枚の持駒
            UtilFeatureVectorOutput.Write_PP_19Mai(fv, fvFolderPath);
            // PP ５枚の持駒、３枚の持駒
            UtilFeatureVectorOutput.Write_PP_5Mai(fv, fvFolderPath);
            UtilFeatureVectorOutput.Write_PP_3Mai(fv, fvFolderPath);
        }

        /// <summary>
        /// 本譜の手をランクアップ。
        /// </summary>
        public static void Do_RankUpHonpu(
            ref bool ref_isRequestShowGohosyu, UcMain uc_Main, Move move1, float tyoseiryo)
        {
            ILogTag logTag = LogTags.ProcessLearnerDefault;

            //----------------------------------------
            // 1P は正の数がグッド、2P は負の数がグッド。
            //----------------------------------------
            float tyoseiryo_bad = -tyoseiryo;//減点に使われる数字です。[局面評価更新]ボタンの場合。
            float tyoseiryo_good = 0.0f;//加点に使われる数字です。

            float badScore_temp = tyoseiryo_bad;
            ISky positionA = uc_Main.LearningData.PositionA;
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
                    ConvMove.ToPlayerside(moveB),
                    ref positionA,//指定局面
                    ref moveB,
                    "E100",
                    logTag
                );


                // 盤上の駒、持駒を数えます。
                N54List childNode_n54List = Util_54List.Calc_54List(positionA, logTag);

                float real_tyoseiryo; //実際に調整した量。
                Util_FvScoreing.UpdateKyokumenHyoka(
                    childNode_n54List,
                    positionA,
                    uc_Main.LearningData.Fv,
                    tyoseiryo_bad,
                    out real_tyoseiryo,
                    logTag
                    );//相手が有利になる点
                tyoseiryo_good += -real_tyoseiryo;


                IIttemodosuResult ittemodosuResult;
                UtilIttemodosuRoutine.UndoMove(
                    out ittemodosuResult,
                    moveB,//この関数が呼び出されたときの指し手☆（＾～＾）
                    ConvMove.ToPlayerside(moveB),
                    positionA,
                    "E900",
                    logTag
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
