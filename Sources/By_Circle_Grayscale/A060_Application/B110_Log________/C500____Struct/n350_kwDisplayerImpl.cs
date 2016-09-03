using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;

namespace Grayscale.A060_Application.B110_Log________.C500____Struct
{
    public class KwDisplayerImpl : KwDisplayer
    {

        /// <summary>
        /// 用途は任意のイベント・ハンドラー＜その１＞。主にフォームにログ出力するのに使う。
        /// </summary>
        public DLGT_OnLogAppend Dlgt_OnLog1Append_or_Null { get; set; }
        public DLGT_OnLogClear Dlgt_OnLog1Clear_or_Null { get; set; }

        /// <summary>
        /// 用途は任意のイベント・ハンドラー＜その２＞。主にフォームにログ出力するのに使う。
        /// </summary>
        public DLGT_OnLogAppend Dlgt_OnNaibuDataAppend_or_Null { get; set; }
        public DLGT_OnLogClear Dlgt_OnNaibuDataClear_or_Null { get; set; }

    }
}
