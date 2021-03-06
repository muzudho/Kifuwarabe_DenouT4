﻿using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
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
        public IPosition GuiSky { get { return this.guiSkyConst; } }
        public void SetGuiSky(IPosition sky)
        {
            this.guiSkyConst = sky;
        }
        private IPosition guiSkyConst;

        public SkyWrapper_GuiImpl()
        {
            //
            // 駒なし
            //
            this.guiSkyConst = UtilSkyCreator.New_Komabukuro();// 描画モデル作成時
        }
    }
}
