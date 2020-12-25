using System;

namespace Grayscale.Kifuwaragyoku.Entities
{
    public class EntitiesLayer
    {
        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }
    }
}
