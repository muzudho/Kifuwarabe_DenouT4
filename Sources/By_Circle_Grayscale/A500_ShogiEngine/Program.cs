using System;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C500____KifuWarabe;

namespace Grayscale.P580_Form_______
{
    /// <summary>
    /// プログラムのエントリー・ポイントです。
    /// </summary>
    class Program
    {
        /// <summary>
        /// Ｃ＃のプログラムは、
        /// この Main 関数から始まり、 Main 関数を抜けて終わります。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // USIフレームワーク
            IUsiFramework usiFramework = new UsiFrameworkImpl();

            // 将棋エンジン　きふわらべ
            KifuWarabeImpl kifuWarabe = new KifuWarabeImpl(usiFramework);

            try
            {
                usiFramework.Execute();
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Util_Loggers.ProcessEngine_DEFAULT.DonimoNaranAkirameta("Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
                //throw ex;//追加
            }
        }
    }
}
