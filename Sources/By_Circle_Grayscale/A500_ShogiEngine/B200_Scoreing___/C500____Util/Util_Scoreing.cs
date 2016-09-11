using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C___500_Hyokakansu;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C510____HyokakansuColl;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using System;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C500____Util
{
    /// <summary>
    /// 得点付けを行います。
    /// </summary>
    public abstract class Util_Scoreing
    {

        /// <summary>
        /// 初期値に使う。
        /// </summary>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static float GetWorstScore(
            Playerside pside// このノードが、どちらの手番か。
            )
        {
            

            float alphabeta_worstScore;// プレイヤー1ならmax値、プレイヤー2ならmin値。
            switch (pside)
            {
                case Playerside.P1:
                    // 1プレイヤーはまだ、大きな数を見つけていないという設定。
                    alphabeta_worstScore = float.MinValue;
                    break;
                case Playerside.P2:
                    // 2プレイヤーはまだ、小さな数を見つけていないという設定。
                    alphabeta_worstScore = float.MaxValue;
                    break;
                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }

            return alphabeta_worstScore;
        }

        public static MoveEx GetHighScore(
            MoveEx moveEx1,
            MoveEx moveEx2,
            Playerside pside// このノードが、どちらの手番か。
        )
        {
            switch (pside)
            {
                case Playerside.P1:
                    // 大きい方を取るぜ☆
                    if (moveEx1.Score < moveEx2.Score)
                    {
                        return moveEx2;
                    }
                    else if (moveEx2.Score < moveEx1.Score)
                    {
                        return moveEx1;
                    }
                    else if (0 < KwRandom.Random.Next(2))
                    {
                        return moveEx1;
                    }
                    else
                    {
                        return moveEx2;
                    }

                case Playerside.P2:
                    // 小さい方を取るぜ☆
                    if (moveEx1.Score < moveEx2.Score)
                    {
                        return moveEx1;
                    }
                    else if (moveEx2.Score < moveEx1.Score)
                    {
                        return moveEx2;
                    }
                    else if (0 < KwRandom.Random.Next(2))
                    {
                        return moveEx1;
                    }
                    else
                    {
                        return moveEx2;
                    }

                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }
        }
        public static int GetHighScore1Or2(
            MoveEx moveEx1,
            MoveEx moveEx2,
            Playerside pside// このノードが、どちらの手番か。
        )
        {
            switch (pside)
            {
                case Playerside.P1:
                    // 大きい方を取るぜ☆
                    if (moveEx1.Score < moveEx2.Score)
                    {
                        return 2;
                    }
                    else if (moveEx2.Score < moveEx1.Score)
                    {
                        return 1;
                    }
                    else if (0 < KwRandom.Random.Next(2))
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }

                case Playerside.P2:
                    // 小さい方を取るぜ☆
                    if (moveEx1.Score < moveEx2.Score)
                    {
                        return 1;
                    }
                    else if (moveEx2.Score < moveEx1.Score)
                    {
                        return 2;
                    }
                    else if (0 < KwRandom.Random.Next(2))
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }

                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }
        }



        /// <summary>
        /// ベスト・スコアを更新します。
        /// アルファ・カットの有無も調べます。
        /// </summary>
        /// <param name="node_yomi"></param>
        /// <param name="parentsiblingBestmove"></param>
        /// <param name="mov4Score"></param>
        /// <param name="mov3"></param>
        /// <param name="alpha_cut"></param>
        public static void Update_BestScore_And_Check_AlphaCut(
            int yomiDeep,//1start
            Playerside pside,// このノードが、どちらの手番か。

            float parentsiblingBestScore,//親の兄弟
            float mov4Score,//子
            ref MoveEx mov3,//自分

            out bool alpha_cut
            )
        {
            alpha_cut = false;
            switch (pside)
            {
                case Playerside.P1:
                    // 1プレイヤーは、大きな数を見つけたい。
                    if (mov3.Score < mov4Score)
                    {
                        mov3.SetScore(mov4Score);
                    }
                    //----------------------------------------
                    // アルファー・カット
                    //----------------------------------------
                    if (1<yomiDeep && parentsiblingBestScore < mov3.Score)
                    {
                        // 親の兄が既に見つけている数字より　大きな数字を見つけた場合
                        alpha_cut = true;//探索を打ち切り
                    }
                    break;
                case Playerside.P2:
                    // 2プレイヤーは、小さな数を見つけたい。
                    if (mov4Score < mov3.Score)
                    {
                        mov3.SetScore(mov4Score);
                    }
                    //----------------------------------------
                    // アルファー・カット
                    //----------------------------------------
                    if (1 < yomiDeep && mov3.Score < parentsiblingBestScore)
                    {
                        // 親の兄が既に見つけている数字より　小さな数字を見つけた場合
                        alpha_cut = true;//探索を打ち切り
                    }
                    break;
                default: throw new Exception("子要素探索中、プレイヤーサイドのエラー");
            }
        }

        /// <summary>
        /// 局面に、評価値を付けます。
        /// </summary>
        public static void DoScoreing_Kyokumen(

            MoveEx moveEx,
            Sky position,

            EvaluationArgs args,
            KwLogger errH
            )
        {
            //----------------------------------------
            // 千日手判定
            //----------------------------------------
            bool isSennitite;
            {
                ulong hash = Conv_Sky.ToKyokumenHash(position);
                if (args.SennititeConfirmer.IsNextSennitite(hash))
                {
                    // 千日手になる場合。
                    isSennitite = true;
                }
                else
                {
                    isSennitite = false;
                }
            }

            if (isSennitite)
            {
                // 千日手用の評価をします。
                Hyokakansu hyokakansu = Util_HyokakansuCollection.Hyokakansu_Sennichite;

                float score;
#if DEBUG || LEARN
                KyHyokaMeisai_Koumoku meisai;
#endif
                hyokakansu.Evaluate(
                    out score,
#if DEBUG || LEARN
                    out meisai,
#endif
                    position,//node_yomi_mutable_KAIZOMAE.Value.Kyokumen,
                    args.FeatureVector,
                    errH
                );

                moveEx.AddScore(score);
#if DEBUG || LEARN
                moveEx.KyHyokaSheet_Mutable.Add(
                    hyokakansu.Name.ToString(),
                    meisai
                );
#endif
            }
            else
            {
                Util_HyokakansuCollection.EvaluateAll_Normal(
                    moveEx,
                    position,
                    args.FeatureVector,
                    errH
                    );
            }
        }
    }
}
