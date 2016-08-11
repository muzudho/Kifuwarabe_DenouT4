using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C500____Util;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using System.Text;

#if DEBUG

#endif

namespace Grayscale.A210_KnowNingen_.B810_LogGraphiEx.C500____Util
{
    public abstract class Conv_KaisetuBoards
    {
        public static string ToJsonStr(KaisetuBoards boards1)
        {
            StringBuilder sb_json_boardsLog = new StringBuilder();

            foreach (KaisetuBoard board1 in boards1.boards)
            {
                // 指し手。分かれば。
                string sasiteStr = Conv_Sasite.Sasite_To_KsString_ForLog(board1.Move, board1.GenTeban);

                //string oldCaption = boardLog1.Caption;
                //boardLog1.Caption += "_" + sasiteStr;
                sb_json_boardsLog.Append(Util_LogWriter_Json.ToJsonStr(board1));
                //boardLog1.Caption = oldCaption;
            }

            return sb_json_boardsLog.ToString();
        }

    }
}
