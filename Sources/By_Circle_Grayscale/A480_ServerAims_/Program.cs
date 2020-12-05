using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B310Settei.C500Struct;
using Grayscale.A060Application.B310Settei.L510Xml;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C500____Server;

namespace Grayscale.P489_Form_______
{
    class Program
    {

        static void Main(string[] args)
        {
            KwLogger errH = Util_Loggers.ProcessAims_DEFAULT;
            MessageBox.Show("AIMSサーバー");


            string filepath = Const_Filepath.m_AIMS_TO_CONFIG + "data_settei.xml";
            MessageBox.Show("設定ファイルパス＝[" + filepath + "]");

            //
            // 設定XMLファイル
            //
            SetteiXmlFile setteiXmlFile;
            {
                setteiXmlFile = new SetteiXmlFile(filepath);
                //if (!setteiXmlFile.Exists())
                //{
                //    // ファイルが存在しませんでした。

                //    // 作ります。
                //    setteiXmlFile.Write();
                //}

                if (!setteiXmlFile.Read())
                {
                    // 読取に失敗しました。
                }

                // デバッグ
                //setteiXmlFile.DebugWrite();
            }


            MessageBox.Show("AIMSサーバー\n将棋エンジン・ファイルパス＝[" + setteiXmlFile.ShogiEngineFilePath + "]");

            Sky src_Sky = Util_SkyCreator.New_Hirate();

            AimsServerImpl aimsServer = new AimsServerImpl(src_Sky);
            aimsServer.ShogiEngineFilePath = setteiXmlFile.ShogiEngineFilePath;

            aimsServer.AtBegin();
            aimsServer.AtBody(errH);
            aimsServer.AtEnd();
        }
    }
}
