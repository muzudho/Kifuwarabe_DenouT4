﻿
namespace Grayscale.A210_KnowNingen_.P510_KifuReaderB.C___500_Reader
{
    public interface KifuReaderB_State
    {

        void Execute(string inputLine, out string nextCommand, out string rest);

    }
}