using System;
using System.Drawing;
using System.IO;
using JpegLibrary;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class Jpeg : ITextureContent
    {
        private readonly ITexture texture;

        private readonly byte[] header;

        internal Jpeg(ITexture texture, BinaryReader reader)
        {
            this.texture = texture;

            var headerSize = reader.ReadUInt32();
            header = reader.ReadBytes((int)headerSize);
        }

        public Color[,] GetPixels(int mipmapLevel)
        {
            var rawBytes = texture.GetRawBytes(mipmapLevel);
            var bytes = GetBytes(rawBytes);

            return texture.GetPixelsFromBytes(texture.Size, bytes);
        }

        // TODO: Implement and extract
        internal byte[] GetBytes(byte[] data)
        {
            var jpegData = new byte[header.Length + data.Length];

            Array.Copy(header, 0, jpegData, 0, header.Length);
            Array.Copy(data, 0, jpegData, header.Length, data.Length);

            var decoder = new JpegDecoder();
            decoder.SetInput(jpegData);
            decoder.Identify();

            var destination = new byte[decoder.Width * decoder.Height * decoder.NumberOfComponents];

            if (decoder.Precision != 8)
            {
                throw new NotImplementedException();
            }
            else
            {
                var outputWriter = new JpegBlockOutputWriter8Bit(decoder, destination);
                decoder.SetOutputWriter(outputWriter);
            }

            decoder.Decode();

            return destination;
        }
    }
}
