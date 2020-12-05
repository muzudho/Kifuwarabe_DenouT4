using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B210_KomanoKidou.C500Struct;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky
{
    public abstract class Util_Sky_SyugoQuery
    {
        /// <summary>
        /// 升コレクション。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static SySet<SyElement> Masus_Now(ISky src_Sky, Playerside pside)
        {
            SySet_Default<SyElement> masus = new SySet_Default<SyElement>("今の升");

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToPlayerside(koma) == pside && Conv_Busstop.ToOkiba(koma) == Okiba.ShogiBan)
                {
                    masus.AddElement(Conv_Busstop.ToMasu(koma));
                }
            });

            return masus;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒の移動可能升
        /// ************************************************************************************************************************
        /// 
        /// ポテンシャルなので、貫通している。
        /// 
        /// </summary>
        /// <param name="light"></param>
        /// <returns></returns>
        public static SySet<SyElement> KomaKidou_Potential(Finger finger, ISky src_Sky)
        {
            src_Sky.AssertFinger(finger);
            Busstop koma = src_Sky.BusstopIndexOf(finger);

            //
            // ポテンシャルなので、貫通しているのは仕様通り。
            //
            // FIXME: 成香が横に進めることが分かっているか？
            //
            return Array_Rule01_PotentialMove15.ItemMethods[(int)Conv_Busstop.ToKomasyurui(koma)](Conv_Busstop.ToPlayerside(koma), Conv_Busstop.ToMasu(koma));
        }

    }
}
