﻿// noop 可
//#define NOOPABLE

using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P003_Log________.L500____Struct;
using Grayscale.P005_Tushin_____.L500____Util;
using Grayscale.P027_Settei_____.L500____Struct;
using Grayscale.P236_KomahaiyaTr.L500____Table;
using Grayscale.P248_Michi______.L500____Word;
using Grayscale.P250_KomahaiyaEx.L500____Util;
using Grayscale.P270_ForcePromot.L250____Struct;
using Grayscale.P523_UtilFv_____.L510____UtilFvLoad;
using Grayscale.P542_Scoreing___.L___005_UsiLoop;
using Grayscale.P542_Scoreing___.L___240_Shogisasi;
using Grayscale.P571_usiFrame1__.L___490_Option__;
using Grayscale.P571_usiFrame1__.L490____Option__;
using Grayscale.P575_KifuWarabe_.L100____Shogisasi;
using Grayscale.P575_KifuWarabe_.L250____UsiLoop;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Grayscale.P571_usiFrame1__.L___500_usiFrame___;//FIXME:

namespace Grayscale.P575_KifuWarabe_.L500____KifuWarabe
{

    public class KifuWarabeImpl : ShogiEngine
    {

        #region プロパティー
        /// <summary>
        /// きふわらべの作者名です。
        /// </summary>
        public string AuthorName { get { return this.authorName; } }
        private string authorName;


        /// <summary>
        /// 製品名です。
        /// </summary>
        public string SeihinName { get { return this.seihinName; } }
        private string seihinName;

        /// <summary>
        /// 将棋エンジンの中の一大要素「思考エンジン」です。
        /// 指す１手の答えを出すのが仕事です。
        /// </summary>
        public Shogisasi shogisasi;

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        public EngineOptions EngineOptions { get; set; }

        #endregion

        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        public void Send(string line)
        {
            // 将棋サーバーに向かってメッセージを送り出します。
            Util_Message.Upload(line);

#if DEBUG
            // 送信記録をつけます。
            Util_OwataMinister.ENGINE_NETWORK.Logger.WriteLine_S(line);
#endif
        }
        #endregion

        #region コンストラクター
        /// <summary>
        /// コンストラクター
        /// </summary>
        public KifuWarabeImpl()
        {
            // 作者名
            this.authorName = "TAKAHASHI Satoshi"; // むずでょ

            // 製品名
            this.seihinName = ((System.Reflection.AssemblyProductAttribute)Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(System.Reflection.AssemblyProductAttribute))).Product;

            //-------------+----------------------------------------------------------------------------------------------------------
            // データ設計  |
            //-------------+----------------------------------------------------------------------------------------------------------
            // 将棋所から送られてくるデータを、一覧表に変えたものです。
            this.EngineOptions = new EngineOptionsImpl();
            this.EngineOptions.AddOption(EngineOptionNames.USI_PONDER, new EngineOption_BoolImpl());// ポンダーに対応している将棋サーバーなら真です。
            this.EngineOptions.AddOption(EngineOptionNames.NOOPABLE, new EngineOption_BoolImpl());// 独自実装のコマンドなので、ＯＦＦにしておきます。
            this.EngineOptions.AddOption(EngineOptionNames.THINKING_MILLI_SECOND, new EngineOption_NumberImpl(4000));
        }
        #endregion

