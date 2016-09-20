using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{

    /// <summary>
    /// ツリー構造です。
    /// 
    /// 局面を入れるのに利用します。
    /// 根ノードに平手局面、最初の子ノードに１手目の局面、を入れるような使い方を想定しています。
    /// </summary>
    public interface KifuNode : MoveNode
    {
        /// <summary>
        /// このノードの値。
        /// </summary>
        Sky GetNodeValue();
        void SetValue(Sky sky);
    }
}
