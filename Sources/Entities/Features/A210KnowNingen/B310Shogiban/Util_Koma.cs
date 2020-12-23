using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public class Util_Koma
    {

        #region 定数

        /// <summary>
        /// 働かない値として、筋に埋めておくためのものです。8～-8 程度だと、角等の射程に入るので、大きく外した数字をフラグに使います。
        /// </summary>
        public const int CTRL_NOTHING_PROPERTY_SUJI = int.MinValue;

        /// <summary>
        /// 働かない値として、段に埋めておくためのものです。8～-8 程度だと、角等の射程に入るので、大きく外した数字をフラグに使います。
        /// </summary>
        public const int CTRL_NOTHING_PROPERTY_DAN = int.MinValue;

        #endregion



        public static Busstop FromFinger(IPosition src_Sky, Finger finger)
        {
            src_Sky.AssertFinger(finger);
            return src_Sky.BusstopIndexOf(finger);
        }

    }
}
