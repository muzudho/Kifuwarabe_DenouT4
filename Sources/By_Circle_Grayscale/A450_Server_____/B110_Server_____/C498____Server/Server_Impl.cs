﻿using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A450_Server_____.B110_Server_____.C___125_Receiver;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;
using Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient;

namespace Grayscale.A450_Server_____.B110_Server_____.C498____Server
{
    /// <summary>
    /// 擬似将棋サーバー。
    /// </summary>
    public class Server_Impl : Server
    {
        #region プロパティ

        public Model_Taikyoku Model_Taikyoku { get { return this.model_Taikyoku; } }
        private Model_Taikyoku model_Taikyoku;


        /// <summary>
        /// サーバーが持つ、将棋エンジン。
        /// </summary>
        public EngineClient EngineClient { get { return this.engineClient; } }
        protected EngineClient engineClient;

        /// <summary>
        /// 将棋エンジンからの入力文字列（入力欄に入ったもの）を、一旦　蓄えたもの。
        /// </summary>
        public string InputString99 { get { return this.inputString99; } }
        public void AddInputString99(string inputString99)
        {
            this.inputString99 += inputString99;
        }
        public void SetInputString99(string inputString99)
        {
            this.inputString99 = inputString99;
        }
        public void ClearInputString99()
        {
            this.inputString99 = "";
        }
        private string inputString99;

        #endregion


        public Server_Impl(SkyConst src_Sky, int temezumi, Receiver receiver)
        {
            this.engineClient = new EngineClient_Impl(receiver);
            this.engineClient.SetOwner_Server(this);

            //----------
            // モデル
            //----------
            this.model_Taikyoku = new Model_TaikyokuImpl(new KifuTreeImpl(
                    new KifuNodeImpl(
                        Conv_Move.GetErrorMove(),
                        new KyokumenWrapper(SkyConst.NewInstance(
                            src_Sky,//model_Manual.GuiSkyConst,
                            temezumi//model_Manual.GuiTemezumi
                            ))
                    )
            ));
            this.Model_Taikyoku.Kifu.SetProperty(Word_KifuTree.PropName_Startpos, "9/9/9/9/9/9/9/9/9");

            this.inputString99 = "";
        }

    }
}