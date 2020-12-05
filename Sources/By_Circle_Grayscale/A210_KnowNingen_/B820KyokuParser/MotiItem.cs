using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;

namespace Grayscale.A210KnowNingen.B820KyokuParser.C500Parser
{
    public interface MotiItem
    {
        /// <summary>
        /// 駒の種類。
        /// </summary>
        Komasyurui14 Komasyurui { get; }

        /// <summary>
        /// 持っている枚数。
        /// </summary>
        int Maisu { get; }

        /// <summary>
        /// プレイヤーサイド。
        /// </summary>
        Playerside Playerside { get; }

    }
}
