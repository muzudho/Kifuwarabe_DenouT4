using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C___500_Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct
{
    public class KifuTreeImpl : TreeImpl<Move, KyokumenWrapper>, KifuTree
    {
        /// <summary>
        /// 千日手カウンター。
        /// </summary>
        /// <returns></returns>
        public SennititeCounter GetSennititeCounter()
        {
            return this.sennititeCounter;
        }
        private SennititeCounter sennititeCounter;

        public KifuTreeImpl(Node<Move, KyokumenWrapper> root)
            : base(root)
        {
            //----------------------------------------
            // 千日手カウンター
            //----------------------------------------
            this.sennititeCounter = new SennititeCounterImpl();
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜を空っぽにします。
        /// ************************************************************************************************************************
        /// 
        /// ルートは残します。
        /// 
        /// </summary>
        public override void Clear()
        {
            base.Clear();

            //----------------------------------------
            // 千日手カウンター
            //----------------------------------------
            this.sennititeCounter.Clear();
        }


        /// <summary>
        /// これから追加する予定のノードの先後を診断します。
        /// </summary>
        /// <param name="node"></param>
        public void AssertChildPside(Playerside parentPside, Playerside childPside)
        {
#if DEBUG
            Debug.Assert(
                (parentPside==Playerside.P1 && childPside==Playerside.P2)
                ||
                (parentPside==Playerside.P2 && childPside==Playerside.P1)
                , "親子の先後に、異順序がありました。現手番[" + parentPside + "]　<> 次手番[" + childPside + "]");
#endif
        }


        ///// <summary>
        ///// この木の、全てのノードを、フォルダーとして作成します。
        ///// </summary>
        ///// <returns></returns>
        //public void CreateAllFolders(string folderpath, int limitDeep)
        //{
        //    int currentDeep = 0;

        //    if (null != this.GetRoot() && currentDeep <= limitDeep)
        //    {
        //        ((KifuNode)this.GetRoot()).CreateAllFolders(folderpath, currentDeep+1, limitDeep);
        //    }
        //}

    }


}
