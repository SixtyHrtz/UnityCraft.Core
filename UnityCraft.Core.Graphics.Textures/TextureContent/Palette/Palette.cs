using System;
using System.Drawing;
using System.IO;
using UnityCraft.Core.Extensions;
using UnityCraft.Core.Graphics.Textures.Interfaces;

namespace UnityCraft.Core.Graphics.Textures.TextureContent.Palette
{
    internal class Palette : ITextureContent
    {
        private const int PaletteSize = 256;

        private readonly ITexture texture;

        private readonly Color[] colors;

        internal Palette(ITexture texture, BinaryReader reader)
        {
            this.texture = texture;

            colors = new Color[PaletteSize];
            colors = reader.ReadColorRgbaArray(colors.Length);
        }

        public Color[,] GetPixels(int mipmapLevel)
        {
            var scale = (int)Math.Pow(2, mipmapLevel);
            var size = new Size(texture.Size.Width / scale, texture.Size.Height / scale);

            var rawBytes = texture.GetRawBytes(mipmapLevel);
            var bytes = GetBytes(size, rawBytes, texture.AlphaBits);

            return texture.GetPixelsFromBytes(size, bytes);
        }

        internal byte[] GetBytes(Size size, byte[] data, uint alphaBits)
        {
            var length = size.Width * size.Height;
            var result = new byte[length * 4];

            for (int i = 0; i < length; i++)
            {
                var color = colors[data[i]];

                result[(i * 4) + 0] = color.R;
                result[(i * 4) + 1] = color.G;
                result[(i * 4) + 2] = color.B;
                result[(i * 4) + 3] = GetAlpha(alphaBits, data, i, length);
            }

            return result;
        }

        private byte GetAlpha(uint alphaBits, byte[] data, int index, int length)
        {
            byte @byte;

            switch (alphaBits)
            {
                case 1:
                    @byte = data[length + (index / 8)];
                    return (@byte & (1 << (index % 8))) == 0 ? byte.MinValue : byte.MaxValue;

                case 4:
                    @byte = data[length + (index / 2)];
                    return (byte)((index % 2 == 0) ? ((@byte & 0x0F) << 4) : (@byte & 0xF0));

                case 8:
                    return data[length + index];

                default:
                    return byte.MaxValue;
            }
        }
    }
}
