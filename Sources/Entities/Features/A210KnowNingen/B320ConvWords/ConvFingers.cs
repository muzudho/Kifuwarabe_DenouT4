using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public abstract class ConvFingers
    {

        /// <summary>
        /// フィンガー番号→駒→駒のある升の集合
        /// </summary>
        /// <param name="fingers"></param>
        /// <param name="src_Sky"></param>
        /// <returns></returns>
        public static SySet<SyElement> ToMasus(Fingers fingers, ISky src_Sky)
        {
            SySet<SyElement> masus = new SySet_Default<SyElement>("何かの升");

            foreach (Finger finger in fingers.Items)
            {
                src_Sky.AssertFinger(finger);
                Busstop koma = src_Sky.BusstopIndexOf(finger);


                masus.AddElement(Conv_Busstop.ToMasu(koma));
            }

            return masus;
        }
    }
}
