using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B640_KifuTree___.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;

namespace Grayscale.A210KnowNingen.B690Ittesasu.C125UtilB
{

    /// <summary>
    /// 棋譜ツリーのユーティリティー。
    /// </summary>
    public abstract class Util_KifuTree282
    {
        /*
        /// <summary>
        /// 『以前の変化カッター』
        /// 
        /// 本譜を残して、カレントノードより以前の変化は　ツリーから削除します。
        /// </summary>
        public static int IzennoHenkaCutter(
            Tree kifu1,
            ILogger errH
            )
        {
            int result_removedCount = 0;

            //----------------------------------------
            // 本譜以外の変化を削除します。
            //----------------------------------------

            if (kifu1.CurNode3okok.IsRoot())
            {
                //----------------------------------------
                // ルートノードでは何もできません。
                //----------------------------------------
                goto gt_EndMethod;
            }

            //----------------------------------------
            // 本譜の手
            //----------------------------------------
            Move move1 = kifu1.CurNode3okok.Key;

            //----------------------------------------
            // 選ばなかった変化を、ここに入れます。
            //----------------------------------------
            List<Move> removeeList = new List<Move>();

            //----------------------------------------
            // 選んだ変化と、選ばなかった変化の一覧
            //----------------------------------------
            kifu1.ParentChildren.Foreach_ChildNodes5((Move move2, ref bool toBreak2) =>
            {
                if (move2 == move1)
                {
                    //----------------------------------------
                    // 本譜の手はスキップ
                    //----------------------------------------
                    //System.Console.WriteLine("残すmoveStr=[" + moveStr + "] key1=[" + key1 + "] ★");
                    goto gt_Next1;
                }
                //else
                //{
                //    System.Console.WriteLine("残すmoveStr=[" + moveStr + "] key1=[" + key1 + "]");
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
                kifu1.ParentChildren.RemoveItem(key);
            }

        gt_EndMethod:
            return result_removedCount;
        }
        */

        /// <summary>
        /// ************************************************************************************************************************
        /// [ここから採譜]機能
        /// ************************************************************************************************************************
        /// </summary>
        public static void Clear_SetStartpos_KokokaraSaifu(
            Earth earth1,
            ISky positionA,//kifu1.GetRoot().GetNodeValue()
            Tree kifu1,
            Playerside pside, ILogger logger)
        {

            //------------------------------------------------------------
            // 棋譜を空に
            //------------------------------------------------------------
            earth1.Clear();

            Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1, positionA, logger);

            earth1.SetProperty(
                Word_KifuTree.PropName_Startpos,
                ConvKifuNode.ToSfenstring(positionA, pside, logger));
        }

    }
}
