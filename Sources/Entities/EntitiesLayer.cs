using System;
using Grayscale.Kifuwaragyoku.Entities.Configuration;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.Entities
{
    public class EntitiesLayer
    {
        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }

        public static void Implement(IEngineConf engineConf)
        {
            SpecifiedFiles.Init(engineConf);
            Logger.Init(engineConf);
            // Util_KifuTreeLogWriter.Init(engineConf);
        }
    }
}
