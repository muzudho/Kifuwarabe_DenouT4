using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public interface ParsedKyokumen
    {
        /// <summary>
        /// 初期局面の先後。
        /// </summary>
        Playerside FirstPside { get; set; }

        /// <summary>
        /// 棋譜ノード。
        /// </summary>
        //KifuNode KifuNode { get; set; }
        Move NewMove { get; set; }
        IPosition NewSky { get; set; }

        /// <summary>
        /// 持ち駒リスト。
        /// </summary>
        List<MotiItem> MotiList { get; set; }

        IPosition Sky { get; set; }
    }
}
