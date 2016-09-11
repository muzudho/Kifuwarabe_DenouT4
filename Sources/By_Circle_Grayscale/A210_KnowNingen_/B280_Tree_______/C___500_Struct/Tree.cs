using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{

    /// <summary>
    /// 記録係が利用します。
    /// </summary>
    /// <param name="temezumi">手目済</param>
    /// <param name="nodeRef">ノードのかたまりのまま。</param>
    /// <param name="toBreak"></param>
    public delegate void DELEGATE_Foreach(int temezumi, Sky sky, Node nodeRef, ref bool toBreak);


    public interface Tree
    {
        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        Node CurNode { get; }
        void SetCurNode(Node node);

        Node NodeAt(int temezumi1);

        /// <summary>
        /// 使い方自由。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetProperty(string key, object value);

        
        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜を空っぽにします。
        /// ************************************************************************************************************************
        /// 
        /// ルートは残します。
        /// 
        /// </summary>
        void Clear();


        
        /// <summary>
        /// ************************************************************************************************************************
        /// 現在の要素を切り取って返します。なければヌル。
        /// ************************************************************************************************************************
        /// 
        /// カレントは、１手前に戻ります。
        /// 
        /// </summary>
        /// <returns>ルートしかないリストの場合、ヌルを返します。</returns>
        Node PopCurrentNode();


        Node GetRoot();

        
        /// <summary>
        /// 使い方自由。
        /// </summary>
        object GetProperty(string key);


        
        /// <summary>
        /// 本譜だけ。
        /// </summary>
        /// <param name="endNode"></param>
        /// <param name="delegate_Foreach"></param>
        void ForeachHonpu(Node endNode, DELEGATE_Foreach delegate_Foreach);

                
        /// <summary>
        /// 全て。
        /// </summary>
        /// <param name="endNode"></param>
        /// <param name="delegate_Foreach"></param>
        void ForeachZenpuku(Node startNode, DELEGATE_Foreach delegate_Foreach);

        /// <summary>
        /// この木の、全てのノード数を数えます。
        /// </summary>
        /// <returns></returns>
        int CountAllNodes();









        /// <summary>
        /// 千日手カウンター。
        /// </summary>
        /// <returns></returns>
        SennititeCounter GetSennititeCounter();

        //void AssertPside(Node<Move, Sky> node, string hint, KwLogger errH);
        /// <summary>
        /// これから追加する予定のノードの先後を診断します。
        /// </summary>
        /// <param name="node"></param>
        void AssertChildPside(Playerside parentPside, Playerside childPside);
        //Playerside CountPside(Node<Move, Sky> node, KwLogger errH);



        ///// <summary>
        ///// ************************************************************************************************************************
        ///// [ここから採譜]機能
        ///// ************************************************************************************************************************
        ///// </summary>
        //void SetStartpos_KokokaraSaifu(Playerside pside, KwLogger errH);

        ///// <summary>
        ///// この木の、全てのノードを、フォルダーとして作成します。
        ///// </summary>
        ///// <returns></returns>
        //void CreateAllFolders(string folderpath, int limitDeep);

    }
}
