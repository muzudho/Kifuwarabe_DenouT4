using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using Grayscale.P324_KifuTree___.L___250_Struct;
using Grayscale.P324_KifuTree___.L250____Struct;
using Grayscale.P542_Scoreing___.L___240_Shogisasi;
using Grayscale.P575_KifuWarabe_.L100____Shogisasi;
using Grayscale.P575_KifuWarabe_.L500____KifuWarabe;
using Grayscale.P579_usiFrame2__.L___500_usiFrame___;
using Grayscale.P579_usiFrame2__.L500____usiFrame___;


namespace P930_SampleGame
{
    class Program
    {
        /// <summary>
        /// プロファイラを使って、実行時速度を計測するためのもの。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            KwErrorHandler errH = Util_OwataMinister.ENGINE_DEFAULT;


            // 将棋エンジン　きふわらべ
            KifuWarabeImpl kifuWarabe = new KifuWarabeImpl();
            kifuWarabe.AtBegin(errH);


            // 将棋指しオブジェクト
            Shogisasi shogisasi = new ShogisasiImpl(kifuWarabe);

            // 棋譜
            KifuTree kifu = new KifuTreeImpl(
                        new KifuNodeImpl(
                            Util_Sky258A.ROOT_SASITE,
                            new KyokumenWrapper(Util_SkyWriter.New_Hirate(Playerside.P1))//日本の符号読取時
                        )
                );

            KifuNode bestmoveNode = shogisasi.WA_Bestmove(true, kifu, errH);

            //bool isTimeoutShutdown_temp;
            //kifuWarabe.AtBody(out isTimeoutShutdown_temp, errH);    // 将棋サーバーからのメッセージの受信や、
                                                                    // 思考は、ここで行っています。
            kifuWarabe.AtEnd();
        }
    }
}
