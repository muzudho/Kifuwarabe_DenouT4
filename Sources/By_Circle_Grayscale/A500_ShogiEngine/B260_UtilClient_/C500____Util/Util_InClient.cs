using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A120_KifuSfen___.B160_ConvSfen___.C500____Converter;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#else
#endif

namespace Grayscale.A500_ShogiEngine.B260_UtilClient_.C500____Util
{
    public abstract class Util_InClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="restText"></param>
        /// <param name="startposImporter"></param>
        /// <param name="logTag"></param>
        public static void OnChangeSky_Im_Client(

            Earth earth1,
            Tree kifu1,

            KifuParserA_Genjo genjo,
            KwLogger logger
            )
        {
            logger.AppendLine("（＾△＾）「" + genjo.InputLine + "」Util_InClient　：　クライアントの委譲メソッドｷﾀｰ☆");
            logger.Flush(LogTypes.Error);


            string old_inputLine = genjo.InputLine;//退避
            string rest;
            RO_Kyokumen2_ForTokenize ro_Kyokumen2_ForTokenize;
            Conv_Sfen.ToKyokumen2(
                genjo.InputLine,
                out rest,
                out ro_Kyokumen2_ForTokenize
                );

            logger.AppendLine("（＾△＾）old_inputLine=「" + old_inputLine + "」 rest=「" + rest + "」 Util_InClient　：　ﾊﾊｯ☆");
            logger.Flush(LogTypes.Error);

            //string old_inputLine = genjo.InputLine;
            //genjo.InputLine = "";


            //----------------------------------------
            // 棋譜を空っぽにし、指定の局面を与えます。
            //----------------------------------------
            {
                earth1.Clear();

                // 棋譜を空っぽにします。
                Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1, null,logger);

                // 文字列から、指定局面を作成します。
                earth1.SetProperty(Word_KifuTree.PropName_Startpos, old_inputLine);//指定の初期局面
            }


        }

    }
}
