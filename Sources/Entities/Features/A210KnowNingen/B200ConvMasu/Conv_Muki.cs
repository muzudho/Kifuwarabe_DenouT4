﻿namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class Conv_Muki
    {

        public static void MukiToOffsetSujiDan(Hogaku muki, Playerside pside, out int offsetSuji, out int offsetDan)
        {
            offsetSuji = 0;
            offsetDan = 0;

            switch (muki)//先手の場合
            {
                case Hogaku.上:
                    offsetDan = -1;
                    break;
                case Hogaku.昇:
                    offsetSuji = -1;
                    offsetDan = -1;
                    break;
                case Hogaku.射:
                    offsetSuji = -1;
                    break;
                case Hogaku.沈:
                    offsetSuji = -1;
                    offsetDan = +1;
                    break;
                case Hogaku.引:
                    offsetDan = +1;
                    break;
                case Hogaku.降:
                    offsetSuji = +1;
                    offsetDan = +1;
                    break;
                case Hogaku.滑:
                    offsetSuji = +1;
                    break;
                case Hogaku.浮:
                    offsetSuji = +1;
                    offsetDan = -1;
                    break;
                default:
                    break;
            }

            if (pside == Playerside.P2)
            {
                offsetSuji *= -1;
                offsetDan *= -1;
            }
        }

    }
}
