
namespace Grayscale.Kifuwaragyoku.Entities.Searching
{
    public interface ICurrentSearch
    {

        ISearchArgs Args { get; }

        /// <summary>
        /// 読み開始手目済み
        /// </summary>
        int YomikaisiTemezumi { get; set; }

        ///// <summary>
        ///// 読んでいる局面
        ///// </summary>
        //KifuNode Node_yomi { get; }
        //void SetNode_yomi(KifuNode node_yomi);
    }
}
