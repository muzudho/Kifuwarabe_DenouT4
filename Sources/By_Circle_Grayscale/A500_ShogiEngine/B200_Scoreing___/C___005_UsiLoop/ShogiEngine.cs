using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi;
using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C___490_Option__;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C___005_Usi_Loop
{
    public interface ShogiEngine
    {
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        void Send(string line);


        /// <summary>
        /// きふわらべの作者名です。
        /// </summary>
        string AuthorName { get; }

        /// <summary>
        /// 製品名です。
        /// </summary>
        string SeihinName { get; }

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        EngineOptions EngineOptions { get; set; }

        KwLogger Logger { get; set; }
        
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
        bool GoPonderNow_AtLoop2 { get; set; }

        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// </summary>
        Dictionary<string, string> GameoverProperties_AtLoop2 { get; set; }

        /// <summary>
        /// 「go」の属性一覧です。
        /// </summary>
        Dictionary<string, string> GoProperties_AtLoop2 { get; set; }

    }
}
