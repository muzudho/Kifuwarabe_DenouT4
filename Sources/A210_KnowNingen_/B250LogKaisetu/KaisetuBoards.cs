using System.Collections.Generic;

namespace Grayscale.A210KnowNingen.B250LogKaisetu.C250Struct
{

    /// <summary>
    /// 複数の盤をもつログ・ファイルです。
    /// </summary>
    public class KaisetuBoards
    {

        public List<KaisetuBoard> boards { get; set; }

        public KaisetuBoards()
        {
            this.boards = new List<KaisetuBoard>();
        }

    }
}
