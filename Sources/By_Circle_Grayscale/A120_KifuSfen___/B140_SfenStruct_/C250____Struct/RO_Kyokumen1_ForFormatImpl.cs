using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using System;

namespace Grayscale.A120_KifuSfen___.B140_SfenStruct_.C250____Struct
{

    /// <summary>
    /// 盤上の駒を２次元配列で。
    /// 持ち駒を種類別の配列で。
    /// </summary>
    public class RO_Kyokumen1_ForFormatImpl : RO_Kyokumen1_ForFormat
    {

        public string[,] Ban { get; set; }

        /// <summary>
        /// 持ち駒の枚数。
        /// 駒の種類は [0]から、空っぽ,空っぽ,▲飛,▲角,▲金,▲銀,▲桂,▲香,▲歩,空っぽ,△飛,△角,△金,△銀,△桂,△香,△歩 の順。
        /// </summary>
        public int[] MotiSu { get; set; }

        /// <summary>
        /// 手目済み。初期局面を 0手目済み、初手を指した後の局面を 1手目済みとカウントします。
        /// </summary>
        public int Temezumi { get; set; }

        public RO_Kyokumen1_ForFormatImpl()
        {
            this.Ban = new string[10,10];// 将棋盤。10×10。0は使わない。
            {// 全クリアー
                // 将棋盤
                for (int suji = 0; suji < 10; suji++)
                {
                    for (int dan = 0; dan < 10; dan++)
                    {
                        this.Ban[suji, dan] = "";
                    }
                }
            }

            // 持ち駒の枚数。
            // 駒の種類は [0]から、空っぽ,空っぽ,▲飛,▲角,▲金,▲銀,▲桂,▲香,▲歩,空っぽ,△飛,△角,△金,△銀,△桂,△香,△歩 の順。
            this.MotiSu = new int[(int)Pieces.Num];

            this.Temezumi = 1;//将棋所に合わせて、 1固定 をデフォルトとする。
        }
    }
}
