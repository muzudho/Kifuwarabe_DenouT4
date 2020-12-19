using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A500ShogiEngine.B200Scoreing.C240Shogisasi;

namespace Grayscale.A500ShogiEngine.B200Scoreing.C005UsiLoop
{
    public interface ShogiEngine
    {
        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        EngineOptions EngineOptions { get; set; }

        /// <summary>
        /// 将棋エンジンの中の一大要素「思考エンジン」です。
        /// 指す１手の答えを出すのが仕事です。
        /// </summary>
        Shogisasi Shogisasi { get; set; }

        /// <summary>
        /// 棋譜です。
        /// </summary>
        Tree Kifu { get; }

        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        bool GoPonder { get; set; }

        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// </summary>
        Dictionary<string, string> GameoverProperties { get; set; }

        /// <summary>
        /// 「go」の属性一覧です。
        /// </summary>
        Dictionary<string, string> GoProperties { get; set; }

    }
}
