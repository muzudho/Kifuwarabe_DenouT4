using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P575_KifuWarabe_.L500____KifuWarabe;
using Grayscale.P571_usiFrame1__.L___500_usiFrame___;
using Grayscale.P579_usiFrame2__.L500____usiFrame___;

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
            KifuWarabeImpl kifuWarabe = new KifuWarabeImpl();

            KwErrorHandler errH = Util_OwataMinister.ENGINE_DEFAULT;


            usiFramework.OnBegin_InAll(()=> {
                kifuWarabe.AtBegin(errH);
            });
            usiFramework.OnBody_InAll(() => {

                try
                {
                    #region ↑詳説
                    // 
                    // 図.
                    // 
                    //     プログラムの開始：  ここの先頭行から始まります。
                    //     プログラムの実行：  この中で、ずっと無限ループし続けています。
                    //     プログラムの終了：  この中の最終行を終えたとき、
                    //                         または途中で Environment.Exit(0); が呼ばれたときに終わります。
                    //                         また、コンソールウィンドウの[×]ボタンを押して強制終了されたときも  ぶつ切り  で突然終わります。
                    #endregion

                    //************************************************************************************************************************
                    // ループ（全体）
                    //************************************************************************************************************************
                    #region ↓詳説
                    //
                    // 図.
                    //
                    //      無限ループ（全体）
                    //          │
                    //          ├─無限ループ（１）
                    //          │                      将棋エンジンであることが認知されるまで、目で訴え続けます(^▽^)
                    //          │                      認知されると、無限ループ（２）に進みます。
                    //          │
                    //          └─無限ループ（２）
                    //                                  対局中、ずっとです。
                    //                                  対局が終わると、無限ループ（１）に戻ります。
                    //
                    // 無限ループの中に、２つの無限ループが入っています。
                    //
                    #endregion

                    bool isTimeoutShutdown_temp;
                    bool isQuit;
                    while (true)//全体ループ
                    {
#if DEBUG_STOPPABLE
        MessageBox.Show("きふわらべのMainの無限ループでブレイク☆！", "デバッグ");
        System.Diagnostics.Debugger.Break();
#endif
                        kifuWarabe.AtBody(out isTimeoutShutdown_temp, out isQuit, errH, usiFramework);    // 将棋サーバーからのメッセージの受信や、
                                                                                                              // 思考は、ここで行っています。
                        if (isTimeoutShutdown_temp || isQuit) { break; }//全体ループを抜けるぜ☆（＾▽＾）
                    }
                }
                catch (Exception ex)
                {
                    // エラーが起こりました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    // どうにもできないので  ログだけ取って無視します。
                    Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta("Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
                }

            });
            usiFramework.OnEnd_InAll(() => {
                kifuWarabe.AtEnd();
            });


        }
    }
}
