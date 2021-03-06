﻿using System;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
// using Grayscale.Kifuwaragyoku.Entities.Features;

#if DEBUG || LEARN
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
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
            Move moveA,
            float scoreA,
            MoveEx moveEx2,
            Playerside pside// このノードが、どちらの手番か。
        )
        {
            switch (pside)
            {
                case Playerside.P1:
                    // 大きい方を取るぜ☆
                    if (scoreA < moveEx2.Score)
                    {
                        return moveEx2;
                    }
                    else if (moveEx2.Score < scoreA)
                    {
                        return new MoveExImpl(moveA, scoreA);
                    }
                    else if (0 < KwRandom.Random.Next(2))
                    {
                        return new MoveExImpl(moveA, scoreA);
                    }
                    else
                    {
                        return moveEx2;
                    }

                case Playerside.P2:
                    // 小さい方を取るぜ☆
                    if (scoreA < moveEx2.Score)
                    {
                        return new MoveExImpl(moveA, scoreA);
                    }
                    else if (moveEx2.Score < scoreA)
                    {
                        return moveEx2;
                    }
                    else if (0 < KwRandom.Random.Next(2))
                    {
                        return new MoveExImpl(moveA, scoreA);
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
        /// <param name="mov4"></param>
        /// <param name="result_moveEx_best"></param>
        /// <param name="alpha_cut"></param>
        public static MoveEx Update_BestScore_And_Check_AlphaCut(
            MoveEx result_moveEx_best,//自分

            int yomiDeep,//1start
            Playerside pside,// このノードが、どちらの手番か。

            float parentsiblingBestScore,//親の兄弟
            MoveEx mov4,//子

            out bool alpha_cut
            )
        {
            alpha_cut = false;
            switch (pside)
            {
                case Playerside.P1:
                    // 1プレイヤーは、大きな数を見つけたい。
                    if (result_moveEx_best.Score < mov4.Score)
                    {
                        result_moveEx_best.SetScore(mov4.Score);
                        result_moveEx_best.SetMove(mov4.Move);
                    }
                    //----------------------------------------
                    // アルファー・カット
                    //----------------------------------------
                    if (1 < yomiDeep && parentsiblingBestScore < result_moveEx_best.Score)
                    {
                        // 親の兄が既に見つけている数字より　大きな数字を見つけた場合
                        alpha_cut = true;//探索を打ち切り
                    }
                    break;
                case Playerside.P2:
                    // 2プレイヤーは、小さな数を見つけたい。
                    if (mov4.Score < result_moveEx_best.Score)
                    {
                        result_moveEx_best.SetScore(mov4.Score);
                        result_moveEx_best.SetMove(mov4.Move);
                    }
                    //----------------------------------------
                    // アルファー・カット
                    //----------------------------------------
                    if (1 < yomiDeep && result_moveEx_best.Score < parentsiblingBestScore)
                    {
                        // 親の兄が既に見つけている数字より　小さな数字を見つけた場合
                        alpha_cut = true;//探索を打ち切り
                    }
                    break;
                default: throw new Exception("子要素探索中、プレイヤーサイドのエラー");
            }

            return result_moveEx_best;
        }

        /// <summary>
        /// 局面に、評価値を付けます。
        /// </summary>
        public static float DoScoreing_Kyokumen(
            IPlaying playing,
            Playerside psideA,
            IPosition positionA,

            EvaluationArgs args)
        {
            float score = 0.0f;

            //----------------------------------------
            // 千日手判定
            //----------------------------------------
            bool isSennitite;
            {
                ulong hash = Conv_Sky.ToKyokumenHash(positionA);
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
                IHyokakansu hyokakansu = UtilHyokakansuCollection.Hyokakansu_Sennichite;

                score += hyokakansu.Evaluate(
                    psideA,
                    positionA,//node_yomi_mutable_KAIZOMAE.Value.Kyokumen,
                    playing.FeatureVector
                );
            }
            else
            {
                score += UtilHyokakansuCollection.EvaluateAll_Normal(
                    psideA,
                    positionA,
                    playing.FeatureVector
                    );
            }

            return score;
        }
    }
}
