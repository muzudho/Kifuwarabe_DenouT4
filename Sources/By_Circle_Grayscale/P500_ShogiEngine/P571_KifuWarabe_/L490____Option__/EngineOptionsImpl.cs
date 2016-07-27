using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P571_KifuWarabe_.L___490_Option__;

namespace Grayscale.P571_KifuWarabe_.L490____Option__
{
    public class EngineOptionsImpl : EngineOptions
    {
        public EngineOptionsImpl()
        {
            this.m_entries_ = new Dictionary<string, EngineOption>();
        }

        private Dictionary<string, EngineOption> m_entries_;
        //public Dictionary<string, EngineOption> Entries
        //{
        //    get { return this.m_entries_; }
        //    set { this.m_entries_ = value; }
        //}

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
        /// 項目を上書き。なければ文字列型として項目を追加。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entry"></param>
        public void ParseValue_AutoAdd(string name, string value)
        {
            if (this.ContainsKey(name))
            {
                this.m_entries_[name].ParseValue(value);
            }
            else
            {
                // 文字列型項目の追加。
                this.m_entries_.Add(name, new EngineOption_StringImpl( value));
            }
        }

        /// <summary>
        /// 項目を追加。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entry"></param>
        public void AddEntry(string name, EngineOption entry)
        {
            this.m_entries_.Add(name, entry);
        }
        /// <summary>
        /// 項目を取得。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EngineOption GetEntry(string name)
        {
            return this.m_entries_[name];
        }

        /// <summary>
        /// 現在値（文字列読取）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void ParseValue(string name,string value)
        {
            this.m_entries_[name].ParseValue( value);
        }

    }
}
