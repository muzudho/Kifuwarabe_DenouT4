using System;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    /// <summary>
    /// 指定局面から始める配置です。
    /// 
    /// 「lnsgkgsnl/1r5b1/ppppppppp/9/9/6P2/PPPPPP1PP/1B5R1/LNSGKGSNL w - 1」といった文字の読込み
    /// </summary>
    public class KifuParserAStateA1bSfenLnsgkgsnl : IKifuParserAState
    {


        public static KifuParserAStateA1bSfenLnsgkgsnl GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserAStateA1bSfenLnsgkgsnl();
            }

            return instance;
        }
        private static KifuParserAStateA1bSfenLnsgkgsnl instance;



        private KifuParserAStateA1bSfenLnsgkgsnl()
        {
        }


        public string Execute(
            out MoveNodeType out_moveNodeType,
            ref IKifuParserAResult result,

            Earth earth1_notUse,
            Move move1_notUse,
            ISky positionA,

            out IKifuParserAState nextState,
            IKifuParserA owner,
            IKifuParserAGenjo genjo,
            ILogTag logTag
            )
        {
            out_moveNodeType = MoveNodeType.None;
            nextState = this;

            try
            {

                Logger.AppendLine(logTag, "（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　さて、どんな内容なんだぜ☆？");
                Logger.Flush(logTag, LogTypes.Error);

                StartposImporter startposImporter1;
                string restText;

                bool successful = StartposImporter.TryParse(
                    genjo.InputLine,
                    out startposImporter1,
                    out restText
                    );
                genjo.StartposImporter_OrNull = startposImporter1;
                Logger.AppendLine(logTag, "（＾△＾）restText=「" + restText + "」 successful=【" + successful + "】");
                Logger.Flush(logTag, LogTypes.Error);

                if (successful)
                {
                    genjo.InputLine = restText;

                    nextState = KifuParserAStateA2SfenMoves.GetInstance();
                }
                else
                {
                    // 解析に失敗しました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    genjo.ToBreak_Abnormal();
                }

            }
            catch (Exception ex)
            {
                Logger.Panic(LogTags.ProcessNoneError, ex, "SFEN解析中☆");
                throw;
            }

            return genjo.InputLine;
        }

    }
}
