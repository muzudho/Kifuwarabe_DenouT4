using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using System.Diagnostics;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C125____UtilB
{

    /// <summary>
    /// 棋譜ツリーのユーティリティー。
    /// </summary>
    public abstract class Util_KifuTree282
    {

        /// <summary>
        /// 『以前の変化カッター』
        /// 
        /// 本譜を残して、カレントノードより以前の変化は　ツリーから削除します。
        /// </summary>
        public static int IzennoHenkaCutter(
            Tree kifu_mutable,
            KwLogger errH
            )
        {
            int result_removedCount = 0;

            //----------------------------------------
            // 本譜以外の変化を削除します。
            //----------------------------------------

            if (kifu_mutable.CurNode.IsRoot())
            {
                //----------------------------------------
                // ルートノードでは何もできません。
                //----------------------------------------
                goto gt_EndMethod;
            }

            //----------------------------------------
            // 本譜の手
            //----------------------------------------
            Move move1 = kifu_mutable.CurNode.Key;


            //----------------------------------------
            // １手前の分岐点
            //----------------------------------------
            Node parentNode = kifu_mutable.CurNode.GetParentNode();

            //----------------------------------------
            // 選ばなかった変化を、ここに入れます。
            //----------------------------------------
            List<Move> removeeList = new List<Move>();

            //----------------------------------------
            // 選んだ変化と、選ばなかった変化の一覧
            //----------------------------------------
            parentNode.Children1.Foreach_ChildNodes((Move move2, Node nextNode2, ref bool toBreak2) =>
            {
                if (move2 == move1)
                {
                    //----------------------------------------
                    // 本譜の手はスキップ
                    //----------------------------------------
                    //System.Console.WriteLine("残すsasiteStr=[" + sasiteStr + "] key1=[" + key1 + "] ★");
                    goto gt_Next1;
                }
                //else
                //{
                //    System.Console.WriteLine("残すsasiteStr=[" + sasiteStr + "] key1=[" + key1 + "]");
                //}

                //----------------------------------------
                // 選ばなかった変化をピックアップ
                //----------------------------------------
                removeeList.Add(move2);

            gt_Next1:
                ;
            });


            //----------------------------------------
            // どんどん削除
            //----------------------------------------
            result_removedCount = removeeList.Count;
            foreach (Move key in removeeList)
            {
                parentNode.Children1.RemoveItem(key);
            }

        gt_EndMethod:
            return result_removedCount;
        }


        /// <summary>
        /// 新しいノードを、次ノードとして追加します。
        /// そして、追加した新しいノードを、カレント・ノードとします。
        /// </summary>
        /// <param name="nextNode_and_nextCurrent"></param>
        public static void AppendChild_And_ChangeCurrentToChild(
            Earth earth1,
            Tree kifu1,
            Node nextNode_and_nextCurrent,
            string hint,
            KwLogger errH
            )
        {
            Move move1 = nextNode_and_nextCurrent.Key;

            if (!kifu1.CurNode.Children1.ContainsKey(move1))
            {
                //----------------------------------------
                // 次ノート追加
                //----------------------------------------
                earth1.GetSennititeCounter().CountUp_New(Conv_Sky.ToKyokumenHash(
                    nextNode_and_nextCurrent.GetValue()), hint+"/AppendChild_And_ChangeCurrentToChild");
                kifu1.CurNode.Children1.PutTuginoitte_New(nextNode_and_nextCurrent, kifu1.CurNode);
            }

            kifu1.SetCurNode( nextNode_and_nextCurrent);//次ノードを、これからのカレントとします。
            Debug.Assert(kifu1.CurNode != null, "カレントノードがヌル。");
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// [ここから採譜]機能
        /// ************************************************************************************************************************
        /// </summary>
        public static void SetStartpos_KokokaraSaifu(
            Earth earth1,
            Tree kifu1, Playerside pside, KwLogger errH)
        {

            //------------------------------------------------------------
            // 棋譜を空に
            //------------------------------------------------------------
            earth1.Clear();
            kifu1.Clear();
            earth1.SetProperty(Word_KifuTree.PropName_Startpos, Conv_KifuNode.ToSfenstring(kifu1.CurNode.GetValue(), pside, errH));
        }

    }
}
