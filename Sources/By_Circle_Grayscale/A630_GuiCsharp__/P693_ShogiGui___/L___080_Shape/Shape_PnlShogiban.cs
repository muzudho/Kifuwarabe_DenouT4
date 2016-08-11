﻿using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using System.Collections.Generic;
using System.Drawing;

namespace Grayscale.P693_ShogiGui___.L___080_Shape
{
    public interface Shape_PnlShogiban
    {

        
        /// <summary>
        /// ************************************************************************************************************************
        /// 筋を指定すると、ｘ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        int SujiToX(int suji);

                
        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋盤の描画はここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="g"></param>
        void Paint(Graphics g);


        /// <summary>
        /// 光らせる利き升ハンドル。
        /// </summary>
        SySet<SyElement> KikiBan { get; set; }


        /// <summary>
        /// 枡毎の、利いている駒ハンドルのリスト。
        /// </summary>
        Dictionary<int, List<int>> HMasu_KikiKomaList { get; set; }


        
        /// <summary>
        /// ************************************************************************************************************************
        /// 段を指定すると、ｙ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="dan"></param>
        /// <returns></returns>
        int DanToY(int dan);


        /// <summary>
        /// ************************************************************************************************************************
        /// 枡に利いている駒への逆リンク（の入れ物を用意）
        /// ************************************************************************************************************************
        /// </summary>
        void ClearHMasu_KikiKomaList();

    }
}
