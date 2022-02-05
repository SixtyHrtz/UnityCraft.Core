using System.IO;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class Palette
    {
        private const int PaletteSize = 256;

        private readonly uint[] colors;

        internal Palette(BinaryReader reader)
        {
            colors = new uint[PaletteSize];
            colors = reader.ReadUInt32Array(colors.Length);
        }
    }
}
