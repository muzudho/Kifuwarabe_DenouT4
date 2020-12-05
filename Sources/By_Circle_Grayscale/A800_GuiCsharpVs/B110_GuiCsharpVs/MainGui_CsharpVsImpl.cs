using System.Text;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B690Ittesasu.C250OperationA;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500GUI;
using Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C492Widget;

namespace Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C500Gui
{
    /// <summary>
    /// 将棋盤ＧＵＩ VS（C#）用
    /// </summary>
    public class MainGui_CsharpVsImpl : MainGui_CsharpImpl, MainGui_Csharp
    {


        public MainGui_CsharpVsImpl()
        {
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 手番が替わったときの挙動を、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        public override void ChangedTurn(

            //MoveEx endNode,
            Tree kifu1,

            Playerside pside,//endNode.GetNodeValue().KaisiPside,
            ILogger errH)
        {
            this.Link_Server.EngineClient.OnChangedTurn(
                this.Link_Server.Earth,
                kifu1,// endNode,//エンドノード
                pside,
                errH);
        }

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public override void Shutdown(ILogger errH)
        {
            this.Link_Server.EngineClient.Send_Shutdown(errH);
        }

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public override void Logdase(ILogger errH)
        {
            this.Link_Server.EngineClient.Send_Logdase(errH);
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンを起動します。
        /// ************************************************************************************************************************
        /// </summary>
        public override void Start_ShogiEngine(string shogiEngineFilePath, ILogger errH)
        {
            this.Link_Server.EngineClient.Start(shogiEngineFilePath);
            this.Link_Server.EngineClient.ShogiEngineProcessWrapper.Send_Usi(errH);
        }

        /// <summary>
        /// コンピューターの先手
        /// </summary>
        public override void Do_ComputerSente(ILogger errH)
        {
            this.Link_Server.EngineClient.ShogiEngineProcessWrapper.Send_Position(
                UtilKirokuGakari.ToSfen_PositionCommand(
                    this.Link_Server.Earth,
                    this.Link_Server.KifuTree//.CurrentNode//エンドノード
                    ), errH);
            this.Link_Server.EngineClient.ShogiEngineProcessWrapper.Send_Go(errH);
        }


        /// <summary>
        /// このアプリケーションソフトの開始時の処理。
        /// </summary>
        public new void Load_AsStart(ILogger errH)
        {
            base.Load_AsStart(errH);

            this.Data_Settei_Csv.Read_Add("../../Engine01_Config/data_settei_vs.csv", Encoding.UTF8);
            this.Data_Settei_Csv.DebugOut();
            this.WidgetLoaders.Add(new WidgetsLoader_CsharpVsImpl("../../Engine01_Config/data_widgets_03_vs.csv", this));
        }

    }
}
