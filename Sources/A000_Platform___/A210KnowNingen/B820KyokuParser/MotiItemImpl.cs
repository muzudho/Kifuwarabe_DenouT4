using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B820KyokuParser.C500Parser;

namespace Grayscale.A210KnowNingen.B820KyokuParser.C500Parser
{
    public class MotiItemImpl : MotiItem
    {

        /// <summary>
        /// 駒の種類。
        /// </summary>
        public Komasyurui14 Komasyurui
        {
            get
            {
                return this.komasyurui;
            }
        }
        private Komasyurui14 komasyurui;

        /// <summary>
        /// 持っている枚数。
        /// </summary>
        public int Maisu
        {
            get
            {
                return this.maisu;
            }
        }
        private int maisu;

        /// <summary>
        /// プレイヤーサイド。
        /// </summary>
        public Playerside Playerside
        {
            get
            {
                return this.playerside;
            }
        }
        private Playerside playerside;

        /// <summary>
        /// コンストラクター。
        /// </summary>
        public MotiItemImpl(Komasyurui14 komasyurui, int maisu, Playerside playerside)
        {
            this.komasyurui = komasyurui;
            this.maisu = maisu;
            this.playerside = playerside;
        }

    }
}
