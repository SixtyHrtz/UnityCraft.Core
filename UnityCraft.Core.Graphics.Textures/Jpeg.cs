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
            throw new System.NotImplementedException();
        }
    }
}
