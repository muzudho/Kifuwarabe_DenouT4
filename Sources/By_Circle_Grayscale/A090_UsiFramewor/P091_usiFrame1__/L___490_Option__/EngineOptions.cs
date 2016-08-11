namespace Grayscale.P091_usiFrame1__.L___490_Option__
{
    /// <summary>
    /// USI「setoption」コマンドのリストです。
    /// </summary>
    public interface EngineOptions
    {

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
        void AddOption(string name, EngineOption entry);

        /// <summary>
        /// 項目を取得。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        EngineOption GetOption(string name);

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
        void AddOption_ByCommandline(string line);
    }
}
