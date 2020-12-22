﻿
namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    /// <summary>
    /// プレイヤーサイドの配列。
    /// </summary>
    public abstract class Array_Playerside
    {
        /// <summary>
        /// プレイヤーのみです。
        /// </summary>
        public static readonly Playerside[] Items_PlayerOnly = new Playerside[] {
            Playerside.P1,
            Playerside.P2
        };

        /// <summary>
        /// Empty値を含みます。
        /// </summary>
        public static readonly Playerside[] Items_AllElements = new Playerside[] {
            Playerside.Empty,
            Playerside.P1,
            Playerside.P2
        };
    }
}
