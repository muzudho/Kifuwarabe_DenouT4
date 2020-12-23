using System;
using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

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
            IPlaying playing_notUse,
            Move move1_notUse,
            IPosition positionA,

            out IKifuParserAState nextState,
            IKifuParserA owner,
            IKifuParserAGenjo genjo)
        {
            out_moveNodeType = MoveNodeType.None;
            nextState = this;

            Logger.Error($"（＾△＾）「{genjo.InputLine}」vs【{this.GetType().Name}】　：　さて、どんな内容なんだぜ☆？");

            StartposImporter startposImporter1;
            string restText;

            bool successful = StartposImporter.TryParse(
                genjo.InputLine,
                out startposImporter1,
                out restText
                );
            genjo.StartposImporter_OrNull = startposImporter1;

            Logger.Error($"（＾△＾）restText=「{restText}」 successful=【{successful}】");

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

            return genjo.InputLine;
        }

    }
}
