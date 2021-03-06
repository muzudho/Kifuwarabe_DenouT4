﻿using Grayscale.Kifuwaragyoku.Entities;
using Grayscale.Kifuwaragyoku.Entities.Configuration;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    /// <summary>
    /// 擬似将棋サーバー。
    /// </summary>
    public class Server_Impl : Server
    {
        public Server_Impl(IEngineConf engineConf, IPosition src_Sky, IReceiver receiver)
        {
            this.engineClient = new EngineClientImpl(receiver);
            this.engineClient.SetOwner_Server(this);

            //----------
            // モデル
            //----------
            IPosition positionInit = new Position(src_Sky);
            this.m_kifuTree_ = new TreeImpl(positionInit);
            this.Playing = new Playing(engineConf);
            this.Playing.SetEarthProperty(Word_KifuTree.PropName_Startpos, "9/9/9/9/9/9/9/9/9");

            this.inputString99 = "";
        }

        public IPlaying Playing { get; private set; }


        public ITree KifuTree { get { return this.m_kifuTree_; } }
        public void SetKifuTree(ITree kifu1)
        {
            this.m_kifuTree_ = kifu1;
        }
        private ITree m_kifuTree_;

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
    }
}
