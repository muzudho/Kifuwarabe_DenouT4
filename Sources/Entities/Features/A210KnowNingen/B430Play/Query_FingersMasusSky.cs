﻿using Grayscale.Kifuwaragyoku.Entities.Logging;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    //public class Arg_KmEffectSameIKUSA()
    //{
    //    public Arg_KmEffectSameIKUSA()
    //    {
    //    }
    //}

    public class Query_FingersMasusSky
    {

        /// <summary>
        /// 盤上の駒の利き（利きを調べる側）
        /// </summary>
        /// <param name="fs_sirabetaiKoma">fingers</param>
        /// <param name="masus_mikata_Banjo"></param>
        /// <param name="masus_aite_Banjo"></param>
        /// <param name="src_Sky"></param>
        /// <param name="errH_orNull"></param>
        /// <returns></returns>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> To_KomabetuKiki_OnBanjo(
            Fingers fs_sirabetaiKoma,
            SySet<SyElement> masus_mikata_Banjo,
            SySet<SyElement> masus_aite_Banjo,
            ISky src_Sky,
            ILogTag errH_orNull
            )
        {
            // 利きを調べる側の利き（戦駒）
            Maps_OneAndOne<Finger, SySet<SyElement>> komabetuKiki = QuerySkyFingers.Get_PotentialMoves(src_Sky, fs_sirabetaiKoma, errH_orNull);

            // 盤上の現手番の駒利きから、 現手番の駒がある枡を除外します。
            komabetuKiki = Play_KomaAndMove.MinusMasus(src_Sky, komabetuKiki, masus_mikata_Banjo, errH_orNull);

            // そこから、相手番の駒がある枡「以降」を更に除外します。
            komabetuKiki = Play_KomaAndMove.Minus_OverThereMasus(src_Sky, komabetuKiki, masus_aite_Banjo, errH_orNull);

            return komabetuKiki;
        }




    }
}
