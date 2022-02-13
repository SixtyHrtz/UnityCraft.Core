using System;
using System.Drawing;
using System.IO;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class Jpeg
    {
        private readonly byte[] header;

        internal Jpeg(BinaryReader reader)
        {
            var headerSize = reader.ReadUInt32();
            header = reader.ReadBytes((int)headerSize);
        }

        internal Color[,] GetPixels(byte[] data)
        {
            var jpegData = new byte[header.Length + data.Length];

            Array.Copy(header, 0, jpegData, 0, header.Length);
            Array.Copy(data, 0, jpegData, header.Length, data.Length);

            throw new NotImplementedException();
        }
    }
}
