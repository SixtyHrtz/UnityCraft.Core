using System.IO;
using System.Linq;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class MipmapLocator
    {
        public const int MaxMipmapsCount = 16;

        internal MipmapLocator(BinaryReader reader)
        {
            MipmapHeaders = new MipmapHeader[MaxMipmapsCount];

            var offsets = reader.ReadUInt32Array(MipmapHeaders.Length);
            var sizes = reader.ReadUInt32Array(MipmapHeaders.Length);

            for (int i = 0; i < MipmapHeaders.Length; i++)
            {
                MipmapHeaders[i] = new MipmapHeader
                {
                    Offset = offsets[i],
                    Size = sizes[i],
                };
            }

            MipmapsCount = MipmapHeaders.Count(x => x.Offset != 0);
        }

        public int MipmapsCount { get; }

        internal MipmapHeader[] MipmapHeaders { get; }
    }
}
