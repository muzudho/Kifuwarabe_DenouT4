using Grayscale.A210_KnowNingen_.B520_SeizaStartp.C500____Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser
{
    public class KifuParserA_GenjoImpl : KifuParserA_Genjo
    {

        public string InputLine { get; set; }

        /// <summary>
        /// パーサーを止めるフラグ。正常時を明示。
        /// </summary>
        public void ToBreak_Normal()
        {
            this.isBreak = true;
        }
        /// <summary>
        /// パーサーを止めるフラグ。異常時を明示。
        /// </summary>
        public void ToBreak_Abnormal()
        {
            this.isBreak = true;
        }
        /// <summary>
        /// パーサーを止めるか。
        /// </summary>
        /// <returns></returns>
        public bool IsBreak()
        {
            return this.isBreak;
        }
        private bool isBreak;

        /// <summary>
        /// 平手初期局面ではない指定があったときに使用。
        /// </summary>
        public StartposImporter StartposImporter_OrNull { get; set; }


        public KifuParserA_GenjoImpl(string inputLine)
        {
            this.InputLine = inputLine;
            this.isBreak = false;
        }

    }
}
