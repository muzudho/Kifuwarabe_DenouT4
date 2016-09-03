using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C___500_usiFrame___;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C500____KifuWarabe;
using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C500____usiFrame___;
using System;

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
            UsiFramework usiFramework = new UsiFrameworkImpl();

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
                Util_Loggers.ENGINE_DEFAULT.DonimoNaranAkirameta("Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
            }
        }
    }
}
