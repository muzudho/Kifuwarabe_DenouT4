﻿
using System.Collections.Generic;


namespace Grayscale.Kifuwaragyoku.Entities.Features
{

    /// <summary>
    /// 升別、駒の利き数
    /// </summary>
    public interface IMasubetuKikisu
    {
        /// <summary>
        /// 枡毎の、利き数。
        /// </summary>
        Dictionary<int, int> Kikisu_AtMasu_1P { get; set; }
        Dictionary<int, int> Kikisu_AtMasu_2P { get; set; }

    }
}
