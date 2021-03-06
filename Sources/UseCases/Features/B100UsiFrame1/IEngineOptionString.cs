﻿namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public interface IEngineOptionString : IEngineOption
    {

        /// <summary>
        /// 既定値
        /// </summary>
        string Default { get; set; }

        /// <summary>
        /// 現在値
        /// </summary>
        string Value { get; set; }

    }
}
