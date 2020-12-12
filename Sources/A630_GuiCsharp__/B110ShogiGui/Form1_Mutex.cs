﻿namespace Grayscale.A630GuiCsharp.B110ShogiGui.C510Form
{
    public enum Form1_Mutex
    {
        /// <summary>
        /// ロックがかかっていない
        /// </summary>
        Empty,

        /// <summary>
        /// タイマー
        /// </summary>
        Timer,

        /// <summary>
        /// マウス操作
        /// </summary>
        MouseOperation,

        /// <summary>
        /// 棋譜再生
        /// </summary>
        Saisei,

        /// <summary>
        /// 起動時
        /// </summary>
        Launch
    }
}
