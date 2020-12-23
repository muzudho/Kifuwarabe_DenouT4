using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
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
            out Fingers out_fingers, SySet<SyElement> srcList, IPosition src_Sky, Playerside pside)
        {
            out_fingers = new Fingers();

            foreach (SyElement masu in srcList.Elements)
            {
                Finger finger = UtilSkyFingerQuery.InMasuNow_FilteringBanjo(src_Sky, pside, masu);
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
