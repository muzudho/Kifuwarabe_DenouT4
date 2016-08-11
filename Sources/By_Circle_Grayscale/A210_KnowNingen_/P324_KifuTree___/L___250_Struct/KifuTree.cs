using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.P323_Sennitite__.C___500_Struct;

namespace Grayscale.P324_KifuTree___.C___250_Struct
{
    public interface KifuTree : Tree<Move, KyokumenWrapper>
    {
        /// <summary>
        /// 千日手カウンター。
        /// </summary>
        /// <returns></returns>
        SennititeCounter GetSennititeCounter();

        //void AssertPside(Node<Move, KyokumenWrapper> node, string hint, KwErrorHandler errH);
        /// <summary>
        /// これから追加する予定のノードの先後を診断します。
        /// </summary>
        /// <param name="node"></param>
        void AssertChildPside(Playerside parentPside, Playerside childPside);
        //Playerside CountPside(Node<Move, KyokumenWrapper> node, KwErrorHandler errH);


        
        ///// <summary>
        ///// ************************************************************************************************************************
        ///// [ここから採譜]機能
        ///// ************************************************************************************************************************
        ///// </summary>
        //void SetStartpos_KokokaraSaifu(Playerside pside, KwErrorHandler errH);

        ///// <summary>
        ///// この木の、全てのノードを、フォルダーとして作成します。
        ///// </summary>
        ///// <returns></returns>
        //void CreateAllFolders(string folderpath, int limitDeep);

    }
}
