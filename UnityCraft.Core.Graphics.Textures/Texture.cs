using System;
using System.Drawing;
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

        private readonly Stream stream;

        private readonly Signature signature;
        private readonly Compression compression;

        private readonly AlphaBits alphaBits;

        private readonly uint width;
        private readonly uint height;

        private readonly Extra extra;
        private readonly bool hasMipmaps;

        private readonly MipmapLocator mipmapLocator;

        private readonly Palette palette;

        public Texture(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                signature = reader.ReadUInt32<Signature>();
                if (!supportedSignatures.Contains(signature))
                {
                    throw new NotSupportedException($"{signature} is not supported.");
                }

                compression = reader.ReadUInt32<Compression>();

                alphaBits = new AlphaBits(reader.ReadUInt32(), compression);

                width = reader.ReadUInt32();
                height = reader.ReadUInt32();

                extra = new Extra(reader.ReadUInt32());
                hasMipmaps = Convert.ToBoolean(reader.ReadUInt32());

                mipmapLocator = new MipmapLocator(reader);

                switch (compression)
                {
                    case Compression.Jpeg:
                        break;

                    case Compression.Palette:
                        palette = new Palette(reader);
                        break;
                }
            }
        }

        public Color[,] GetPixels(int mipmapLevel)
        {
            mipmapLevel = MathHelper.Clamp(mipmapLevel, 0, 16);

            var data = GetRawData(mipmapLevel);

            var scale = (int)Math.Pow(2, mipmapLevel);
            var size = new Size((int)width / scale, (int)height / scale);

            switch (compression)
            {
                case Compression.Jpeg:
                    // TODO: Implement
                    throw new NotImplementedException();

                case Compression.Palette:
                    if (palette == null)
                    {
                        throw new NullReferenceException(nameof(palette));
                    }

                    return palette.GetPixels(size, data);

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        private byte[] GetRawData(int mipmapLevel)
        {
            if (stream == null)
            {
                throw new ObjectDisposedException(nameof(stream));
            }

            var mipmap = mipmapLocator.MipmapHeaders[mipmapLevel];

            var data = new byte[mipmap.Size];
            stream.Position = mipmap.Offset;
            stream.Read(data, 0, data.Length);

            return data;
        }
    }
}
