using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P056_Syugoron___.L250____Struct;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P215_KomanoKidou.L500____Struct;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P258_UtilSky258_.L500____UtilSky
{
    public abstract class Util_Sky_SyugoQuery
    {
        /// <summary>
        /// 升コレクション。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static SySet<SyElement> Masus_Now(SkyConst src_Sky, Playerside pside)
        {
            SySet_Default<SyElement> masus = new SySet_Default<SyElement>("今の升");

            src_Sky.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                if (Conv_Busstop.ToPlayerside( koma) == pside && Conv_Busstop.ToOkiba(koma) == Okiba.ShogiBan)
                {
                    masus.AddElement(Conv_Busstop.ToMasu( koma));
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
        public static SySet<SyElement> KomaKidou_Potential(Finger finger, SkyConst src_Sky)
        {
            src_Sky.AssertFinger(finger);
            Busstop koma = src_Sky.BusstopIndexOf(finger);

            //
            // ポテンシャルなので、貫通しているのは仕様通り。
            //
            // FIXME: 成香が横に進めることが分かっているか？
            //
            return Array_Rule01_PotentialMove15.ItemMethods[(int)Conv_Busstop.ToKomasyurui( koma)](Conv_Busstop.ToPlayerside( koma), Conv_Busstop.ToMasu( koma));
        }

    }
}
