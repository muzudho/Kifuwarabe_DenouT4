﻿namespace Grayscale.Kifuwaragyoku.Entities.Features
{

    /// <summary>
    /// マス番号の一時代替。
    /// </summary>
    public class BashoImpl : SyElement_Default, INewBasho
    {

        /// <summary>
        /// 升番号 0～201。
        /// ビットフィールドの下位8bit（0～255）。
        /// </summary>
        public int MasuNumber
        {
            get
            {
                // 下位8bit（0～255）升番号0～201
                return BashoImpl.ToMasuNumber(this.bitfield);
            }
        }

        public BashoImpl(ulong bitfield)
            : base(bitfield)
        {
            this.bitfield = bitfield;
        }

        public override int GetHashCode()
        {
            return this.MasuNumber;
        }

        public static ulong ToBitfield(int masuNumber)
        {
            ulong bitfield = (ulong)masuNumber;

            return bitfield;
        }

        public static int ToMasuNumber(ulong bitfield)
        {
            return (int)(bitfield & 0xff);
        }

    }
}
