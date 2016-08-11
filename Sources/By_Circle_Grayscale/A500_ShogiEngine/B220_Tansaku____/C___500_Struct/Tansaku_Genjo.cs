
namespace Grayscale.A500_ShogiEngine.B220_Tansaku____.C___500_Tansaku
{
    public interface Tansaku_Genjo
    {

        Tansaku_Args Args { get; }

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
