using System.Linq;

namespace UnityCraft.Core.Graphics.Textures.Models
{
    internal class Extra
    {
        private const uint DefaultValue = 5;

        private static readonly uint[] ValidExtraValues = new uint[] { 3, 4, 5 };

        internal Extra(uint value)
        {
            Value = Normalize(value);
        }

        internal uint Value { get; private set; }

        private uint Normalize(uint value)
        {
            return ValidExtraValues.Contains(value) ? value : DefaultValue;
        }
    }
}
