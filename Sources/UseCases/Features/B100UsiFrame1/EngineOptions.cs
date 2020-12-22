using System.Collections.Generic;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    /// <summary>
    /// USI「setoption」コマンドのリストです。
    /// </summary>
    public interface EngineOptions
    {
        Dictionary<string, IEngineOption> m_entries_ { get; set; }

        void Clear();

        /// <summary>
        /// 項目の有無
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ContainsKey(string name);

        /// <summary>
        /// 項目を追加。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entry"></param>
        void AddOption(string name, IEngineOption entry);

        /// <summary>
        /// 項目を取得。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEngineOption GetOption(string name);
    }
}
