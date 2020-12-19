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
            this.OnApplicationEnd = this.m_nullFunc01;
            this.OnCommandlineAtLoop1 = this.m_nullFunc05;
            this.OnCommandlineAtLoop2 = this.m_nullFunc05;
            this.OnGameover = this.m_nullFunc02;
            this.OnGoponder = this.m_nullFunc02;
            this.OnGo = this.m_nullFunc02;
            this.OnIsready = this.m_nullFunc04;
            this.OnLogDase = this.m_nullFunc02;
            this.OnLoop2Begin = this.m_nullFunc01;
            this.OnLoop2End = this.m_nullFunc01;
            this.OnPosition = this.m_nullFunc02;
            this.OnQuit = this.m_nullFunc04;
            this.OnSetoption = this.m_nullFunc04;
            this.OnStop = this.m_nullFunc02;
            this.OnUsinewgame = this.m_nullFunc04;
            this.OnUsi = this.m_nullFunc04;
        }

        public Func01 m_nullFunc01 = delegate ()
        {

        };
        public Func02 m_nullFunc02 = delegate (string line)
        {
            return PhaseResultUsiLoop2.None;
        };
        public Func04 m_nullFunc04 = delegate (string line)
        {
            return PhaseResultUsiLoop1.None;
        };
        public Func05 m_nullFunc05 = delegate ()
        {
            return "";
        };

        public Func01 OnApplicationBegin { get; set; }
        public Func01 OnApplicationEnd { get; set; }
        public Func01 OnLoop2Begin { get; set; }

        /// <summary>
        /// Loop2のEnd部で呼ばれます。
        /// </summary>
        public Func01 OnLoop2End { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func05 OnCommandlineAtLoop2 { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnPosition { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnGoponder { get; set; }

        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnGo { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnStop { get; set; }

        /// <summary>
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnGameover { get; set; }

        /// <summary>
        /// 独自コマンド「ログ出せ」
        /// Loop2のBody部で呼ばれます。
        /// </summary>
        public Func02 OnLogDase { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnUsi { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnSetoption { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnIsready { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnUsinewgame { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func04 OnQuit { get; set; }

        /// <summary>
        /// Loop1のBody部で呼ばれます。
        /// </summary>
        public Func05 OnCommandlineAtLoop1 { get; set; }
    }
}
