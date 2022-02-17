using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    public class Texture : ITexture, IDisposable
    {
        private readonly Signature[] supportedSignatures = new Signature[]
        {
            Signature.BLP1,
        };

        private readonly Signature signature;
        private readonly Compression compression;

        private readonly AlphaBits alphaBits;

        private readonly Size size;

        private readonly Extra extra;
        private readonly bool hasMipmaps;

        private readonly MipmapLocator mipmapLocator;

        private readonly ITextureContent textureContent;

        private Stream stream;

        public Texture(Stream stream)
        {
            this.stream = stream;

            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                signature = reader.ReadUInt32<Signature>();
                if (!supportedSignatures.Contains(signature))
                {
                    throw new NotSupportedException($"{signature} is not supported.");
                }

                compression = reader.ReadUInt32<Compression>();

                alphaBits = new AlphaBits(reader.ReadUInt32(), compression);

                size = reader.ReadSize();

                extra = new Extra(reader.ReadUInt32());
                hasMipmaps = Convert.ToBoolean(reader.ReadUInt32());

                mipmapLocator = new MipmapLocator(reader);

                switch (compression)
                {
                    case Compression.Jpeg:
                        textureContent = new Jpeg(this, reader);
                        break;

                    case Compression.Palette:
                        textureContent = new Palette(this, reader);
                        break;
                }
            }
        }

        public uint AlphaBits => alphaBits.Value;

        public Size Size => size;

        public int MipmapCount => mipmapLocator.MipmapsCount;

        public Color[,] GetPixels(int mipmapLevel)
        {
            mipmapLevel = MipmapLocator.NormalizeMipmapLevel(mipmapLevel);
            return textureContent.GetPixels(mipmapLevel);
        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        public Color[,] GetPixelsFromBytes(Size size, byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var result = new Color[size.Width, size.Height];

            var index = 0;
            for (var y = 0; y < size.Height; y++)
            {
                for (var x = 0; x < size.Width; x++)
                {
                    var blue = bytes[index++];
                    var green = bytes[index++];
                    var red = bytes[index++];
                    var alpha = bytes[index++];

                    result[x, y] = Color.FromArgb(alpha, red, green, blue);
                }
            }

            return result;
        }

        public byte[] GetRawBytes(int mipmapLevel)
        {
            mipmapLevel = MipmapLocator.NormalizeMipmapLevel(mipmapLevel);

            var mipmapHeader = mipmapLocator.MipmapHeaders[mipmapLevel];

            var data = new byte[mipmapHeader.Size];
            stream.Position = mipmapHeader.Offset;
            stream.Read(data, 0, data.Length);

            return data;
        }
    }
}
