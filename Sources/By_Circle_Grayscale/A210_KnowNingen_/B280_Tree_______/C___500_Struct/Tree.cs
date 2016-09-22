using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{

    /// <summary>
    /// 記録係が利用します。
    /// </summary>
    /// <param name="temezumi">手目済</param>
    /// <param name="node">ノードのかたまりのまま。</param>
    /// <param name="toBreak"></param>
    public delegate void DELEGATE_Foreach1(int temezumi, Move move, KifuNode node, ref bool toBreak);
    public delegate void DELEGATE_Foreach2(int temezumi, Move move, ref bool toBreak);


    public interface Tree
    {
        Sky GetSky();

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        KifuNode CurNode { get; }
        void SetCurNode(KifuNode node);

        KifuNode NodeAt(int temezumi1);

        
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
        KifuNode PopCurrentNode();


        KifuNode GetRoot();

        


        

        /*
        /// <summary>
        /// これから追加する予定のノードの先後を診断します。
        /// </summary>
        /// <param name="node"></param>
        void AssertChildPside(Playerside parentPside, Playerside childPside);
        */

    }
}
