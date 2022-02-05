using System.IO;
using System.Linq;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class MipmapLocator
    {
        private const int MaxMipmapsCount = 16;

        private readonly MipmapHeader[] mipmapHeaders;
        private readonly int mipmapsCount = 0;

        internal MipmapLocator(BinaryReader reader)
        {
            mipmapHeaders = new MipmapHeader[MaxMipmapsCount];

            var offsets = reader.ReadUInt32Array(mipmapHeaders.Length);
            var sizes = reader.ReadUInt32Array(mipmapHeaders.Length);

            for (int i = 0; i < mipmapHeaders.Length; i++)
            {
                mipmapHeaders[i] = new MipmapHeader
                {
                    Offset = offsets[i],
                    Size = sizes[i],
                };
            }

            mipmapsCount = mipmapHeaders.Count(x => x.Offset != 0);
        }
    }
}
