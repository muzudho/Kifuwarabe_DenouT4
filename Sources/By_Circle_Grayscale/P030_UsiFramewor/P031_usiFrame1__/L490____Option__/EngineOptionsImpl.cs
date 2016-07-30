using Grayscale.P031_usiFrame1__.L___490_Option__;
using System.Collections.Generic;
using System;
using System.Text;

namespace Grayscale.P031_usiFrame1__.L490____Option__
{
    public class EngineOptionsImpl : EngineOptions
    {
        public EngineOptionsImpl()
        {
            this.m_entries_ = new Dictionary<string, EngineOption>();
        }

        private Dictionary<string, EngineOption> m_entries_;

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
        public void AddOption(string name, EngineOption entry)
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
                string valueType="";
                string valueDefault = "";
                List<string> valueVars = new List<string>();
                string valueMin = "";
                string valueMax = "";

                // 部品ごとに、ばらばらにするぜ☆（＾▽＾）
                index++;
                for (;index<tokens.Length;index++ )
                {
                    string token = tokens[index];

                    if ("" == propertyName)
                    {
                        // プロパティ名
                        if (token== "name"|| token == "type" || token == "default" || token == "var" || token == "min" || token == "max")
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
                EngineOption option =null;
                switch (valueType)
                {
                    case "check": option = new EngineOption_BoolImpl(valueDefault); break;
                    case "spin": option = new EngineOption_SpinImpl(valueDefault, valueMin, valueMax); break;
                    case "combo": option = new EngineOption_ComboImpl(valueDefault,valueVars); break;
                    case "button": option = new EngineOption_ButtonImpl(valueDefault); break;
                    case "filename": option = new EngineOption_FilenameImpl(valueDefault); break;

                    case "string": //thru
                    default: option = new EngineOption_StringImpl(valueDefault); break;
                }

                // 同じものがすでにないか調べるぜ☆（＾▽＾）
                if (this.ContainsKey(valueName))
                {
                    // 既に同じものがある場合。
                    bool typeCheck;
                    switch (valueType)
                    {
                        case "check": typeCheck = this.m_entries_[valueName] is EngineOption_BoolImpl; break;
                        case "spin": typeCheck = this.m_entries_[valueName] is EngineOption_SpinImpl; break;
                        case "combo": typeCheck = this.m_entries_[valueName] is EngineOption_ComboImpl; break;
                        case "button": typeCheck = this.m_entries_[valueName] is EngineOption_ButtonImpl; break;
                        case "filename": typeCheck = this.m_entries_[valueName] is EngineOption_FilenameImpl; break;

                        case "string": //thru
                        default: typeCheck = this.m_entries_[valueName] is EngineOption_StringImpl; break;
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
        public EngineOption GetOption(string name)
        {
            return this.m_entries_[name];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("┏━━━━━設定━━━━━┓");
            foreach (KeyValuePair<string,EngineOption> entry in this.m_entries_)
            {
                sb.AppendLine(entry.Key + "=" + entry.Value);
            }
            sb.AppendLine("┗━━━━━━━━━━━━┛");

            return base.ToString();
        }
    }
}
