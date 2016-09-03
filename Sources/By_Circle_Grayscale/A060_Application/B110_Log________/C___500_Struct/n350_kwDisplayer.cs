namespace Grayscale.A060_Application.B110_Log________.C___500_Struct
{

    /// <summary>
    /// ログを書くタイミングで。
    /// </summary>
    /// <param name="log"></param>
    public delegate void DLGT_OnLogAppend(string log);
    public delegate void DLGT_OnLogClear();

    public interface KwDisplayer
    {

        /// <summary>
        /// 用途は任意のイベント・ハンドラー＜その１＞。主にフォームにログ出力するのに使う。任意に着脱可。
        /// </summary>
        DLGT_OnLogAppend Dlgt_OnLog1Append_or_Null { get; set; }
        DLGT_OnLogClear Dlgt_OnLog1Clear_or_Null { get; set; }

        ///// <summary>
        ///// 用途は任意のイベント・ハンドラー＜その２＞。主にフォームにログ出力するのに使う。任意に着脱可。
        ///// </summary>
        //DLGT_OnLogAppend Dlgt_OnNaibuDataAppend_or_Null { get; set; }
        //DLGT_OnLogClear Dlgt_OnNaibuDataClear_or_Null { get; set; }

    }
}
