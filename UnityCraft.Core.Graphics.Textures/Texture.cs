using System;
using System.IO;
using System.Linq;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    public class Texture
    {
        private readonly Signature[] supportedSignatures = new Signature[]
        {
            Signature.BLP1,
        };

        private readonly Signature signature;
        private readonly AlphaBits alphaBits;

        private readonly uint width;
        private readonly uint height;

        private readonly Extra extra;
        private readonly bool hasMipmaps;

        public Texture(BinaryReader reader)
        {
            signature = reader.ReadUInt32<Signature>();
            if (!supportedSignatures.Contains(signature))
            {
                throw new NotSupportedException($"{signature} is not supported.");
            }

            var compression = reader.ReadUInt32<Compression>();

            alphaBits = new AlphaBits(reader.ReadUInt32(), compression);

            width = reader.ReadUInt32();
            height = reader.ReadUInt32();

            extra = new Extra(reader.ReadUInt32());
            hasMipmaps = Convert.ToBoolean(reader.ReadUInt32());

            var mipmapLocator = new MipmapLocator(reader);

            switch (compression)
            {
                case Compression.Jpeg:
                    break;

                case Compression.Palette:
                    new Palette(reader);
                    break;
            }
        }
    }
}
