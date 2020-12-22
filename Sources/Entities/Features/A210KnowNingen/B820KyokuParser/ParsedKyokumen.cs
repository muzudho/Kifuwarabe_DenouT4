using System.Collections.Generic;

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
        ISky NewSky { get; set; }

        /// <summary>
        /// 持ち駒リスト。
        /// </summary>
        List<MotiItem> MotiList { get; set; }

        ISky Sky { get; set; }
    }
}
