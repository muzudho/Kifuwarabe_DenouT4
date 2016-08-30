using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;

namespace Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C250____Struct
{
    public class Model_ManualImpl : Model_Manual
    {


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// GUI用局面データ。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        /// 局面が進むごとに更新されていきます。
        /// 
        /// </summary>
        public SkyImpl GuiSkyConst { get { return this.guiSkyConst; } }
        public void SetGuiSky(SkyImpl sky)
        {
            this.guiSkyConst = sky;
        }
        private SkyImpl guiSkyConst;
        public int GuiTemezumi { get; set; }
        public Playerside GuiPside { get; set; }

        public Model_ManualImpl()
        {
            //
            // 駒なし
            //
            this.GuiTemezumi = 0;
            Playerside firstPside = Playerside.P1;
            this.GuiPside = firstPside;
            this.guiSkyConst = Util_SkyWriter.New_Komabukuro(firstPside);// 描画モデル作成時

        }
    }
}
