﻿namespace Grayscale.A690FvLearn.B110FvLearn.C___490_StopLearning
{
    public interface StopLearning
    {

        /// <summary>
        /// Stop_learning.txt ファイルへのパス。
        /// </summary>
        string StopLearningFilePath { get; }
        void SetStopLearningFilePath(string value);

        /// <summary>
        /// 停止させるなら真。
        /// </summary>
        /// <returns></returns>
        bool IsStop();
    }
}