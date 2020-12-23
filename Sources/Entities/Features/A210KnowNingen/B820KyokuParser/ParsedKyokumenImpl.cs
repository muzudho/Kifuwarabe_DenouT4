using System.Collections.Generic;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
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
        public IPosition NewSky { get; set; }

        /// <summary>
        /// 持ち駒リスト。
        /// </summary>
        public List<MotiItem> MotiList { get; set; }

        public IPosition Sky { get; set; }

        public ParsedKyokumenImpl()
        {
            this.MotiList = new List<MotiItem>();
        }

    }
}