        #region 処理の流れ
        public void AtBegin(KwErrorHandler errH)
        {
            int exception_area = 0;
            try
            {
                exception_area = 500;
                //-------------------+----------------------------------------------------------------------------------------------------
                // ログファイル削除  |
                //-------------------+----------------------------------------------------------------------------------------------------
                {
                    #region ↓詳説
                    //
                    // 図.
                    //
                    //      フォルダー
                    //          ├─ Engine.KifuWarabe.exe
                    //          └─ log.txt               ←これを削除
                    //
                    #endregion
                    Util_OwataMinister.Remove_AllLogFiles();
                }


                exception_area = 1000;
                //------------------------------------------------------------------------------------------------------------------------
                // 思考エンジンの、記憶を読み取ります。
                //------------------------------------------------------------------------------------------------------------------------
                {
                    this.shogisasi = new ShogisasiImpl(this);
                    Util_FvLoad.OpenFv(this.shogisasi.FeatureVector, Const_Filepath.m_EXE_TO_CONFIG + "fv/fv_00_Komawari.csv", errH);
                }


                exception_area = 2000;
                //------------------------------------------------------------------------------------------------------------------------
                // ファイル読込み
                //------------------------------------------------------------------------------------------------------------------------
                {
                    string dataFolder = Path.Combine(Application.StartupPath, Const_Filepath.m_EXE_TO_CONFIG);
                    string logsFolder = Path.Combine(Application.StartupPath, Const_Filepath.m_EXE_TO_LOGGINGS);

                    // データの読取「道」
                    string filepath_Michi = Path.Combine(dataFolder, "data_michi187.csv");
                    if (Michi187Array.Load(filepath_Michi))
                    {
                    }

                    // データの読取「配役」
                    string filepath_Haiyaku = Path.Combine(dataFolder, "data_haiyaku185_UTF-8.csv");
                    Util_Array_KomahaiyakuEx184.Load(filepath_Haiyaku, Encoding.UTF8);

                    // データの読取「強制転成表」　※駒配役を生成した後で。
                    string filepath_ForcePromotion = Path.Combine(dataFolder, "data_forcePromotion_UTF-8.csv");
                    Array_ForcePromotion.Load(filepath_ForcePromotion, Encoding.UTF8);

#if DEBUG
                    {
                        string filepath_LogKyosei = Path.Combine(logsFolder, "_log_強制転成表.html");
                        File.WriteAllText(filepath_LogKyosei, Array_ForcePromotion.LogHtml());
                    }
#endif

                    // データの読取「配役転換表」
                    string filepath_HaiyakuTenkan = Path.Combine(dataFolder, "data_syuruiToHaiyaku.csv");
                    Data_KomahaiyakuTransition.Load(filepath_HaiyakuTenkan, Encoding.UTF8);

#if DEBUG
                    {
                        string filepath_LogHaiyakuTenkan = Path.Combine(logsFolder, "_log_配役転換表.html");
                        File.WriteAllText(filepath_LogHaiyakuTenkan, Data_KomahaiyakuTransition.Format_LogHtml());
                    }
#endif
                }

                exception_area = 4000;
                //-------------+----------------------------------------------------------------------------------------------------------
                // ログ書込み  |  ＜この将棋エンジン＞  製品名、バージョン番号
                //-------------+----------------------------------------------------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      │2014/08/02 1:04:59> v(^▽^)v ｲｪｰｲ☆ ... fugafuga 1.00.0
                //      │
                //      │
                //
                //
                // 製品名とバージョン番号は、次のファイルに書かれているものを使っています。
                // 場所：  [ソリューション エクスプローラー]-[ソリューション名]-[プロジェクト名]-[Properties]-[AssemblyInfo.cs] の中の、[AssemblyProduct]と[AssemblyVersion] を参照。
                //
                // バージョン番号を「1.00.0」形式（メジャー番号.マイナー番号.ビルド番号)で書くのは作者の趣味です。
                //
                #endregion
                {
                    string versionStr;

                    // バージョン番号
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    versionStr = String.Format("{0}.{1}.{2}", version.Major, version.Minor.ToString("00"), version.Build);

                    //seihinName += " " + versionStr;
#if DEBUG
                    Util_OwataMinister.ENGINE_DEFAULT.Logger.WriteLine_AddMemo("v(^▽^)v ｲｪｰｲ☆ ... " + this.SeihinName + " " + versionStr);
#endif
                }

            }
            catch (Exception ex)
            {
                switch (exception_area)
                {
                    case 1000:
                        Util_OwataMinister.ENGINE_DEFAULT.DonimoNaranAkirameta("フィーチャーベクターCSVを読み込んでいるとき。" + ex.GetType().Name + "：" + ex.Message);
                        break;
                }
                throw ex;
            }
        }


        public void AtEnd()
        {
        }
        #endregion

    }
}
