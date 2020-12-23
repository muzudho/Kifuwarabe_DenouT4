
namespace Grayscale.Kifuwaragyoku.Entities.Features
{

    public delegate void DELEGATE_Masu201(int masuHandle, string masuString, ref bool toBreak);

    /// <summary>
    /// 「position sfen ～ moves」の、「sfen ～」の部分を読み込んだあとの、局面情報。
    /// 
    /// 指し手情報を含まない。
    /// 
    /// プロパティ１つ１つごとにメソッド（アクセッサー）を用意している。
    /// </summary>
    public interface ISfenFormat2
    {

        ISfenPosition1 ToKyokumen1();

        void Foreach_Masu201(DELEGATE_Masu201 delegate_method);

        string GetKomaAs(int suji, int dan);

        /// <summary>
        /// 持駒の枚数。
        /// </summary>
        int[] MotiSu { get; }



        /// <summary>
        /// 駒袋 王
        /// </summary>
        int UselessK { get; }

        /// <summary>
        /// 駒袋 飛
        /// </summary>
        int UselessR { get; }

        /// <summary>
        /// 駒袋 角
        /// </summary>
        int UselessB { get; }

        /// <summary>
        /// 駒袋 金
        /// </summary>
        int UselessG { get; }

        /// <summary>
        /// 駒袋 銀
        /// </summary>
        int UselessS { get; }

        /// <summary>
        /// 駒袋 桂
        /// </summary>
        int UselessN { get; }

        /// <summary>
        /// 駒袋 香
        /// </summary>
        int UselessL { get; }

        /// <summary>
        /// 駒袋 歩
        /// </summary>
        int UselessP { get; }


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
