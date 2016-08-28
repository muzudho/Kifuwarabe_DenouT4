
namespace Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct
{

    public delegate void DELEGATE_Masu201(int masuHandle, string masuString, ref bool toBreak);

    /// <summary>
    /// 「position sfen ～ moves」の、「sfen ～」の部分を読み込んだあとの、局面情報。
    /// 
    /// 指し手情報を含まない。
    /// 
    /// プロパティ１つ１つごとにメソッド（アクセッサー）を用意している。
    /// </summary>
    public interface RO_Kyokumen2_ForTokenize
    {

        RO_Kyokumen1_ForFormat ToKyokumen1();

        void Foreach_Masu201(DELEGATE_Masu201 delegate_method);

        string GetKomaAs(int suji, int dan);

        /// <summary>
        /// 持駒の枚数。
        /// </summary>
        int[] MotiSu { get; }



        /// <summary>
        /// 駒袋 王
        /// </summary>
        int FukuroK { get; }

        /// <summary>
        /// 駒袋 飛
        /// </summary>
        int FukuroR { get; }

        /// <summary>
        /// 駒袋 角
        /// </summary>
        int FukuroB { get; }

        /// <summary>
        /// 駒袋 金
        /// </summary>
        int FukuroG { get; }

        /// <summary>
        /// 駒袋 銀
        /// </summary>
        int FukuroS { get; }

        /// <summary>
        /// 駒袋 桂
        /// </summary>
        int FukuroN { get; }

        /// <summary>
        /// 駒袋 香
        /// </summary>
        int FukuroL { get; }

        /// <summary>
        /// 駒袋 歩
        /// </summary>
        int FukuroP { get; }


        /// <summary>
        /// 先後。
        /// </summary>
        bool PsideIsBlack { get; }




        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 手目済
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        int Temezumi { get; }

    }
}
