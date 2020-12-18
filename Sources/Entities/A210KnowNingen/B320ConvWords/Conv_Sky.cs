using System.Diagnostics;
using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B220ZobrishHash.C500Struct;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B310Shogiban.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.A210KnowNingen.B320ConvWords.C500Converter
{
    public abstract class Conv_Sky
    {
        public static ShogibanImpl ToShogiban(Playerside pside, ISky src_Sky, ILogger logger)
        {
            ShogibanImpl shogiban = new ShogibanImpl();

            shogiban.KaisiPside = pside;// src_Sky.GetKaisiPside();// TODO:


            // 将棋の駒４０個の場所を確認します。
            foreach (Finger finger in src_Sky.Fingers_All().Items)
            {
                src_Sky.AssertFinger(finger);
                Busstop busstop = src_Sky.BusstopIndexOf(finger);

                SyElement masu = Conv_Busstop.ToMasu(busstop);
                Debug.Assert(Conv_Masu.OnAll(Conv_Masu.ToMasuHandle(masu)), "(int)koma.Masu=[" + Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(busstop)) + "]");//升番号

                // マスが重複することがある☆？（＾～＾）？
                shogiban.AddKoma(
                    masu,
                    busstop,
                    logger
                );
            }

            return shogiban;
        }

        /// <summary>
        /// 千日手判定用の、局面ハッシュを返します。
        /// 
        /// TODO: 持ち駒も判定したい。
        /// </summary>
        /// <returns></returns>
        public static ulong ToKyokumenHash(ISky sky)
        {
            ulong hash = 0;

            foreach (Finger fig in sky.Fingers_All().Items)
            {
                sky.AssertFinger(fig);
                Busstop koma = sky.BusstopIndexOf(fig);

                // 盤上の駒。 FIXME: 持ち駒はまだ見ていない。
                ulong value = Util_ZobristHashing.GetValue(
                    Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma)),
                    Conv_Busstop.ToPlayerside(koma),
                    Conv_Busstop.ToKomasyurui(koma)
                    );

                hash ^= value;
            }

            return hash;
        }
    }
}
