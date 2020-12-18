namespace Grayscale.Kifuwaragyoku.Entities.Logging
{

    /// <summary>
    /// ログを書くタイミングで。
    /// </summary>
    /// <param name="log"></param>
    public delegate void OnAppendLogDelegater(string log);
    public delegate void OnClearLogDelegater();

    public interface IErrorController
    {
        /// <summary>
        /// 用途は任意のイベント・ハンドラー＜その１＞。主にフォームにログ出力するのに使う。任意に着脱可。Null可。
        /// </summary>
        OnAppendLogDelegater OnAppendLog { get; set; }

        /// <summary>
        /// 用途は任意のイベント・ハンドラー＜その１＞。主にフォームにログ出力するのに使う。任意に着脱可。Null可。
        /// </summary>
        OnClearLogDelegater OnClearLog { get; set; }
    }
}
