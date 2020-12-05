using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;

namespace Grayscale.A210KnowNingen.B650PnlTaikyoku.C250Struct
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
        public ISky GuiSky { get { return this.guiSkyConst; } }
        public void SetGuiSky(ISky sky)
        {
            this.guiSkyConst = sky;
        }
        private ISky guiSkyConst;

        public SkyWrapper_GuiImpl()
        {
            //
            // 駒なし
            //
            this.guiSkyConst = UtilSkyCreator.New_Komabukuro();// 描画モデル作成時
        }
    }
}
