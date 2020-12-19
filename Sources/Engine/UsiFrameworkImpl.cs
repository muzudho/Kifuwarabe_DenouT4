using System;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C250UsiLoop;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C500UsiFrame;

namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C500____usiFrame___
{
    public class UsiFrameworkImpl : IUsiFramework
    {
        public UsiFrameworkImpl()
        {
            this.OnApplicationBegin = this.m_nullFunc01;
            this.OnLogDase = this.m_nullFunc02;
        }

        public Func01 m_nullFunc01 = delegate ()
        {

        };
        public Func02 m_nullFunc02 = delegate (string line)
        {
            return PhaseResultUsiLoop2.None;
        };
        public Func05 m_nullFunc05 = delegate ()
        {
            return "";
        };

        public Func01 OnApplicationBegin { get; set; }

        /// <summary>
        /// 独自コマンド「ログ出せ」
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnLogDase { get; set; }
    }
}
