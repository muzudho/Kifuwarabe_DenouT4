using Grayscale.A060Application.B110Log.C500Struct;

namespace Grayscale.A060Application.B110Log.C500Struct
{
    public class ErrorControllerImpl : IErrorController
    {

        /// <summary>
        /// 用途は任意のイベント・ハンドラー＜その１＞。主にフォームにログ出力するのに使う。
        /// </summary>
        public OnAppendLogDelegater OnAppendLog { get; set; }
        public OnClearLogDelegater OnClearLog { get; set; }
    }
}
