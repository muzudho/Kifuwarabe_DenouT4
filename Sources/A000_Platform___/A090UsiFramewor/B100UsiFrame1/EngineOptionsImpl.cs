using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;

namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option
{
    public class EngineOptionsImpl : EngineOptions
    {
        public EngineOptionsImpl()
        {
            this.m_entries_ = new Dictionary<string, IEngineOption>();
        }

        private Dictionary<string, IEngineOption> m_entries_;

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
        /// 既存項目の場合、型に合わせて上書き。なければ文字列型として項目を新規追加。
        /// 
        /// GUIから思考エンジンへ送られてくる方は１つ目が "setoption"、思考エンジンからGUIへ送る方は"option"。
        /// setoptionの場合、プロパティは name と value だけになり、型情報がない（全て文字列型）。
        /// 
        /// チェックボックス
        /// "option name 子 type check default true"
        /// 
        /// スピンボックス
        /// "option name USI type spin default 2 min 1 max 13"
        /// 
        /// コンボボックス
        /// "option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー"
        /// 
        /// ボタン
        /// "option name 卯 type button default うさぎ"
        /// 
        /// 文字列
        /// "option name 辰 type string default DRAGON"
        /// 
        /// ファイル名
        /// "option name 巳 type filename default スネーク.html"
        /// </summary>
        /// <param name="line">コマンドライン</param>
        public void AddOption_ByCommandline(string line)
        {
            string[] tokens = line.Split(' ');
            int index = 0;
            if ("setoption" == tokens[index] || "option" == tokens[index])
            {
                // プロパティ名
                string propertyName = "";

                // プロパティ値
                string valueName = "";
                string valueType = "";
                string valueDefault = "";
                List<string> valueVars = new List<string>();
                string valueMin = "";
                string valueMax = "";

                // 部品ごとに、ばらばらにするぜ☆（＾▽＾）
                index++;
                for (; index < tokens.Length; index++)
                {
                    string token = tokens[index];

                    if ("" == propertyName)
                    {
                        // プロパティ名
                        if (token == "name" || token == "type" || token == "default" || token == "var" || token == "min" || token == "max")
                        {
                            propertyName = token;
                        }
                    }
                    else
                    {
                        // プロパティ値
                        switch (token)
                        {
                            case "name": valueName = token; break;
                            case "type": valueType = token; break;
                            case "default": valueDefault = token; break;
                            case "var": valueVars.Add(token); break;
                            case "min": valueMin = token; break;
                            case "max": valueMax = token; break;
                            default: break;
                        }
                    }
                }

                // 部品を組み立てるぜ☆（＾▽＾）
                IEngineOption option = null;
                switch (valueType)
                {
                    case "check": option = new EngineOptionBoolImpl(valueDefault); break;
                    case "spin": option = new EngineOptionSpinImpl(valueDefault, valueMin, valueMax); break;
                    case "combo": option = new EngineOptionComboImpl(valueDefault, valueVars); break;
                    case "button": option = new EngineOptionButtonImpl(valueDefault); break;
                    case "filename": option = new EngineOptionFilenameImpl(valueDefault); break;

                    case "string": //thru
                    default: option = new EngineOptionStringImpl(valueDefault); break;
                }

                // 同じものがすでにないか調べるぜ☆（＾▽＾）
                if (this.ContainsKey(valueName))
                {
                    // 既に同じものがある場合。
                    bool typeCheck;
                    switch (valueType)
                    {
                        case "check": typeCheck = this.m_entries_[valueName] is EngineOptionBoolImpl; break;
                        case "spin": typeCheck = this.m_entries_[valueName] is EngineOptionSpinImpl; break;
                        case "combo": typeCheck = this.m_entries_[valueName] is EngineOptionComboImpl; break;
                        case "button": typeCheck = this.m_entries_[valueName] is EngineOptionButtonImpl; break;
                        case "filename": typeCheck = this.m_entries_[valueName] is EngineOptionFilenameImpl; break;

                        case "string": //thru
                        default: typeCheck = this.m_entries_[valueName] is EngineOptionStringImpl; break;
                    }

                    if (!typeCheck)
                    {
                        throw new ApplicationException("オプションの型変換エラー。");
                    }

                    this.m_entries_[valueName].Reset(
                        valueDefault,
                        valueVars,
                        valueMin,
                        valueMax
                        );
                }
                else
                {
                    // 新規の場合
                    this.m_entries_.Add(valueName, option);
                }
            }
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
