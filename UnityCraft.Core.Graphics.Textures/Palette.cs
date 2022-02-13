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

        internal Color[,] GetPixels(Size size, AlphaBits alphaBits, byte[] data)
        {
            var result = new Color[size.Width, size.Height];

            for (var y = 0; y < size.Height; y++)
            {
                for (var x = 0; x < size.Width; x++)
                {
                    var index = x + (size.Width * y);

                    var color = colors[data[index]];
                    var alpha = GetAlpha(result.Length, alphaBits, data, index);

                    result[x, y] = Color.FromArgb(alpha, color.B, color.G, color.R);
                }
            }

            return result;
        }

        private byte GetAlpha(int length, AlphaBits alphaBits, byte[] data, int index)
        {
            byte b;

            switch (alphaBits.Value)
            {
                case 1:
                    b = data[length + (index / 8)];
                    return (byte)((b & (1 << (index % 8))) == 0 ? 0 : 1);

                case 4:
                    b = data[length + (index / 2)];
                    return (byte)((index % 2 == 0) ? ((b & 0x0F) << 4) : (b & 0xF0));

                case 8:
                    return data[length + index];

                default:
                    return byte.MaxValue;
            }
        }
    }
}
