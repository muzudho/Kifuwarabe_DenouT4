using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C___500_struct__;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi
{

    /// <summary>
    /// 将棋指し。
    /// </summary>
    public interface Shogisasi
    {
        /// <summary>
        /// 右脳。
        /// </summary>
        FeatureVector FeatureVector { get; set; }

        /// <summary>
        /// 対局開始のとき。
        /// </summary>
        void OnTaikyokuKaisi();

        /// <summary>
        /// 時間管理
        /// </summary>
        TimeManager TimeManager { get; set; }


        /// <summary>
        /// 指し手を決めます。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="kifu1"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        MoveEx WA_Bestmove(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            bool isHonshogi,

            Earth earth1,
            Tree kifu1,
            Playerside psideA,
            Sky positionA,

            KwLogger errH
            );

    }

}
