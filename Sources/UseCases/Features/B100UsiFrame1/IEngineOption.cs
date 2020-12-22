using System.Collections.Generic;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public interface IEngineOption
    {
        void Reset(
            string valueDefault,
            List<string> valueVars,
            string valueMin,
            string valueMax
            );

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

        /// <summary>
        /// 数値型でのみ使用可能。数値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        long GetNumber();
    }
}
