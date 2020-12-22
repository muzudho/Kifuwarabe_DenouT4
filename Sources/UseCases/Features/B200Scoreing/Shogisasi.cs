using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
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
        /// <param name="logTag"></param>
        /// <returns></returns>
        MoveEx WA_Bestmove(
            ref int searchedMaxDepth,
            ref ulong searchedNodes,
            string[] searchedPv,
            bool isHonshogi,

            Earth earth1,
            Tree kifu1,
            Playerside psideA,
            ISky positionA,

            ILogTag logTag
            );

    }

}
