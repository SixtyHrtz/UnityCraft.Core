using System.Drawing;
using System.IO;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class Palette
    {
        private const int PaletteSize = 256;

        private readonly Color[] colors;

        internal Palette(BinaryReader reader)
        {
            colors = new Color[PaletteSize];
            colors = reader.ReadColorRgbaArray(colors.Length);
        }

        internal Color[,] GetPixels(Size size, byte[] data)
        {
            var result = new Color[size.Width, size.Height];

            for (var y = 0; y < size.Height; y++)
            {
                for (var x = 0; x < size.Width; x++)
                {
                    var index = x + (size.Width * y);
                    result[x, y] = colors[data[index]];
                }
            }

            return result;
        }
    }
}
