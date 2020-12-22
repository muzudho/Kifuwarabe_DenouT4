using System.IO;
using System.Windows.Forms;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Nett;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    class Program
    {

        static void MainOfAims(string[] args)
        {
            ILogTag logTag = LogTags.ProcessAimsDefault;
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
            aimsServer.AtBody(logTag);
            aimsServer.AtEnd();
        }
    }
}
