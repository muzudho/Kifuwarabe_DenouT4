﻿namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class Conv_AgaruHiku
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// 上がる、引く
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="agaruHiku"></param>
        /// <returns></returns>
        public static string ToStr(AgaruHiku agaruHiku)
        {
            string str;

            switch (agaruHiku)
            {
                case AgaruHiku.Yoru:
                    str = "寄";
                    break;

                case AgaruHiku.Hiku:
                    str = "引";
                    break;

                case AgaruHiku.Agaru:
                    str = "上";
                    break;

                default:
                    str = "";
                    break;
            }

            return str;
        }

    }
}
