using System.IO;
using System.Linq;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class MipmapLocator
    {
        internal const int MaxMipmapsCount = 16;

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

        internal MipmapHeader[] MipmapHeaders { get; }

        internal int MipmapsCount { get; }

        internal static int NormalizeMipmapLevel(int mipmapLevel)
        {
            return MathHelper.Clamp(mipmapLevel, 0, MaxMipmapsCount - 1);
        }
    }
}
