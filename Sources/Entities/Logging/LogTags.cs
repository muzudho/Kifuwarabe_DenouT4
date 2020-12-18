namespace Grayscale.Kifuwaragyoku.Entities.Logging
{
    /// <summary>
    /// ログのタグ全集。
    /// </summary>
    public class LogTags
    {
        public static readonly ILogTag Default = new LogTag("Default");
        public static readonly ILogTag ProcessNoneError = new LogTag("ProcessNoneError");
        public static readonly ILogTag PeocessNoneSennitite = new LogTag("PeocessNoneSennitite");
        public static readonly ILogTag ProcessServerDefault = new LogTag("ProcessServerDefault");
        public static readonly ILogTag ProcessServerNetworkAsync = new LogTag("ProcessServerNetworkAsync");
        public static readonly ILogTag ProcessGuiDefault = new LogTag("ProcessGuiDefault");
        public static readonly ILogTag ProcessGuiKifuYomitori = new LogTag("ProcessGuiKifuYomitori");
        public static readonly ILogTag ProcessGuiNetwork = new LogTag("ProcessGuiNetwork");
        public static readonly ILogTag ProcessGuiPaint = new LogTag("ProcessGuiPaint");
        public static readonly ILogTag ProcessGuiSennitite = new LogTag("ProcessGuiSennitite");
        public static readonly ILogTag ProcessAimsDefault = new LogTag("ProcessAimsDefault");
        public static readonly ILogTag ProcessEngineDefault = new LogTag("ProcessEngineDefault");
        public static readonly ILogTag ProcessEngineNetwork = new LogTag("ProcessEngineNetwork");
        public static readonly ILogTag ProcessEngineSennitite = new LogTag("ProcessEngineSennitite");
        public static readonly ILogTag ProcessTestProgramDefault = new LogTag("ProcessTestProgramDefault");
        public static readonly ILogTag ProcessLearnerDefault = new LogTag("ProcessLearnerDefault");
        public static readonly ILogTag ProcessSpeedTestKeisoku = new LogTag("ProcessSpeedTestKeisoku");
        public static readonly ILogTag ProcessUnitTestDefault = new LogTag("ProcessUnitTestDefault");
    }
}
