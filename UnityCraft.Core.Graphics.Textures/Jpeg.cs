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
            var jpegBody = texture.GetRawBytes(mipmapLevel);
            var jpegData = GetJpegBytes(jpegBody);
            var decodedData = GetDecodedBytes(jpegData);

            return texture.GetPixelsFromBytes(texture.Size, decodedData);
        }

        private byte[] GetJpegBytes(byte[] jpegData)
        {
            var result = new byte[header.Length + jpegData.Length];

            Array.Copy(header, 0, result, 0, header.Length);
            Array.Copy(jpegData, 0, result, header.Length, jpegData.Length);

            return result;
        }

        private byte[] GetDecodedBytes(byte[] jpegData)
        {
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
