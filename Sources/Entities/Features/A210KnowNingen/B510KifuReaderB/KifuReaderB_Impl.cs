﻿namespace Grayscale.Kifuwaragyoku.Entities.Features
{


    public class KifuReaderB_Impl
    {

        public IKifuReaderBState State { get; set; }

        public KifuReaderB_Impl()
        {
            this.State = new KifuReaderB_StateB0();
        }


        public void Execute(string inputLine, out string nextCommand, out string rest)
        {
            this.State.Execute(inputLine, out nextCommand, out rest);
        }

    }


}
