﻿using System;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___;
using Grayscale.A500ShogiEngine.B280KifuWarabe.C500KifuWarabe;

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
            ProgramSupport programSupport = new ProgramSupport(usiFramework);

            try
            {
                usiFramework.Execute();
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.Panic(LogTags.ProcessEngineDefault,"Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
                //throw;//追加
            }
        }
    }
}