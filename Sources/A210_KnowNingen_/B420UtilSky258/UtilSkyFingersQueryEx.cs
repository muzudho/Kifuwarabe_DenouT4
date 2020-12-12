using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B410SeizaFinger.C500Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky
{

    /// <summary>
    /// 指定局面から、『指差し番号』を問い合わせます。
    /// 
    /// 特殊なもの。
    /// </summary>
    public abstract class UtilSkyFingersQueryEx
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 軌道上の駒たち
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static void Fingers_EachSrcNow(
            out Fingers out_fingers, SySet<SyElement> srcList, ISky src_Sky, Playerside pside,
            ILogger errH)
        {
            out_fingers = new Fingers();

            foreach (SyElement masu in srcList.Elements)
            {
                Finger finger = UtilSkyFingerQuery.InMasuNow_FilteringBanjo(src_Sky, pside, masu, errH);
                if (Util_Finger.ForHonshogi(finger))
                {
                    // 指定の升に駒がありました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    out_fingers.Add(finger);
                }
            }
        }

    }
}
