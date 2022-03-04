using System.Linq;
using UnityCraft.Core.Graphics.Textures.Enums;

namespace UnityCraft.Core.Graphics.Textures.Models
{
    internal class AlphaBits
    {
        private const uint DefaultValue = 0;

        private static readonly uint[] ValidJpegAlphaBits = new uint[] { 0, 8 };
        private static readonly uint[] ValidPaletteAlphaBits = new uint[] { 0, 1, 4, 8 };

        internal AlphaBits(uint value, Compression compression)
        {
            Value = Normalize(value, compression);
        }

        internal uint Value { get; private set; }

        private uint Normalize(uint value, Compression compression)
        {
            switch (compression)
            {
                case Compression.Jpeg:
                    return ValidJpegAlphaBits.Contains(value) ? value : DefaultValue;

                case Compression.Palette:
                    return ValidPaletteAlphaBits.Contains(value) ? value : DefaultValue;

                default:
                    return 0;
            }
        }
    }
}
