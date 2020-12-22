using System.Collections.Generic;
using System.Text;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public class EngineOptionsImpl : EngineOptions
    {
        public EngineOptionsImpl()
        {
            this.m_entries_ = new Dictionary<string, IEngineOption>();
        }

        public Dictionary<string, IEngineOption> m_entries_ { get; set; }

        public void Clear()
        {
            this.m_entries_.Clear();
        }

        /// <summary>
        /// 項目の有無
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsKey(string name)
        {
            return this.m_entries_.ContainsKey(name);
        }

        /// <summary>
        /// 項目を追加。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entry"></param>
        public void AddOption(string name, IEngineOption entry)
        {
            this.m_entries_.Add(name, entry);
        }

        /// <summary>
        /// 項目を取得。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEngineOption GetOption(string name)
        {
            return this.m_entries_[name];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("┏━━━━━設定━━━━━┓");
            foreach (KeyValuePair<string, IEngineOption> entry in this.m_entries_)
            {
                sb.AppendLine(entry.Key + "=" + entry.Value);
            }
            sb.AppendLine("┗━━━━━━━━━━━━┛");

            return base.ToString();
        }
    }
}
