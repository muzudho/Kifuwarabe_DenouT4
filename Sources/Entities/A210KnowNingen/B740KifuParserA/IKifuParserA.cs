using System.Runtime.CompilerServices;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;

namespace Grayscale.A210KnowNingen.B740KifuParserA.C500Parser
{

    public interface IKifuParserA
    {

        /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        string Execute_Step_CurrentMutable(
            ref IKifuParserAResult result,

            Earth earth1,
            Tree kifu1,

            IKifuParserAGenjo genjo,
            ILogTag logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );

        /// <summary>
        /// 最初から最後まで実行します。（きふわらべCOMP用）
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        void Execute_All_CurrentMutable(
            ref IKifuParserAResult result,

            Earth earth1,
            Tree kifu1,

            IKifuParserAGenjo genjo,
            ILogTag logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );
    }
}
