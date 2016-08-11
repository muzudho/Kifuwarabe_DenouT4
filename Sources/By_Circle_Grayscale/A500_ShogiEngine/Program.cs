using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P091_usiFrame1__.L___500_usiFrame___;
using Grayscale.P575_KifuWarabe_.L500____KifuWarabe;
using Grayscale.P091_usiFrame1__.L500____usiFrame___;
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
                Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta("Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
            }
        }
    }
}
