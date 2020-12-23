using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;

#if DEBUG
// using Grayscale.Kifuwaragyoku.Entities.Logging;
#else
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
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

            IKifuParserAGenjo genjo)
        {
            Logger.Trace($"（＾△＾）「{genjo.InputLine}」Util_InClient　：　クライアントの委譲メソッドｷﾀｰ☆");


            string old_inputLine = genjo.InputLine;//退避
            string rest;
            ISfenFormat2 ro_Kyokumen2_ForTokenize;
            SfenConf.ToKyokumen2(
                genjo.InputLine,
                out rest,
                out ro_Kyokumen2_ForTokenize
                );

            Logger.Trace($"（＾△＾）old_inputLine=「{old_inputLine}」 rest=「{rest}」 Util_InClient　：　ﾊﾊｯ☆");

            //string old_inputLine = genjo.InputLine;
            //genjo.InputLine = "";


            //----------------------------------------
            // 棋譜を空っぽにし、指定の局面を与えます。
            //----------------------------------------
            {
                earth1.Clear();

                // 棋譜を空っぽにします。
                Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(kifu1, null);

                // 文字列から、指定局面を作成します。
                earth1.SetProperty(Word_KifuTree.PropName_Startpos, old_inputLine);//指定の初期局面
            }


        }

    }
}
