using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi;
using System;
using System.Collections.Generic;

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C125____ScoreSibori
{

    /// <summary>
    /// 棋譜ツリーのスコアの低いノードを捨てていきます。
    /// </summary>
    public class ScoreSiboriEngine
    {

        /// <summary>
        /// 棋譜ツリーの　評価値の高いノードだけを残して、評価値の低いノードをばっさばっさ捨てて行きます。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="atamanosumiCollection"></param>
        public void SelectHighScoreNode_OnRoot(KifuTree kifu, Shogisasi shogisasi, KwLogger errH)
        {
            int exception_area = 0;

            try
            {
                //
                // ノードが２つもないようなら、スキップします。
                //
                if (kifu.CurNode.Count_ChildNodes < 2)
                {
                    goto gt_EndMethod;
                }


                exception_area = 1000;

                // ソートしたいので、リスト構造に移し変えます。
                List<Node<Move, KyokumenWrapper>> rankedNodes = new List<Node<Move, KyokumenWrapper>>();
                {
                    try
                    {
                        kifu.CurNode.Foreach_ChildNodes((Move key, Node<Move, KyokumenWrapper> node, ref bool toBreak) =>
                        {
                            rankedNodes.Add(node);
                        });

                        exception_area = 1000;

                        // ソートします。
                        rankedNodes.Sort((a, b) =>
                        {
                            float bScore;
                            float aScore;

                            // 比較できないものは 0 にしておく必要があります。
                            if (!(a is KifuNode) || !(b is KifuNode))
                            {
                                return 0;
                            }

                            bScore = ((KifuNode)b).Score;
                            aScore = ((KifuNode)a).Score;

                            return (int)aScore.CompareTo(bScore);//点数が大きいほうが前に行きます。
                        });
                    }
                    catch (Exception ex)
                    {
                        errH.DonimoNaranAkirameta(ex, "ベストムーブ／ハイスコア抽出中 exception_area=[" + exception_area + "]"); throw ex;
                    }
                }

                exception_area = 1500;

                // 同着もいる☆
                Dictionary<Move, Node<Move, KyokumenWrapper>> dic = new Dictionary<Move, Node<Move, KyokumenWrapper>>();

                // 先手は先頭、後手は最後尾の要素が、一番高いスコア（同着あり）
                float goodestScore;
                if (kifu.CurNode.Value.Kyokumen.KaisiPside == Playerside.P2)
                {
                    // 1番高いスコアを調べます。
                    goodestScore = ((KifuNode)rankedNodes[0]).Score;
                    for (int iNode=0; iNode< rankedNodes.Count; iNode++)
                    {
                        if (goodestScore == ((KifuNode)rankedNodes[iNode]).Score)
                        {
                            dic.Add(rankedNodes[iNode].Key, rankedNodes[iNode]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // 2Pは、マイナスの方が良い。
                    goodestScore = ((KifuNode)rankedNodes[rankedNodes.Count-1]).Score;
                    for (int iNode = rankedNodes.Count-1; -1 < iNode; iNode--)
                    {
                        if (goodestScore == ((KifuNode)rankedNodes[iNode]).Score)
                        {
                            dic.Add(rankedNodes[iNode].Key, rankedNodes[iNode]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // 枝を更新します。
                kifu.CurNode.Set_ChildNodes(dic);
            }
            catch (Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "ベストムーブ／ハイスコア抽出中 exception_area=[" + exception_area + "]"); throw ex;
            }

        gt_EndMethod:
            ;
        }
    }
}
