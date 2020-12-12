using System.Collections.Generic;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A500ShogiEngine.B200Scoreing.C240Shogisasi;

namespace Grayscale.A500ShogiEngine.B200Scoreing.C005UsiLoop
{
    public interface ShogiEngine
    {
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        void Send(string line);

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        EngineOptions EngineOptions { get; set; }

        ILogger Logger { get; set; }

        /// <summary>
        /// 将棋エンジンの中の一大要素「思考エンジン」です。
        /// 指す１手の答えを出すのが仕事です。
        /// </summary>
        Shogisasi Shogisasi { get; set; }

        /// <summary>
        /// 棋譜です。
        /// </summary>
        Tree Kifu_AtLoop2 { get; }

        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        bool GoPonderNow { get; set; }

        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// </summary>
        Dictionary<string, string> GameoverProperties { get; set; }

        /// <summary>
        /// 「go」の属性一覧です。
        /// </summary>
        Dictionary<string, string> GoProperties_AtLoop2 { get; set; }

    }
}
