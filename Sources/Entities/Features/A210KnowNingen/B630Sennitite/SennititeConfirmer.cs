﻿
namespace Grayscale.A210KnowNingen.B630Sennitite.C500Struct
{

    /// <summary>
    /// 千日手になるかどうかを判定するだけのクラスです。
    /// </summary>
    public interface SennititeConfirmer
    {
        /// <summary>
        /// 次に足したら、４回目以上になる場合、真。
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool IsNextSennitite(ulong hash);
    }
}