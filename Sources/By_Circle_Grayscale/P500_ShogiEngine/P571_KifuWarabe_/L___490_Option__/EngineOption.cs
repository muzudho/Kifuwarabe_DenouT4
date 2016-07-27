namespace Grayscale.P571_KifuWarabe_.L___490_Option__
{
    public interface EngineOption
    {

        /// <summary>
        /// 現在値（文字列読取）
        /// </summary>
        /// <param name="value"></param>
        void ParseValue(string value);

        /// <summary>
        /// 論理値型でのみ使用可能。論理値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        bool IsTrue();
    }
}
