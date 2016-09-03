using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C250____Struct
{
    public class SkyWrapper_GuiImpl : SkyWrapper_Gui
    {


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// GUI用局面データ。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        /// 局面が進むごとに更新されていきます。
        /// 
        /// </summary>
        public Sky GuiSky { get { return this.guiSkyConst; } }
        public void SetGuiSky(Sky sky)
        {
            this.guiSkyConst = sky;
        }
        private Sky guiSkyConst;

        public SkyWrapper_GuiImpl()
        {
            //
            // 駒なし
            //
            this.guiSkyConst = Util_SkyCreator.New_Komabukuro();// 描画モデル作成時
        }
    }
}
