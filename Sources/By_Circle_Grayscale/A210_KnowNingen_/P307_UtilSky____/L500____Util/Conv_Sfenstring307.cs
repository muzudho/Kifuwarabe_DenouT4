using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C250____Struct;
using Grayscale.A120_KifuSfen___.B160_ConvSfen___.C500____Converter;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.P276_SeizaStartp.C500____Struct;


namespace Grayscale.P307_UtilSky____.C500____Util
{
    public abstract class Conv_Sfenstring307 : Conv_Sfenstring146
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sky"></param>
        /// <param name="sfenStartpos"></param>
        public static SkyConst ToSkyConst(SfenstringImpl startposString, Playerside pside, int temezumi)
        {
            StartposImporter startposImporter;
            string restText;
            StartposImporter.TryParse(
                startposString.ValueStr,
                out startposImporter,
                out restText
                );

            return startposImporter.ToSky(pside, temezumi);
        }

    }
}
