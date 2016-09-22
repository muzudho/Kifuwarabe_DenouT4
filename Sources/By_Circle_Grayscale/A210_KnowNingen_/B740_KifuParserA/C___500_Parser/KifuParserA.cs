using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using System.Runtime.CompilerServices;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser
{

    public interface KifuParserA
    {

        /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        string Execute_Step_CurrentMutable(
            ref KifuParserA_Result result,

            Earth earth1,
            Tree kifu1,

            KifuParserA_Genjo genjo,
            KwLogger errH
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
            ref KifuParserA_Result result,

            Earth earth1,
            Tree kifu1,

            KifuParserA_Genjo genjo,
            KwLogger errH
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );
    }
}
