using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B820_KyokuParser.C___500_Parser;

namespace Grayscale.A210_KnowNingen_.B820_KyokuParser.C500____Parser
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
