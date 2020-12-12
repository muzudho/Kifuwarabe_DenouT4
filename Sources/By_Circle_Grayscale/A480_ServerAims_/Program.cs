using System.IO;
using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B310Settei.C500Struct;
using Grayscale.A060Application.B310Settei.L510Xml;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A480ServerAims.B110AimsServer.C500Server;
using Nett;

namespace Grayscale.P489Form
{
    class Program
    {

        static void Main(string[] args)
        {
            ILogger errH = ErrorControllerReference.ProcessAimsDefault;
            MessageBox.Show("AIMSサーバー");

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));


            string filepath = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("AimsDataSetteiXml"));
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

            ISky src_Sky = UtilSkyCreator.New_Hirate();

            AimsServerImpl aimsServer = new AimsServerImpl(src_Sky);
            aimsServer.ShogiEngineFilePath = setteiXmlFile.ShogiEngineFilePath;

            aimsServer.AtBegin();
            aimsServer.AtBody(errH);
            aimsServer.AtEnd();
        }
    }
}
