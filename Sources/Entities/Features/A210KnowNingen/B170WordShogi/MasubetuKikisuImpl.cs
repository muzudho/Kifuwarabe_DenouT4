﻿using System.Collections.Generic;
using Grayscale.A210KnowNingen.B170WordShogi.C510Komanokiki;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;

namespace Grayscale.A210KnowNingen.B170WordShogi.C510Komanokiki
{

    /// <summary>
    /// 升別、駒の利き数
    /// </summary>
    public class MasubetuKikisuImpl : IMasubetuKikisu
    {
        /// <summary>
        /// 枡毎の、味方、敵。
        /// </summary>
        public Dictionary<int, Playerside> HMasu_PlayersideList { get; set; }

        /// <summary>
        /// 枡毎の、利き数。
        /// key=升番号
        /// value=利きの数
        /// </summary>
        public Dictionary<int, int> Kikisu_AtMasu_1P { get; set; }
        public Dictionary<int, int> Kikisu_AtMasu_2P { get; set; }



        public MasubetuKikisuImpl()
        {
            this.HMasu_PlayersideList = new Dictionary<int, Playerside>();
            for (int masuIndex = 0; masuIndex < 202; masuIndex++)
            {
                this.HMasu_PlayersideList.Add(masuIndex, Playerside.Empty);
            }

            this.Kikisu_AtMasu_1P = new Dictionary<int, int>();
            for (int masuIndex = 0; masuIndex < 202; masuIndex++)
            {
                this.Kikisu_AtMasu_1P.Add(masuIndex, 0);
            }

            this.Kikisu_AtMasu_2P = new Dictionary<int, int>();
            for (int masuIndex = 0; masuIndex < 202; masuIndex++)
            {
                this.Kikisu_AtMasu_2P.Add(masuIndex, 0);
            }
        }



    }

}
