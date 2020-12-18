﻿namespace Grayscale.Kifuwaragyoku.Entities.Logging
{
    /// <summary>
    /// ログのタグ。
    /// </summary>
    public class LogTag : ILogTag
    {
        public LogTag(string value)
        {
            this.Name = value;
        }

        /// <summary>
        /// 名前。
        /// </summary>
        public string Name { get; private set; }
    }
}
