using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C125____UtilB
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
            KwLogger errH
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
            Sky positionA,//kifu1.GetRoot().GetNodeValue()
            Tree kifu1,
            Playerside pside, KwLogger logger)
        {

            //------------------------------------------------------------
            // 棋譜を空に
            //------------------------------------------------------------
            earth1.Clear();

            Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1, positionA, logger);

            earth1.SetProperty(
                Word_KifuTree.PropName_Startpos,
                Conv_KifuNode.ToSfenstring(positionA, pside, logger));
        }

    }
}
