using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A120KifuSfen.B140SfenStruct.C250Struct;
using Grayscale.A120KifuSfen.B160ConvSfen.C500Converter;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B640_KifuTree___.C250Struct;
using Grayscale.A210KnowNingen.B740KifuParserA.C500Parser;

#if DEBUG
// using Grayscale.A060Application.B110Log.C500Struct;
#else
#endif

namespace Grayscale.A500ShogiEngine.B260UtilClient.C500Util
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

            IKifuParserAGenjo genjo,
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
                Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1, null, logger);

                // 文字列から、指定局面を作成します。
                earth1.SetProperty(Word_KifuTree.PropName_Startpos, old_inputLine);//指定の初期局面
            }


        }

    }
}
