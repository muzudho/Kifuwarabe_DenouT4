using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B820_KyokuParser.C___500_Parser;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B820_KyokuParser.C500____Parser
{
    /// <summary>
    /// 解析された局面
    /// </summary>
    public class ParsedKyokumenImpl : ParsedKyokumen
    {

        /// <summary>
        /// 初期局面の先後。
        /// </summary>
        public Playerside FirstPside { get; set; }

        /// <summary>
        /// 棋譜ノード。
        /// </summary>
        //public KifuNode KifuNode { get; set; }
        public Move NewMove { get; set; }
        public Sky NewSky { get; set; }

        /// <summary>
        /// 持ち駒リスト。
        /// </summary>
        public List<MotiItem> MotiList { get; set; }

        public Sky Sky { get; set; }

        public ParsedKyokumenImpl()
        {
            this.MotiList = new List<MotiItem>();
        }

    }
}
