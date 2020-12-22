using System;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Features;

namespace Grayscale.A500ShogiEngine.B180Hyokakansu.C490UtilSokutei
{
    /// <summary>
    /// 近似測定エンジン。
    /// </summary>
    public abstract class Util_KomanoKyori
    {

        /// <summary>
        /// 距離の近さ。盤上とします。
        /// </summary>
        /// <returns></returns>
        public static int GetBanjoKyori(SyElement mokuhyo, SyElement genzai)
        {
            int masuNumber = Conv_Masu.ToMasuHandle(mokuhyo);
            return Util_KomanoKyori.GetBanjoKyori(masuNumber, genzai);
        }

        /// <summary>
        /// 距離の近さ。盤上とします。
        /// </summary>
        /// <returns></returns>
        public static int GetBanjoKyori(int masuNumber, SyElement genzai)
        {
            //
            // とりあえず　おおざっぱに計算します。
            //

            int mokuhyoDan;
            Conv_Masu.ToDan_FromBanjoMasu(masuNumber, out mokuhyoDan);

            int mokuhyoSuji;
            Conv_Masu.ToSuji_FromBanjoMasu(masuNumber, out mokuhyoSuji);

            int genzaiDan;
            Conv_Masu.ToDan_FromBanjoMasu(genzai, out genzaiDan);

            int genzaiSuji;
            Conv_Masu.ToSuji_FromBanjoMasu(genzai, out genzaiSuji);

            int kyori = Math.Abs(mokuhyoDan - genzaiDan) + Math.Abs(mokuhyoSuji - genzaiSuji);

            return kyori;
        }

    }
}
