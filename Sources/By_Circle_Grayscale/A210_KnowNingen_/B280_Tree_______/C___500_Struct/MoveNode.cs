using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{
    public interface MoveNode
    {
        /// <summary>
        /// このノードのキー。インスタンスを作った後では変更できません。
        /// </summary>
        Move Key { get; }

        /// <summary>
        /// 親ノード。変更可能。
        /// </summary>
        KifuNode GetParentNode();
        void SetParentNode(KifuNode parent);

        /// <summary>
        /// ルート・ノードなら真。
        /// </summary>
        /// <returns></returns>
        bool IsRoot();


        MoveEx MoveEx { get; set; }

        Children Children1 { get; set; }
    }
}
